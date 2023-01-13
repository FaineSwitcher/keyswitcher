## A companion to Mahou/NMahou.

## Simple hook with dll injection, and IPC messaging through PostMessage and WndProc, from DLL to exe, with support of x86 and x64 apps.

x86 helper will die if x64 is not running, but if you need only x86 you can compile the x64 version as x86, and it will work just as if it x86.
Its purpose of creation was to listen for WM_INPUTLANGCHANGE message, and it is succesfully done that task, try switching layout with alt+shift or any other way.
Too bad that same can't be done in C#... (sigh)