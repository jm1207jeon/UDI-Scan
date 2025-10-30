using System;

namespace UDIScan.Core.Models
{
    /// <summary>
    /// 이미지 캡처 결과
    /// </summary>
    public class ImageCaptureResult
    {
        public byte[] ImageData { get; set; } = Array.Empty<byte>();
        public string FilePath { get; set; } = string.Empty;
        public DateTime CaptureTime { get; set; }
        public int ScannerId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public ImageCaptureResult()
        {
            CaptureTime = DateTime.Now;
        }

        public static ImageCaptureResult CreateSuccess(byte[] imageData, string filePath, int scannerId)
        {
            return new ImageCaptureResult
            {
                ImageData = imageData,
                FilePath = filePath,
                ScannerId = scannerId,
                Success = true
            };
        }

        public static ImageCaptureResult CreateFailure(string errorMessage, int scannerId = 0)
        {
            return new ImageCaptureResult
            {
                Success = false,
                ErrorMessage = errorMessage,
                ScannerId = scannerId
            };
        }
    }
}
