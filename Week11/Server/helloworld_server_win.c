/*
 * helloworld_server_win.c
 * Written by SW. YOON
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <winsock2.h>

void ErrorHandling(char *message);

// 1) ���α׷� ���� - Debug - �˻�â(cmd)
// 2) ������Ʈ - �Ӽ� - ���� �Ӽ� - ����� - ���� �μ� - ��Ʈ��ȣ : 1234

// args -> 2000 -> server.exe
//		-> 3000 -> 8888

int main(int argc, char **argv)			// main �Լ����� ������ ���ڸ� ���� ����, cmd ���� �� server.exe 8888�̶�� ġ�� argv�� ���� ��Ʈ��ȣ 8888�� ���� �ȴ�
{
	WSADATA	wsaData;
	SOCKET hServSock;		
	SOCKET hClntSock;		
	SOCKADDR_IN servAddr;	
	SOCKADDR_IN clntAddr;	
	unsigned char *p;
	int szClntAddr;
	char message[]="Hello World!\n";

	if(argc!=2){						// ���� ���
		printf("Usage : %s <port>\n", argv[0]);		
		exit(1);
	}
  
	if(WSAStartup(MAKEWORD(2, 2), &wsaData) != 0) /* Load Winsock 2.2 DLL */
		ErrorHandling("WSAStartup() error!"); 
	
	hServSock=socket(PF_INET, SOCK_STREAM, 0); /* ���� ���� ���� */
	if(hServSock==INVALID_SOCKET)
		ErrorHandling("socket() error");
  
	memset(&servAddr, 0, sizeof(servAddr));
	servAddr.sin_family=AF_INET;
	servAddr.sin_addr.s_addr=htonl(INADDR_ANY);
	servAddr.sin_port=htons(atoi(argv[1]));			// argv[1]�� 8888�� ��
	
	if(bind(hServSock, (SOCKADDR*) &servAddr, sizeof(servAddr))==SOCKET_ERROR) /* ���Ͽ� �ּ� �Ҵ� */
		ErrorHandling("bind() error");  
	
	if(listen(hServSock, 5)==SOCKET_ERROR) /* ���� ��û ��� ���� */
		ErrorHandling("listen() error");

	szClntAddr=sizeof(clntAddr);    	
	hClntSock=accept(hServSock, (SOCKADDR*)&clntAddr,&szClntAddr); /* ���� ��û ���� */
	if(hClntSock==INVALID_SOCKET)
		ErrorHandling("accept() error");  
	p=&(clntAddr.sin_addr.S_un.S_un_b.s_b1);
	printf("Connected Client from IP : %d.%d.%d.%d\n", *p, *(p+1), *(p+2), *(p+3));
	send(hClntSock, message, sizeof(message), 0); /* ������ ���� */

	closesocket(hClntSock);		/* ���� ���� */
	WSACleanup();
	return 0;
}

void ErrorHandling(char *message)
{
	fputs(message, stderr);
	fputc('\n', stderr);
	exit(1);
}