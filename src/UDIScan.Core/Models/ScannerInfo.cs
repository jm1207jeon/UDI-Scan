using System;

namespace UDIScan.Core.Models
{
    /// <summary>
    /// 스캐너 장치 정보
    /// </summary>
    public class ScannerInfo
    {
        public int ScannerId { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public string FirmwareVersion { get; set; } = string.Empty;
        public bool IsConnected { get; set; }

        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(ModelNumber))
                    return $"{ModelNumber} ({SerialNumber})";
                return SerialNumber;
            }
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
