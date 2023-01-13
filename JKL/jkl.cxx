#include <windows.h>
#include <stdio.h>
#include "jklarch.h"
 
HHOOK global;
UINT uMSG;
HINSTANCE hInst;
 
static LRESULT CALLBACK procedure(int nCode, WPARAM wParam, LPARAM lParam){
    if (nCode == HC_ACTION){
        CWPSTRUCT *data = (CWPSTRUCT*)lParam;
        if (data->message == WM_INPUTLANGCHANGE) {
			HWND Server = FindWindow(NULL, "777_MAHOU_777_HIDDEN_HWND_SERVER");
			PostMessage(Server, uMSG, data->wParam, data->lParam);
        }
    }
    return CallNextHookEx(global, nCode, wParam, lParam); 
}
// Export alike dll function, that can be called from dll.
extern "C" __declspec(dllexport) BOOL setHook(HWND hWnd) {
	global = SetWindowsHookEx(WH_CALLWNDPROC, procedure, hInst, 0);
	return global != NULL;
}
extern "C" __declspec(dllexport) BOOL unHook() {
	return UnhookWindowsHookEx(global);
}
extern "C" __declspec(dllexport) UINT getUMsg() {
	return uMSG;
}
BOOL APIENTRY DllMain (HINSTANCE hInstance, DWORD  Reason, LPVOID Reserved) {
	switch(Reason) { 
		case DLL_PROCESS_ATTACH:
			uMSG = RegisterWindowMessage(JKL_LAYOUTCHANGE); // Message for layout change
			hInst = hInstance;
			return TRUE;
		case DLL_PROCESS_DETACH:
			unHook();
			FreeLibrary(hInstance);
			return TRUE;
	}
	return TRUE;
}