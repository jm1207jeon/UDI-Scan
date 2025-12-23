# 🚀 .NET SDK 설치 가이드 (간편 버전)

**Visual Studio 없이 프로그램 빌드하기!**

---

## 📥 Step 1: .NET SDK 다운로드

### **다운로드 링크**

브라우저에서 다음 주소를 엽니다:

```
https://dotnet.microsoft.com/download/dotnet/8.0
```

또는 직접 다운로드:

```
https://aka.ms/dotnet/8.0/dotnet-sdk-win-x64.exe
```

### **파일 정보**
- 파일명: `dotnet-sdk-8.0.xxx-win-x64.exe`
- 크기: 약 **200MB** (Visual Studio 3GB보다 훨씬 작음!)
- 설치 크기: 약 500MB

---

## 💾 Step 2: .NET SDK 설치

### **설치 방법**

1. **다운로드한 파일 실행**
   ```
   dotnet-sdk-8.0.xxx-win-x64.exe
   ```

2. **설치 마법사 진행**
   - "Install" 버튼 클릭
   - 기본 설정 그대로 사용
   - 추가 옵션 선택 필요 없음

3. **설치 완료**
   - 약 2~3분 소요
   - PC 재시작 **필요 없음**

---

## ✅ Step 3: 설치 확인

### **명령 프롬프트 열기**

1. Windows 키 + R
2. `cmd` 입력 후 Enter

### **설치 확인 명령**

```bash
dotnet --version
```

**정상 출력 예시:**
```
8.0.101
```

또는

```bash
dotnet --info
```

**정상 출력 예시:**
```
.NET SDK:
 Version:   8.0.101
 Commit:    ...

런타임 환경:
 OS Name:     Windows
 OS Version:  10.0.19045
 ...
```

---

## 🎯 Step 4: 프로그램 빌드

### **UDI-Scan 폴더로 이동**

```bash
cd C:\UDI-Scan
```

### **빌드 스크립트 실행**

```bash
build-dotnet.bat
```

(제가 이 파일을 지금 만들어드리겠습니다!)

---

## 🔧 문제 해결

### **"dotnet: 명령을 찾을 수 없습니다" 에러**

**해결 방법:**

1. **명령 프롬프트 새로 열기**
   - 기존 창 닫기
   - 새 명령 프롬프트 열기

2. **PATH 확인**
   ```bash
   echo %PATH%
   ```

   다음 경로가 있는지 확인:
   ```
   C:\Program Files\dotnet\
   ```

3. **수동으로 PATH 추가** (위 경로가 없는 경우)
   - 시작 → "환경 변수" 검색
   - "시스템 환경 변수 편집" 클릭
   - "환경 변수" 버튼
   - "시스템 변수"에서 "Path" 선택 → 편집
   - "새로 만들기" → `C:\Program Files\dotnet\` 추가
   - 확인 → 명령 프롬프트 재시작

---

### **.NET SDK 재설치**

설치가 제대로 안 된 것 같으면:

1. **제어판** → 프로그램 제거
2. **.NET SDK** 검색 후 제거
3. PC 재시작
4. 다시 설치

---

## 📊 비교: Build Tools vs .NET SDK

| 항목 | Build Tools | .NET SDK |
|------|-------------|----------|
| 다운로드 크기 | 3GB | 200MB |
| 설치 크기 | 10GB+ | 500MB |
| 설치 시간 | 20~30분 | 2~3분 |
| 빌드 명령 | msbuild | dotnet build |
| 재시작 필요 | 권장 | 불필요 |

---

## 🎉 설치 완료 체크리스트

- [ ] .NET SDK 다운로드 완료
- [ ] 설치 완료
- [ ] `dotnet --version` 확인
- [ ] 버전 번호 출력됨 (예: 8.0.101)
- [ ] `build-dotnet.bat` 실행 준비 완료!

---

**다음: build-dotnet.bat 실행**

설치가 완료되면 알려주세요! build-dotnet.bat 파일을 만들어드리겠습니다.

---

**작성일**: 2025-01-22
**방법**: .NET SDK (Visual Studio 불필요)
