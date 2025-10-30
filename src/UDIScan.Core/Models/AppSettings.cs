using System;
using System.IO;

namespace UDIScan.Core.Models
{
    /// <summary>
    /// 애플리케이션 설정 모델
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// 이미지 저장 경로 (기본값: 내 문서\UDIScan\Images)
        /// </summary>
        public string ImageSavePath { get; set; } = GetDefaultImagePath();

        /// <summary>
        /// 이미지 캡처 활성화 여부 (기본값: true)
        /// </summary>
        public bool IsImageCaptureEnabled { get; set; } = true;

        /// <summary>
        /// 마지막으로 연결된 스캐너 ID
        /// </summary>
        public string LastScannerId { get; set; } = string.Empty;

        /// <summary>
        /// 자동으로 Enter 키 입력 여부 (기본값: true)
        /// </summary>
        public bool AutoSendEnter { get; set; } = true;

        /// <summary>
        /// 스캔 이력 최대 개수 (기본값: 100)
        /// </summary>
        public int MaxScanHistoryCount { get; set; } = 100;

        private static string GetDefaultImagePath()
        {
            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Path.Combine(myDocuments, "UDIScan", "Images");
        }

        /// <summary>
        /// 설정 유효성 검사
        /// </summary>
        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(ImageSavePath))
            {
                ImageSavePath = GetDefaultImagePath();
            }

            if (MaxScanHistoryCount <= 0)
            {
                MaxScanHistoryCount = 100;
            }

            return true;
        }
    }
}
