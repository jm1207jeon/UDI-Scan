# UDI-Scan 개발자 가이드

## 개발 환경 설정

### 필수 도구

1. **Visual Studio 2022** (Community Edition 이상)
   - Workload: ".NET desktop development"
   - Component: .NET Framework 4.8 SDK

2. **Zebra CoreScanner Driver**
   - 개발 시에도 CoreScanner Driver 설치 필요
   - COM Interop을 위해 필수

3. **Git** (선택 사항)
   - 소스 코드 버전 관리

### 프로젝트 클론

```bash
git clone https://github.com/your-repo/UDI-Scan.git
cd UDI-Scan
```

### Visual Studio에서 열기

1. `src/UDIScan.sln` 파일을 Visual Studio에서 열기
2. NuGet 패키지 자동 복원 대기
3. CoreScanner COM Interop 자동 생성 확인

## 프로젝트 구조

```
UDI-Scan/
├── src/
│   ├── UDIScan.App/          # WPF UI 프로젝트
│   ├── UDIScan.Core/         # 비즈니스 로직 라이브러리
│   └── UDIScan.Native/       # Win32 API Interop
├── docs/                     # 문서
├── installer/                # Inno Setup 스크립트
└── README.md
```

## 빌드 방법

### Visual Studio에서 빌드

1. 솔루션 구성: **Release** 선택
2. 플랫폼: **Any CPU** 선택
3. **빌드 → 솔루션 빌드** (Ctrl+Shift+B)

### 명령줄에서 빌드

```bash
# Windows
build.bat

# 또는 MSBuild 직접 실행
msbuild src\UDIScan.sln /p:Configuration=Release /p:Platform="Any CPU" /t:Rebuild
```

### 출력 위치

- `src/UDIScan.App/bin/Release/net48/UDIScan.exe`

## 디버깅

### 1. 하드웨어 연결

- DS9908 스캐너를 PC에 USB로 연결
- 장치 관리자에서 인식 확인

### 2. Visual Studio 디버깅

1. `UDIScan.App` 프로젝트를 시작 프로젝트로 설정
2. **F5** 키로 디버깅 시작
3. 중단점 설정 가능

### 3. 로깅

현재 버전은 `System.Diagnostics.Debug.WriteLine()`을 사용:
- Visual Studio 출력 창에서 확인 가능
- 디버그 빌드에서만 출력

## 핵심 컴포넌트

### 1. CoreScannerService

**위치**: `UDIScan.Core/Services/CoreScannerService.cs`

**주요 메서드**:
- `InitializeAsync()`: CoreScanner 초기화
- `ConnectAsync()`: 스캐너 연결
- `CaptureImageAsync()`: 이미지 캡처 명령

**이벤트**:
- `BarcodeScanned`: 바코드 스캔 시 발생
- `ImageCaptured`: 이미지 캡처 완료 시 발생

### 2. KeyboardSimulator

**위치**: `UDIScan.Core/Services/KeyboardSimulator.cs`

**핵심 API**: `SendInput` (user32.dll)

### 3. MainViewModel

**위치**: `UDIScan.Core/ViewModels/MainViewModel.cs`

**역할**: UI 로직 및 이벤트 처리

## 테스트

### 수동 테스트

1. **바코드 스캔 테스트**
   - 메모장 열기 → 스캔 → 입력 확인

2. **이미지 캡처 테스트**
   - 이미지 캡처 활성화 → 스캔 → 폴더에서 이미지 확인

3. **연속 스캔 테스트**
   - Excel에서 빠르게 연속 스캔 → 데이터 손실 확인

### 단위 테스트 (향후 추가 예정)

```csharp
// 예시
[TestClass]
public class KeyboardSimulatorTests
{
    [TestMethod]
    public void TypeText_Should_SimulateKeyboardInput()
    {
        // Arrange
        var simulator = new KeyboardSimulator();

        // Act
        simulator.TypeText("12345");

        // Assert
        // 실제 키 입력 확인 로직
    }
}
```

