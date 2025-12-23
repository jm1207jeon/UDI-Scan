# 🚀 빠른 시작 가이드 - UDI-Scan (.NET SDK 버전)

**Visual Studio 없이 5분 만에 시작하기!**

---

## ⚡ 3단계로 끝내기

### **1단계: .NET SDK 설치 (2분)**

다운로드:
```
https://aka.ms/dotnet/8.0/dotnet-sdk-win-x64.exe
```

설치 후 확인:
```bash
dotnet --version
```

**버전 번호가 나오면 성공!** (예: 8.0.101)

---

### **2단계: 프로그램 빌드 (1분)**

```bash
cd C:\UDI-Scan
build-dotnet.bat
```

**"Build completed successfully!" 메시지 확인**

---

### **3단계: 실행 (5초)**

```bash
run.bat
```

또는 직접 실행:
```bash
cd src\UDIScan.App\bin\Release\net48
UDIScan.exe
```

---

## 📋 사전 준비 (필수!)

빌드 전에 다음을 먼저 설치하세요:

### **1. CoreScanner Driver 설치**

다운로드:
```
https://www.zebra.com/us/en/support-downloads/software/developer-tools/corescanner-driver.html
```

**설치 후 PC 재부팅 필수!**

### **2. DS9908 스캐너 연결**

- USB 케이블로 PC 연결
- 장치 관리자에서 "Symbol USB CDC Device" 확인

### **3. .NET Framework 4.8 설치** (Windows 10/11은 이미 설치됨)

대부분의 PC에는 이미 설치되어 있습니다.

확인 방법:
```
설정 → 앱 → 앱 및 기능 → "Microsoft .NET Framework 4.8" 검색
```

없으면 다운로드:
```
https://dotnet.microsoft.com/download/dotnet-framework/net48
```

---

## 🎯 전체 명령어 (복사해서 사용)

```bash
# 1. 폴더 이동
cd C:\UDI-Scan

# 2. .NET SDK 설치 확인
dotnet --version

# 3. 빌드
build-dotnet.bat

# 4. 실행
run.bat
```

---

## ❌ 에러 해결

### **"dotnet: 명령을 찾을 수 없습니다"**

→ .NET SDK 설치 필요
```
https://aka.ms/dotnet/8.0/dotnet-sdk-win-x64.exe
```

### **"CoreScanner 형식을 찾을 수 없습니다"**

→ CoreScanner Driver 설치 필요
```
https://www.zebra.com/us/en/support-downloads/software/developer-tools/corescanner-driver.html
```
설치 후 **PC 재부팅!**

### **"Build failed"**

1. CoreScanner Driver 설치 확인
2. PC 재부팅
3. 빌드 폴더 삭제 후 재시도:
```bash
rmdir /s /q src\UDIScan.App\bin
rmdir /s /q src\UDIScan.App\obj
rmdir /s /q src\UDIScan.Core\bin
rmdir /s /q src\UDIScan.Core\obj
build-dotnet.bat
```

### **"No scanner found"**

1. USB 재연결
2. 장치 관리자에서 "Symbol USB CDC Device" 확인
3. 스캐너를 Snapshot Mode로 설정 (EXECUTION_GUIDE.md 참고)

---

## 📂 파일 설명

| 파일 | 용도 |
|------|------|
| `build-dotnet.bat` | 프로그램 빌드 (.NET SDK 사용) |
| `build-manual.bat` | 프로그램 빌드 (MSBuild 자동 검색) |
| `run.bat` | 빌드된 프로그램 실행 |
| `DOTNET_SDK_INSTALL.md` | .NET SDK 상세 설치 가이드 |
| `EXECUTION_GUIDE.md` | 전체 실행 가이드 (스캐너 설정 포함) |

---

## 🎉 성공 확인!

프로그램이 실행되면 다음과 같은 화면이 나타납니다:

```
┌─────────────────────────────────────┐
│ 🟢 Connected: DS9908-SR (S/N: ...) │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│ Last Captured Image                 │
│                                     │
│   (이미지 프리뷰 영역)                │
│                                     │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│ [Start Capture]                     │
│ Save Path: C:\Users\...\DS9908_Images
│ [Browse...]                         │
└─────────────────────────────────────┘

Images Captured: 0 | Queue: 0
```

**"Start Capture" 클릭 → 트리거 당기면 → 이미지 저장!** 📸

---

## 🔗 더 자세한 정보

- **스캐너 설정**: EXECUTION_GUIDE.md (Section 4)
- **고급 사용법**: EXECUTION_GUIDE.md (Section 5, 6)
- **문제 해결**: EXECUTION_GUIDE.md (Section 7)

---

**즐거운 이미지 캡처 되세요!** ✨
