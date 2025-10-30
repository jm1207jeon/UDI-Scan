using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UDIScan.Core.Models;
using UDIScan.Core.Services;

namespace UDIScan.Core.ViewModels
{
    /// <summary>
    /// Main Window ViewModel
    /// </summary>
    public class MainViewModel : BaseViewModel, IDisposable
    {
        private readonly ICoreScannerService _coreScannerService;
        private readonly IKeyboardSimulator _keyboardSimulator;
        private readonly IImageService _imageService;
        private readonly ISettingsService _settingsService;
        private readonly ConcurrentQueue<BarcodeData> _scanQueue;

        private AppSettings _settings;
        private bool _isImageCaptureEnabled;
        private string _imageSavePath;
        private string _scannerStatus;
        private bool _isConnected;
        private BitmapImage? _lastCapturedImage;
        private int _totalScansCount;
        private int _imagesSavedCount;
        private BarcodeData? _lastScanData;

        public ObservableCollection<BarcodeData> ScanHistory { get; }

        // Properties
        public bool IsImageCaptureEnabled
        {
            get => _isImageCaptureEnabled;
            set
            {
                if (SetProperty(ref _isImageCaptureEnabled, value))
                {
                    _settings.IsImageCaptureEnabled = value;
                    _ = SaveSettingsAsync();
                }
            }
        }

        public string ImageSavePath
        {
            get => _imageSavePath;
            set
            {
                if (SetProperty(ref _imageSavePath, value))
                {
                    _settings.ImageSavePath = value;
                    _ = SaveSettingsAsync();
                }
            }
        }

        public string ScannerStatus
        {
            get => _scannerStatus;
            set => SetProperty(ref _scannerStatus, value);
        }

        public bool IsConnected
        {
            get => _isConnected;
            set => SetProperty(ref _isConnected, value);
        }

        public BitmapImage? LastCapturedImage
        {
            get => _lastCapturedImage;
            set => SetProperty(ref _lastCapturedImage, value);
        }

        public int TotalScansCount
        {
            get => _totalScansCount;
            set => SetProperty(ref _totalScansCount, value);
        }

        public int ImagesSavedCount
        {
            get => _imagesSavedCount;
            set => SetProperty(ref _imagesSavedCount, value);
        }

        public string StatusText => $"Ready | Total Scans: {TotalScansCount} | Images Saved: {ImagesSavedCount}";

        // Commands
        public ICommand ToggleImageCaptureCommand { get; }
        public ICommand SelectSavePathCommand { get; }
        public ICommand ClearHistoryCommand { get; }
        public ICommand ReconnectCommand { get; }

        public MainViewModel(
            ICoreScannerService coreScannerService,
            IKeyboardSimulator keyboardSimulator,
            IImageService imageService,
            ISettingsService settingsService)
        {
            _coreScannerService = coreScannerService;
            _keyboardSimulator = keyboardSimulator;
            _imageService = imageService;
            _settingsService = settingsService;
            _scanQueue = new ConcurrentQueue<BarcodeData>();

            ScanHistory = new ObservableCollection<BarcodeData>();

            _settings = new AppSettings();
            _isImageCaptureEnabled = true;
            _imageSavePath = _settings.ImageSavePath;
            _scannerStatus = "Initializing...";
            _isConnected = false;

            // Commands 초기화
            ToggleImageCaptureCommand = new RelayCommand(() =>
            {
                IsImageCaptureEnabled = !IsImageCaptureEnabled;
            });

            SelectSavePathCommand = new RelayCommand(SelectSavePath);
            ClearHistoryCommand = new RelayCommand(ClearHistory);
            ReconnectCommand = new RelayCommand(async () => await ReconnectAsync());

            // 이벤트 핸들러 등록
            _coreScannerService.BarcodeScanned += OnBarcodeScanned;
            _coreScannerService.ImageCaptured += OnImageCaptured;
            _coreScannerService.ConnectionChanged += OnConnectionChanged;

            // 백그라운드 스캔 처리 시작
            _ = Task.Run(ProcessScanQueueAsync);
        }

