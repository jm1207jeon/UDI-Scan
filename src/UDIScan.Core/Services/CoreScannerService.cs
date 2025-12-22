using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using CoreScanner;
using UDIScan.Core.Models;

namespace UDIScan.Core.Services
{
    /// <summary>
    /// CoreScanner SDK 서비스 (이미지 캡처 전용)
    /// </summary>
    public class CoreScannerService : ICoreScannerService
    {
        private CCoreScanner? _coreScanner;
        private bool _isConnected;
        private string _scannerInfo = "Not Connected";

        public event EventHandler<CapturedImage>? ImageCaptured;
        public event EventHandler<bool>? ConnectionChanged;

        public bool IsConnected => _isConnected;
        public string ScannerInfo => _scannerInfo;

        public async Task<bool> ConnectAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    _coreScanner = new CCoreScanner();

                    // CoreScanner 열기
                    short[] scannerTypes = new short[1];
                    scannerTypes[0] = 1; // All scanner types
                    int status;

                    _coreScanner.Open(0, scannerTypes, 1, out status);

                    if (status != 0)
                    {
                        _scannerInfo = "Failed to open CoreScanner";
                        return false;
                    }

                    // 이미지 이벤트만 구독
                    _coreScanner.ImageEvent += OnImageEvent;
                    _coreScanner.PNPEvent += OnPNPEvent;

                    // 연결된 스캐너 조회
                    int numberOfScanners = 0;
                    short[] scannerIdList = new short[255];
                    string outXml;

                    _coreScanner.GetScanners(out numberOfScanners, scannerIdList, out outXml, out status);

                    if (status != 0 || numberOfScanners == 0)
                    {
                        _scannerInfo = "No scanner found";
                        _coreScanner.Close(0, out status);
                        return false;
                    }

                    // 스캐너 정보 파싱
                    _scannerInfo = ParseScannerInfo(outXml);

                    // 이미지 이벤트 등록 (SUBSCRIBE_IMAGE = 8)
                    int opcode = 1001; // REGISTER_FOR_EVENTS
                    string inXml = "<inArgs><cmdArgs><arg-int>1</arg-int><arg-int>8</arg-int></cmdArgs></inArgs>";
                    _coreScanner.ExecCommand(opcode, ref inXml, out outXml, out status);

                    if (status != 0)
                    {
                        _scannerInfo = "Failed to register image events";
                        return false;
                    }

                    _isConnected = true;
                    ConnectionChanged?.Invoke(this, true);

                    return true;
                }
                catch (Exception ex)
                {
                    _scannerInfo = $"Error: {ex.Message}";
                    System.Diagnostics.Debug.WriteLine($"CoreScanner 연결 실패: {ex.Message}");
                    return false;
                }
            });
        }

        public void Disconnect()
        {
            try
            {
                if (_coreScanner != null && _isConnected)
                {
                    // 이벤트 구독 해제
                    int opcode = 1002; // UNREGISTER_FOR_EVENTS
                    string inXml = "<inArgs><cmdArgs><arg-int>8</arg-int></cmdArgs></inArgs>";
                    string outXml;
                    int status;
                    _coreScanner.ExecCommand(opcode, ref inXml, out outXml, out status);

                    // CoreScanner 닫기
                    _coreScanner.Close(0, out status);
                }

                _isConnected = false;
                _scannerInfo = "Disconnected";
                ConnectionChanged?.Invoke(this, false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"연결 해제 실패: {ex.Message}");
            }
        }

        // 이미지 이벤트 핸들러 (트리거 당기면 호출됨)
        private void OnImageEvent(short eventType, int size, ref string imageData, ref string pScannerData)
        {
            try
            {
                // Base64 문자열을 바이트 배열로 변환
                byte[] imageBytes = Convert.FromBase64String(imageData);

                var capturedImage = new CapturedImage(imageBytes);

                // 이벤트 발생 (MainViewModel에서 큐에 추가)
                ImageCaptured?.Invoke(this, capturedImage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"이미지 이벤트 처리 실패: {ex.Message}");
            }
        }

        // PNP 이벤트 핸들러 (스캐너 연결/해제)
        private void OnPNPEvent(short eventType, ref string ppnpData)
        {
            try
            {
                // eventType: 0 = Device Arrival, 1 = Device Removal
                bool isConnected = eventType == 0;
                _isConnected = isConnected;
                _scannerInfo = isConnected ? "Scanner Connected" : "Scanner Disconnected";
                ConnectionChanged?.Invoke(this, isConnected);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PNP 이벤트 처리 실패: {ex.Message}");
            }
        }

        // 스캐너 정보 파싱
        private string ParseScannerInfo(string xmlData)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xmlData);

                var scannerNode = doc.SelectSingleNode("//scanner");
                if (scannerNode != null)
                {
                    var modelNode = scannerNode.SelectSingleNode("modelnumber");
                    var serialNode = scannerNode.SelectSingleNode("serialnumber");

                    string model = modelNode?.InnerText ?? "Unknown";
                    string serial = serialNode?.InnerText ?? "Unknown";

                    return $"{model} ({serial})";
                }

                return "Scanner Connected";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"스캐너 정보 파싱 실패: {ex.Message}");
                return "Scanner Connected";
            }
        }

        public void Dispose()
        {
            Disconnect();
            _coreScanner = null;
        }
    }
}
