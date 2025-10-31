# 🔧 빌드 오류 해결 가이드

Visual Studio에서 UDI-Scan 빌드 시 발생할 수 있는 오류와 해결 방법입니다.

---

## ⚠️ 가장 중요: CoreScanner Driver 필수 설치!

**UDI-Scan을 빌드하려면 반드시 CoreScanner Driver가 먼저 설치되어 있어야 합니다!**

### CoreScanner Driver 설치 확인

빌드 전에 다음을 확인하세요:

1. **CoreScanner Driver 설치 여부 확인**
   ```
   C:\Program Files (x86)\Zebra Technologies\CoreScanner Driver\
   ```
   이 경로에 폴더가 있으면 설치됨

2. **레지스트리 확인 (고급)**
   - `Win + R` → `regedit` 입력
   - `HKEY_CLASSES_ROOT\TypeLib\{D106C043-6E5F-4987-9E6E-F5796D398F23}` 경로 확인
   - 이 키가 존재하면 CoreScanner COM이 등록됨

### CoreScanner Driver 설치 방법

1. **다운로드**
   - https://www.zebra.com/us/en/support-downloads/software/developer-tools/corescanner-driver.html

2. **설치**
   - 다운로드한 `CoreScannerDriver_vX.X.exe` 실행
   - 설치 완료 후 **PC 재부팅** (중요!)

3. **Visual Studio 재시작**
   - PC 재부팅 후 Visual Studio를 다시 실행
   - 프로젝트를 다시 열기

---

## 🐛 주요 빌드 오류 및 해결 방법

### 오류 1: "CoreScanner 형식을 찾을 수 없습니다"

**전체 오류 메시지:**
```
오류 CS0246: 'CoreScanner' 형식 또는 네임스페이스 이름을 찾을 수 없습니다.
오류 CS0246: 'CCoreScanner' 형식 또는 네임스페이스 이름을 찾을 수 없습니다.
```

**원인:**
- CoreScanner Driver가 설치되지 않았거나
- COM Type Library가 등록되지 않음

**해결 방법:**

#### 방법 1: CoreScanner Driver 설치 (권장)
1. CoreScanner Driver를 설치합니다
2. PC를 재부팅합니다
3. Visual Studio를 재시작합니다
4. 솔루션을 다시 엽니다
5. **빌드 → 솔루션 정리** (Clean Solution)
6. **빌드 → 솔루션 다시 빌드** (Rebuild Solution)

#### 방법 2: COM Interop 수동 생성 (고급)
1. CoreScanner Driver 설치 후
2. Developer Command Prompt for VS 2022 실행
3. 다음 명령어 실행:
   ```cmd
   tlbimp "C:\Program Files (x86)\Zebra Technologies\CoreScanner Driver\Bin\CoreScanner.dll" /out:Interop.CoreScanner.dll
   ```
