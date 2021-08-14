#include <winsock2.h>
#include <stdlib.h>
#include <stdio.h>
#include <windows.h>

#define BUFSIZE 512
SOCKET listen_sock;

POINT p[1000];
int iCount;

LRESULT CALLBACK WndProc(HWND, UINT, WPARAM, LPARAM);
HINSTANCE g_hInst;
HWND hWndMain;
LPCTSTR lpszClass = TEXT("Client");

int APIENTRY WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpszCmdParam, int nCmdShow)
{
	HWND hWnd;
	MSG Message;
	WNDCLASS WndClass;
	g_hInst = hInstance;

	WndClass.cbClsExtra = 0;
	WndClass.cbWndExtra = 0;
	WndClass.hbrBackground = (HBRUSH)GetStockObject(WHITE_BRUSH);
	WndClass.hCursor = LoadCursor(NULL, IDC_ARROW);
	WndClass.hIcon = LoadIcon(NULL, IDI_APPLICATION);
	WndClass.hInstance = hInstance;
	WndClass.lpfnWndProc = (WNDPROC)WndProc;
	WndClass.lpszClassName = lpszClass;
	WndClass.lpszMenuName = NULL;
	WndClass.style = CS_HREDRAW | CS_VREDRAW;
	RegisterClass(&WndClass);

	hWnd = CreateWindow(lpszClass, lpszClass, WS_OVERLAPPEDWINDOW,
		CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT,
		NULL, (HMENU)NULL, hInstance, NULL);
	ShowWindow(hWnd, nCmdShow);
	hWndMain = hWnd;

	while (GetMessage(&Message, 0, 0, 0)) {
		TranslateMessage(&Message);
		DispatchMessage(&Message);
	}
	return Message.wParam;
}

// 소켓 함수 오류 출력 후 종료
void err_quit(char *msg)
{
	LPVOID lpMsgBuf;
	FormatMessage(
		FORMAT_MESSAGE_ALLOCATE_BUFFER |
		FORMAT_MESSAGE_FROM_SYSTEM,
		NULL, WSAGetLastError(),
		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
		(LPTSTR)&lpMsgBuf, 0, NULL);
	MessageBox(NULL, (LPCTSTR)lpMsgBuf, msg, MB_ICONERROR);
	LocalFree(lpMsgBuf);
	exit(-1);
}

// 소켓 함수 오류 출력
void err_display(char *msg)
{
	LPVOID lpMsgBuf;
	FormatMessage(
		FORMAT_MESSAGE_ALLOCATE_BUFFER |
		FORMAT_MESSAGE_FROM_SYSTEM,
		NULL, WSAGetLastError(),
		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
		(LPTSTR)&lpMsgBuf, 0, NULL);
	MessageBox(NULL, (LPCTSTR)lpMsgBuf, msg, MB_ICONERROR);
	LocalFree(lpMsgBuf);
	exit(-1);
}

DWORD WINAPI ThreadFunc(LPVOID temp) // Server Thread
{
	int retval;

	// 윈속 초기화
	WSADATA wsa;
	if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
		return -1;

	// socket()
	SOCKET listen_sock = socket(AF_INET, SOCK_STREAM, 0);
	if (listen_sock == INVALID_SOCKET) err_quit("socket()");

	// bind()
	SOCKADDR_IN serveraddr;
	ZeroMemory(&serveraddr, sizeof(serveraddr));
	serveraddr.sin_family = AF_INET;
	serveraddr.sin_port = htons(5000);							// 포트 번호 5000을 넘겨받아 수행, 즉 Server에서 그린 사각형을 넘겨받아 그림
	serveraddr.sin_addr.s_addr = htonl(INADDR_ANY);
	retval = bind(listen_sock, (SOCKADDR *)&serveraddr, sizeof(serveraddr));
	if (retval == SOCKET_ERROR) err_quit("bind()");

	// listen()
	retval = listen(listen_sock, SOMAXCONN);
	if (retval == SOCKET_ERROR) err_quit("listen()");

	// 데이터 통신에 사용할 변수
	SOCKET client_sock;
	SOCKADDR_IN clientaddr;
	int addrlen;
	int buf1;
	int buf2;

	while (1){
		// accept()
		addrlen = sizeof(clientaddr);
		client_sock = accept(listen_sock, (SOCKADDR *)&clientaddr, &addrlen);
		if (client_sock == INVALID_SOCKET){
			err_display("accept()");
			continue;
		}
		printf("\n[TCP 서버] 클라이언트 접속: IP 주소=%s, 포트 번호=%d\n", inet_ntoa(clientaddr.sin_addr), ntohs(clientaddr.sin_port));

		// 클라이언트와 데이터 통신
		while (1){
			// 데이터 받기
			retval = recv(client_sock, (char *)&buf1, sizeof(int), 0);
			retval = recv(client_sock, (char *)&buf2, sizeof(int), 0);
			if (retval == SOCKET_ERROR){
				err_display("recv()");
				break;
			}
			else if (retval == 0)
				break;

			p[iCount].x = buf1;
			p[iCount++].y = buf2;
			InvalidateRect(hWndMain, NULL, TRUE);
		}

		// closesocket()
		closesocket(client_sock);
		printf("[TCP 서버] 클라이언트 종료: IP 주소=%s, 포트 번호=%d\n", inet_ntoa(clientaddr.sin_addr), ntohs(clientaddr.sin_port));
	}

	// closesocket()
	closesocket(listen_sock);

	// 윈속 종료
	WSACleanup();
	return 0;
}

