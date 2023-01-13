@call build Release x86_x64
@if not [%errorlevel%] == 1 (bin\Release_x86_x64\Mahou.exe) else echo Build error.