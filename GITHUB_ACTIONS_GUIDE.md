# 🚀 GitHub Actions 자동 빌드 가이드

**PC에 아무것도 설치하지 않고 프로그램 실행하기!**

---

## 📋 개요

GitHub Actions가 자동으로 프로그램을 빌드해줍니다!
- ✅ PC에 Visual Studio/Build Tools 설치 불필요
- ✅ 클라우드에서 자동 빌드
- ✅ 빌드된 실행 파일만 다운로드
- ✅ 코드 수정 시 자동으로 다시 빌드

---

## 🎯 사용 방법

### **Step 1: 코드를 GitHub에 푸시**

이 프로젝트를 GitHub 저장소에 푸시하세요.

**이미 GitHub 저장소가 있다면:**
```bash
git add .
git commit -m "Add GitHub Actions workflow"
git push
```

**저장소가 없다면:**
1. GitHub.com에서 새 저장소 생성
2. 로컬 코드를 푸시:
```bash
git remote add origin https://github.com/사용자명/저장소명.git
git branch -M main
git push -u origin main
```

---

### **Step 2: GitHub Actions 자동 빌드 확인**

1. **GitHub 저장소 페이지로 이동**
   ```
   https://github.com/사용자명/저장소명
   ```

2. **"Actions" 탭 클릭**

3. **빌드 진행 상황 확인**
   - 🟡 노란색: 빌드 중 (약 2~3분 소요)
   - 🟢 초록색: 빌드 성공!
   - 🔴 빨간색: 빌드 실패 (에러 확인 필요)

---

### **Step 3: 빌드된 실행 파일 다운로드**

#### **방법 1: Artifacts 다운로드 (가장 빠름)**

1. **"Actions" 탭** → 최근 완료된 빌드 클릭

2. 페이지 하단 **"Artifacts"** 섹션 찾기

3. **"UDIScan-Release-ZIP"** 다운로드 (클릭)

4. 다운로드한 ZIP 파일 압축 해제

5. **UDIScan.exe** 실행!

---

#### **방법 2: Release 다운로드 (태그 버전용)**

버전 태그를 푸시하면 자동으로 Release가 생성됩니다.

**Release 만들기:**
```bash
git tag v1.0.0
git push origin v1.0.0
```

**다운로드:**
1. GitHub 저장소 → **"Releases"** 클릭
2. 최신 버전 찾기
3. **"UDIScan-Release.zip"** 다운로드
4. 압축 해제 후 **UDIScan.exe** 실행

---

## 🔄 자동 빌드 트리거 조건

다음 상황에서 자동으로 빌드됩니다:

### **1. 코드 푸시 시**
```bash
git push
```
→ main, master, feature/*, claude/* 브랜치에 푸시하면 자동 빌드

### **2. Pull Request 시**
→ main, master 브랜치로 PR 생성 시 자동 빌드

### **3. 수동 실행**
1. GitHub → "Actions" 탭
2. "Build UDI-Scan" 선택
3. "Run workflow" 버튼 클릭

---

## 📦 다운로드한 파일 구조

압축 해제하면 다음 파일들이 나옵니다:

```
UDIScan-Release/
├── UDIScan.exe           ← 실행 파일 (이것만 실행하면 됨!)
├── UDIScan.Core.dll
├── UDIScan.Core.pdb
├── Interop.CoreScanner.dll
└── (기타 DLL 파일들)
```

---

## 🖥️ 실행 방법

### **프로그램 실행**

1. 압축 해제한 폴더에서 **UDIScan.exe** 더블클릭

2. 프로그램이 실행됩니다!

### **사전 준비 (필수!)**

실행 전에 다음을 먼저 설치하세요:

#### **1. CoreScanner Driver 설치**
```
https://www.zebra.com/us/en/support-downloads/software/developer-tools/corescanner-driver.html
```
→ 설치 후 **PC 재부팅** 필수!

#### **2. DS9908 스캐너 연결**
- USB 케이블로 PC 연결
- 장치 관리자에서 "Symbol USB CDC Device" 확인

#### **3. 스캐너를 Snapshot Mode로 설정**

자세한 방법은 **EXECUTION_GUIDE.md** 파일 참고

---

## 🔧 문제 해결

### **빌드가 실패했어요 (🔴 빨간색)**

1. GitHub → "Actions" → 실패한 빌드 클릭
2. 에러 메시지 확인
3. 주요 원인:
   - 프로젝트 파일 오류
   - NuGet 패키지 복원 실패
   - COM 참조 문제

### **Artifacts를 다운로드할 수 없어요**

- Artifacts는 **30일 후 자동 삭제**됩니다
- 다시 빌드하려면: Actions → "Run workflow" 클릭

### **"UDIScan.exe를 실행할 수 없습니다" 에러**

#### **원인 1: CoreScanner Driver 미설치**
→ CoreScanner Driver 설치 후 PC 재부팅

#### **원인 2: .NET Framework 4.8 미설치**
→ Windows 10/11은 기본 설치되어 있음
→ 없으면 다운로드:
```
https://dotnet.microsoft.com/download/dotnet-framework/net48
```

#### **원인 3: Windows Defender 차단**
→ 파일 우클릭 → 속성 → "차단 해제" 체크 → 확인

---

## 📊 빌드 상태 확인

### **실시간 빌드 로그 보기**

1. GitHub → "Actions" → 최근 빌드 클릭
2. "build" job 클릭
3. 각 단계별 로그 확인

### **빌드 시간**

- 일반적으로 **2~3분** 소요
- 첫 빌드는 조금 더 걸릴 수 있음 (NuGet 패키지 캐시)

---

## 🎉 성공 체크리스트

- [ ] 코드를 GitHub에 푸시 완료
- [ ] GitHub Actions 빌드 성공 (🟢)
- [ ] Artifacts 다운로드 완료
- [ ] ZIP 파일 압축 해제
- [ ] CoreScanner Driver 설치 완료
- [ ] DS9908 USB 연결 완료
- [ ] UDIScan.exe 실행 성공!
- [ ] 🟢 "Connected" 상태 확인
- [ ] 트리거 당겨서 이미지 캡처 테스트

---

## 🔄 코드 수정 시

코드를 수정하고 다시 빌드하려면:

```bash
# 1. 코드 수정

# 2. 커밋 및 푸시
git add .
git commit -m "수정 내용"
git push

# 3. GitHub Actions 자동 빌드 시작
# 4. 빌드 완료 후 새 Artifacts 다운로드
```

---

## 💡 장점 정리

### **PC에 설치 불필요**
- Visual Studio ❌
- Build Tools ❌
- .NET SDK ❌
- MSBuild ❌

### **필요한 것**
- ✅ GitHub 계정
- ✅ 인터넷 연결
- ✅ CoreScanner Driver (스캐너 사용을 위해)

### **자동화**
- 코드 푸시 → 자동 빌드 → 실행 파일 다운로드

---

## 📞 추가 도움말

### **GitHub Actions 문서**
```
https://docs.github.com/en/actions
```

### **빌드 실패 시**
1. Actions 탭에서 에러 로그 확인
2. 프로젝트 파일 검증
3. Issue 등록

---

**작성일**: 2025-01-22
**방법**: GitHub Actions (클라우드 자동 빌드)
**장점**: PC에 설치 없음, 자동화

**즐거운 이미지 캡처 되세요!** 📸✨
