/*
 * chat_server_win.c
 * Written by SW. YOON
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <winsock2.h>
#include <process.h>

#define BUFSIZE 100

DWORD WINAPI ClientConn(void *arg);
void SendMSG(char* message, int len);
void ErrorHandling(char *message);

int clntNumber = 0;
SOCKET clntSocks[10];
HANDLE hMutex;

int main(int argc, char **argv)
{
	WSADATA wsaData;
	SOCKET servSock;
	SOCKET clntSock;

	SOCKADDR_IN servAddr;
	SOCKADDR_IN clntAddr;
	int clntAddrSize;

	HANDLE hThread;
	DWORD dwThreadID;

	if (argc != 2){
		printf("Usage : %s <port>\n", argv[0]);
		exit(1);
	}
	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0) /* Load Winsock 2.2 DLL */
		ErrorHandling("WSAStartup() error!");

	hMutex = CreateMutex(NULL, FALSE, NULL);
	if (hMutex == NULL){
		ErrorHandling("CreateMutex() error");
	}

	servSock = socket(PF_INET, SOCK_STREAM, 0);
	if (servSock == INVALID_SOCKET)
		ErrorHandling("socket() error");

	memset(&servAddr, 0, sizeof(servAddr));
	servAddr.sin_family = AF_INET;
	servAddr.sin_addr.s_addr = htonl(INADDR_ANY);
	servAddr.sin_port = htons(atoi(argv[1]));

	// bind()
	if (bind(servSock, (SOCKADDR*)&servAddr, sizeof(servAddr)) == SOCKET_ERROR)
		ErrorHandling("bind() error");

	// listen()
	if (listen(servSock, 5) == SOCKET_ERROR)
		ErrorHandling("listen() error");

	while (1){
		// accept()
		clntAddrSize = sizeof(clntAddr);
		clntSock = accept(servSock, (SOCKADDR*)&clntAddr, &clntAddrSize);
		if (clntSock == INVALID_SOCKET)
			ErrorHandling("accept() error");

		// ����ȭ
		WaitForSingleObject(hMutex, INFINITE);
		clntSocks[clntNumber++] = clntSock;					// clntSocks : ����ڵ鳢���� �迭
		ReleaseMutex(hMutex);
		printf("���ο� ����, Ŭ���̾�Ʈ IP : %s \n", inet_ntoa(clntAddr.sin_addr));

		// ������ ����, _beginthreadex : createThread�� ȣ��
		hThread = (HANDLE)_beginthreadex(NULL, 0, ClientConn, (void*)clntSock, 0, (unsigned *)&dwThreadID);		
		if (hThread == 0) {
			ErrorHandling("������ ���� ����");
		}
	}

	WSACleanup();
	return 0;
}

DWORD WINAPI ClientConn(void *arg)
{
	SOCKET clntSock = (SOCKET)arg;
	int strLen = 0;
	char message[BUFSIZE];
	int i;

	// recv(), SendMSG()
	while ((strLen = recv(clntSock, message, BUFSIZE, 0)) != 0)
		SendMSG(message, strLen);								// SendMSG() : ����ڵ鿡�� ���� �޼����� �� ����

	// �����ϸ� ����ڵ� �迭���� �ش� ����ڸ� ����
	WaitForSingleObject(hMutex, INFINITE);
	for (i = 0; i < clntNumber; i++){							// Ŭ���̾�Ʈ ���� �����
		if (clntSock == clntSocks[i]){
			for (; i < clntNumber - 1; i++)
				clntSocks[i] = clntSocks[i + 1];
			break;
		}
	}
	clntNumber--;
	ReleaseMutex(hMutex);

	closesocket(clntSock);
	return 0;
}

void SendMSG(char* message, int len)
{
	int i;
	// �������� ������ ����
	WaitForSingleObject(hMutex, INFINITE);
	for (i = 0; i < clntNumber; i++)
		send(clntSocks[i], message, len, 0);
	ReleaseMutex(hMutex);
}

void ErrorHandling(char *message)
{
	fputs(message, stderr);
	fputc('\n', stderr);
	exit(1);
}
