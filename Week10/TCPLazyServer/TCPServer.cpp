#include <winsock2.h>
#include <stdlib.h>
#include <stdio.h>

#define BUFSIZE 512

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
	printf("[%s] %s", msg, (LPCTSTR)lpMsgBuf);
	LocalFree(lpMsgBuf);
}

// 소켓을 생성하기 위해서는 윈속 초기화 후 소켓 함수 생성
// TCP Lazy 서버 : socket - bind - listen - accept - recv - recv - send

int main(int argc, char* argv[])
{
	int retval;
	int totretval;

	// 윈속(윈도우용 소켓 프로그래밍) 초기화
	WSADATA wsa;
	if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0)		// WSAStartup(윈속의 버전값, WSADATA 구조체의 포인터 주소) : 윈속 초기화 함수, 함수가 정상적으로 수행되면 0을 반환, 아니면 0이 아닌 값 반환
		return -1;

	// socket() : 소켓 생성, 통신을 위한 작은 자료구조, 파일 디스크립터 혹은 랜들과 유사한 개념, 운영체제가 통신을 위해 관리하는 데이터를 간접적으로 참조할 수 있도록 만든 것
	// socket(주소 패밀리, 소켓 타입, 프로토콜), 주소 패밀리 : 인터넷을 사용한다면 AF_INET, 소켓 타입 : SOCK_STREAM(TCP 방식), SOCK_DGRAM(UDP 방식), 프로토콜 : 0으로 지정하면 그에 맞는 프로토콜 사용
	SOCKET listen_sock = socket(AF_INET, SOCK_STREAM, 0);
	if (listen_sock == INVALID_SOCKET) err_quit("socket()");	// 소켓 생성에 실패한 경우 INVALID_SOCKET 반환

	// bind(소켓, 주소, 크기) : 소켓 설정
	SOCKADDR_IN serveraddr;							// SOCKADDR_IN 구조체에는 4개의 인수가 있음
	ZeroMemory(&serveraddr, sizeof(serveraddr));	// ZeroMemory(0으로 채울 메모리 시작 주소, 0으로 채울 크기), 함수가 아니라 매크로
	serveraddr.sin_family = AF_INET;				// sin_family : 주소체계(항상 AF_INET)
	serveraddr.sin_port = htons(9000);				// sin_port : 포트 번호
	serveraddr.sin_addr.s_addr = htonl(INADDR_ANY);	// sin_addr : 호스트 IP주소, s_addr : IP주소를 저장할 구조체, htonl() : 주소 지정, INADDR_ANY : 서버의 IP주소를 자동으로 찾아 대입
	retval = bind(listen_sock, (SOCKADDR *)&serveraddr, sizeof(serveraddr));
	if (retval == SOCKET_ERROR) err_quit("bind()");	// bind 실패한 경우 SOCKET_ERROR(-1) 반환, 성공시 0 반환

	// listen(소켓, 대기 가능한 최대 연결 갯수(백로그)) : 수신 대기열 생성
	retval = listen(listen_sock, SOMAXCONN);
	if (retval == SOCKET_ERROR) err_quit("listen()");

	// 데이터 통신에 사용할 변수
	SOCKET client_sock;
	SOCKADDR_IN clientaddr;		// Client 주소
	int addrlen;
	char buf1[BUFSIZE*2 + 1];
	char buf2[BUFSIZE + 1];

	while (1){
		// accept(연결된 소켓, Client 주소, 길이) : 연결 대기
		addrlen = sizeof(clientaddr);
		client_sock = accept(listen_sock, (SOCKADDR *)&clientaddr, &addrlen);	// Client에 대한 정보를 Client 소켓에 넣음, 2명이 접속하면 1명은 accept에 머물러있음, accept : 사실상 여기서 접속되길 기다림(block 상태)
		if (client_sock == INVALID_SOCKET){			// 연결에 실패한 경우 INVALID_SOCKET 반환
			err_display("accept()");
			continue;
		}
		printf("\n[TCP 서버] 클라이언트 접속: IP 주소=%s, 포트 번호=%d\n", inet_ntoa(clientaddr.sin_addr), ntohs(clientaddr.sin_port));

		// 클라이언트와 데이터 통신
		while (1){
			// 데이터 받기1 : recv(소켓, 수신할 데이터, 크기, flags), flags : MSG_DONTWAIT(수신 데이터가 없다면 -1 반환), MSG_NOSIGNAL(상대방과 연결이 끊겼을 때 시그널을 받지 않음)
			retval = recv(client_sock, buf1, BUFSIZE, 0);		// recv : 데이터 보내길 기다림(block 상태) 
			if (retval == SOCKET_ERROR){						// 데이터 받기를 실패한 경우 SOCKET_ERROR(-1) 반환, 성공 시 수신한 바이트 수 반환
				err_display("recv()");
				break;
			}
			else if (retval == 0)								// 아무것도 받아오지 않았을 경우
				break;

			totretval = retval;

			// 받은 데이터 출력1
			buf1[retval] = '\0';
			printf("[TCP/%s:%d | data1] %s\n", inet_ntoa(clientaddr.sin_addr), ntohs(clientaddr.sin_port), buf1);


			// 데이터 받기2 : recv(소켓, 수신할 데이터, 크기, flags), flags : MSG_DONTWAIT(수신 데이터가 없다면 -1 반환), MSG_NOSIGNAL(상대방과 연결이 끊겼을 때 시그널을 받지 않음)
			retval = recv(client_sock, buf2, BUFSIZE, 0);		// recv : 데이터 보내길 기다림(block 상태) 
			if (retval == SOCKET_ERROR){						// 데이터 받기를 실패한 경우 SOCKET_ERROR(-1) 반환, 성공 시 수신한 바이트 수 반환
				err_display("recv()");
				break;
			}
			else if (retval == 0)								// 아무것도 받아오지 않았을 경우
				break;

			// 받은 데이터 출력2
			buf2[retval] = '\0';								// 0으로 초기화
			printf("[TCP/%s:%d | data2] %s\n", inet_ntoa(clientaddr.sin_addr), ntohs(clientaddr.sin_port), buf2);

			totretval += retval;

			// send(소켓, 전송할 데이터, 크기, flags) : 데이터 보내기(1과 2를 연결해서 Client에게 전달), flags : MSG_DONTWAIT(수신 데이터가 없다면 -1 반환), MSG_NOSIGNAL(상대방과 연결이 끊겼을 때 시그널을 받지 않음)
			strcat(buf1, buf2);									// strcat() : 문자열 연결 함수
			buf1[totretval+1] = '\0';
			retval = send(client_sock, buf1, totretval, 0);		// 데이터 보내기를 실패한 경우 SOCKET_ERROR(-1) 반환, 성공 시 전송한 바이트 수 반환
			if (retval == SOCKET_ERROR){
				err_display("send()");
				break;
			}
		}

		// closesocket(소켓 함수가 리턴한 소켓값) : 소켓 함수로 생성한 소켓을 제거하는 용도
		closesocket(client_sock);
		printf("[TCP 서버] 클라이언트 종료: IP 주소=%s, 포트 번호=%d\n", inet_ntoa(clientaddr.sin_addr), ntohs(clientaddr.sin_port));
	}

	// closesocket(소켓 함수가 리턴한 소켓값) : 소켓 함수로 생성한 소켓을 제거하는 용도
	closesocket(listen_sock);

	// 윈속 종료 : 소켓을 그만 쓰겠다. WSACleanup() <-> WSAStartup()
	WSACleanup();
	return 0;
}