using System;
using System.Threading.Tasks;
using UDIScan.Core.Models;

namespace UDIScan.Core.Services
{
    /// <summary>
    /// CoreScanner SDK 서비스 인터페이스 (이미지 캡처 전용)
    /// </summary>
    public interface ICoreScannerService : IDisposable
    {
        /// <summary>
        /// 이미지 캡처 이벤트
        /// </summary>
        event EventHandler<CapturedImage>? ImageCaptured;

        /// <summary>
        /// 스캐너 연결 상태 변경 이벤트
        /// </summary>
        event EventHandler<bool>? ConnectionChanged;

        /// <summary>
        /// 스캐너 연결
        /// </summary>
        Task<bool> ConnectAsync();

        /// <summary>
        /// 스캐너 연결 해제
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 연결 여부
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 스캐너 정보
        /// </summary>
        string ScannerInfo { get; }
    }
}
