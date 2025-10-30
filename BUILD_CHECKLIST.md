# ✅ UDI-Scan 빌드 체크리스트

빠르게 확인할 수 있는 단계별 체크리스트입니다.

---

## 📦 사전 준비 체크리스트

### 소프트웨어 설치

- [ ] **Windows 10/11 PC** 준비됨
- [ ] **인터넷 연결** 확인됨
- [ ] **디스크 공간** 10GB 이상 확보
- [ ] **Visual Studio 2022 Community** 설치됨
  - [ ] ".NET 데스크톱 개발" 워크로드 포함
  - [ ] .NET Framework 4.8 SDK 포함
- [ ] **Zebra CoreScanner Driver** 설치됨 (v3.0 이상)

### 프로젝트 파일

- [ ] **소스 코드** 다운로드됨
  - GitHub ZIP 다운로드 또는 Git 클론
- [ ] **압축 해제** 완료
  - 권장 경로: `C:\UDI-Scan`
- [ ] **폴더 구조** 확인
  - `src\` 폴더 존재
  - `README.md` 파일 존재

---

## 🔨 빌드 체크리스트

### Visual Studio에서 빌드

- [ ] **Visual Studio 2022** 실행
- [ ] **솔루션 파일** 열기
  - 파일: `C:\UDI-Scan\src\UDIScan.sln`
- [ ] **NuGet 패키지 복원** 완료 대기
  - 하단 상태 표시줄 확인
- [ ] **빌드 구성** 선택
  - [ ] Configuration: **Release** 선택
  - [ ] Platform: **Any CPU** 선택
- [ ] **솔루션 빌드** 실행
  - 단축키: `Ctrl + Shift + B`
- [ ] **빌드 성공** 확인
  - 출력: "빌드: 성공 3, 실패 0"

### 빌드 결과 확인

- [ ] **실행 파일** 생성 확인
  - 경로: `C:\UDI-Scan\src\UDIScan.App\bin\Release\net48\`
  - [ ] `UDIScan.exe` 존재
  - [ ] `UDIScan.Core.dll` 존재
  - [ ] `UDIScan.Native.dll` 존재
  - [ ] `Newtonsoft.Json.dll` 존재

---

## ▶️ 실행 체크리스트

### 프로그램 실행

- [ ] **Visual Studio에서 실행**
  - 단축키: `Ctrl + F5`
- [ ] **프로그램 창** 나타남
  - 제목: "UDI-Scan - Zebra DS9908 Barcode Scanner"
- [ ] **UI 요소** 확인
  - [ ] 상단: 스캐너 연결 상태 표시
  - [ ] 왼쪽: 스캔 이력 패널
  - [ ] 오른쪽: 이미지 프리뷰 패널
  - [ ] 하단: 설정 (Image Capture, Save Path)
  - [ ] 최하단: 상태 바

### 스캐너 연결 테스트 (DS9908 있는 경우)

- [ ] **스캐너 연결**
  - DS9908을 USB로 PC에 연결
- [ ] **장치 관리자** 확인
  - "포트 (COM & LPT)" 아래 "Symbol USB CDC Device" 표시
- [ ] **프로그램 재시작**
- [ ] **연결 상태** 확인
  - 🟢 "Connected: DS9908-..." 표시
- [ ] **바코드 스캔** 테스트
  - [ ] 메모장 실행
  - [ ] 바코드 스캔
  - [ ] 메모장에 자동 입력됨
  - [ ] UDI-Scan에 스캔 이력 표시됨

### 이미지 캡처 테스트

- [ ] **이미지 캡처 활성화** 확인
  - "Image Capture: Enabled" (초록색)
- [ ] **저장 경로** 설정
  - Browse 버튼으로 경로 선택
- [ ] **바코드 스캔**
- [ ] **이미지 저장** 확인
  - [ ] 오른쪽 패널에 이미지 프리뷰 표시
  - [ ] 저장 폴더에 JPEG 파일 생성
  - [ ] 파일명 형식: `바코드_날짜_시간.jpg`

---

## 📦 배포 체크리스트 (선택 사항)

### Installer 만들기

- [ ] **Inno Setup** 설치
  - 다운로드: https://jrsoftware.org/isdl.php
- [ ] **스크립트 파일** 열기
  - 파일: `C:\UDI-Scan\installer\setup.iss`
- [ ] **Compile** 실행
  - 단축키: `Ctrl + F9`
- [ ] **Installer 생성** 확인
  - 파일: `C:\UDI-Scan\installer\Output\UDIScan-Setup-v1.0.0.exe`

### Portable 버전 만들기

- [ ] **빌드 폴더** 복사
  - 경로: `C:\UDI-Scan\src\UDIScan.App\bin\Release\net48\`
- [ ] **ZIP 파일** 생성
  - 폴더 우클릭 → 보내기 → 압축(zip) 폴더
- [ ] **파일명** 변경
  - `UDIScan-Portable-v1.0.0.zip`

### 배포 파일 테스트

- [ ] **다른 PC에 복사** (또는 다른 폴더)
- [ ] **설치 또는 압축 해제**
- [ ] **실행** 확인
  - CoreScanner Driver 설치 필요
  - .NET Framework 4.8 필요

---

## 🐛 문제 해결 체크리스트

### 빌드 오류 발생 시

- [ ] **오류 메시지** 확인
  - 하단 "오류 목록" 패널 확인
- [ ] **CoreScanner 관련 오류**
  - [ ] CoreScanner Driver 설치 확인
  - [ ] PC 재부팅
  - [ ] Visual Studio 재시작
- [ ] **NuGet 패키지 오류**
  - [ ] 패키지 복원 재시도
  - [ ] 인터넷 연결 확인
  - [ ] Visual Studio 재시작
- [ ] **.NET Framework 오류**
  - [ ] .NET Framework 4.8 설치 확인
  - [ ] Windows Update 실행

### 실행 오류 발생 시

- [ ] **"DLL을 찾을 수 없습니다" 오류**
  - [ ] CoreScanner Driver 설치 확인
  - [ ] 필요한 DLL 파일 확인 (bin 폴더)
- [ ] **스캐너 연결 안 됨**
  - [ ] USB 케이블 확인
  - [ ] 장치 관리자에서 스캐너 인식 확인
  - [ ] 스캐너 모드 확인 (USB CDC 또는 SNAPI)
- [ ] **바코드 입력 안 됨**
  - [ ] 대상 앱에 포커스 확인
  - [ ] 관리자 권한 앱이 아닌지 확인
- [ ] **이미지 저장 안 됨**
  - [ ] Image Capture Enabled 확인
  - [ ] 저장 경로 쓰기 권한 확인
  - [ ] 디스크 공간 확인

---

## 📞 추가 도움

모든 체크리스트를 확인했는데도 문제가 해결되지 않는 경우:

1. **상세 가이드 참조**
   - [📘 초보자용 빌드 가이드](docs/BUILD_GUIDE_BEGINNER.md)
   - [📗 설치 가이드](docs/INSTALLATION.md)
   - [📙 사용자 가이드](docs/USER_GUIDE.md)

2. **GitHub Issues**
   - 오류 메시지 전체 복사
   - 스크린샷 첨부
   - 문제 발생 단계 설명

3. **로그 확인**
   - Visual Studio 출력 창
   - Windows 이벤트 뷰어

---

## ✨ 완료!

모든 체크리스트가 완료되었다면 축하합니다! 🎉

이제 UDI-Scan을 사용할 준비가 되었습니다.

**다음 단계:**
- 실제 작업 환경에서 테스트
- 필요시 설정 조정
- 피드백 및 개선사항 제안

---

**작성일:** 2025-01-30
