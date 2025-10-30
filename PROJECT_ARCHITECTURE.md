# UDI-Scan Project Architecture

## 프로젝트 개요
Zebra DS9908-SR 바코드 스캐너를 CoreScanner SDK로 제어하여 바코드 스캔 데이터를 외부 프로그램에 자동 입력하고, 스캔된 이미지를 캡처/저장하는 Windows 애플리케이션

## 기술 스택
- **언어**: C# 10.0
- **프레임워크**: .NET Framework 4.8
- **UI**: WPF (Windows Presentation Foundation)
- **SDK**: Zebra CoreScanner Driver COM Interop
- **아키텍처 패턴**: MVVM (Model-View-ViewModel)

## 프로젝트 구조

```
UDI-Scan/
├── src/
│   ├── UDIScan.App/                      # WPF Application
│   │   ├── App.xaml
│   │   ├── App.xaml.cs
│   │   ├── MainWindow.xaml               # Main UI
│   │   ├── MainWindow.xaml.cs
│   │   └── UDIScan.App.csproj
│   │
│   ├── UDIScan.Core/                     # Core Business Logic
│   │   ├── Services/
│   │   │   ├── ICoreScannerService.cs    # Scanner interface
│   │   │   ├── CoreScannerService.cs     # CoreScanner SDK wrapper
│   │   │   ├── IKeyboardSimulator.cs     # Keyboard input interface
│   │   │   ├── KeyboardSimulator.cs      # SendInput API wrapper
│   │   │   ├── IImageService.cs          # Image processing interface
│   │   │   ├── ImageService.cs           # Image capture & save
│   │   │   ├── ISettingsService.cs       # Settings interface
│   │   │   └── SettingsService.cs        # Configuration persistence
│   │   │
│   │   ├── Models/
│   │   │   ├── BarcodeData.cs            # Barcode scan result
│   │   │   ├── ScannerInfo.cs            # Scanner device info
│   │   │   ├── AppSettings.cs            # Application settings
│   │   │   └── ImageCaptureResult.cs     # Image capture result
│   │   │
│   │   ├── ViewModels/
│   │   │   ├── MainViewModel.cs          # Main window ViewModel
│   │   │   └── BaseViewModel.cs          # Base ViewModel (INotifyPropertyChanged)
│   │   │
│   │   └── UDIScan.Core.csproj
│   │
│   └── UDIScan.Native/                   # Native Win32 API Interop
│       ├── NativeMethods.cs              # P/Invoke declarations
│       ├── InputSimulator.cs             # Low-level keyboard input
│       └── UDIScan.Native.csproj
│
├── libs/                                  # External libraries
│   └── CoreScanner/                      # CoreScanner interop assemblies
│       └── Interop.CoreScanner.dll
│
├── docs/                                  # Documentation
│   ├── USER_GUIDE.md                     # 사용자 가이드
│   ├── INSTALLATION.md                   # 설치 가이드
│   └── API_REFERENCE.md                  # CoreScanner API 참조
│
├── installer/                             # Deployment package
│   ├── setup.iss                         # Inno Setup script
│   └── dependencies/                     # Bundled dependencies
│       └── CoreScannerDriver.exe
│
├── README.md                              # 프로젝트 소개
└── PROJECT_ARCHITECTURE.md               # 이 문서

```

## 핵심 컴포넌트

### 1. CoreScannerService
**책임**: Zebra CoreScanner SDK와의 통신 관리

**주요 기능**:
- 스캐너 연결 및 초기화
- 바코드 스캔 이벤트 구독
- 이미지 캡처 명령 실행
- 이미지 데이터 이벤트 수신
- 스캐너 상태 모니터링

**핵심 API**:
```csharp
public interface ICoreScannerService
{
    event EventHandler<BarcodeData> BarcodeScanned;
    event EventHandler<ImageCaptureResult> ImageCaptured;
    event EventHandler<bool> ScannerConnectionChanged;

    Task<bool> InitializeAsync();
    Task<bool> ConnectScannerAsync();
    void Disconnect();
    Task<bool> CaptureImageAsync(int scannerId);
    List<ScannerInfo> GetConnectedScanners();
}
```

### 2. KeyboardSimulator
**책임**: 외부 애플리케이션에 키보드 입력 시뮬레이션

**주요 기능**:
- 바코드 데이터를 개별 키 입력으로 변환
- Windows SendInput API를 통한 키 입력
- 특수 문자 처리 (Shift, Alt 조합)
- Enter 키 자동 입력

**핵심 API**:
```csharp
public interface IKeyboardSimulator
{
    void TypeText(string text);
    void SendKey(VirtualKeyCode key);
    void SendEnter();
}
```

**기술 세부사항**:
- Win32 API: `user32.dll`의 `SendInput` 함수 사용
- `INPUT` 구조체로 키보드 이벤트 생성
- 관리자 권한 필요 없음 (일반 애플리케이션 대상)

