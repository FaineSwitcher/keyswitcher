#include <windows.h>
#include <stdio.h>
#include "jkl.h"
 
HWND MAIN;
HMODULE lib;
UINT uMSG; 
void PostQuit() {
	FUNC unHook = (FUNC)GetProcAddress(lib, "unHook");
	unHook();
    FreeLibrary(lib);
}
LRESULT CALLBACK WndProc(HWND hWnd, UINT Msg, WPARAM wParam, LPARAM lParam) {
	// if (Msg == uMSG) // MEOW!
		// printf("MEOW, MEOW! W: %i, L: %i\n", wParam, lParam);
	// else 
		// printf("MSG: %i, %i\n", Msg, uMSG);
	if (Msg == WM_DESTROY || Msg == WM_QUIT) {
		// printf("POST QUIT PASSED");
		HWND X86 = FindWindow("_HIDDEN_X86_HELPER", NULL);
		PostQuit();
		PostMessage(X86, WM_QUIT, 0, 0);
	}
	return DefWindowProc(hWnd, Msg, wParam, lParam);
}

void getdir(char* fullPath) { 
    char *dir = &fullPath[strlen(fullPath)];
    while (dir > fullPath && *dir != '\\')
        dir--;
    if (*dir ==  '\\')
        *dir = '\0';
    return;
}

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR nCmdLine, int nCmdShow) {
	if (FindWindow("_HIDDEN_HWND_SERVER", NULL))
		exit(0); // Already exist
	MAIN = CreateHWND(hInstance, "_HIDDEN_HWND_SERVER", (WNDPROC)WndProc);
    lib = LoadLibrary("jkl.dll");
	FUNC setHook = (FUNC)GetProcAddress(lib, "setHook");
	if (!setHook())
		printf("HOOK SET FAILED!");
	UFUNC getUMsg = (UFUNC)GetProcAddress(lib, "getUMsg");
	uMSG = getUMsg();
	FILE* fp;
	fp = fopen("umsg.id", "w");
	if(fp == NULL)
		exit(-1);
	fprintf(fp, "%i", uMSG);
	fclose(fp);
	// char dir[1536];
	// GetModuleFileName(NULL, dir, sizeof(dir)-1);
	// char jklx86[2048];
	// getdir(dir);
	// strcat(jklx86, dir);
	// strcat(jklx86, "\\jklx86.exe");
  // char temppath[2048];
  // GetTempPathA(sizeof(temppath)-1, temppath);
	// if (FileExist(jklx86))
		// ShellExecute(0, "open", jklx86, NULL, temppath, SW_HIDE);
	// else
		// printf("%s support won't be available...\n", ARCH);
	MSG Msg;
	// ShowWindow(MAIN, nCmdShow);
	// SetForegroundWindow(MAIN);
	// printf("MAIN: %i\n", MAIN);
	SetProcessWorkingSetSize(GetCurrentProcess(), -1, -1);
	if(MAIN) {
		while (GetMessage(&Msg, MAIN, 0, 0) > 0) {
			if(!IsDialogMessage(MAIN, &Msg)) {
				TranslateMessage(&Msg);
				DispatchMessage(&Msg);
			}
		}
	}
	PostQuit();
	return(0);
}