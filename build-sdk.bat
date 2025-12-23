@echo off
REM UDI-Scan Build Script (.NET SDK + Developer Pack version)

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
    pause
    exit /b 1
)

echo Found .NET SDK version:
dotnet --version
echo.

REM .NET Framework MSBuild 경로 찾기
SET FRAMEWORK_MSBUILD=""

REM Visual Studio 2022의 MSBuild 찾기
if exist "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" (
    SET FRAMEWORK_MSBUILD="C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
    echo Found: Visual Studio 2022 Community MSBuild
) else if exist "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" (
    SET FRAMEWORK_MSBUILD="C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
    echo Found: Visual Studio 2022 Professional MSBuild
) else if exist "C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe" (
    SET FRAMEWORK_MSBUILD="C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
    echo Found: Build Tools 2022 MSBuild
) else if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe" (
    SET FRAMEWORK_MSBUILD="C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
    echo Found: Build Tools 2019 MSBuild
)

REM MSBuild를 못 찾으면 에러
if %FRAMEWORK_MSBUILD%=="" (
    echo.
    echo ERROR: .NET Framework MSBuild not found!
    echo.
    echo COM references require .NET Framework version of MSBuild.
    echo.
    echo Please install ONE of the following:
    echo.
    echo Option 1 - Build Tools for Visual Studio 2022 ^(recommended^):
    echo   https://aka.ms/vs/17/release/vs_BuildTools.exe
    echo   Select: ".NET desktop build tools"
    echo   Size: ~3GB
    echo.
    echo Option 2 - Visual Studio 2022 Community:
    echo   https://visualstudio.microsoft.com/vs/community/
    echo   Select: ".NET desktop development"
    echo   Size: ~20GB
    echo.
    pause
    exit /b 1
)

echo Using MSBuild: %FRAMEWORK_MSBUILD%
echo.

echo [1/3] Cleaning previous builds...
if exist "src\UDIScan.App\bin" rmdir /s /q "src\UDIScan.App\bin"
if exist "src\UDIScan.App\obj" rmdir /s /q "src\UDIScan.App\obj"
if exist "src\UDIScan.Core\bin" rmdir /s /q "src\UDIScan.Core\bin"
if exist "src\UDIScan.Core\obj" rmdir /s /q "src\UDIScan.Core\obj"

echo [2/3] Building solution (Release configuration)...
cd src
%FRAMEWORK_MSBUILD% UDIScan.sln /p:Configuration=Release /p:Platform="Any CPU" /t:Rebuild /v:minimal
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
echo Executable: src\UDIScan.App\bin\Release\net48\UDIScan.exe
echo.
echo To run the program:
echo   run.bat
echo.
pause
