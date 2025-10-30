; UDI-Scan Installer Script for Inno Setup
; Requires Inno Setup 6.0 or later
; Download from: https://jrsoftware.org/isdl.php

#define MyAppName "UDI-Scan"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "UDI-Scan Project"
#define MyAppURL "https://github.com/your-repo/UDI-Scan"
#define MyAppExeName "UDIScan.exe"
#define MyAppAssocName "UDI-Scan Application"
#define MyAppAssocExt ""
#define MyAppAssocKey StringChange(MyAppAssocName, " ", "") + MyAppAssocExt

[Setup]
; Application Information
AppId={{A1B2C3D4-E5F6-4A5B-8C7D-9E1F2A3B4C5D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}

; Installation Paths
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes

; Output
OutputDir=.\Output
OutputBaseFilename=UDIScan-Setup-v{#MyAppVersion}
Compression=lzma2
SolidCompression=yes

; UI
WizardStyle=modern
SetupIconFile=..\src\UDIScan.App\app.ico
UninstallDisplayIcon={app}\{#MyAppExeName}

; Privileges
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog

; Architecture
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64

; Version Info
VersionInfoVersion={#MyAppVersion}
VersionInfoCompany={#MyAppPublisher}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "korean"; MessagesFile: "compiler:Languages\Korean.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
; Main Application Files
Source: "..\src\UDIScan.App\bin\Release\net48\UDIScan.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\src\UDIScan.App\bin\Release\net48\UDIScan.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\src\UDIScan.App\bin\Release\net48\UDIScan.Native.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\src\UDIScan.App\bin\Release\net48\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion

; Documentation
Source: "..\README.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\docs\USER_GUIDE.md"; DestDir: "{app}\docs"; Flags: ignoreversion
Source: "..\docs\INSTALLATION.md"; DestDir: "{app}\docs"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
function InitializeSetup(): Boolean;
var
  ResultCode: Integer;
  DotNetVersion: Cardinal;
begin
  Result := True;

  // Check for .NET Framework 4.8
  if RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', DotNetVersion) then
  begin
    if DotNetVersion < 528040 then
    begin
      MsgBox('.NET Framework 4.8 or later is required.'#13#13'Please install .NET Framework 4.8 and try again.'#13#13'Download from: https://dotnet.microsoft.com/download/dotnet-framework/net48', mbError, MB_OK);
      Result := False;
    end;
  end
  else
  begin
    MsgBox('.NET Framework 4.8 or later is required.'#13#13'Please install .NET Framework 4.8 and try again.'#13#13'Download from: https://dotnet.microsoft.com/download/dotnet-framework/net48', mbError, MB_OK);
    Result := False;
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    // Post-installation tasks
    // Create initial settings directory
    ForceDirectories(ExpandConstant('{userappdata}\UDIScan'));
  end;
end;

[UninstallDelete]
Type: filesandordirs; Name: "{userappdata}\UDIScan"

[Messages]
english.WelcomeLabel2=This will install [name/ver] on your computer.%n%nIMPORTANT: Zebra CoreScanner Driver must be installed before using this application.%n%nIt is recommended that you close all other applications before continuing.
korean.WelcomeLabel2=[name/ver]을(를) 컴퓨터에 설치합니다.%n%n중요: 이 애플리케이션을 사용하기 전에 Zebra CoreScanner Driver를 설치해야 합니다.%n%n계속하기 전에 다른 모든 애플리케이션을 닫는 것이 좋습니다.
