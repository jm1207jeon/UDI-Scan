using System;

namespace UDIScan.Core.Models
{
    /// <summary>
    /// 바코드 스캔 결과 데이터
    /// </summary>
    public class BarcodeData
    {
        public string Barcode { get; set; } = string.Empty;
        public string BarcodeType { get; set; } = string.Empty;
        public DateTime ScanTime { get; set; }
        public int ScannerId { get; set; }

        public BarcodeData()
        {
            ScanTime = DateTime.Now;
        }

        public BarcodeData(string barcode, string barcodeType = "Unknown", int scannerId = 0)
        {
            Barcode = barcode;
            BarcodeType = barcodeType;
            ScannerId = scannerId;
            ScanTime = DateTime.Now;
        }

        public string FormattedScanTime => ScanTime.ToString("HH:mm:ss");

        public override string ToString()
        {
            return $"{FormattedScanTime} - {Barcode} ({BarcodeType})";
        }
    }
}
