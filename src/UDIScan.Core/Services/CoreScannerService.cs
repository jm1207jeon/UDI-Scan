using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using CoreScanner;
using UDIScan.Core.Models;

namespace UDIScan.Core.Services
{
    /// <summary>
    /// CoreScanner SDK 서비스 구현
    /// </summary>
    public class CoreScannerService : ICoreScannerService
    {
        private CCoreScanner? _coreScanner;
        private bool _isInitialized;
        private bool _isConnected;
        private List<ScannerInfo> _connectedScanners = new List<ScannerInfo>();

        public event EventHandler<BarcodeData>? BarcodeScanned;
        public event EventHandler<ImageCaptureResult>? ImageCaptured;
        public event EventHandler<bool>? ConnectionChanged;

        public bool IsInitialized => _isInitialized;
        public bool IsConnected => _isConnected;

        public async Task<bool> InitializeAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    _coreScanner = new CCoreScanner();

                    // Open CoreScanner
                    short[] scannerTypes = new short[1];
                    scannerTypes[0] = 1; // All scanner types
                    short numberOfScannerTypes = 1;
                    int status;

                    _coreScanner.Open(0, scannerTypes, numberOfScannerTypes, out status);

                    if (status != 0)
                    {
                        return false;
                    }

                    // 이벤트 핸들러 등록
                    _coreScanner.BarcodeEvent += OnBarcodeEvent;
                    _coreScanner.ImageEvent += OnImageEvent;
                    _coreScanner.VideoEvent += OnVideoEvent;
                    _coreScanner.PNPEvent += OnPNPEvent;