4. 생성된 `Interop.CoreScanner.dll`을 `src\UDIScan.Core\` 폴더에 복사
5. UDIScan.Core.csproj 수정:
   ```xml
   <!-- COMReference 제거하고 -->
   <ItemGroup>
     <Reference Include="Interop.CoreScanner">
       <HintPath>Interop.CoreScanner.dll</HintPath>
       <EmbedInteropTypes>false</EmbedInteropTypes>
     </Reference>
   </ItemGroup>
   ```

---

### 오류 2: "Microsoft.NET.Sdk.WindowsDesktop을 찾을 수 없습니다"

**전체 오류 메시지:**
```
오류 MSB4236: 지정한 SDK 'Microsoft.NET.Sdk.WindowsDesktop'을(를) 찾을 수 없습니다.
```

**원인:**
- Visual Studio에 .NET desktop development 워크로드가 설치되지 않음
- 또는 .NET Framework 4.8 SDK 누락

**해결 방법:**

1. **Visual Studio Installer 실행**
   - 시작 메뉴 → "Visual Studio Installer" 검색

2. **Visual Studio 2022 옆의 "수정" 버튼 클릭**

3. **워크로드 탭에서 다음을 체크:**
   - ✅ .NET 데스크톱 개발 (.NET desktop development)

4. **개별 구성 요소 탭에서 확인:**
   - ✅ .NET Framework 4.8 SDK
   - ✅ .NET Framework 4.8 targeting pack

5. **"수정" 버튼 클릭하여 설치**

6. **설치 완료 후 Visual Studio 재시작**

---

### 오류 3: "Newtonsoft.Json 패키지를 복원할 수 없습니다"

**전체 오류 메시지:**
```
오류 NU1101: 'Newtonsoft.Json' 패키지를 찾을 수 없습니다.
```

**원인:**
- NuGet 패키지 복원 실패
- 인터넷 연결 문제
- NuGet 캐시 손상

**해결 방법:**

#### 방법 1: NuGet 패키지 복원
1. Visual Studio 메뉴: **도구(Tools)** → **NuGet 패키지 관리자** → **패키지 관리자 콘솔**
2. 콘솔에서 다음 명령어 실행:
   ```
   Update-Package -reinstall
   ```
3. 완료 후 다시 빌드

#### 방법 2: NuGet 캐시 정리
1. Visual Studio 종료
2. 다음 폴더 삭제:
   ```
   %USERPROFILE%\.nuget\packages
   ```
3. Visual Studio 재시작
4. 솔루션 열기 (NuGet 자동 복원)

#### 방법 3: 인터넷 연결 확인
- 인터넷이 연결되어 있는지 확인
- 방화벽이 NuGet (nuget.org)을 차단하지 않는지 확인

---

### 오류 4: "PresentationCore를 찾을 수 없습니다"

**전체 오류 메시지:**
```
오류: 'PresentationCore' 참조를 확인할 수 없습니다.
```

**원인:**
- WPF SDK가 제대로 설치되지 않음
- 프로젝트 파일에서 `UseWPF=true`가 누락됨

**해결 방법:**

1. **프로젝트 파일 확인**
   - `UDIScan.Core.csproj`와 `UDIScan.App.csproj` 열기
   - 다음 내용 확인:
   ```xml
   <Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">
     <PropertyGroup>
       <UseWPF>true</UseWPF>
     </PropertyGroup>
   </Project>
   ```

2. **Visual Studio 재시작**
   - 프로젝트를 다시 로드합니다

---

### 오류 5: "LangVersion 10.0을 지원하지 않습니다"

**전체 오류 메시지:**
```
오류 CS8652: 기능 'file-scoped namespaces'은(는) C# 10.0에서 사용할 수 없습니다.
```

**원인:**
- C# 10 기능을 사용했지만 컴파일러가 오래됨

**해결 방법:**

#### 방법 1: LangVersion 낮추기
1. 모든 `.csproj` 파일에서 다음을 변경:
   ```xml
   <LangVersion>10.0</LangVersion>
   ↓
   <LangVersion>9.0</LangVersion>
   ```

2. 소스 코드에서 C# 10 기능 제거:
   - File-scoped namespace 제거
   - Global using 제거

#### 방법 2: Visual Studio 업데이트
1. Visual Studio를 최신 버전으로 업데이트
2. Visual Studio 2022 17.0 이상 필요

---

### 오류 6: "obj 또는 bin 폴더에 액세스할 수 없습니다"

**전체 오류 메시지:**
```
오류: 액세스가 거부되었습니다: 'obj\Debug\net48\UDIScan.exe'
```

**원인:**
- 프로그램이 실행 중인 상태에서 빌드 시도
- 파일이 잠겨 있음
- 바이러스 백신 프로그램이 차단

**해결 방법:**

1. **실행 중인 프로그램 종료**
   - 작업 관리자 (`Ctrl + Shift + Esc`)
   - `UDIScan.exe` 프로세스 종료

2. **bin/obj 폴더 삭제**
   - Visual Studio에서: **빌드** → **솔루션 정리**
   - 또는 수동으로:
   ```
   src\UDIScan.App\bin\
   src\UDIScan.App\obj\
   src\UDIScan.Core\bin\
   src\UDIScan.Core\obj\
   src\UDIScan.Native\bin\
   src\UDIScan.Native\obj\
   ```
   이 폴더들을 삭제

3. **Visual Studio를 관리자 권한으로 실행**
   - Visual Studio 아이콘 오른쪽 클릭
   - "관리자 권한으로 실행" 선택

---

### 오류 7: "XAML 파일을 찾을 수 없습니다"

**전체 오류 메시지:**
```
오류: 'MainWindow.xaml' 파일을 찾을 수 없습니다.
```

**원인:**
- XAML 파일이 프로젝트에 포함되지 않음
- 파일 경로 문제

**해결 방법:**

1. **솔루션 탐색기에서 확인**
   - `UDIScan.App` 프로젝트 확장
   - `App.xaml`, `MainWindow.xaml` 파일이 있는지 확인

2. **파일이 없다면:**
   - Git에서 다시 pull
   - 또는 파일을 수동으로 프로젝트에 추가

3. **빌드 작업 확인**
   - `MainWindow.xaml`을 선택
   - 속성 창에서:
     - 빌드 작업: `Page`
     - `MainWindow.xaml.cs`는 `Compile`

---

## 🔍 빌드 문제 진단 체크리스트

빌드 오류가 발생하면 다음을 순서대로 확인하세요:

### 1단계: 필수 소프트웨어 설치 확인
- [ ] Windows 10/11 (64bit)
- [ ] Visual Studio 2022 설치됨
- [ ] ".NET 데스크톱 개발" 워크로드 설치됨
- [ ] .NET Framework 4.8 SDK 설치됨
- [ ] **Zebra CoreScanner Driver 설치됨** ⭐ 가장 중요!
- [ ] PC 재부팅 완료

### 2단계: 프로젝트 설정 확인
- [ ] 솔루션 파일 경로에 한글 없음
- [ ] 솔루션 파일 경로에 특수문자 없음
- [ ] 권장 경로: `C:\UDI-Scan\src\UDIScan.sln`

### 3단계: Visual Studio 설정 확인
- [ ] 빌드 구성: Release 선택
- [ ] 플랫폼: Any CPU 선택
- [ ] NuGet 패키지 복원 완료 대기

### 4단계: 빌드 재시도
1. **빌드 → 솔루션 정리** (Clean Solution)
2. **Visual Studio 재시작**
3. **빌드 → 솔루션 다시 빌드** (Rebuild Solution)

### 5단계: 오류 메시지 확인
- [ ] "출력" 창의 전체 오류 메시지 복사
- [ ] "오류 목록" 패널의 모든 오류 확인
- [ ] 위의 해결 방법 섹션에서 해당 오류 찾기

---

## 📞 추가 도움

모든 방법을 시도했지만 여전히 빌드가 안 되는 경우:

### 정보 수집

1. **Visual Studio 버전**
   - 도움말 → Visual Studio 정보
   - 버전 번호 복사 (예: 17.5.3)

2. **오류 메시지 전체 복사**
   - 출력 창의 전체 내용 복사
   - 오류 목록의 모든 오류 복사

3. **시스템 정보**
   - Windows 버전: `Win + R` → `winver`
   - .NET Framework: 제어판 → 프로그램 및 기능

4. **CoreScanner 설치 여부**
   ```
   C:\Program Files (x86)\Zebra Technologies\CoreScanner Driver\
   ```
   폴더 존재 여부 확인

### GitHub Issues에 보고

다음 정보와 함께 Issues에 등록해주세요:

1. Visual Studio 버전
2. Windows 버전
3. CoreScanner Driver 설치 여부
4. 전체 오류 메시지 (코드 블록으로)
5. 스크린샷 (`Win + Shift + S`)
6. 시도한 해결 방법 나열

---

## ✅ 빌드 성공 확인

다음이 표시되면 빌드 성공입니다:

```
========== 빌드: 성공 3, 실패 0, 최신 0, 건너뜀 0 ==========
========== 빌드 완료: 00:00:15.234 ==========
```

**출력 파일 확인:**
```
C:\UDI-Scan\src\UDIScan.App\bin\Release\net48\UDIScan.exe
```

이 파일이 생성되었다면 빌드 완료! 🎉

---

**최종 업데이트:** 2025-01-30