        public async Task InitializeAsync()
        {
            try
            {
                // 설정 로드
                _settings = await _settingsService.LoadSettingsAsync();
                IsImageCaptureEnabled = _settings.IsImageCaptureEnabled;
                ImageSavePath = _settings.ImageSavePath;

                // CoreScanner 초기화
                bool initialized = await _coreScannerService.InitializeAsync();
                if (!initialized)
                {
                    ScannerStatus = "Failed to initialize CoreScanner";
                    return;
                }

                // 스캐너 연결
                bool connected = await _coreScannerService.ConnectAsync();
                if (!connected)
                {
                    ScannerStatus = "No scanner found";
                    return;
                }

                // 연결된 스캐너 정보 표시
                var scanners = _coreScannerService.GetConnectedScanners();
                if (scanners.Any())
                {
                    var scanner = scanners.First();
                    ScannerStatus = $"Connected: {scanner.DisplayName}";
                    IsConnected = true;
                }
            }
            catch (Exception ex)
            {
                ScannerStatus = $"Error: {ex.Message}";
            }
        }

        private void OnBarcodeScanned(object? sender, BarcodeData e)
        {
            // 큐에 추가 (비동기 처리)
            _scanQueue.Enqueue(e);
        }

        private async Task ProcessScanQueueAsync()
        {
            while (true)
            {
                try
                {
                    if (_scanQueue.TryDequeue(out var barcodeData))
                    {
                        _lastScanData = barcodeData;

                        // UI 스레드에서 실행
                        await Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            ScanHistory.Insert(0, barcodeData);

                            // 이력 개수 제한
                            if (ScanHistory.Count > _settings.MaxScanHistoryCount)
                            {
                                ScanHistory.RemoveAt(ScanHistory.Count - 1);
                            }

                            TotalScansCount++;
                            OnPropertyChanged(nameof(StatusText));
                        });

                        // 키보드 입력 시뮬레이션
                        _keyboardSimulator.TypeText(barcodeData.Barcode);

                        if (_settings.AutoSendEnter)
                        {
                            _keyboardSimulator.SendEnter();
                        }

                        // 이미지 캡처 (활성화된 경우)
                        if (IsImageCaptureEnabled)
                        {
                            var scanners = _coreScannerService.GetConnectedScanners();
                            if (scanners.Any())
                            {
                                await _coreScannerService.CaptureImageAsync(scanners.First().ScannerId);
                            }
                        }
                    }

                    await Task.Delay(10);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"스캔 처리 오류: {ex.Message}");
                }
            }
        }

        private void OnImageCaptured(object? sender, ImageCaptureResult e)
        {
            if (!e.Success)
            {
                System.Diagnostics.Debug.WriteLine($"이미지 캡처 실패: {e.ErrorMessage}");
                return;
            }

            try
            {
                // 이미지 저장
                string barcode = _lastScanData?.Barcode ?? "unknown";
                _ = Task.Run(async () =>
                {
                    try
                    {
                        string filePath = await _imageService.SaveImageAsync(e.ImageData, barcode, ImageSavePath);

                        // UI 업데이트
                        await Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            LastCapturedImage = _imageService.CreateThumbnail(e.ImageData, 300, 300);
                            ImagesSavedCount++;
                            OnPropertyChanged(nameof(StatusText));
                        });
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"이미지 저장 실패: {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"이미지 처리 오류: {ex.Message}");
            }
        }

        private void OnConnectionChanged(object? sender, bool isConnected)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                IsConnected = isConnected;
                ScannerStatus = isConnected ? "Scanner Connected" : "Scanner Disconnected";
            });
        }

        private void SelectSavePath()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Select image save folder",
                SelectedPath = ImageSavePath
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImageSavePath = dialog.SelectedPath;
            }
        }

        private void ClearHistory()
        {
            ScanHistory.Clear();
        }

        private async Task ReconnectAsync()
        {
            ScannerStatus = "Reconnecting...";
            _coreScannerService.Disconnect();
            await Task.Delay(1000);
            await InitializeAsync();
        }

        private async Task SaveSettingsAsync()
        {
            try
            {
                await _settingsService.SaveSettingsAsync(_settings);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"설정 저장 실패: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _coreScannerService?.Dispose();
        }
    }

    /// <summary>
    /// Simple RelayCommand implementation
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object? parameter)
        {
            _execute();
        }
    }
}
