# 🔰 UDI-Scan 빌드 가이드 (완전 초보자용)

이 가이드는 **프로그래밍 경험이 전혀 없는 분**도 따라할 수 있도록 모든 단계를 자세히 설명합니다.

---

## 📋 목차

1. [시작하기 전에](#시작하기-전에)
2. [1단계: Visual Studio 설치](#1단계-visual-studio-설치)
3. [2단계: CoreScanner Driver 설치](#2단계-corescanner-driver-설치)
4. [3단계: 프로젝트 파일 준비](#3단계-프로젝트-파일-준비)
5. [4단계: Visual Studio로 프로젝트 열기](#4단계-visual-studio로-프로젝트-열기)
6. [5단계: 프로젝트 빌드하기](#5단계-프로젝트-빌드하기)
7. [6단계: 프로그램 실행하기](#6단계-프로그램-실행하기)
8. [7단계: 설치 파일 만들기 (선택)](#7단계-설치-파일-만들기-선택)
9. [문제 해결](#문제-해결)

---

## 시작하기 전에

### 필요한 것들

✅ **필수**
- Windows 10 또는 Windows 11 컴퓨터
- 인터넷 연결 (프로그램 다운로드용)
- 최소 10GB 이상의 빈 디스크 공간
- Zebra DS9908 스캐너 (실제 테스트용, 없어도 빌드는 가능)

✅ **시간**
- 전체 과정: 약 1~2시간 소요
- 빌드만: 5~10분

⚠️ **주의사항**
- 관리자 권한이 필요합니다 (프로그램 설치 시)
- 인터넷이 느리면 다운로드에 시간이 오래 걸릴 수 있습니다

---

## 1단계: Visual Studio 설치

**Visual Studio**는 C# 프로그램을 만들 수 있는 무료 개발 도구입니다.

### 1-1. Visual Studio 다운로드

1. **웹 브라우저(크롬, 엣지 등)를 엽니다**

2. **주소창에 다음 주소를 입력합니다**
   ```
   https://visualstudio.microsoft.com/ko/downloads/
   ```

3. **페이지가 열리면 "Community 2022" 버전을 찾습니다**
   - "Community"는 무료 버전입니다
   - 파란색 "무료 다운로드" 버튼을 클릭합니다

4. **다운로드가 시작됩니다**
   - 파일 이름: `VisualStudioSetup.exe` (약 3~5MB)
   - 다운로드 폴더에 저장됩니다

### 1-2. Visual Studio 설치 실행

1. **다운로드 폴더를 엽니다**
   - 탐색기를 열고 왼쪽에서 "다운로드" 클릭
   - 또는 `Win + E` 키를 누른 후 "다운로드" 클릭

2. **`VisualStudioSetup.exe` 파일을 더블클릭합니다**

3. **사용자 계정 컨트롤 창이 나타나면**
   - "예" 버튼을 클릭합니다 (관리자 권한 허용)

4. **Visual Studio Installer가 실행됩니다**
   - 잠시 기다리면 설치 옵션 화면이 나타납니다

### 1-3. 필요한 구성 요소 선택

1. **"워크로드" 탭에서 다음을 찾아 체크합니다**
   - ✅ **.NET 데스크톱 개발** (영어: .NET desktop development)
   - 이것만 체크하면 됩니다!

2. **오른쪽 "설치 세부 정보" 패널을 확인합니다**
   - ".NET Framework 4.8 개발 도구"가 포함되어 있는지 확인
   - 기본적으로 포함되어 있으니 변경하지 마세요

3. **오른쪽 하단의 "설치" 버튼을 클릭합니다**

### 1-4. 설치 대기

1. **다운로드 및 설치가 진행됩니다**
   - 진행 상황이 표시됩니다
   - 약 20~60분 소요 (인터넷 속도에 따라 다름)
   - 용량: 약 5~10GB

2. **설치 중에는 컴퓨터를 사용할 수 있습니다**
   - 다만 다른 프로그램 설치는 하지 마세요

3. **설치가 완료되면 "시작" 버튼이 나타납니다**
   - 아직 클릭하지 마세요!
   - 먼저 2단계로 이동합니다

---

## 2단계: CoreScanner Driver 설치

**CoreScanner Driver**는 Zebra 스캐너와 통신하기 위해 필요합니다.

### 2-1. CoreScanner Driver 다운로드

1. **웹 브라우저에서 다음 주소를 입력합니다**
   ```
   https://www.zebra.com/us/en/support-downloads/software/developer-tools/corescanner-driver.html
   ```

2. **페이지가 로딩되면 "Downloads" 섹션을 찾습니다**

3. **최신 버전을 선택합니다**
   - 예: "CoreScanner Driver v3.5 for Windows"
   - 파일명 예: `CoreScannerDriver_v3.5.exe`

4. **다운로드 버튼을 클릭합니다**
   - 로그인이 필요할 수 있습니다 (무료 계정 생성)
   - 또는 "Guest Download" 옵션 선택

5. **다운로드가 완료될 때까지 기다립니다**
   - 파일 크기: 약 50~100MB

### 2-2. CoreScanner Driver 설치

1. **다운로드 폴더에서 설치 파일을 찾습니다**
   - 파일명: `CoreScannerDriver_v3.5.exe` (버전에 따라 다름)

2. **파일을 더블클릭하여 실행합니다**

3. **설치 마법사가 시작됩니다**
   - "Next" 버튼 클릭

4. **라이선스 동의 화면**
   - "I accept the terms in the License Agreement" 체크
   - "Next" 클릭

5. **설치 경로 선택**
   - 기본 경로 그대로 사용하는 것을 권장합니다
   - 경로 예: `C:\Program Files (x86)\Zebra Technologies\CoreScanner Driver`
   - "Next" 클릭

6. **"Install" 버튼 클릭**
   - 설치가 시작됩니다 (약 1~2분 소요)

7. **설치 완료**
   - "Finish" 버튼 클릭

8. **설치 확인 (선택 사항)**
   - 시작 메뉴를 엽니다
   - "Zebra Technologies" 폴더를 찾습니다
   - "CoreScanner Driver" 항목이 있으면 설치 성공

---

## 3단계: 프로젝트 파일 준비

프로젝트 소스 코드를 컴퓨터로 가져옵니다.

### 방법 A: GitHub에서 다운로드 (추천)

1. **웹 브라우저에서 GitHub 저장소로 이동합니다**
   - 주소: `https://github.com/your-repo/UDI-Scan` (실제 저장소 주소로 교체)

2. **초록색 "Code" 버튼을 클릭합니다**

3. **"Download ZIP" 옵션을 클릭합니다**
   - ZIP 파일 다운로드가 시작됩니다
   - 파일명: `UDI-Scan-main.zip` 또는 유사

4. **다운로드가 완료되면 압축을 해제합니다**
   - 다운로드 폴더에서 ZIP 파일을 찾습니다
   - 파일을 **오른쪽 클릭** → "압축 풀기" 또는 "여기에 압축 풀기"

5. **압축 해제된 폴더를 이동합니다 (권장)**
   - 압축 푼 폴더를 `C:\UDI-Scan`으로 이동
   - 또는 원하는 위치 (예: `D:\Projects\UDI-Scan`)
   - **중요**: 경로에 한글이나 특수문자가 없어야 합니다!

### 방법 B: Git으로 클론 (고급 사용자)

```bash
git clone https://github.com/your-repo/UDI-Scan.git C:\UDI-Scan
```

### 3-1. 폴더 구조 확인

압축을 푼 폴더(`C:\UDI-Scan`)를 열어 다음 폴더들이 있는지 확인합니다:

```
C:\UDI-Scan\
├── src\              ✅ (있어야 함)
├── docs\             ✅ (있어야 함)
├── README.md         ✅ (있어야 함)
└── build.bat         ✅ (있어야 함)
```

---

## 4단계: Visual Studio로 프로젝트 열기

이제 Visual Studio에서 프로젝트를 엽니다.

### 4-1. Visual Studio 실행

1. **시작 메뉴를 엽니다**
   - Windows 키를 누르거나 작업 표시줄의 시작 버튼 클릭

2. **"Visual Studio 2022"를 검색합니다**
   - 검색창에 "visual" 입력
   - "Visual Studio 2022" 아이콘을 클릭

3. **처음 실행 시 로그인 화면이 나타날 수 있습니다**
   - Microsoft 계정으로 로그인 (선택 사항)
   - 또는 "나중에 로그인" 클릭해도 됩니다

4. **테마 선택 화면**
   - 원하는 테마 선택 (예: "Blue" 또는 "Dark")
   - "Visual Studio 시작" 클릭

### 4-2. 솔루션 파일 열기

1. **Visual Studio 시작 화면에서 "프로젝트 또는 솔루션 열기"를 클릭합니다**
   - 영어 버전: "Open a project or solution"

2. **파일 탐색 창이 나타나면:**
   - 왼쪽에서 "이 PC" 클릭
   - `C:` 드라이브 클릭
   - `UDI-Scan` 폴더 더블클릭
   - `src` 폴더 더블클릭

3. **`UDIScan.sln` 파일을 선택합니다**
   - 파일 형식: "Solution File" (.sln)
   - 아이콘: Visual Studio 로고가 그려진 파일

4. **"열기" 버튼을 클릭합니다**

### 4-3. 프로젝트 로딩 대기

1. **Visual Studio가 프로젝트를 로딩합니다**
   - 하단 상태 표시줄에 "프로젝트 로드 중..." 메시지
   - 약 10~30초 소요

2. **NuGet 패키지 복원이 자동으로 시작됩니다**
   - 하단 상태 표시줄: "NuGet 패키지 복원 중..."
   - 인터넷에서 필요한 라이브러리를 다운로드합니다
   - 약 1~5분 소요 (처음 한 번만)

3. **모든 로딩이 완료되면:**
   - 오른쪽 "솔루션 탐색기"에 프로젝트 트리가 표시됩니다
   - 하단 상태 표시줄: "준비" 메시지

### 4-4. 솔루션 탐색기 확인

오른쪽 "솔루션 탐색기" 패널을 확인합니다:

```
솔루션 'UDIScan' (3/3개 프로젝트)
├── UDIScan.App          ✅
├── UDIScan.Core         ✅
└── UDIScan.Native       ✅
```

**만약 솔루션 탐색기가 보이지 않는다면:**
- 메뉴: **보기(View)** → **솔루션 탐색기(Solution Explorer)**
- 단축키: `Ctrl + Alt + L`

---

## 5단계: 프로젝트 빌드하기

이제 프로그램을 컴파일(빌드)합니다!

### 5-1. 빌드 구성 선택

1. **상단 도구 모음을 확인합니다**
   - 메뉴 바로 아래에 드롭다운이 2개 있습니다

2. **첫 번째 드롭다운 (구성):**
   - 현재 값 확인: "Debug" 또는 "Release"
   - **"Release"로 변경합니다** (클릭하여 선택)
   - Release는 최적화된 버전입니다

3. **두 번째 드롭다운 (플랫폼):**
   - "Any CPU" 또는 "x64"
   - **"Any CPU"를 선택합니다**

### 5-2. 솔루션 빌드 실행

이제 빌드를 시작합니다!

**방법 1: 메뉴 사용**
1. 상단 메뉴에서 **"빌드(Build)"** 클릭
2. **"솔루션 빌드(Build Solution)"** 클릭

**방법 2: 단축키 사용 (추천)**
- `Ctrl + Shift + B` 키를 동시에 누릅니다

### 5-3. 빌드 진행 상황 확인

1. **하단 "출력(Output)" 패널을 확인합니다**
   - 빌드 로그가 실시간으로 표시됩니다
   - 검은색 배경에 흰색 텍스트

2. **진행 과정:**
   ```
   빌드 시작됨...
   1>------ 빌드 시작: 프로젝트: UDIScan.Native, 구성: Release Any CPU ------
   1>  UDIScan.Native -> C:\UDI-Scan\src\UDIScan.Native\bin\Release\net48\UDIScan.Native.dll
   2>------ 빌드 시작: 프로젝트: UDIScan.Core, 구성: Release Any CPU ------
   2>  UDIScan.Core -> C:\UDI-Scan\src\UDIScan.Core\bin\Release\net48\UDIScan.Core.dll
   3>------ 빌드 시작: 프로젝트: UDIScan.App, 구성: Release Any CPU ------
   3>  UDIScan.App -> C:\UDI-Scan\src\UDIScan.App\bin\Release\net48\UDIScan.exe
   ========== 빌드: 성공 3, 실패 0, 최신 0, 건너뜀 0 ==========
   ```

3. **소요 시간:**
   - 처음 빌드: 30초 ~ 2분
   - 이후 빌드: 5~10초

### 5-4. 빌드 결과 확인

빌드가 끝나면 하단에 다음 메시지가 표시됩니다:

**✅ 성공한 경우:**
```
========== 빌드: 성공 3, 실패 0, 최신 0, 건너뜀 0 ==========
```
- 모든 프로젝트가 성공적으로 빌드되었습니다!
- 6단계로 이동합니다

**❌ 실패한 경우:**
```
========== 빌드: 성공 2, 실패 1, 최신 0, 건너뜀 0 ==========
```
- "오류 목록(Error List)" 패널에 오류가 표시됩니다
- [문제 해결](#문제-해결) 섹션을 참고하세요

### 5-5. 빌드된 파일 확인

빌드가 성공하면 실행 파일이 생성됩니다:

1. **파일 탐색기를 엽니다** (`Win + E`)

2. **다음 경로로 이동합니다:**
   ```
   C:\UDI-Scan\src\UDIScan.App\bin\Release\net48\
   ```

3. **다음 파일들이 있는지 확인합니다:**
   - ✅ `UDIScan.exe` (실행 파일)
   - ✅ `UDIScan.Core.dll`
   - ✅ `UDIScan.Native.dll`
   - ✅ `Newtonsoft.Json.dll`

---

## 6단계: 프로그램 실행하기

이제 만든 프로그램을 실행해봅니다!

### 6-1. Visual Studio에서 실행 (디버깅)

**방법 1: 메뉴 사용**
1. 상단 메뉴: **"디버그(Debug)"** → **"디버깅하지 않고 시작(Start Without Debugging)"**
2. 또는 단축키: `Ctrl + F5`

**방법 2: 도구 모음 버튼**
- 상단 도구 모음에서 초록색 ▶ 버튼 클릭

### 6-2. 프로그램 창 확인

1. **UDI-Scan 프로그램 창이 나타납니다**
   - 제목: "UDI-Scan - Zebra DS9908 Barcode Scanner"

2. **상단 상태 확인:**
   - ⚪ Scanner Disconnected (스캐너가 연결되지 않음)
   - 🟢 Connected: DS9908-... (스캐너 연결됨)

3. **스캐너가 없어도 프로그램은 정상 실행됩니다**
   - 단지 스캔 기능만 사용할 수 없습니다

### 6-3. 스캐너 연결 테스트 (DS9908이 있는 경우)

1. **DS9908 스캐너를 PC에 USB로 연결합니다**

2. **프로그램을 재시작합니다**
   - 프로그램을 종료하고 다시 실행 (`Ctrl + F5`)

3. **연결 상태 확인:**
   - 상단에 🟢 "Connected: DS9908-SR00004ZZWW" 표시

4. **테스트 스캔:**
   - 메모장(Notepad)을 엽니다
   - UDI-Scan이 실행된 상태에서 바코드를 스캔합니다
   - 메모장에 바코드 값이 자동으로 입력됩니다!

### 6-4. 이미지 캡처 테스트

1. **이미지 저장 경로 확인:**
   - 프로그램 하단: "Save Path: C:\Users\...\Documents\UDI-Scan\Images"

2. **이미지 캡처 활성화 확인:**
   - "Image Capture: Enabled" (초록색)

3. **바코드 스캔:**
   - 스캔하면 이미지가 자동으로 저장됩니다
   - 오른쪽 패널에 이미지 프리뷰가 표시됩니다

4. **저장된 이미지 확인:**
   - 파일 탐색기로 저장 경로를 엽니다
   - JPEG 파일이 생성되었는지 확인

### 6-5. 프로그램 종료

- 창 오른쪽 위 ❌ 버튼 클릭
- 또는 `Alt + F4` 키

---

## 7단계: 설치 파일 만들기 (선택 사항)

다른 PC에 배포하려면 설치 파일을 만들 수 있습니다.

### 7-1. Inno Setup 다운로드

1. **웹 브라우저에서 다음 주소로 이동:**
   ```
   https://jrsoftware.org/isdl.php
   ```

2. **"Inno Setup 6.x.x" 버전을 다운로드합니다**
   - 파일명: `innosetup-6.x.x.exe`

3. **다운로드한 파일을 실행하여 설치합니다**
   - Next → I Agree → Next → Install → Finish

### 7-2. Installer 스크립트 열기

1. **Inno Setup Compiler를 실행합니다**
   - 시작 메뉴 → "Inno Setup Compiler"

2. **메뉴: File → Open**

3. **다음 파일을 선택합니다:**
   ```
   C:\UDI-Scan\installer\setup.iss
   ```

### 7-3. Installer 빌드

1. **메뉴: Build → Compile (또는 Ctrl+F9)**

2. **빌드가 시작됩니다**
   - 진행 상황이 표시됩니다
   - 약 10~30초 소요

3. **빌드 완료 메시지:**
   ```
   Successful compile (0 errors, 0 warnings)
   ```

4. **생성된 설치 파일:**
   ```
   C:\UDI-Scan\installer\Output\UDIScan-Setup-v1.0.0.exe
   ```

### 7-4. Portable 버전 만들기

설치 없이 실행 가능한 버전을 만들려면:

1. **다음 폴더를 통째로 복사합니다:**
   ```
   C:\UDI-Scan\src\UDIScan.App\bin\Release\net48\
   ```

2. **복사한 폴더를 ZIP으로 압축합니다:**
   - 폴더를 오른쪽 클릭 → "보내기" → "압축(zip) 폴더"
   - 파일명: `UDIScan-Portable-v1.0.0.zip`

3. **배포:**
   - 이 ZIP 파일을 다른 PC에 복사
   - 압축을 풀고 `UDIScan.exe` 실행

---

## 문제 해결

### 문제 1: "CoreScanner를 찾을 수 없습니다" 오류

**증상:**
```
오류 CS0246: 'CCoreScanner' 형식 또는 네임스페이스 이름을 찾을 수 없습니다.
```

**원인:** CoreScanner Driver가 설치되지 않았습니다.

**해결:**
1. CoreScanner Driver를 설치합니다 (2단계 참조)
2. Visual Studio를 **완전히 종료**합니다
3. PC를 **재부팅**합니다
4. Visual Studio를 다시 실행하고 프로젝트를 엽니다
5. 다시 빌드합니다

### 문제 2: NuGet 패키지 복원 실패

**증상:**
```
오류: 'Newtonsoft.Json' 패키지를 복원할 수 없습니다.
```

**해결:**
1. Visual Studio 상단 메뉴: **도구(Tools)** → **NuGet 패키지 관리자** → **패키지 관리자 콘솔**
2. 하단 콘솔 창에서 다음 명령어 입력:
   ```
   Update-Package -reinstall
   ```
3. Enter 키를 누르고 완료될 때까지 대기
4. 다시 빌드합니다

### 문제 3: ".NET Framework 4.8을 찾을 수 없습니다"

**증상:**
```
오류: The reference assemblies for .NETFramework,Version=v4.8 were not found.
```

**해결:**
1. Windows Update 실행
2. 또는 [.NET Framework 4.8 다운로드](https://dotnet.microsoft.com/download/dotnet-framework/net48)
3. 설치 후 PC 재부팅
4. Visual Studio 재시작 후 다시 빌드

### 문제 4: "MSBuild를 찾을 수 없습니다" (build.bat 실행 시)

**증상:**
```
ERROR: MSBuild not found.
```

**해결:**
1. Visual Studio Installer를 실행합니다
2. "수정" 버튼 클릭
3. ".NET 데스크톱 개발" 워크로드가 설치되어 있는지 확인
4. 없으면 체크하고 "수정" 클릭

### 문제 5: 프로그램은 실행되는데 스캐너 연결 안 됨

**증상:**
- 프로그램: "Scanner Disconnected"
- 장치 관리자: DS9908 인식됨

**원인:** 스캐너가 HID Keyboard 모드로 설정됨

**해결:**
1. [DS9908 Product Reference Guide](https://www.zebra.com/content/dam/zebra_new_ia/en-us/manuals/scanners/ds9908-product-reference-guide-en.pdf) 다운로드
2. PDF 237페이지: "USB CDC Virtual COM Port" 바코드 스캔
3. 스캐너 재부팅 (자동)
4. UDI-Scan 프로그램 재시작

### 문제 6: "권한이 거부되었습니다" 오류

**증상:**
```
오류: Access to the path 'C:\UDI-Scan\...' is denied.
```

**해결:**
1. Visual Studio를 **관리자 권한으로 실행**합니다
   - Visual Studio 아이콘을 오른쪽 클릭
   - "관리자 권한으로 실행" 선택

### 추가 도움이 필요한 경우

1. **Visual Studio 출력 창의 오류 메시지를 복사합니다**
   - 출력 패널에서 오류 부분을 마우스로 드래그
   - `Ctrl + C`로 복사

2. **GitHub Issues에 질문을 올립니다**
   - 오류 메시지 전체를 붙여넣기
   - 어느 단계에서 오류가 발생했는지 설명

3. **스크린샷을 첨부합니다**
   - `Win + Shift + S` 키로 화면 캡처
   - 오류 화면을 캡처하여 첨부

---

## 🎉 축하합니다!

모든 단계를 완료하셨다면, 이제 UDI-Scan 프로그램을:
- ✅ 빌드할 수 있습니다
- ✅ 실행할 수 있습니다
- ✅ 테스트할 수 있습니다
- ✅ 배포할 수 있습니다

### 다음 단계

- 📖 [사용자 가이드](USER_GUIDE.md) 읽기
- 🔧 코드를 수정하여 기능 추가하기
- 🐛 버그를 발견하면 GitHub Issues에 보고하기
- 💡 새로운 기능 제안하기

---

**마지막 업데이트:** 2025-01-30
**작성자:** UDI-Scan 프로젝트 팀
