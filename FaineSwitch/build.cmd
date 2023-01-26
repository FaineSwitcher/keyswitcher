@ECHO OFF
@CHCP 65001>nul
SET configuration=%1
SET platform=%2
SET const=TRACE;
IF /I [%1] == [clean] GOTO CLEAN
IF /I [%1] == [] SET configuration=Release
IF /I [%1] == [debug] SET const=DEBUG;%const%
IF /I [%2] == [x86_x64] SET platform="Any CPU"
IF /I [%2] == [] SET platform="Any CPU"
IF /I [%1] == [-h] GOTO HELP
IF /I [%1] == [--help] GOTO HELP
IF /I [%1] == [/?] GOTO HELP
IF NOT [%3] == [] SET const=%3;%const%
cd %~dp0 
FOR /F "tokens=* USEBACKQ" %%F in (
`"git.exe log -1 --pretty=%%h"`
) DO (
ECHO static class ____ { public static string commit="%%F"; } > ____commit.cs
)
echo %const%
ECHO %windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe .\Mahou.sln /nologo /m /t:Build /p:Configuration=%configuration% /p:Platform=%platform% /p:DefineConstants="%const%"
@%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe .\Mahou.sln /nologo /m /t:Build /p:Configuration=%configuration% /p:Platform=%platform% /p:DefineConstants="%const%"
ECHO static class ____ { public static string commit=""; } > ____commit.cs
GOTO EOF
:CLEAN
clean.cmd
:HELP
ECHO Usage:
ECHO   build.cmd [configuration] [platform]
ECHO By default configuration is "release" and platform is "Any CPU".
ECHO Exapmles:
ECHO   build.cmd debug x86         -^> Builds Mahou debug x86.
ECHO   build.cmd release "Any CPU" -^> Builds Mahou release any cpu.
ECHO   build.cmd debug x86_x64     -^> Builds Mahou debug any cpu.
ECHO   build.cmd clean             -^> calls clean.cmd.
:EOF
