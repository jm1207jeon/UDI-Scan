# 🚀 실행 가이드 - DS9908 Image Capture Program

**완전 자동화된 빌드 및 실행 가이드**

---

## 📋 목차

1. [사전 준비](#1-사전-준비)
2. [빌드 실행](#2-빌드-실행)
3. [프로그램 실행](#3-프로그램-실행)
4. [스캐너 설정](#4-스캐너-설정)
5. [문제 해결](#5-문제-해결)

---

## 1. 사전 준비

### ✅ 체크리스트

프로그램을 빌드하고 실행하기 전에 다음 항목들이 준비되어야 합니다:

- [ ] **CoreScanner Driver 설치 완료** (필수!)
- [ ] **PC 재부팅 완료** (드라이버 설치 후)
- [ ] **DS9908 USB 연결**
- [ ] **Build Tools for Visual Studio 2022 설치** (또는 Visual Studio)

---

### 1.1 CoreScanner Driver 설치

**중요**: 이것이 가장 중요한 단계입니다!

1. 브라우저에서 다음 링크 접속:
   ```
   https://www.zebra.com/us/en/support-downloads/software/developer-tools/corescanner-driver.html
   ```

2. 최신 버전 다운로드 (v3.5 이상 권장)

3. 다운로드한 설치 파일 실행

4. **설치 완료 후 PC 재부팅** (필수!)

5. 재부팅 후 장치 관리자 확인:
   - Windows 키 + X → "장치 관리자"
   - "포트 (COM & LPT)" 또는 "범용 직렬 버스 컨트롤러"에서
   - **"Symbol USB CDC Device"** 확인

---

### 1.2 Build Tools 설치

**Option A: Build Tools for Visual Studio (권장 - 3GB)**

1. 다음 링크에서 다운로드:
   ```
   https://visualstudio.microsoft.com/downloads/
   ```
   → "Tools for Visual Studio" 섹션 → "Build Tools for Visual Studio 2022"

2. 설치 시 다음 항목 선택:
   - ✅ ".NET desktop build tools"
   - ✅ "Windows 10 SDK" (또는 최신 버전)

3. 설치 완료

**Option B: Visual Studio Community (20GB)**

- Visual Studio Community 2022 설치
- 워크로드: ".NET desktop development" 선택

---

## 2. 빌드 실행

### 2.1 명령 프롬프트 열기

1. Windows 키 + R
2. `cmd` 입력 후 Enter

### 2.2 프로젝트 폴더로 이동

```bash
cd C:\UDI-Scan
```

(또는 프로젝트가 있는 실제 경로)

### 2.3 빌드 스크립트 실행

```bash
build.bat
```

### 2.4 빌드 과정 확인

화면에 다음과 같이 표시됩니다:

```
====================================
UDI-Scan Build Script
====================================

[1/3] Cleaning previous builds...
[2/3] Building solution (Release configuration)...
[3/3] Build completed successfully!

Output location: src\UDIScan.App\bin\Release\net48\

To create installer:
  1. Install Inno Setup from https://jrsoftware.org/isdl.php
  2. Open installer\setup.iss
  3. Build the installer

계속하려면 아무 키나 누르십시오...
```

✅ **"Build completed successfully!"** 메시지가 나오면 성공!

❌ 에러가 발생하면 → [5. 문제 해결](#5-문제-해결) 참고

---

## 3. 프로그램 실행

### 3.1 빌드된 실행 파일 위치

```
C:\UDI-Scan\src\UDIScan.App\bin\Release\net48\UDIScan.exe
```

### 3.2 실행 방법

**방법 1: 명령 프롬프트에서 실행**

```bash
cd src\UDIScan.App\bin\Release\net48
UDIScan.exe
```

**방법 2: 탐색기에서 더블클릭**

1. Windows 탐색기 열기
2. 위 경로로 이동
3. `UDIScan.exe` 더블클릭

---

### 3.3 프로그램 초기 화면

프로그램이 실행되면 다음과 같은 화면이 나타납니다:

```
┌─────────────────────────────────────┐
│ 🟢 Connected: DS9908-SR (S/N: ...) │  ← 초록불이면 연결 성공!
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│ Last Captured Image                 │
│                                     │
│   (이미지 프리뷰 영역)                │
│                                     │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│ [Start Capture]  (초록 버튼)         │
│                                     │
│ Save Path: C:\Users\...\DS9908_Images
│ [Browse...]                         │
└─────────────────────────────────────┘

Images Captured: 0 | Queue: 0
```

---

## 4. 스캐너 설정

### 4.1 DS9908을 Snapshot Mode로 설정

**중요**: 트리거를 당기면 이미지를 캡처하도록 설정해야 합니다!

#### 방법 1: Product Reference Guide 사용 (권장)

1. PDF 다운로드:
   ```
   https://www.zebra.com/content/dam/zebra_new_ia/en-us/manuals/scanners/ds9908-product-reference-guide-en.pdf
   ```

2. PDF에서 다음 바코드들을 **순서대로** 스캔:

   **① Factory Default (초기화)**
   - PDF Page 30: "Factory Default" 바코드 스캔
   - 스캐너가 재부팅됩니다

   **② USB CDC Mode 활성화**
   - PDF Page 237: "USB CDC Virtual COM Port" 바코드 스캔

   **③ Snapshot Mode 활성화**
   - PDF Page 313: "Enable Snapshot Mode" 바코드 스캔

   **④ Image Capture Illumination 활성화**
   - PDF Page 316: "Enable Image Capture Illumination" 바코드 스캔

3. 설정 완료! (스캐너 LED가 깜빡이면 성공)

#### 방법 2: 123Scan 유틸리티 사용

1. 123Scan 다운로드:
   ```
   https://www.zebra.com/us/en/support-downloads/software/utilities/123scan.html
   ```

2. 실행 후 스캐너 선택

3. Configuration → Imaging → **"Snapshot Mode" 체크**

4. Apply 클릭

---

### 4.2 설정 확인

스캐너가 올바르게 설정되었는지 확인:

1. 프로그램 상단에 **🟢 "Connected: DS9908-SR..."** 표시 확인

2. "Start Capture" 버튼 클릭

3. 트리거 당기기

4. **삐** 소리와 함께 프로그램 UI에 이미지 표시되면 성공!

---

## 5. 프로그램 사용 방법

### 5.1 기본 사용 흐름

```
1. 프로그램 실행
   ↓
2. 연결 상태 확인 (🟢 초록불)
   ↓
3. 저장 경로 설정 (Browse 버튼)
   ↓
4. "Start Capture" 버튼 클릭
   ↓
5. 트리거 당기면 이미지 자동 저장!
```

### 5.2 저장 경로 변경

1. **"Browse..."** 버튼 클릭

2. 원하는 폴더 선택 (또는 새 폴더 생성)

3. 확인 클릭

기본 경로:
```
C:\Users\[사용자이름]\Documents\DS9908_Images
```

### 5.3 이미지 캡처

1. **"Start Capture"** 클릭 (버튼이 빨간색 "Stop Capture"로 변경)

2. 스캐너 트리거 당기기

3. **삐** 소리 → 이미지 캡처됨!

4. 프로그램 화면:
   - 이미지 프리뷰 자동 갱신
   - "Images Captured" 카운트 증가
   - "Queue" 사이즈 표시 (저장 진행 중)

### 5.4 저장된 파일 확인

설정한 폴더를 열면 다음과 같은 파일들이 생성됩니다:

```
DS9908_Images/
├── 20250130_143052_123.jpg  ← 2025년 1월 30일 14:30:52.123
├── 20250130_143053_456.jpg
├── 20250130_143054_789.jpg
└── ...
```

**파일명 규칙**: `yyyyMMdd_HHmmss_fff.jpg` (밀리초 단위)

---

## 6. 고속 연속 촬영 테스트

### 테스트 1: 저속 연속 (안정성)

```
목표: 10장 촬영, 1초 간격

1. "Start Capture" 클릭
2. 트리거 10회 당기기 (천천히)
3. 폴더에 10개 파일 확인

예상 결과:
✅ 10개 파일 모두 생성
✅ 모든 파일 정상 열림
```

### 테스트 2: 고속 연속 (성능)

```
목표: 20장 촬영, 최대한 빠르게

1. "Start Capture" 클릭
2. 트리거 20회 빠르게 연속으로 당기기
3. UI에서 "Queue Size" 변동 확인
4. 모든 촬영 완료 후 큐 사이즈 0 확인

예상 결과:
✅ 20개 파일 모두 생성
✅ UI 지연 없음 (부드러움)
✅ 큐 사이즈 최종적으로 0
✅ 초당 15~20장 촬영 가능
```

---

## 7. 문제 해결

### 문제 1: "MSBuild not found" 에러

**증상**:
```
ERROR: MSBuild not found. Please install Visual Studio or Build Tools.
```

**해결**:
1. Build Tools for Visual Studio 2022 설치 (위 1.2 참고)
2. PC 재시작
3. `build.bat` 다시 실행

---

### 문제 2: "CoreScanner 형식을 찾을 수 없습니다" 빌드 에러

**증상**:
```
error CS0246: 형식 또는 네임스페이스 이름 'CoreScanner'를 찾을 수 없습니다.
```

**해결**:
1. CoreScanner Driver 설치 확인
2. PC 재부팅
3. 빌드 폴더 삭제 후 재빌드:
   ```bash
   rmdir /s /q src\UDIScan.App\bin
   rmdir /s /q src\UDIScan.App\obj
   rmdir /s /q src\UDIScan.Core\bin
   rmdir /s /q src\UDIScan.Core\obj
   build.bat
   ```

---

### 문제 3: "No scanner found" (프로그램 실행 시)

**증상**: 프로그램 상단에 🔴 "Scanner Disconnected"

**해결**:
1. USB 케이블 재연결
2. 장치 관리자에서 "Symbol USB CDC Device" 확인
3. 스캐너 설정 재확인 (USB CDC 모드)
4. 프로그램 재시작

---

### 문제 4: 트리거 당겨도 이미지 안 찍힘

**증상**: 소리만 나고 이미지 캡처 안 됨

**해결**:
1. **"Start Capture" 버튼 클릭 확인**
2. Snapshot Mode 재설정:
   - PDF에서 "Enable Snapshot Mode" 바코드 재스캔
3. 스캐너 재부팅 (USB 재연결)

---

### 문제 5: 이미지 저장 안 됨

**증상**: UI에 카운트 증가하는데 파일 안 생김

**해결**:
1. 저장 경로 권한 확인
2. 다른 폴더로 변경 (예: `C:\Images`)
3. 디스크 공간 확인
4. 경로에 한글/특수문자 제거

---

### 문제 6: 프로그램이 느려짐

**해결**:
1. 프로그램 재시작
2. 오래된 이미지 파일 삭제
3. 디스크 정리

---

## 8. 추가 정보

### 8.1 로그 확인

프로그램 실행 중 문제 발생 시, Visual Studio Output 창에서 로그 확인 가능:

```
System.Diagnostics.Debug.WriteLine() 출력 확인
```

### 8.2 프로그램 종료

- 창 닫기 버튼 (X) 클릭
- 또는 Alt + F4

### 8.3 프로그램 위치 이동

빌드된 `UDIScan.exe` 파일과 같은 폴더 내 모든 파일을 함께 복사하면 다른 PC에서도 실행 가능합니다.

**필요 파일**:
```
UDIScan.exe
UDIScan.Core.dll
(기타 DLL 파일들)
```

**주의**: 다른 PC에서도 CoreScanner Driver가 설치되어 있어야 합니다!

---

## 9. 성공 확인!

다음 항목들을 모두 확인하셨다면 성공입니다! 🎉

- [x] 빌드 성공 ("Build completed successfully!")
- [x] 프로그램 실행됨
- [x] 🟢 "Connected" 상태
- [x] "Start Capture" 클릭
- [x] 트리거 당기면 이미지 캡처됨
- [x] 파일 생성 확인 (yyyyMMdd_HHmmss_fff.jpg)
- [x] 프리뷰 이미지 표시됨
- [x] 고속 연속 촬영 가능 (20장 테스트)

---

## 📞 지원

문제가 계속되면 다음 정보와 함께 GitHub Issues에 문의:

1. 에러 메시지 전체 복사
2. 스크린샷
3. Windows 버전
4. CoreScanner Driver 버전
5. DS9908 모델명

---

**작성일**: 2025-01-22
**버전**: 1.0.0-image-only
**Branch**: feature/image-capture-only

**즐거운 이미지 캡처 되세요!** 📸✨
