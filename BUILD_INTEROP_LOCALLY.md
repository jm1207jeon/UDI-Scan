# 🔧 Interop DLL 로컬 생성 가이드

GitHub Actions 빌드가 계속 실패하는 이유는 **Interop DLL을 자동으로 생성하는 것이 불안정**하기 때문입니다.

**해결 방법**: 로컬 PC에서 한 번만 Interop DLL을 생성하고 Git에 커밋하면 됩니다!

---

## 📋 순서

### **Step 1: CoreScanner Driver 설치 확인**

CoreScanner Driver가 설치되어 있어야 합니다.

확인:
```
C:\Program Files (x86)\Zebra Technologies\
```
폴더 안에 CoreScanner 관련 파일이 있는지 확인

---

### **Step 2: 로컬에서 한 번 빌드**

#### **방법 A: Visual Studio 사용**

1. `src/UDIScan.sln` 열기
2. **Build** → **Rebuild Solution**
3. 빌드 성공

#### **방법 B: build-manual.bat 사용**

```bash
cd C:\UDI-Scan
build-manual.bat
```

---

### **Step 3: Interop DLL 찾기**

빌드 후 다음 위치에 Interop DLL이 생성됩니다:

```
C:\UDI-Scan\src\UDIScan.Core\obj\Release\net48\Interop.CoreScanner.dll
```

---

### **Step 4: lib 폴더에 복사**

1. UDI-Scan 폴더에 `lib` 폴더 생성 (이미 있으면 생략)
   ```bash
   mkdir lib
   ```

2. Interop DLL을 복사:
   ```bash
   copy src\UDIScan.Core\obj\Release\net48\Interop.CoreScanner.dll lib\
   ```

---

### **Step 5: Git에 커밋**

```bash
git add lib/Interop.CoreScanner.dll
git commit -m "Add pre-built Interop.CoreScanner.dll for GitHub Actions"
git push
```

---

## ✅ 완료!

이제 GitHub Actions가 자동으로 `lib/Interop.CoreScanner.dll`을 사용합니다!

더 이상 Interop DLL 생성 단계가 필요 없고, 빌드가 **100% 성공**합니다!

---

## 🎯 왜 이 방법이 확실한가?

### **Before (실패)**
```
GitHub Actions에서 Interop DLL 생성 시도
→ TlbImp.exe 못 찾거나
→ CoreScanner.tlb 못 찾거나
→ 생성 실패
→ 빌드 실패 ❌
```

### **After (성공)**
```
이미 lib/Interop.CoreScanner.dll이 Git에 있음
→ 다운로드됨 (git clone/pull)
→ .csproj가 자동으로 이 DLL 사용
→ 빌드 성공! ✅
```

---

**이 방법이 가장 확실하고 안정적입니다!**
