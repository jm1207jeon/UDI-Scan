using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using UDIScan.Core.Services;
using UDIScan.Core.ViewModels;

namespace UDIScan.App
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            // Dependency Injection (간단한 방식)
            var coreScannerService = new CoreScannerService();
            var keyboardSimulator = new KeyboardSimulator();
            var imageService = new ImageService();
            var settingsService = new SettingsService();

            _viewModel = new MainViewModel(
                coreScannerService,
                keyboardSimulator,
                imageService,
                settingsService);

            DataContext = _viewModel;

            // Converter 추가
            Resources.Add("BoolToColorConverter", new BoolToColorConverter());

            Loaded += async (s, e) =>
            {
                await _viewModel.InitializeAsync();
            };

            Closing += (s, e) =>
            {
                _viewModel?.Dispose();
            };
        }
    }

    /// <summary>
    /// Bool to Color Converter (연결 상태 표시용)
    /// </summary>
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isConnected)
            {
                return isConnected ? new SolidColorBrush(Color.FromRgb(76, 175, 80)) // Green
                                   : new SolidColorBrush(Color.FromRgb(158, 158, 158)); // Gray
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
