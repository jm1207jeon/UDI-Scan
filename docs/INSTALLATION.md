# UDI-Scan 설치 가이드

## 목차
1. [시스템 요구사항](#시스템-요구사항)
2. [사전 준비](#사전-준비)
3. [CoreScanner Driver 설치](#corescanner-driver-설치)
4. [UDI-Scan 설치](#udi-scan-설치)
5. [스캐너 설정](#스캐너-설정)
6. [설치 확인](#설치-확인)
7. [문제 해결](#문제-해결)

## 시스템 요구사항

### 필수 요구사항

| 항목 | 요구사항 |
|------|----------|
| **운영체제** | Windows 10 (64bit) 이상 또는 Windows 11 |
| **.NET Framework** | 4.8 이상 (Windows 10 기본 포함) |
| **RAM** | 최소 2GB (권장 4GB 이상) |
| **디스크 공간** | 최소 100MB + 이미지 저장 공간 |
| **USB 포트** | USB 2.0 이상 |
| **권한** | 일반 사용자 권한 (관리자 권한 불필요) |

### 지원 하드웨어

- **Zebra DS9908-SR** (Standard Range)
- **Zebra DS9908-HD** (High Density)
- **Zebra DS9908R** (Retail)
- 기타 CoreScanner를 지원하는 Zebra 이미징 스캐너

## 사전 준비

### 1. Windows 버전 확인

1. `Win + R` 키를 눌러 실행 창 열기
2. `winver` 입력 후 Enter
3. Windows 버전이 10 이상인지 확인

### 2. .NET Framework 확인

1. `Win + R` 키를 눌러 실행 창 열기
2. `cmd` 입력 후 Enter (명령 프롬프트 열기)
3. 다음 명령어 실행:
   ```cmd
   reg query "HKLM\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" /v Release
   ```
4. `Release` 값이 528040 이상이면 .NET 4.8 설치됨

**설치되지 않은 경우**:
- [Microsoft .NET Framework 4.8 다운로드](https://dotnet.microsoft.com/download/dotnet-framework/net48)

## CoreScanner Driver 설치

### 중요: 먼저 CoreScanner Driver를 설치해야 합니다!

### 1단계: Driver 다운로드

1. [Zebra CoreScanner Driver 다운로드 페이지](https://www.zebra.com/us/en/support-downloads/software/developer-tools/corescanner-driver.html) 접속
2. 최신 버전 다운로드 (권장: v3.0 이상)
3. 파일명 예: `CoreScannerDriver_v3.5.exe`

### 2단계: Driver 설치

1. 다운로드한 `CoreScannerDriver_vX.X.exe` 실행
2. **Next** 클릭
3. 라이선스 동의 후 **I Agree** 클릭
4. 설치 경로 확인 (기본값 권장)
   - 기본 경로: `C:\Program Files (x86)\Zebra Technologies\CoreScanner Driver`
5. **Install** 클릭
6. 설치 완료 후 **Finish** 클릭

### 3단계: Driver 설치 확인

1. **시작 메뉴 → Zebra Technologies → CoreScanner Driver**에서 설치 확인
2. 또는 `C:\Program Files (x86)\Zebra Technologies\CoreScanner Driver` 폴더 확인

## UDI-Scan 설치

### 방법 A: Installer 사용 (권장)

#### 1단계: Installer 다운로드

- GitHub Releases 페이지에서 `UDIScan-Setup.exe` 다운로드

#### 2단계: 설치 실행

1. `UDIScan-Setup.exe` 실행
2. 보안 경고가 나타나면 **실행** 클릭
3. **Next** 클릭
4. 설치 경로 선택 (기본값: `C:\Program Files\UDIScan`)
5. **Install** 클릭
6. 설치 완료 후 **Launch UDI-Scan** 체크 → **Finish**

#### 3단계: 바로가기 확인

- 바탕화면에 **UDI-Scan** 아이콘 생성됨
- 시작 메뉴에서도 찾을 수 있음

### 방법 B: Portable 버전 (ZIP)

#### 1단계: ZIP 다운로드

- GitHub Releases 페이지에서 `UDIScan-Portable-v1.0.0.zip` 다운로드

#### 2단계: 압축 해제

1. ZIP 파일을 원하는 위치에 압축 해제
   - 권장 위치: `C:\UDIScan` 또는 `D:\UDIScan`
   - USB 드라이브에도 설치 가능

#### 3단계: 실행

1. 압축 해제한 폴더로 이동
2. `UDIScan.exe` 실행

## 스캐너 설정

### DS9908을 CoreScanner 모드로 설정

DS9908 스캐너가 CoreScanner Driver와 통신하려면 **USB CDC (Virtual COM)** 또는 **USB HID (Non-Keyboard)** 모드로 설정되어야 합니다.

### 방법 1: 제품 매뉴얼의 바코드 스캔

1. [DS9908 Product Reference Guide](https://www.zebra.com/content/dam/zebra_new_ia/en-us/manuals/scanners/ds9908-product-reference-guide-en.pdf) 다운로드
2. PDF를 열어 다음 바코드를 찾아 스캔:

   **USB CDC (Virtual COM Port) 활성화**
   - 페이지 237: "USB CDC Virtual COM Port" 바코드 스캔

   또는

   **USB SNAPI 활성화**
   - 페이지 239: "Enable SNAPI Status Handshaking" 바코드 스캔

3. 스캔 후 스캐너가 재부팅됩니다 (약 5초 소요)

### 방법 2: 123Scan 유틸리티 사용

1. [123Scan 다운로드](https://www.zebra.com/us/en/support-downloads/software/utilities/123scan.html)
2. 123Scan 실행
3. 스캐너 연결 확인
4. **Configuration → USB Interface → USB CDC Virtual COM Port** 선택
5. **Apply** 클릭

### 설정 확인

1. **장치 관리자** 열기 (`Win + X` → 장치 관리자)
2. **포트 (COM & LPT)** 확장
3. "Symbol USB CDC Device (COMx)" 또는 유사한 이름 확인

## 설치 확인

### 1단계: UDI-Scan 실행

1. 바탕화면의 **UDI-Scan** 아이콘을 더블 클릭
2. 또는 시작 메뉴에서 실행

### 2단계: 스캐너 연결 확인

프로그램 상단에서 연결 상태 확인:

✅ **정상**: 🟢 Connected: DS9908-SR00004ZZWW
❌ **오류**: ⚪ Scanner Disconnected

### 3단계: 테스트 스캔

1. 메모장(Notepad) 실행
2. UDI-Scan이 실행된 상태에서 바코드 스캔
3. 메모장에 바코드 값이 자동 입력되는지 확인

✅ **성공**: 바코드 값이 메모장에 입력됨
✅ **이미지**: UDI-Scan의 "Last Captured Image"에 이미지 표시

## 문제 해결

### 문제 1: "CoreScanner 초기화 실패"

**원인**: CoreScanner Driver가 설치되지 않았거나 손상됨

**해결**:
1. CoreScanner Driver 재설치
2. PC 재시작
3. UDI-Scan 다시 실행

### 문제 2: "No scanner found"

**원인**: 스캐너가 PC에 인식되지 않음

**해결**:
1. USB 케이블 연결 확인
2. 장치 관리자에서 DS9908 인식 여부 확인
3. 다른 USB 포트에 연결 시도
4. 스캐너 설정 확인 (USB CDC/SNAPI 모드)

### 문제 3: 프로그램이 실행되지 않음

**원인**: .NET Framework 미설치 또는 버전 불일치

**해결**:
1. .NET Framework 4.8 설치 확인
2. Windows Update 실행하여 최신 업데이트 설치
3. Visual C++ Redistributable 설치 (필요 시)

### 문제 4: "Access Denied" 오류

**원인**: 설치 경로에 쓰기 권한 없음

**해결**:
1. Portable 버전 사용
2. 사용자 폴더에 설치 (예: `C:\Users\[사용자]\UDIScan`)

### 문제 5: 스캐너는 인식되는데 UDI-Scan에서 연결 안 됨

**원인**: 스캐너가 HID Keyboard 모드로 설정됨

**해결**:
1. 제품 매뉴얼의 "USB CDC" 또는 "USB SNAPI" 바코드 스캔
2. 스캐너 재부팅 후 UDI-Scan 재시작

## 네트워크 환경에서 배포

### 여러 PC에 동시 배포

1. **Silent Install 모드** (관리자용)
   ```cmd
   UDIScan-Setup.exe /SILENT /DIR="C:\Program Files\UDIScan"
   ```

2. **Group Policy를 통한 배포**
   - MSI 패키지 생성 (Inno Setup → MSI)
   - GPO에서 소프트웨어 배포 설정

3. **Portable 버전 배포**
   - ZIP 파일을 네트워크 드라이브에 복사
   - 사용자가 로컬로 압축 해제 후 실행

## 제거 (Uninstall)

### Installer 버전

1. **제어판 → 프로그램 추가/제거**
2. **UDI-Scan** 선택
3. **제거** 클릭

### Portable 버전

- 폴더 전체 삭제

### 설정 파일 삭제 (선택 사항)

설정을 완전히 제거하려면:
1. `Win + R` → `%APPDATA%` 입력
2. `UDIScan` 폴더 삭제

## 추가 리소스

- [Zebra Support Portal](https://www.zebra.com/us/en/support-downloads.html)
- [CoreScanner SDK Documentation](https://techdocs.zebra.com/dcs/scanners/sdk-windows/)
- [DS9908 Product Reference Guide](https://www.zebra.com/content/dam/zebra_new_ia/en-us/manuals/scanners/ds9908-product-reference-guide-en.pdf)

---

**설치 지원**: GitHub Issues 또는 이메일 문의
**최종 업데이트**: 2025-01-30
