using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace UDIScan.Core.Services
{
    /// <summary>
    /// 이미지 처리 서비스 (이미지 캡처 전용)
    /// </summary>
    public class ImageService : IImageService
    {
        public async Task<string> SaveImageAsync(byte[] imageData, string savePath)
        {
            try
            {
                // 저장 경로 디렉토리 확인 및 생성
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                // 파일명 생성: 타임스탬프만 사용 (yyyyMMdd_HHmmss_fff.jpg)
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                string fileName = $"{timestamp}.jpg";
                string fullPath = Path.Combine(savePath, fileName);

                // 비동기로 파일 저장 (UI 블로킹 없음)
                await File.WriteAllBytesAsync(fullPath, imageData);

                return fullPath;
            }
            catch (Exception ex)
            {
                throw new IOException($"이미지 저장 실패: {ex.Message}", ex);
            }
        }

        public BitmapImage CreateBitmapImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                throw new ArgumentException("이미지 데이터가 비어있습니다.", nameof(imageData));

            var bitmap = new BitmapImage();
            using (var stream = new MemoryStream(imageData))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                bitmap.Freeze(); // UI 스레드 간 공유를 위해 Freeze
            }

            return bitmap;
        }

        public BitmapImage CreateThumbnail(byte[] imageData, int maxWidth, int maxHeight)
        {
            if (imageData == null || imageData.Length == 0)
                throw new ArgumentException("이미지 데이터가 비어있습니다.", nameof(imageData));

            var bitmap = new BitmapImage();
            using (var stream = new MemoryStream(imageData))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.DecodePixelWidth = maxWidth;
                bitmap.DecodePixelHeight = maxHeight;
                bitmap.EndInit();
                bitmap.Freeze();
            }

            return bitmap;
        }
    }
}
