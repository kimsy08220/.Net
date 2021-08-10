/*
 * helloworld_server_win.c
 * Written by SW. YOON
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <winsock2.h>

void ErrorHandling(char *message);

// 1) 프로그램 폴더 - Debug - 검색창(cmd)
// 2) 프로젝트 - 속성 - 구성 속성 - 디버깅 - 명령 인수 - 포트번호 : 1234

// args -> 2000 -> server.exe
//		-> 3000 -> 8888

int main(int argc, char **argv)			// main 함수에서 명령행 인자를 전달 가능, cmd 실행 후 server.exe 8888이라고 치면 argv를 통해 포트번호 8888가 오게 된다
{
	WSADATA	wsaData;
	SOCKET hServSock;		
	SOCKET hClntSock;		
	SOCKADDR_IN servAddr;	
	SOCKADDR_IN clntAddr;	
	unsigned char *p;
	int szClntAddr;
	char message[]="Hello World!\n";

	if(argc!=2){						// 오류 출력
		printf("Usage : %s <port>\n", argv[0]);		
		exit(1);
	}
  
	if(WSAStartup(MAKEWORD(2, 2), &wsaData) != 0) /* Load Winsock 2.2 DLL */
		ErrorHandling("WSAStartup() error!"); 
	
	hServSock=socket(PF_INET, SOCK_STREAM, 0); /* 서버 소켓 생성 */
	if(hServSock==INVALID_SOCKET)
		ErrorHandling("socket() error");
  
	memset(&servAddr, 0, sizeof(servAddr));
	servAddr.sin_family=AF_INET;
	servAddr.sin_addr.s_addr=htonl(INADDR_ANY);
	servAddr.sin_port=htons(atoi(argv[1]));			// argv[1]에 8888이 들어감
	
	if(bind(hServSock, (SOCKADDR*) &servAddr, sizeof(servAddr))==SOCKET_ERROR) /* 소켓에 주소 할당 */
		ErrorHandling("bind() error");  
	
	if(listen(hServSock, 5)==SOCKET_ERROR) /* 연결 요청 대기 상태 */
		ErrorHandling("listen() error");

	szClntAddr=sizeof(clntAddr);    	
	hClntSock=accept(hServSock, (SOCKADDR*)&clntAddr,&szClntAddr); /* 연결 요청 수락 */
	if(hClntSock==INVALID_SOCKET)
		ErrorHandling("accept() error");  
	p=&(clntAddr.sin_addr.S_un.S_un_b.s_b1);
	printf("Connected Client from IP : %d.%d.%d.%d\n", *p, *(p+1), *(p+2), *(p+3));
	send(hClntSock, message, sizeof(message), 0); /* 데이터 전송 */

	closesocket(hClntSock);		/* 연결 종료 */
	WSACleanup();
	return 0;
}

void ErrorHandling(char *message)
{
	fputs(message, stderr);
	fputc('\n', stderr);
	exit(1);
}
