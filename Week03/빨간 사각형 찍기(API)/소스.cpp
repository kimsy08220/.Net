#include <windows.h>

LRESULT CALLBACK WndProcedure(HWND, UINT, WPARAM, LPARAM);
HINSTANCE g_hInst;
HWND hWndMain;
LPCTSTR lpszClass = TEXT("Class");

int APIENTRY WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpszCmdParam, int nCmdShow)
{
	HWND hWnd;
	MSG Message;
	WNDCLASS WndClass;
	g_hInst = hInstance;

	WndClass.cbClsExtra = 0;
	WndClass.cbWndExtra = 0;
	WndClass.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
	WndClass.hCursor = LoadCursor(NULL, IDC_ARROW);
	WndClass.hIcon = LoadIcon(NULL, IDI_APPLICATION);
	WndClass.hInstance = hInstance;
	WndClass.lpfnWndProc = WndProcedure;
	WndClass.lpszClassName = lpszClass;
	WndClass.lpszMenuName = NULL;
	WndClass.style = CS_HREDRAW | CS_VREDRAW;
	RegisterClass(&WndClass);

	hWnd = CreateWindow(lpszClass, lpszClass, WS_OVERLAPPEDWINDOW,
		CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT,
		NULL, (HMENU)NULL, hInstance, NULL);
	ShowWindow(hWnd, nCmdShow);

	while (GetMessage(&Message, NULL, 0, 0)) {
		TranslateMessage(&Message);
		DispatchMessage(&Message);
	}
	return (int)Message.wParam;
}

#include "points.h"		// Code reusability 코드 재사용
mypoints points;		// 정의한 클래스의 (instance, object) 선언

LRESULT CALLBACK WndProcedure(HWND hWnd, UINT iMessage, WPARAM wParam, LPARAM lParam)
{ 
	HDC hdc;
	PAINTSTRUCT ps;
	HBRUSH hBrush;
	int x, y;
	
	switch (iMessage) {
	case WM_CREATE :		// WM_CREATE == 0x0001
		hWndMain = hWnd;
		return 0;
	case WM_LBUTTONDOWN:	// 왼쪽 버튼을 눌렀을 때
		hdc = GetDC(hWnd);
		x = LOWORD(lParam);	// Macro
		y = HIWORD(lParam);
		points.Add(x, y);
		InvalidateRect(hWnd, NULL, FALSE);		// hWnd를 무효화하고 그려야 하는 부분만 갱신
		return 0;
	
	case WM_PAINT:			// 화면을 다시 그려라 os가 보내는 MSG
		hdc = BeginPaint(hWnd, &ps);   
		points.Draw(hdc);
		EndPaint(hWnd, &ps);	// 화면복구 routine
		return 0;
	case WM_DESTROY:
		PostQuitMessage(0);
		return 0;
	}
	return(DefWindowProc(hWnd, iMessage, wParam, lParam));
}
