using System.Threading.Tasks;
using UDIScan.Core.Models;

namespace UDIScan.Core.Services
{
    /// <summary>
    /// 설정 관리 서비스 인터페이스
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// 설정 로드
        /// </summary>
        Task<AppSettings> LoadSettingsAsync();

        /// <summary>
        /// 설정 저장
        /// </summary>
        Task SaveSettingsAsync(AppSettings settings);

        /// <summary>
        /// 설정 파일 경로
        /// </summary>
        string SettingsFilePath { get; }
    }
}