## 배포 패키지 생성

### 1. Release 빌드

```bash
build.bat
```

### 2. Inno Setup Installer 생성

1. [Inno Setup 6.0+](https://jrsoftware.org/isdl.php) 설치
2. `installer/setup.iss` 파일을 Inno Setup Compiler로 열기
3. **Build → Compile** (Ctrl+F9)
4. 출력: `installer/Output/UDIScan-Setup-v1.0.0.exe`

### 3. Portable ZIP 생성

1. Release 빌드 완료 후
2. `src/UDIScan.App/bin/Release/net48/` 폴더 전체를 ZIP으로 압축
3. 파일명: `UDIScan-Portable-v1.0.0.zip`

## 코딩 스타일

### C# 컨벤션

- **네이밍**:
  - 클래스/메서드: PascalCase
  - 프라이빗 필드: _camelCase (underscore prefix)
  - 로컬 변수: camelCase

- **들여쓰기**: 4 spaces (탭 사용 금지)

- **코드 정리**:
  - 사용하지 않는 using 제거
  - 공백 줄 정리
  - XML 주석 작성 (public 멤버)

### XAML 스타일

- **들여쓰기**: 4 spaces
- **리소스 키**: PascalCase
- **바인딩**: `{Binding PropertyName}` 형식 사용

## 기여 가이드

### 1. 브랜치 전략

- `main`: 안정적인 릴리스 브랜치
- `develop`: 개발 브랜치
- `feature/기능명`: 새 기능 개발
- `bugfix/이슈명`: 버그 수정

### 2. Pull Request

1. Feature 브랜치 생성
2. 변경 사항 커밋
3. PR 생성 (develop 브랜치로)
4. 코드 리뷰 후 머지

### 3. 커밋 메시지

```
[타입] 제목 (50자 이내)

상세 설명 (필요 시)

- 변경 사항 1
- 변경 사항 2
```

**타입**:
- `feat`: 새 기능
- `fix`: 버그 수정
- `docs`: 문서 수정
- `style`: 코드 스타일 변경
- `refactor`: 리팩토링
- `test`: 테스트 추가
- `chore`: 빌드/설정 변경

## 알려진 이슈 및 제한사항

1. **CoreScanner COM Interop**
   - CoreScanner Driver 설치 필수
   - 개발 PC에서도 설치 필요

2. **이미지 캡처**
   - DS9908의 이미지 캡처 API 응답 시간이 느릴 수 있음
   - 고속 스캔 시 이미지 캡처 지연 발생 가능

3. **키보드 입력**
   - SendInput은 관리자 권한 앱에 입력 불가 (Windows 보안 제한)

4. **멀티 스캐너**
   - 현재 버전은 단일 스캐너만 지원
   - 향후 업데이트에서 다중 스캐너 지원 예정

## 로드맵

### v1.1.0 (계획 중)
- [ ] 스캔 이력 CSV 내보내기
- [ ] 로그 파일 저장 기능
- [ ] 설정 UI 개선

### v1.2.0 (계획 중)
- [ ] 다중 스캐너 지원
- [ ] 이미지 포맷 선택 (JPEG/PNG/BMP)
- [ ] 데이터베이스 연동 옵션

### v2.0.0 (미정)
- [ ] 웹 기반 대시보드
- [ ] 원격 관리 기능
- [ ] OCR 처리

## 리소스

- [CoreScanner SDK Documentation](https://techdocs.zebra.com/dcs/scanners/sdk-windows/)
- [DS9908 Product Reference](https://www.zebra.com/us/en/support-downloads/scanners/general-purpose-scanners/ds9908.html)
- [WPF MVVM Tutorial](https://learn.microsoft.com/en-us/dotnet/architecture/maui/mvvm)
- [SendInput API](https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendinput)

## 문의

- **GitHub Issues**: 버그 리포트, 기능 제안
- **Discussions**: 개발 관련 질문

---

**마지막 업데이트**: 2025-01-30
