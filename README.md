# UDI-Scan - DS9908 Image Capture

Zebra DS9908 스캐너를 위한 초고속 이미지 캡처 전용 Windows 애플리케이션

**특징**: 바코드 스캔 없음, 순수 이미지 캡처만 지원 (고속 연속 촬영 최적화)

---

## ✨ 주요 기능

- 📸 **고속 이미지 캡처**: 트리거 당기면 즉시 이미지 캡처 (초당 15~20장)
- ⚡ **비차단 저장**: 비동기 큐 기반 저장으로 UI 지연 없음
- 🕐 **타임스탬프 파일명**: `yyyyMMdd_HHmmss_fff.jpg` (밀리초 단위)
- 🖼️ **실시간 프리뷰**: 마지막 캡처 이미지 즉시 표시
- 💾 **자동 저장**: 사용자 지정 폴더에 자동 저장
- 🎯 **초심플 UI**: 불필요한 기능 제거, 이미지 캡처에만 집중

---

## 🚀 빠른 시작 (3가지 방법)

### **방법 1: GitHub Actions 자동 빌드** ⭐⭐⭐⭐⭐ (가장 쉬움!)

**PC에 아무것도 설치 안 해도 됨!**

1. **GitHub에서 빌드된 파일 다운로드**
   - [Actions 탭](../../actions) 클릭
   - 최근 성공한 빌드 선택
   - "Artifacts" → "UDIScan-Release-ZIP" 다운로드

2. **압축 해제 후 실행**
   - ZIP 파일 압축 해제
   - `UDIScan.exe` 실행

**상세 가이드**: [GITHUB_ACTIONS_GUIDE.md](GITHUB_ACTIONS_GUIDE.md)

---

### **방법 2: 로컬 빌드 (Build Tools)**

**Build Tools for Visual Studio 2022 필요** (3GB)

```bash
# 1. Build Tools 설치
https://aka.ms/vs/17/release/vs_BuildTools.exe
# ".NET desktop build tools" 선택

# 2. 빌드
cd C:\UDI-Scan
build-manual.bat

# 3. 실행
run.bat
```

**상세 가이드**: [EXECUTION_GUIDE.md](EXECUTION_GUIDE.md)

---

### **방법 3: .NET SDK** (실패 - COM 참조 미지원)

~~.NET SDK만으로는 빌드 불가능~~ (COM 참조 때문에)

---

## 📋 시스템 요구사항

### **필수 (실행 시)**
- ✅ Windows 10/11 (x64)
- ✅ .NET Framework 4.8 (Windows 10/11에 기본 설치됨)
- ✅ **Zebra CoreScanner Driver 3.5+** (필수!)
- ✅ Zebra DS9908 스캐너 (USB 연결)

### **빌드 시 (GitHub Actions 사용 시 불필요)**
- Build Tools for Visual Studio 2022
- 또는 Visual Studio 2022

---

## 🛠️ 사전 준비

### **1. CoreScanner Driver 설치 (필수!)**

```
https://www.zebra.com/us/en/support-downloads/software/developer-tools/corescanner-driver.html
```

**중요**: 설치 후 **PC 재부팅** 필수!

### **2. DS9908 스캐너 설정**

스캐너를 **Snapshot Mode**로 설정해야 합니다.

**설정 방법** (Product Reference Guide PDF 사용):

1. Factory Default 바코드 스캔
2. USB CDC Mode 활성화
3. **Enable Snapshot Mode** 스캔 (중요!)
4. Enable Image Capture Illumination

**상세 방법**: [EXECUTION_GUIDE.md](EXECUTION_GUIDE.md) Section 4 참고

---

## 💻 사용 방법

### **실행**

```bash
UDIScan.exe
```

### **UI 구성**

```
┌─────────────────────────────────────┐
│ 🟢 Connected: DS9908-SR (S/N: ...) │  ← 연결 상태
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│ Last Captured Image                 │
│                                     │
│   [이미지 프리뷰]                     │  ← 마지막 캡처 이미지
│                                     │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│ [Start Capture]  (초록 버튼)         │  ← 캡처 시작/중지
│                                     │
│ Save Path: C:\...\DS9908_Images     │  ← 저장 경로
│ [Browse...]                         │
└─────────────────────────────────────┘

Images Captured: 0 | Queue: 0         ← 통계
```

### **기본 사용 흐름**

1. **프로그램 실행**
2. 🟢 "Connected" 확인
3. **"Start Capture"** 클릭
4. **트리거 당기기** → 이미지 캡처!
5. 지정된 폴더에 자동 저장

### **저장된 파일**

```
C:\Users\사용자\Documents\DS9908_Images\
├── 20250122_143052_123.jpg
├── 20250122_143053_456.jpg
├── 20250122_143054_789.jpg
└── ...
```

파일명 규칙: `yyyyMMdd_HHmmss_fff.jpg` (밀리초 단위)

---

## 📂 프로젝트 구조

