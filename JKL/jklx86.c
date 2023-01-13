#include <windows.h>
#include <stdio.h>
#include <pthread.h>
#include <unistd.h>
#include "jkl.h"

HWND MAIN;
HMODULE lib32;
UINT uMSG;

void PostQuit() {
	FUNC unHook32 = (FUNC)GetProcAddress(lib32, "unHook");
	unHook32();
    FreeLibrary(lib32);
}

LRESULT CALLBACK WndProc(HWND hWnd, UINT Msg, WPARAM wParam, LPARAM lParam) {
	if (Msg == WM_QUIT || Msg == WM_DESTROY)
		PostQuit();
	return DefWindowProc(hWnd, Msg, wParam, lParam);
}
// Checks if x64 version of jkl alive every 2s, if not kills self.
void *Is64Alive() {
	while(1) {
		sleep(2);
		HWND X64 = FindWindow("_HIDDEN_HWND_SERVER", NULL);
		HWND Mahou = FindWindow(NULL, "777_MAHOU_777_HIDDEN_HWND_SERVER");
		if (!Mahou) {
			printf("Mahou is not with us...");
			PostMessage(X64, WM_QUIT, 0, 0);
			printf("I'm sleepy... %i ", X64);
			if (!X64) {
				printf("Gone sleeping...");
				PostMessage(MAIN, WM_QUIT, 0, 0);
			}
		}
	}
}
// Setts pthread with a task(as a timer) to keep checking if x64 version is alive, on separate thread.
void SetTimerThread() {
	pthread_t alive_x64;
	if(pthread_create(&alive_x64, NULL, Is64Alive, NULL)) {
		fprintf(stderr, "Error creating timer thread...\n");
	}
}

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR nCmdLine, int nCmdShow) {
	if (FindWindow("_HIDDEN_X86_HELPER", NULL))
		exit(0); 
	MAIN = CreateHWND(hInstance, "_HIDDEN_X86_HELPER", (WNDPROC)WndProc);
    lib32 = LoadLibrary("jklx86.dll");
	SetTimerThread();
  UFUNC getUMsg = (UFUNC)GetProcAddress(lib32, "getUMsg");
  uMSG = getUMsg();
	FUNC setHook32 = (FUNC)GetProcAddress(lib32, "setHook");
	setHook32();
	MSG Msg;
  FILE* fp;
  fp = fopen("umsg32.id", "w");
  if(fp == NULL)
    exit(-1);
  fprintf(fp, "%i", uMSG);
  fclose(fp);
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