                    _isInitialized = true;
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"CoreScanner 초기화 실패: {ex.Message}");
                    return false;
                }
            });
        }

        public async Task<bool> ConnectAsync()
        {
            if (!_isInitialized || _coreScanner == null)
            {
                return false;
            }

            return await Task.Run(() =>
            {
                try
                {
                    // 연결된 스캐너 조회
                    int numberOfScanners = 0;
                    short[] scannerIdList = new short[255];
                    string outXml;
                    int status;

                    _coreScanner.GetScanners(out numberOfScanners, scannerIdList, out outXml, out status);

                    if (status != 0 || numberOfScanners == 0)
                    {
                        return false;
                    }

                    // 스캐너 정보 파싱
                    _connectedScanners = ParseScannerInfo(outXml);

                    // 바코드 이벤트 구독
                    int opcode = 1001; // REGISTER_FOR_EVENTS
                    string inXml = "<inArgs><cmdArgs><arg-int>1</arg-int><arg-int>1</arg-int></cmdArgs></inArgs>";
                    _coreScanner.ExecCommand(opcode, ref inXml, out outXml, out status);

                    // 이미지 이벤트 구독
                    inXml = "<inArgs><cmdArgs><arg-int>1</arg-int><arg-int>8</arg-int></cmdArgs></inArgs>";
                    _coreScanner.ExecCommand(opcode, ref inXml, out outXml, out status);

                    _isConnected = true;
                    ConnectionChanged?.Invoke(this, true);

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"스캐너 연결 실패: {ex.Message}");
                    return false;
                }
            });
        }

        public void Disconnect()
        {
            try
            {
                if (_coreScanner != null)
                {
                    // 이벤트 구독 해제
                    if (_isConnected)
                    {
                        int opcode = 1002; // UNREGISTER_FOR_EVENTS
                        string inXml = "<inArgs><cmdArgs><arg-int>1</arg-int></cmdArgs></inArgs>";
                        string outXml;
                        int status;
                        _coreScanner.ExecCommand(opcode, ref inXml, out outXml, out status);
                    }

                    // Close CoreScanner
                    int closeStatus;
                    _coreScanner.Close(0, out closeStatus);
                }

                _isConnected = false;
                _isInitialized = false;
                ConnectionChanged?.Invoke(this, false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"스캐너 연결 해제 실패: {ex.Message}");
            }
        }

        public async Task<bool> CaptureImageAsync(int scannerId)
        {
            if (!_isConnected || _coreScanner == null)
            {
                return false;
            }

            return await Task.Run(() =>
            {
                try
                {
                    int opcode = 6000; // DEVICE_CAPTURE_IMAGE
                    string inXml = $"<inArgs><scannerID>{scannerId}</scannerID></inArgs>";
                    string outXml;
                    int status;

                    _coreScanner.ExecCommand(opcode, ref inXml, out outXml, out status);

                    return status == 0;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"이미지 캡처 실패: {ex.Message}");
                    return false;
                }
            });
        }

        public List<ScannerInfo> GetConnectedScanners()
        {
            return new List<ScannerInfo>(_connectedScanners);
        }

        // 바코드 스캔 이벤트 핸들러
        private void OnBarcodeEvent(short eventType, ref string pscanData)
        {
            try
            {
                var barcodeData = ParseBarcodeData(pscanData);
                BarcodeScanned?.Invoke(this, barcodeData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"바코드 이벤트 처리 실패: {ex.Message}");
            }
        }

        // 이미지 캡처 이벤트 핸들러
        private void OnImageEvent(short eventType, int size, ref string imageData, ref string pScannerData)
        {
            try
            {
                // Base64 문자열을 바이트 배열로 변환
                byte[] imageBytes = Convert.FromBase64String(imageData);

                var result = ImageCaptureResult.CreateSuccess(imageBytes, "", 0);
                ImageCaptured?.Invoke(this, result);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"이미지 이벤트 처리 실패: {ex.Message}");
                var errorResult = ImageCaptureResult.CreateFailure($"이미지 처리 실패: {ex.Message}");
                ImageCaptured?.Invoke(this, errorResult);
            }
        }

        // 비디오 이벤트 핸들러 (사용하지 않지만 구현 필요)
        private void OnVideoEvent(short eventType, int size, ref string imageData, ref string pScannerData)
        {
            // 비디오 이벤트는 현재 사용하지 않음
        }

        // PNP 이벤트 핸들러 (스캐너 연결/해제)
        private void OnPNPEvent(short eventType, ref string ppnpData)
        {
            try
            {
                // eventType: 0 = Device Arrival, 1 = Device Removal
                bool isConnected = eventType == 0;
                ConnectionChanged?.Invoke(this, isConnected);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PNP 이벤트 처리 실패: {ex.Message}");
            }
        }

        // 바코드 데이터 파싱
        private BarcodeData ParseBarcodeData(string xmlData)
        {
            var barcodeData = new BarcodeData();

            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xmlData);

                // 바코드 값 추출
                var barcodeNode = doc.SelectSingleNode("//datalabel");
                if (barcodeNode != null)
                {
                    barcodeData.Barcode = barcodeNode.InnerText ?? "";
                }

                // 바코드 타입 추출
                var typeNode = doc.SelectSingleNode("//datatype");
                if (typeNode != null)
                {
                    barcodeData.BarcodeType = GetBarcodeTypeName(typeNode.InnerText ?? "");
                }

                // 스캐너 ID 추출
                var scannerIdNode = doc.SelectSingleNode("//scannerID");
                if (scannerIdNode != null && int.TryParse(scannerIdNode.InnerText, out int scannerId))
                {
                    barcodeData.ScannerId = scannerId;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"바코드 데이터 파싱 실패: {ex.Message}");
            }

            return barcodeData;
        }

        // 스캐너 정보 파싱
        private List<ScannerInfo> ParseScannerInfo(string xmlData)
        {
            var scanners = new List<ScannerInfo>();

            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xmlData);

                var scannerNodes = doc.SelectNodes("//scanner");
                if (scannerNodes != null)
                {
                    foreach (XmlNode node in scannerNodes)
                    {
                        var scanner = new ScannerInfo
                        {
                            IsConnected = true
                        };

                        var idNode = node.SelectSingleNode("scannerID");
                        if (idNode != null && int.TryParse(idNode.InnerText, out int scannerId))
                        {
                            scanner.ScannerId = scannerId;
                        }

                        var serialNode = node.SelectSingleNode("serialnumber");
                        if (serialNode != null)
                        {
                            scanner.SerialNumber = serialNode.InnerText ?? "";
                        }

                        var modelNode = node.SelectSingleNode("modelnumber");
                        if (modelNode != null)
                        {
                            scanner.ModelNumber = modelNode.InnerText ?? "";
                        }

                        var firmwareNode = node.SelectSingleNode("fw");
                        if (firmwareNode != null)
                        {
                            scanner.FirmwareVersion = firmwareNode.InnerText ?? "";
                        }

                        scanners.Add(scanner);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"스캐너 정보 파싱 실패: {ex.Message}");
            }

            return scanners;
        }

        // 바코드 타입 코드를 이름으로 변환
        private string GetBarcodeTypeName(string typeCode)
        {
            // 주요 바코드 타입 매핑 (필요시 확장)
            var typeMap = new Dictionary<string, string>
            {
                { "1", "Code 39" },
                { "2", "Codabar" },
                { "3", "Code 128" },
                { "8", "EAN-8" },
                { "9", "EAN-13" },
                { "10", "UPC-E" },
                { "11", "UPC-A" },
                { "15", "QR Code" },
                { "17", "Data Matrix" },
                { "25", "PDF417" }
            };

            return typeMap.TryGetValue(typeCode, out string? typeName) ? typeName : $"Type {typeCode}";
        }

        public void Dispose()
        {
            Disconnect();
            _coreScanner = null;
        }
    }
}
