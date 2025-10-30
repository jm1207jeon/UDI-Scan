using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UDIScan.Core.Models;

namespace UDIScan.Core.Services
{
    /// <summary>
    /// 설정 관리 서비스
    /// </summary>
    public class SettingsService : ISettingsService
    {
        private readonly string _settingsDirectory;
        private readonly string _settingsFilePath;

        public string SettingsFilePath => _settingsFilePath;

        public SettingsService()
        {
            // %APPDATA%\UDIScan 디렉토리에 설정 저장
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _settingsDirectory = Path.Combine(appDataPath, "UDIScan");
            _settingsFilePath = Path.Combine(_settingsDirectory, "settings.json");
        }

        public async Task<AppSettings> LoadSettingsAsync()
        {
            try
            {
                if (!File.Exists(_settingsFilePath))
                {
                    // 설정 파일이 없으면 기본 설정 반환
                    return new AppSettings();
                }

                string json = await File.ReadAllTextAsync(_settingsFilePath);
                var settings = JsonConvert.DeserializeObject<AppSettings>(json);

                if (settings == null)
                {
                    return new AppSettings();
                }

                // 설정 유효성 검사
                settings.Validate();

                return settings;
            }
            catch (Exception ex)
            {
                // 로드 실패 시 기본 설정 반환
                System.Diagnostics.Debug.WriteLine($"설정 로드 실패: {ex.Message}");
                return new AppSettings();
            }
        }

        public async Task SaveSettingsAsync(AppSettings settings)
        {
            try
            {
                // 디렉토리 확인 및 생성
                if (!Directory.Exists(_settingsDirectory))
                {
                    Directory.CreateDirectory(_settingsDirectory);
                }

                // 설정 유효성 검사
                settings.Validate();

                // JSON으로 직렬화
                string json = JsonConvert.SerializeObject(settings, Formatting.Indented);

                // 파일로 저장
                await File.WriteAllTextAsync(_settingsFilePath, json);
            }
            catch (Exception ex)
            {
                throw new IOException($"설정 저장 실패: {ex.Message}", ex);
            }
        }
    }
}
