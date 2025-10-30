using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using UDIScan.Core.Models;

namespace UDIScan.Core.Services
{
    /// <summary>
    /// 이미지 처리 서비스 인터페이스
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// 이미지 데이터를 파일로 저장
        /// </summary>
        /// <param name="imageData">이미지 바이트 배열</param>
        /// <param name="barcode">바코드 값 (파일명에 사용)</param>
        /// <param name="savePath">저장 경로</param>
        /// <returns>저장된 파일 경로</returns>
        Task<string> SaveImageAsync(byte[] imageData, string barcode, string savePath);

        /// <summary>
        /// 이미지 데이터를 BitmapImage로 변환
        /// </summary>
        BitmapImage CreateBitmapImage(byte[] imageData);

        /// <summary>
        /// 썸네일 생성
        /// </summary>
        BitmapImage CreateThumbnail(byte[] imageData, int maxWidth, int maxHeight);
    }
}
