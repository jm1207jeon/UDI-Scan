# UDI-Scan

Zebra DS9908 바코드 스캐너를 위한 Windows 애플리케이션으로, CoreScanner SDK를 활용하여 바코드 스캔과 이미지 캡처 기능을 제공합니다.

## 주요 기능

- **바코드 스캔**: DS9908-SR 스캐너로 바코드 자동 스캔
- **자동 키보드 입력**: 스캔된 바코드를 Excel, Word, Notepad 등 외부 프로그램에 자동 입력
- **이미지 캡처**: 바코드 스캔 시 이미지 자동 캡처 및 저장
- **실시간 프리뷰**: 스캔 이력 및 캡처된 이미지 실시간 표시
- **설정 관리**: 이미지 저장 경로 및 캡처 활성화 상태 영구 저장

## 시스템 요구사항

### 필수
- Windows 10/11 (x64)
- .NET Framework 4.8 이상
- Zebra CoreScanner Driver 3.0 이상
- Zebra DS9908-SR 바코드 스캐너

### 권장
- 4GB RAM 이상
- 500MB 이상 디스크 공간 (이미지 저장용)

## 빠른 시작

### 사용자용 (이미 빌드된 프로그램)

설치 및 사용 방법은 [설치 가이드](docs/INSTALLATION.md)를 참고하세요.

### 개발자용 (소스 코드 빌드)

**🔰 초보자용 빌드 가이드:** [완전 초보자용 빌드 가이드](docs/BUILD_GUIDE_BEGINNER.md) (추천!)

**⚡ 빠른 체크리스트:** [빌드 체크리스트](BUILD_CHECKLIST.md)

**📖 개발자 가이드:** [개발 가이드](docs/DEVELOPMENT.md)

#### 빠른 빌드 (경험자용)

```bash
# 1. 필수 사항
- Visual Studio 2022 (.NET desktop development)
- CoreScanner Driver v3.0+

# 2. 프로젝트 열기
src/UDIScan.sln

# 3. 빌드
Ctrl + Shift + B (Release 모드)

# 4. 실행
Ctrl + F5
```

---

## 설치 방법

### 1. CoreScanner Driver 설치

1. [Zebra 공식 사이트](https://www.zebra.com/us/en/support-downloads/software/developer-tools/corescanner-driver.html)에서 CoreScanner Driver 다운로드
2. 다운로드한 설치 파일 실행
3. 설치 마법사 지시에 따라 설치 완료

### 2. UDI-Scan 애플리케이션 설치

**방법 A: Installer 사용 (권장)**
1. `UDIScan-Setup.exe` 다운로드
2. 설치 파일 실행
3. 설치 경로 선택 후 설치 완료

**방법 B: Portable 버전**
1. `UDIScan-Portable.zip` 다운로드
2. 원하는 위치에 압축 해제
3. `UDIScan.exe` 실행

### 3. 스캐너 설정

DS9908 스캐너를 PC에 USB로 연결한 후, 아래 바코드를 스캔하여 CoreScanner 모드로 설정합니다:

**USB CDC (Virtual COM) 모드 활성화**
- Product Reference Guide 참조하여 해당 바코드 스캔

## 사용 방법

### 기본 사용법

1. **애플리케이션 실행**
   - `UDIScan.exe` 실행
   - 스캐너가 자동으로 연결됨 (상단에 "Scanner Connected" 표시)

2. **바코드 스캔**
   - Excel, Word, Notepad 등 원하는 프로그램 열기
   - 입력하고 싶은 위치에 커서 위치
   - DS9908으로 바코드 스캔
   - 스캔된 데이터가 자동으로 입력됨

3. **이미지 캡처**
   - 기본적으로 이미지 캡처 활성화 상태
   - 비활성화하려면 "Image Capture: Enabled" 버튼 클릭
   - 캡처된 이미지는 설정된 경로에 자동 저장
   - 파일명 형식: `바코드데이터_yyyyMMddHHmmss.jpg`

4. **저장 경로 변경**
   - "Save Path" 옆 "Browse" 버튼 클릭
   - 원하는 폴더 선택
   - 설정은 자동으로 저장되며 재시작 후에도 유지됨

### UI 구성

```
┌─────────────────────────────────────────────────────────────┐
│  [●] Scanner Connected: DS9908-SR00004ZZWW                  │
├─────────────────────────────────────────────────────────────┤
│  [Scan History]                    [Last Captured Image]    │
│                                                              │
│  Image Capture: [Enabled/Disabled]                          │
│  Save Path: C:\...\BarcodeImages  [Browse]                  │
└─────────────────────────────────────────────────────────────┘
```

## 기술 스택

- **언어**: C# 10.0
- **프레임워크**: .NET Framework 4.8
- **UI**: WPF (Windows Presentation Foundation)
- **SDK**: Zebra CoreScanner Driver COM Interop
- **패턴**: MVVM (Model-View-ViewModel)

## 프로젝트 구조

```
UDI-Scan/
├── src/
│   ├── UDIScan.App/          # WPF Application
│   ├── UDIScan.Core/         # Business Logic
│   └── UDIScan.Native/       # Win32 API Interop
├── docs/                     # Documentation
├── installer/                # Deployment package
└── README.md
```

자세한 아키텍처는 [PROJECT_ARCHITECTURE.md](PROJECT_ARCHITECTURE.md) 참조

## 문제 해결

### 스캐너가 연결되지 않을 때

1. CoreScanner Driver가 설치되었는지 확인
2. 장치 관리자에서 DS9908 인식 확인
3. USB 케이블 재연결
4. 애플리케이션 재시작

### 키보드 입력이 안 될 때

1. 입력하려는 애플리케이션에 포커스가 있는지 확인
2. 관리자 권한으로 실행된 프로그램에는 입력 불가
3. 스캔 후 약간의 지연 시간 필요

### 이미지가 저장되지 않을 때

1. 저장 경로가 유효한지 확인
2. 폴더 쓰기 권한 확인
3. 디스크 용량 확인
4. "Image Capture" 버튼이 활성화되어 있는지 확인

## 개발

### 빌드 방법

```bash
# 솔루션 빌드
cd src
dotnet build UDIScan.sln

# 또는 Visual Studio에서 열기
start UDIScan.sln
```

### 개발 환경

- Visual Studio 2022
- .NET Framework 4.8 SDK
- CoreScanner Driver 설치 필요

## 라이선스

MIT License

## 문의

이슈가 있을 경우 GitHub Issues에 등록해주세요.

## 참고 자료

- [Zebra CoreScanner SDK Documentation](https://techdocs.zebra.com/dcs/scanners/sdk-windows/)
- [DS9908 Product Reference Guide](https://www.zebra.com/us/en/support-downloads/scanners/general-purpose-scanners/ds9908.html)
- [Project Architecture](PROJECT_ARCHITECTURE.md)

## 버전 히스토리

### v1.0.0 (Initial Release)
- 바코드 스캔 및 자동 키보드 입력
- 이미지 캡처 및 저장
- 설정 관리 (저장 경로, 캡처 활성화)
- 실시간 UI 업데이트
