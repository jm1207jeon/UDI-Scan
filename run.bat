@echo off
REM UDI-Scan Quick Run Script

echo ====================================
echo UDI-Scan Quick Run
echo ====================================
echo.

REM 실행 파일이 있는지 확인
if not exist "src\UDIScan.App\bin\Release\net48\UDIScan.exe" (
    echo ERROR: Program not built yet!
    echo.
    echo Please build the program first:
    echo   build-dotnet.bat
    echo.
    pause
    exit /b 1
)

echo Starting UDI-Scan...
echo.
cd src\UDIScan.App\bin\Release\net48
start UDIScan.exe
cd ..\..\..\..\..\

echo Program started!
echo If the program didn't open, check:
echo 1. CoreScanner Driver is installed
echo 2. DS9908 scanner is connected via USB
echo.