### 3. ImageService
**책임**: 이미지 캡처 및 저장 관리

**주요 기능**:
- BinaryDataEvent에서 이미지 데이터 추출
- Base64 → Bitmap 변환
- 파일명 생성 (바코드_타임스탬프.jpg)
- 비동기 파일 저장
- 썸네일 생성 (UI 프리뷰용)

**핵심 API**:
```csharp
public interface IImageService
{
    Task<string> SaveImageAsync(byte[] imageData, string barcode, string savePath);
    BitmapImage CreateThumbnail(byte[] imageData, int width, int height);
}
```

### 4. SettingsService
**책임**: 애플리케이션 설정 영구 저장

**주요 기능**:
- 설정 JSON 파일 저장/로드
- 이미지 저장 경로 관리
- 이미지 캡처 활성화 상태 저장

**저장 위치**: `%APPDATA%\UDIScan\settings.json`

**설정 모델**:
```csharp
public class AppSettings
{
    public string ImageSavePath { get; set; }
    public bool IsImageCaptureEnabled { get; set; } = true;
    public string LastScannerId { get; set; }
}
```

### 5. MainViewModel
**책임**: UI 상태 관리 및 비즈니스 로직 연결

**주요 속성**:
- `ObservableCollection<BarcodeData> ScanHistory` - 스캔 이력
- `BitmapImage LastCapturedImage` - 최근 캡처된 이미지
- `bool IsImageCaptureEnabled` - 이미지 캡처 토글 상태
- `string ImageSavePath` - 저장 경로
- `string ScannerStatus` - 스캐너 연결 상태

**주요 커맨드**:
- `ToggleImageCaptureCommand` - 이미지 캡처 활성화/비활성화
- `SelectSavePathCommand` - 저장 경로 선택
- `ClearHistoryCommand` - 스캔 이력 삭제

## 데이터 흐름

### 바코드 스캔 플로우

```
[DS9908 Scanner]
    → [CoreScanner Driver]
    → [CoreScannerService.BarcodeEvent]
    → [MainViewModel]
    → [KeyboardSimulator.TypeText()]
    → [Active Window (Excel/Word/Notepad)]

    동시에:
    → [UI Update: Add to ScanHistory]
```

### 이미지 캡처 플로우

```
[Barcode Scanned + IsImageCaptureEnabled=true]
    → [CoreScannerService.CaptureImageAsync()]
    → [CoreScanner DEVICE_CAPTURE_IMAGE command]
    → [DS9908 captures image]
    → [CoreScanner Driver]
    → [CoreScannerService.BinaryDataEvent]
    → [ImageService.SaveImageAsync()]
    → [File saved to disk]
    → [UI Update: Display thumbnail]
```

### 설정 저장 플로우

```
[User changes ImageSavePath or ToggleImageCapture]
    → [MainViewModel property changed]
    → [SettingsService.SaveSettings()]
    → [JSON written to %APPDATA%\UDIScan\settings.json]
```

## UI 레이아웃 설계

### MainWindow 구조

```
┌─────────────────────────────────────────────────────────────┐
│  UDI-Scan - Zebra DS9908 Barcode Scanner                    │
├─────────────────────────────────────────────────────────────┤
│  [●] Scanner Connected: DS9908-SR00004ZZWW                  │
├─────────────────────────────────────────────────────────────┤
│  ┌─────────────────────┐  ┌──────────────────────────────┐ │
│  │  Scan History       │  │  Last Captured Image         │ │
│  │                     │  │                              │ │
│  │  12:34:56           │  │  ┌────────────────────────┐ │ │
│  │  8806085124567      │  │  │                        │ │ │
│  │                     │  │  │   [Image Preview]      │ │ │
│  │  12:34:45           │  │  │                        │ │ │
│  │  1234567890128      │  │  └────────────────────────┘ │ │
│  │                     │  │                              │ │
│  │  12:34:30           │  │                              │ │
│  │  9780123456789      │  │                              │ │
│  │                     │  │                              │ │
│  │  [Clear History]    │  │                              │ │
│  └─────────────────────┘  └──────────────────────────────┘ │
├─────────────────────────────────────────────────────────────┤
│  Image Capture: [🟢 Enabled ]                               │
│                                                             │
│  Save Path: C:\Users\...\BarcodeImages [📁 Browse]         │
├─────────────────────────────────────────────────────────────┤
│  Status: Ready | Last scan: 0.023s | Images saved: 45      │
└─────────────────────────────────────────────────────────────┘
```

**UI 컬러 스키마** (Modern & Clean):
- Background: #F5F5F5 (Light Gray)
- Primary: #2196F3 (Blue)
- Accent: #4CAF50 (Green) / #F44336 (Red for disabled)
- Text: #212121 (Dark Gray)
- Border: #E0E0E0 (Light Gray)

**UI 컨트롤**:
- ToggleButton: Modern switch style (Enable/Disable)
- Button: Rounded corners, shadow effect
- ListBox: Smooth scrolling, alternating row colors
- Image: Border with shadow

