using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UDIScan.Core.Models;

namespace UDIScan.Core.Services
{
    /// <summary>
    /// CoreScanner SDK 서비스 인터페이스
    /// </summary>
    public interface ICoreScannerService : IDisposable
    {
        /// <summary>
        /// 바코드 스캔 이벤트
        /// </summary>
        event EventHandler<BarcodeData>? BarcodeScanned;

        /// <summary>
        /// 이미지 캡처 이벤트
        /// </summary>
        event EventHandler<ImageCaptureResult>? ImageCaptured;

        /// <summary>
        /// 스캐너 연결 상태 변경 이벤트
        /// </summary>
        event EventHandler<bool>? ConnectionChanged;

        /// <summary>
        /// CoreScanner 초기화
        /// </summary>
        Task<bool> InitializeAsync();

        /// <summary>
        /// 스캐너 연결
        /// </summary>
        Task<bool> ConnectAsync();

        /// <summary>
        /// 스캐너 연결 해제
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 이미지 캡처 실행
        /// </summary>
        Task<bool> CaptureImageAsync(int scannerId);

        /// <summary>
        /// 연결된 스캐너 목록 조회
        /// </summary>
        List<ScannerInfo> GetConnectedScanners();

        /// <summary>
        /// 초기화 여부
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// 연결 여부
        /// </summary>
        bool IsConnected { get; }
    }
}
