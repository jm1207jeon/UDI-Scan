@echo off
REM UDI-Scan Build Script (.NET SDK version - Visual Studio 불필요!)

echo ====================================
echo UDI-Scan Build Script (.NET SDK)
echo ====================================
echo.

REM .NET SDK 설치 확인
dotnet --version >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: .NET SDK not found!
    echo.
    echo Please install .NET SDK 8.0 first:
    echo https://dotnet.microsoft.com/download/dotnet/8.0
    echo.
    echo Or download directly:
    echo https://aka.ms/dotnet/8.0/dotnet-sdk-win-x64.exe
    echo.
    echo After installation, close this window and try again.
    echo.
    pause
    exit /b 1
)

echo Found .NET SDK version:
dotnet --version
echo.

echo [1/3] Cleaning previous builds...
if exist "src\UDIScan.App\bin" rmdir /s /q "src\UDIScan.App\bin"
if exist "src\UDIScan.App\obj" rmdir /s /q "src\UDIScan.App\obj"
if exist "src\UDIScan.Core\bin" rmdir /s /q "src\UDIScan.Core\bin"
if exist "src\UDIScan.Core\obj" rmdir /s /q "src\UDIScan.Core\obj"

echo [2/3] Building solution (Release configuration)...
cd src
dotnet build UDIScan.sln -c Release -v minimal
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ERROR: Build failed!
    echo.
    echo Common issues:
    echo 1. CoreScanner Driver not installed
    echo 2. .NET Framework 4.8 not installed
    echo.
    echo Please check DOTNET_SDK_INSTALL.md for troubleshooting.
    echo.
    cd ..
    pause
    exit /b 1
)
cd ..

echo [3/3] Build completed successfully!
echo.
echo Output location: src\UDIScan.App\bin\Release\net48\
echo Executable: src\UDIScan.App\bin\Release\net48\UDIScan.exe
echo.
echo To run the program:
echo   cd src\UDIScan.App\bin\Release\net48
echo   UDIScan.exe
echo.
pause
