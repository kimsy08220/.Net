#include <winsock2.h>
#include <stdlib.h>
#include <stdio.h>

#define BUFSIZE 512

// 소켓 함수 오류 출력 후 종료
void err_quit(char *msg)
{
	LPVOID lpMsgBuf;
	FormatMessage( 
		FORMAT_MESSAGE_ALLOCATE_BUFFER|
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
		FORMAT_MESSAGE_ALLOCATE_BUFFER|
		FORMAT_MESSAGE_FROM_SYSTEM,
		NULL, WSAGetLastError(),
		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
		(LPTSTR)&lpMsgBuf, 0, NULL);
	printf("[%s] %s", msg, (LPCTSTR)lpMsgBuf);
	LocalFree(lpMsgBuf);
}

// 사용자 정의 데이터 수신 함수
int recvn(SOCKET s, char *buf, int len, int flags)
{
	int received;
	char *ptr = buf;
	int left = len;

	while(left > 0){
		received = recv(s, ptr, left, flags);
		if(received == SOCKET_ERROR) 
			return SOCKET_ERROR;
		else if(received == 0) 
			break;
		left -= received;
		ptr += received;
	}

	return (len - left);
}

// 동기(blocking, 보내고 올때까지 기다림), 비동기(non-blocking) : 개념만 알아라

// 43p
// 소켓을 생성하기 위해서는 윈속 초기화 후 소켓 함수 생성
// TCP 클라이언트 : socket - connect - send - recv

// closed port : 18, 19, 20, 22, 78, 79, 81, 82
// opened port : 21, 80

int main(int argc, char* argv[])
{
	int retval;

	// 윈속(윈도우용 소켓 프로그래밍) 초기화
	WSADATA wsa;
	if(WSAStartup(MAKEWORD(2,2), &wsa) != 0)			// WSAStartup(윈속의 버전값, WSADATA 구조체의 포인터 주소) : 윈속 초기화 함수, 함수가 정상적으로 수행되면 0을 반환, 아니면 0이 아닌 값 반환
		return -1;

	// socket() : 소켓 생성, 통신을 위한 작은 자료구조, 파일 디스크립터 혹은 랜들과 유사한 개념, 운영체제가 통신을 위해 관리하는 데이터를 간접적으로 참조할 수 있도록 만든 것
	// socket(주소 패밀리, 소켓 타입, 프로토콜), 주소 패밀리 : 인터넷을 사용한다면 AF_INET, 소켓 타입 : SOCK_STREAM(TCP 방식), SOCK_DGRAM(UDP 방식), 프로토콜 : 0으로 지정하면 그에 맞는 프로토콜 사용
	SOCKET sock = socket(AF_INET, SOCK_STREAM, 0);
	if(sock == INVALID_SOCKET) err_quit("socket()");	// 소켓 생성에 실패한 경우 INVALID_SOCKET 반환
	
	//for (int i = atoi(argv[2]); i <= atoi(argv[3]); i++) {
		// connect(소켓, 연결 요청을 보낼 서버의 주소, 크기) : 연결 요청
		SOCKADDR_IN serveraddr;								// SOCKADDR_IN 구조체에는 4개의 인수가 있음
		ZeroMemory(&serveraddr, sizeof(serveraddr));		// ZeroMemory(0으로 채울 메모리 시작 주소, 0으로 채울 크기), 함수가 아니라 매크로
		serveraddr.sin_family = AF_INET;					// sin_family : 주소체계(항상 AF_INET)
		serveraddr.sin_port = htons(82);					// sin_port : 포트 번호(접속 시 새로운 소켓 포트 번호가 생성), htons() : short integer 데이터를 네트워크 byte order
		serveraddr.sin_addr.s_addr = inet_addr("203.241.228.120");				// argv[1], sin_addr : 호스트 IP주소, s_addr : IP주소를 저장할 구조체, inet_addr() : 문자열 형태를 정수 형태로 변환, 127.0.0.1 : 자기 네트워크, 192.168.51.206 : 교수님 네트워크
		retval = connect(sock, (SOCKADDR *)&serveraddr, sizeof(serveraddr));	// connect : 주소로 접속(데이터를 주고받을 준비가 끝남), serveraddr와 연결한 것을 retval에 저장

		// 열려있으면 바로 opened port를 보내지만 닫혀있으면 시간이 오래 걸림
		if (retval == SOCKET_ERROR) 
			printf("%s\n", "closed port");
		else
			printf("%s\n", "opened port");
	//}
}