## 성능 최적화

### 고속 스캔 처리
- **비동기 큐**: `ConcurrentQueue<BarcodeData>` 사용
- **백그라운드 스레드**: `Task.Run()` 으로 별도 처리
- **UI 스레드 분리**: `Dispatcher.Invoke()` 최소화

### 메모리 관리
- **이미지 버퍼**: 최대 10개까지만 UI에 유지
- **스캔 이력**: 최대 100개 제한 (FIFO)
- **Dispose 패턴**: Bitmap, Stream 등 리소스 명시적 해제

### 파일 I/O 최적화
- **비동기 저장**: `File.WriteAllBytesAsync()`
- **디렉토리 사전 생성**: 첫 저장 시 디렉토리 확인
- **이미지 압축**: JPEG quality 85%

## 에러 처리

### 예외 처리 전략

1. **CoreScanner 연결 실패**
   - 재시도 로직 (3회, 2초 간격)
   - 사용자에게 드라이버 설치 안내

2. **이미지 저장 실패**
   - 디스크 용량 확인
   - 권한 오류 시 다른 경로 제안
   - 임시 폴더에 백업 저장

3. **키보드 입력 실패**
   - 포커스 손실 감지
   - 사용자에게 알림 (Toast notification)

4. **메모리 부족**
   - 스캔 이력 자동 정리
   - 이미지 버퍼 크기 축소

## 배포 전략

### 배포 패키지 구성

1. **실행 파일**: UDIScan.exe (WPF application)
2. **종속 DLL**:
   - UDIScan.Core.dll
   - UDIScan.Native.dll
   - Interop.CoreScanner.dll
3. **설정 파일**: appsettings.json (optional)
4. **문서**:
   - USER_GUIDE.pdf
   - INSTALLATION_GUIDE.pdf

### 설치 요구사항

**필수**:
- Windows 10/11 (x64)
- .NET Framework 4.8 (Windows 10 이상에 기본 포함)
- Zebra CoreScanner Driver v3.0+ (별도 설치)

**권장**:
- 4GB RAM
- 500MB 디스크 공간 (이미지 저장용)

### 배포 방식

**옵션 1: Inno Setup Installer (권장)**
- CoreScanner Driver 포함
- .NET Framework 자동 확인
- 바탕화면 바로가기 생성
- 프로그램 추가/제거 등록

**옵션 2: Portable Zip**
- 압축 해제 후 바로 실행
- 사전 요구사항 수동 설치 필요
- USB 드라이브 등 이동 가능

## 보안 고려사항

1. **권한**: 일반 사용자 권한으로 실행 가능 (관리자 권한 불필요)
2. **데이터 보호**: 로컬 저장만 사용, 네트워크 전송 없음
3. **설정 파일**: 평문 저장 (민감 정보 없음)

## 확장 가능성

### 향후 추가 가능 기능

1. **다중 스캐너 지원**: 여러 DS9908 동시 연결
2. **데이터베이스 연동**: 스캔 데이터 저장
3. **네트워크 공유**: 이미지 자동 업로드
4. **OCR 처리**: 이미지에서 텍스트 추출
5. **바코드 검증**: 체크섬 검증, 중복 확인
6. **통계 대시보드**: 스캔 속도, 성공률 분석

## 테스트 전략

### 단위 테스트
- KeyboardSimulator: 키 입력 정확성
- ImageService: 이미지 변환 및 저장
- SettingsService: JSON 직렬화/역직렬화

### 통합 테스트
- CoreScanner 연결 및 스캔
- 전체 플로우 (스캔 → 입력 → 저장)

### 성능 테스트
- 연속 스캔 (초당 10개 이상)
- 메모리 누수 확인 (1시간 연속 실행)
- 대용량 이미지 처리 (2MB+)

### 사용자 테스트
- 실제 DS9908-SR 하드웨어 테스트
- Excel, Word, Notepad 등 다양한 앱 테스트
- 다양한 바코드 타입 (EAN, Code128, QR 등)

## 개발 일정 (예상)

- Day 1-2: 프로젝트 설정, CoreScanner 연동
- Day 3-4: 바코드 스캔 및 키보드 입력
- Day 5-6: 이미지 캡처 및 저장
- Day 7-8: UI 디자인 및 설정 관리
- Day 9-10: 테스트 및 버그 수정
- Day 11-12: 배포 패키지 준비 및 문서화

**총 예상 기간**: 12일 (full-time 기준)

## 참고 자료

- [Zebra CoreScanner SDK Documentation](https://techdocs.zebra.com/dcs/scanners/sdk-windows/)
- [DS9908 Product Reference Guide](https://www.zebra.com/content/dam/support-dam/en/documentation/unrestricted/guide/product/ds9908-prg-en.pdf)
- [Windows SendInput API](https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendinput)
- [WPF MVVM Pattern](https://learn.microsoft.com/en-us/dotnet/architecture/maui/mvvm)