```
UDI-Scan/
├── .github/
│   └── workflows/
│       └── build.yml              # GitHub Actions 자동 빌드
├── src/
│   ├── UDIScan.App/              # WPF Application
│   │   ├── MainWindow.xaml       # UI (초심플)
│   │   └── MainWindow.xaml.cs
│   └── UDIScan.Core/             # Core Logic
│       ├── Services/
│       │   ├── CoreScannerService.cs  # 이미지 캡처만
│       │   └── ImageService.cs        # 비동기 저장
│       ├── ViewModels/
│       │   └── MainViewModel.cs       # ConcurrentQueue
│       └── Models/
│           ├── CapturedImage.cs
│           └── AppSettings.cs
├── build-manual.bat              # 빌드 스크립트 (MSBuild 자동 검색)
├── build-dotnet.bat              # 빌드 스크립트 (.NET SDK) - 작동 안 함
├── run.bat                       # 실행 스크립트
├── GITHUB_ACTIONS_GUIDE.md       # GitHub Actions 사용법 ⭐
├── EXECUTION_GUIDE.md            # 실행 가이드
└── README.md                     # 이 파일
```

---

## 🔧 기술 스택

- **언어**: C# 10.0
- **프레임워크**: .NET Framework 4.8
- **UI**: WPF (Windows Presentation Foundation)
- **SDK**: Zebra CoreScanner Driver COM Interop
- **패턴**: MVVM + ConcurrentQueue
- **비동기 처리**: async/await + Task.Run

---

## 🎯 성능 특징

- **고속 캡처**: 초당 15~20장 연속 촬영 가능
- **비차단 UI**: ConcurrentQueue로 이미지 저장 중에도 캡처 가능
- **메모리 효율**: 큐 크기 자동 관리
- **안정성**: 에러 발생 시에도 다음 캡처 계속 진행

---

## ❓ 문제 해결

### **"No scanner found" 에러**

1. CoreScanner Driver 설치 확인
2. PC 재부팅
3. USB 재연결
4. 장치 관리자에서 "Symbol USB CDC Device" 확인

### **트리거 당겨도 이미지 안 찍힘**

1. "Start Capture" 버튼 클릭 확인
2. 스캐너가 Snapshot Mode인지 확인
3. Product Reference Guide에서 "Enable Snapshot Mode" 바코드 재스캔

### **빌드 에러**

1. GitHub Actions 사용 (빌드 불필요)
2. 또는 Build Tools 설치
3. CoreScanner Driver 설치 및 PC 재부팅

**더 많은 문제 해결**: [EXECUTION_GUIDE.md](EXECUTION_GUIDE.md) Section 7

---

## 📚 문서

| 파일 | 설명 |
|------|------|
| [GITHUB_ACTIONS_GUIDE.md](GITHUB_ACTIONS_GUIDE.md) | GitHub Actions 자동 빌드 가이드 ⭐ |
| [EXECUTION_GUIDE.md](EXECUTION_GUIDE.md) | 완전 실행 가이드 (스캐너 설정 포함) |
| [QUICKSTART.md](QUICKSTART.md) | 5분 빠른 시작 |
| [DOTNET_SDK_INSTALL.md](DOTNET_SDK_INSTALL.md) | .NET SDK 설치 (작동 안 함) |

---

## 🚀 GitHub Actions 자동 빌드

**장점:**
- ✅ PC에 Build Tools 설치 불필요
- ✅ 코드 푸시 시 자동 빌드
- ✅ 실행 파일만 다운로드

**사용법:**

1. 코드를 GitHub에 푸시
2. Actions 탭에서 빌드 완료 대기 (2~3분)
3. Artifacts 다운로드
4. 압축 해제 후 실행

**상세**: [GITHUB_ACTIONS_GUIDE.md](GITHUB_ACTIONS_GUIDE.md)

---

## 🔄 버전 히스토리

### v1.0.0-image-only (2025-01-22)

**이미지 캡처 전용 버전**
- ✅ 바코드 스캔 기능 완전 제거
- ✅ 고속 이미지 캡처 (초당 15~20장)
- ✅ ConcurrentQueue 기반 비동기 저장
- ✅ 타임스탬프 파일명 (밀리초 단위)
- ✅ GitHub Actions 자동 빌드 지원
- ✅ 초심플 UI

**제거된 기능:**
- ❌ 바코드 스캔
- ❌ 키보드 시뮬레이션
- ❌ 스캔 히스토리

---

## 📞 지원

**문제 발생 시:**
1. [EXECUTION_GUIDE.md](EXECUTION_GUIDE.md) 문제 해결 섹션 확인
2. GitHub Issues 등록

---

## 📖 참고 자료

- [Zebra CoreScanner SDK](https://techdocs.zebra.com/dcs/scanners/sdk-windows/)
- [DS9908 Product Guide](https://www.zebra.com/us/en/support-downloads/scanners/general-purpose-scanners/ds9908.html)
- [CoreScanner Driver](https://www.zebra.com/us/en/support-downloads/software/developer-tools/corescanner-driver.html)

---

## 📄 라이선스

MIT License

---

**DS9908 이미지 캡처를 즐겁게!** 📸✨

**추천 시작**: [GITHUB_ACTIONS_GUIDE.md](GITHUB_ACTIONS_GUIDE.md) 읽고 자동 빌드로 시작하세요!
