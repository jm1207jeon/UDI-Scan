@echo off
REM UDI-Scan Build Script

echo ====================================
echo UDI-Scan Build Script
echo ====================================
echo.

REM Check if MSBuild exists
where msbuild >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: MSBuild not found. Please install Visual Studio or Build Tools.
    pause
    exit /b 1
)

echo [1/3] Cleaning previous builds...
if exist "src\UDIScan.App\bin" rmdir /s /q "src\UDIScan.App\bin"
if exist "src\UDIScan.App\obj" rmdir /s /q "src\UDIScan.App\obj"
if exist "src\UDIScan.Core\bin" rmdir /s /q "src\UDIScan.Core\bin"
if exist "src\UDIScan.Core\obj" rmdir /s /q "src\UDIScan.Core\obj"
if exist "src\UDIScan.Native\bin" rmdir /s /q "src\UDIScan.Native\bin"
if exist "src\UDIScan.Native\obj" rmdir /s /q "src\UDIScan.Native\obj"

echo [2/3] Building solution (Release configuration)...
cd src
msbuild UDIScan.sln /p:Configuration=Release /p:Platform="Any CPU" /t:Rebuild /v:minimal
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ERROR: Build failed!
    cd ..
    pause
    exit /b 1
)
cd ..

echo [3/3] Build completed successfully!
echo.
echo Output location: src\UDIScan.App\bin\Release\net48\
echo.
echo To create installer:
echo   1. Install Inno Setup from https://jrsoftware.org/isdl.php
echo   2. Open installer\setup.iss
echo   3. Build the installer
echo.
pause