SOCKET sock;
bool isConnect;

LRESULT CALLBACK WndProc(HWND hWnd, UINT iMessage, WPARAM wParam, LPARAM lParam)
{
	DWORD ThreadID;
	HANDLE hThread;

	HDC hdc;
	PAINTSTRUCT ps;

	int x, y;
	int retval;
	
	HBRUSH hBrush, oldBrush;

	switch (iMessage) {
	case WM_CREATE:
		hThread = CreateThread(NULL, 0, ThreadFunc, NULL, 0, &ThreadID); // Client Thread Start
		CloseHandle(hThread);

		CreateWindow(TEXT("button"), TEXT("Connect"), WS_CHILD | WS_VISIBLE | BS_PUSHBUTTON, 20, 20, 100, 25, hWnd, (HMENU)0, g_hInst, NULL);
		isConnect = false;
		return 0;

	case WM_COMMAND:
		switch (LOWORD(wParam)) {
		case 0:
			// 윈속 초기화
			WSADATA wsa;
			if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
				return -1;
			// socket()
			sock = socket(AF_INET, SOCK_STREAM, 0);
			if (sock == INVALID_SOCKET) err_quit("socket()");
			// connect()
			SOCKADDR_IN serveraddr;
			ZeroMemory(&serveraddr, sizeof(serveraddr));
			serveraddr.sin_family = AF_INET;
			serveraddr.sin_port = htons(6000);						// 포트 번호 6000을 넘겨줌, 즉 Client에서 그린 사각형을 넘겨줌
			serveraddr.sin_addr.s_addr = inet_addr("127.0.0.1");
			retval = connect(sock, (SOCKADDR *)&serveraddr, sizeof(serveraddr));

			if (retval == SOCKET_ERROR) {
				MessageBox(hWnd, TEXT("CONNECT Fail!"), TEXT("MeesageBox"), MB_OK);
				err_quit("connect()");
			}
			else {
				EnableWindow(GetDlgItem(hWnd, 0), false);
				isConnect = true;
				MessageBox(hWnd, TEXT("CONNECT!"), TEXT("MessageBox"), MB_OK);
			}
			break;
		}
		return 0;

	case WM_LBUTTONDOWN:
		if (isConnect) {
			x = LOWORD(lParam);
			y = HIWORD(lParam);
			p[iCount].x = LOWORD(lParam);
			p[iCount++].y = HIWORD(lParam);
			retval = send(sock, (char *)&x, sizeof(int), 0);
			retval = send(sock, (char *)&y, sizeof(int), 0);
			if (retval == SOCKET_ERROR){
				err_display("send()");
				return 0;
			}
			hdc = GetDC(hWnd);
			hBrush = CreateSolidBrush(RGB(255, 0, 0));
			oldBrush = (HBRUSH)SelectObject(hdc, hBrush);
			Rectangle(hdc, x - 8, y - 8, x + 8, y + 8);
			SelectObject(hdc, oldBrush);
			ReleaseDC(hWnd, hdc);
		}
		else
		{
			x = LOWORD(lParam);
			y = HIWORD(lParam);
			p[iCount].x = LOWORD(lParam);
			p[iCount++].y = HIWORD(lParam);
			hdc = GetDC(hWnd);
			hBrush = CreateSolidBrush(RGB(255, 0, 0));
			oldBrush = (HBRUSH)SelectObject(hdc, hBrush);
			Rectangle(hdc, x - 8, y - 8, x + 8, y + 8);
			SelectObject(hdc, oldBrush);
			ReleaseDC(hWnd, hdc);
		}
		return 0;
	
	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);
		hBrush = CreateSolidBrush(RGB(255, 0, 0));
		oldBrush = (HBRUSH)SelectObject(hdc, hBrush);
		for (int i = 0; i < iCount; i++)
			Rectangle(hdc, p[i].x - 8, p[i].y - 8, p[i].x + 8, p[i].y + 8);
		SelectObject(hdc, oldBrush);
		DeleteObject(hBrush);
		EndPaint(hWnd, &ps);
		return 0;

	case WM_DESTROY:
		closesocket(sock);
		// 윈속 종료
		WSACleanup();
		PostQuitMessage(0);
		return 0;
	}
	return(DefWindowProc(hWnd, iMessage, wParam, lParam));
}