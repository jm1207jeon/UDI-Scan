using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UDIScan.Core.Models;
using UDIScan.Core.Services;

namespace UDIScan.Core.ViewModels
{
    /// <summary>
    /// Main Window ViewModel (이미지 캡처 전용 - 초심플)
    /// </summary>
    public class MainViewModel : BaseViewModel, IDisposable
    {
        private readonly ICoreScannerService _coreScannerService;
        private readonly IImageService _imageService;
        private readonly ConcurrentQueue<CapturedImage> _imageQueue;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private AppSettings _settings;
        private string _imageSavePath;
        private string _scannerStatus;
        private bool _isConnected;
        private BitmapImage? _lastCapturedImage;
        private int _imagesCapturedCount;
        private int _queueSize;
        private bool _isCapturing;

        // Properties
        public string ImageSavePath
        {
            get => _imageSavePath;
            set
            {
                if (SetProperty(ref _imageSavePath, value))
                {
                    _settings.ImageSavePath = value;
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

        public int ImagesCapturedCount
        {
            get => _imagesCapturedCount;
            set
            {
                if (SetProperty(ref _imagesCapturedCount, value))
                {
                    OnPropertyChanged(nameof(StatusText));
                }
            }
        }

        public int QueueSize
        {
            get => _queueSize;
            set
            {
                if (SetProperty(ref _queueSize, value))
                {
                    OnPropertyChanged(nameof(StatusText));
                }
            }
        }

        public bool IsCapturing
        {
            get => _isCapturing;
            set => SetProperty(ref _isCapturing, value);
        }

        public string StatusText => $"Images Captured: {ImagesCapturedCount} | Queue: {QueueSize}";

        // Commands
        public ICommand SelectSavePathCommand { get; }
        public ICommand ToggleCaptureCommand { get; }
        public ICommand ReconnectCommand { get; }

        public MainViewModel(
            ICoreScannerService coreScannerService,
            IImageService imageService)
        {
            _coreScannerService = coreScannerService;
            _imageService = imageService;
            _imageQueue = new ConcurrentQueue<CapturedImage>();
            _cancellationTokenSource = new CancellationTokenSource();

            _settings = new AppSettings();
            _settings.Validate();
            _imageSavePath = _settings.ImageSavePath;
            _scannerStatus = "Initializing...";
            _isConnected = false;
            _isCapturing = false;

            // Commands 초기화
            SelectSavePathCommand = new RelayCommand(SelectSavePath);
            ToggleCaptureCommand = new RelayCommand(ToggleCapture);
            ReconnectCommand = new RelayCommand(async () => await ReconnectAsync());

            // 이벤트 핸들러 등록
            _coreScannerService.ImageCaptured += OnImageCaptured;
            _coreScannerService.ConnectionChanged += OnConnectionChanged;

            // 백그라운드 이미지 저장 처리 시작
            _ = Task.Run(() => ProcessImageQueueAsync(_cancellationTokenSource.Token));
        }

        public async Task InitializeAsync()
        {
            try
            {
                // CoreScanner 연결
                bool connected = await _coreScannerService.ConnectAsync();
                if (!connected)
                {
                    ScannerStatus = _coreScannerService.ScannerInfo;
                    IsConnected = false;
                    return;
                }

                // 연결 성공
                ScannerStatus = _coreScannerService.ScannerInfo;
                IsConnected = true;
            }
            catch (Exception ex)
            {
                ScannerStatus = $"Error: {ex.Message}";
                IsConnected = false;
            }
        }

        private void OnImageCaptured(object? sender, CapturedImage e)
        {
            // 이미지 큐에 추가 (비동기 저장)
            _imageQueue.Enqueue(e);

            // UI 카운트 업데이트
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                ImagesCapturedCount++;
                QueueSize = _imageQueue.Count;
            });
        }

        private async Task ProcessImageQueueAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (_imageQueue.TryDequeue(out var capturedImage))
                    {
                        // 이미지 저장
                        string filePath = await _imageService.SaveImageAsync(capturedImage.ImageData, ImageSavePath);

                        // UI 업데이트
                        await Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            try
                            {
                                // 썸네일 생성 및 표시
                                LastCapturedImage = _imageService.CreateThumbnail(capturedImage.ImageData, 300, 300);
                                QueueSize = _imageQueue.Count;
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"UI 업데이트 실패: {ex.Message}");
                            }
                        });

                        System.Diagnostics.Debug.WriteLine($"이미지 저장 완료: {filePath}");
                    }
                    else
                    {
                        // 큐가 비어있으면 잠시 대기
                        await Task.Delay(50, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"이미지 저장 오류: {ex.Message}");

                    // 큐 사이즈 업데이트
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        QueueSize = _imageQueue.Count;
                    });
                }
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
                SelectedPath = ImageSavePath,
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImageSavePath = dialog.SelectedPath;
            }
        }

        private void ToggleCapture()
        {
            IsCapturing = !IsCapturing;
            // Note: DS9908은 항상 트리거를 당기면 이미지를 캡처하도록 설정되어 있습니다.
            // 이 버튼은 사용자에게 "캡처 활성화" 상태를 표시하는 용도입니다.
        }

        private async Task ReconnectAsync()
        {
            ScannerStatus = "Reconnecting...";
            _coreScannerService.Disconnect();
            await Task.Delay(1000);
            await InitializeAsync();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
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
