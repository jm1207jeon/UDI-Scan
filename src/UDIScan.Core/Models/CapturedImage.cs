using System;

namespace UDIScan.Core.Models
{
    /// <summary>
    /// 캡처된 이미지 데이터 모델
    /// </summary>
    public class CapturedImage
    {
        /// <summary>
        /// 이미지 바이트 데이터
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// 캡처 시간
        /// </summary>
        public DateTime CaptureTime { get; set; }

        /// <summary>
        /// 파일 경로 (저장 후)
        /// </summary>
        public string FilePath { get; set; }

        public CapturedImage(byte[] imageData)
        {
            ImageData = imageData;
            CaptureTime = DateTime.Now;
            FilePath = string.Empty;
        }
    }
}
