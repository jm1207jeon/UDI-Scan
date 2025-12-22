@echo off
REM UDI-Scan Manual Build Script (MSBuild 경로 지정)

echo ====================================
echo UDI-Scan Manual Build Script
echo ====================================
echo.

REM 가능한 MSBuild 경로들
SET MSBUILD_2022_BT="C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
SET MSBUILD_2022_COM="C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
SET MSBUILD_2022_PRO="C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
SET MSBUILD_2019="C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe"

SET MSBUILD_PATH=

REM MSBuild 찾기
IF EXIST %MSBUILD_2022_BT% (
    SET MSBUILD_PATH=%MSBUILD_2022_BT%
    echo Found: Build Tools 2022
) ELSE IF EXIST %MSBUILD_2022_COM% (
    SET MSBUILD_PATH=%MSBUILD_2022_COM%
    echo Found: Visual Studio 2022 Community
) ELSE IF EXIST %MSBUILD_2022_PRO% (
    SET MSBUILD_PATH=%MSBUILD_2022_PRO%
    echo Found: Visual Studio 2022 Professional
) ELSE IF EXIST %MSBUILD_2019% (
    SET MSBUILD_PATH=%MSBUILD_2019%
    echo Found: Build Tools 2019
) ELSE (
    echo ERROR: MSBuild not found in any known location!
    echo.
    echo Please install Build Tools for Visual Studio 2022:
    echo https://visualstudio.microsoft.com/downloads/
    echo.
    echo Or download from this direct link:
    echo https://aka.ms/vs/17/release/vs_BuildTools.exe
    echo.
    pause
    exit /b 1
)

echo Using MSBuild: %MSBUILD_PATH%
echo.

echo [1/3] Cleaning previous builds...
if exist "src\UDIScan.App\bin" rmdir /s /q "src\UDIScan.App\bin"
if exist "src\UDIScan.App\obj" rmdir /s /q "src\UDIScan.App\obj"
if exist "src\UDIScan.Core\bin" rmdir /s /q "src\UDIScan.Core\bin"
if exist "src\UDIScan.Core\obj" rmdir /s /q "src\UDIScan.Core\obj"

echo [2/3] Building solution (Release configuration)...
cd src
%MSBUILD_PATH% UDIScan.sln /p:Configuration=Release /p:Platform="Any CPU" /t:Rebuild /v:minimal
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
pause
