using System.Windows;
using UDIScan.Core.Services;
using UDIScan.Core.ViewModels;

namespace UDIScan.App
{
    /// <summary>
    /// Main Window (이미지 캡처 전용)
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            // Dependency Injection (초심플 방식)
            var coreScannerService = new CoreScannerService();
            var imageService = new ImageService();

            _viewModel = new MainViewModel(
                coreScannerService,
                imageService);

            DataContext = _viewModel;

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
}
