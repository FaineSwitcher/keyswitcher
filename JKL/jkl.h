#include <windows.h>
#include "jklarch.h"

typedef BOOL (*FUNC)();
typedef UINT (*UFUNC)();

HWND CreateHWND(HINSTANCE hInstance, CHAR* className, WNDPROC WndProc) {
	WNDCLASSEX wc;
    wc.cbSize        = sizeof(WNDCLASSEX);
    wc.style         = 0;
    wc.lpfnWndProc   = WndProc;
    wc.cbClsExtra    = 0;
    wc.cbWndExtra    = 0;
    wc.hInstance     = hInstance;
    wc.hIcon         = LoadIcon(NULL, IDI_APPLICATION);
    wc.hCursor       = LoadCursor(NULL, IDC_ARROW);
    wc.hbrBackground = (HBRUSH)(COLOR_WINDOW+1);
    wc.lpszMenuName  = NULL;
    wc.lpszClassName = className;
    wc.hIconSm       = LoadIcon(NULL, IDI_APPLICATION);

    if(!RegisterClassEx(&wc)) {
		CHAR buf[60];
		sprintf(buf, "Window Registration Failed for [%s].", className);
        MessageBox(NULL, buf, "Error!", MB_ICONEXCLAMATION | MB_OK);
    }
    return CreateWindowEx(0, className, className, WS_OVERLAPPEDWINDOW, 
						  CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, 
						  CW_USEDEFAULT, NULL, NULL, (HINSTANCE) hInstance, NULL);
}

BOOL FileExist(CHAR* path) {
	DWORD _fa = GetFileAttributes(path);
	return !(INVALID_FILE_ATTRIBUTES == _fa && GetLastError()==ERROR_FILE_NOT_FOUND);
}