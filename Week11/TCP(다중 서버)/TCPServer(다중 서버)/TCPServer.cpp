#include <winsock2.h>
#include <stdlib.h>
#include <stdio.h>

#define BUFSIZE 512

// ��Ʈ ��ȣ�� �ٲٸ� ���� 2���� ���� �۵� ����

// ���� �Լ� ���� ��� �� ����
void err_quit(char *msg)
{
	LPVOID lpMsgBuf;
	FormatMessage( 
		FORMAT_MESSAGE_ALLOCATE_BUFFER|
		FORMAT_MESSAGE_FROM_SYSTEM,
		NULL, WSAGetLastError(),						// WSAGetLastError() : ���� ��ȣ�� ��ȯ�ϴ� �Լ�
		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
		(LPTSTR)&lpMsgBuf, 0, NULL);					// lpMsgBuf : ������ ���� ������ �������
	MessageBox(NULL, (LPCTSTR)lpMsgBuf, msg, MB_ICONERROR);
	LocalFree(lpMsgBuf);
	exit(-1);
}

// ���� �Լ� ���� ���
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

// ws2_32.lib; : ������Ʈ - �Ӽ� - ���� ��� - ��Ŀ - �Է¿��� ���� ������ �������߸� ���� ����
// ������ �����ϱ� ���ؼ��� ���� �ʱ�ȭ �� ���� �Լ� ����
// TCP ���� : socket - bind - listen - accept - recv - send

int main(int argc, char* argv[])
{
	int retval;

	// ����(������� ���� ���α׷���) �ʱ�ȭ : �޸𸮿� Load
	WSADATA wsa;
	if(WSAStartup(MAKEWORD(2,2), &wsa) != 0)	// WSAStartup(������ ������, WSADATA ����ü�� ������ �ּ�) : ���� �ʱ�ȭ �Լ�, �Լ��� ���������� ����Ǹ� 0�� ��ȯ, �ƴϸ� 0�� �ƴ� �� ��ȯ
		return -1;

	// socket() : ���� ����, ����� ���� ���� �ڷᱸ��, ���� ��ũ���� Ȥ�� ����� ������ ����, �ü���� ����� ���� �����ϴ� �����͸� ���������� ������ �� �ֵ��� ���� ��
	// socket(�ּ� �йи�, ���� Ÿ��, ��������), �ּ� �йи� : ���ͳ��� ����Ѵٸ� AF_INET, ���� Ÿ�� : SOCK_STREAM(TCP ���:������), SOCK_DGRAM(UDP ���:�񿬰���), �������� : 0���� �����ϸ� �׿� �´� �������� ���
	SOCKET listen_sock = socket(AF_INET, SOCK_STREAM, 0);
	if(listen_sock == INVALID_SOCKET) err_quit("socket()");		// ���� ������ ������ ��� INVALID_SOCKET ��ȯ
	
	// bind(����, �ּ�, ũ��) : ���� ����
	SOCKADDR_IN serveraddr;							// SOCKADDR_IN(4���� �μ�) ����ü ����
	ZeroMemory(&serveraddr, sizeof(serveraddr));	// ZeroMemory(0���� ä�� �޸� ���� �ּ�, 0���� ä�� ũ��), �Լ��� �ƴ϶� ��ũ��
	serveraddr.sin_family = AF_INET;				// sin_family : �ּ�ü��(�׻� AF_INET)
	serveraddr.sin_port = htons(8000);				// sin_port : ��Ʈ ��ȣ(���� �� ���ο� ���� ��Ʈ ��ȣ�� ����), htons() : host network short, ����Ʈ ����(�޸𸮿� �����͸� ������ ���� ����Ʈ ����) �� �ص�� ��ķ� ��ȯ�ϴ� �Լ�
	serveraddr.sin_addr.s_addr = htonl(INADDR_ANY);	// sin_addr : ȣ��Ʈ IP�ּ�, s_addr : IP�ּҸ� ������ ����ü, htonl() : host network long, �ּ� ����, INADDR_ANY : ������ IP�ּҸ� �ڵ����� ã�� 32bit ���� Ÿ������ ����
	retval = bind(listen_sock, (SOCKADDR *)&serveraddr, sizeof(serveraddr));	
	if(retval == SOCKET_ERROR) err_quit("bind()");	// bind ������ ��� SOCKET_ERROR(-1) ��ȯ, ������ 0 ��ȯ
	
	// listen(����, ��� ������ �ִ� ���� ����(��α�)) : ���� ��⿭ ����
	retval = listen(listen_sock, SOMAXCONN);				
	if(retval == SOCKET_ERROR) err_quit("listen()");

	// ������ ��ſ� ����� ����
	SOCKET client_sock;
	SOCKADDR_IN clientaddr;		// Client �ּ�
	int addrlen;
	char buf[BUFSIZE + 1];

	while(1){
		// accept(����� ����, Client �ּ�, ����) : ���� ���
		printf("Listening...\n");
		addrlen = sizeof(clientaddr);
		client_sock = accept(listen_sock, (SOCKADDR *)&clientaddr, &addrlen);	// Client�� ���� ������ Client ���Ͽ� ����, 2���� �����ϸ� 1���� accept�� �ӹ�������, accept : ��ǻ� ���⼭ ���ӵǱ� ��ٸ�(block ����)
		if(client_sock == INVALID_SOCKET){			// ���ῡ ������ ��� INVALID_SOCKET ��ȯ
			err_display("accept()");
			continue;
		}
		printf("\n[TCP ����] Ŭ���̾�Ʈ ����: IP �ּ�=%s, ��Ʈ ��ȣ=%d\n", inet_ntoa(clientaddr.sin_addr), ntohs(clientaddr.sin_port));	// ����, inet_ntoa, ntohs : htons�� �ݴ�

		// Ŭ���̾�Ʈ�� ������ ���
		while(1){
			// recv(����, ������ ������, ũ��, flags) : ������ �ޱ�, flags : MSG_DONTWAIT(���� �����Ͱ� ���ٸ� -1 ��ȯ), MSG_NOSIGNAL(����� ������ ������ �� �ñ׳��� ���� ����)
			retval = recv(client_sock, buf, BUFSIZE, 0);			// recv : ������ ������ ��ٸ�(block ����) 
			if(retval == SOCKET_ERROR){								// ������ �ޱ⸦ ������ ��� SOCKET_ERROR(-1) ��ȯ, ���� �� ������ ����Ʈ �� ��ȯ
				err_display("recv()");								
				break;
			}
			else if(retval == 0)									// �ƹ��͵� �޾ƿ��� �ʾ��� ���
				break;

			// ���� ������ ���
			buf[retval] = '\0';
			printf("[TCP/%s:%d] %s\n", inet_ntoa(clientaddr.sin_addr), ntohs(clientaddr.sin_port), buf);

			// send(����, ������ ������, ũ��, flags) : ������ ������, flags : MSG_DONTWAIT(���� �����Ͱ� ���ٸ� -1 ��ȯ), MSG_NOSIGNAL(����� ������ ������ �� �ñ׳��� ���� ����)
			retval = send(client_sock, buf, retval, 0);				
			if(retval == SOCKET_ERROR){								// ������ �����⸦ ������ ��� SOCKET_ERROR(-1) ��ȯ, ���� �� ������ ����Ʈ �� ��ȯ
				err_display("send()");
				break;
			}
		}

		// closesocket(���� �Լ��� ������ ���ϰ�) : ���� �Լ��� ������ ������ �����ϴ� �뵵
		closesocket(client_sock);			
		printf("[TCP ����] Ŭ���̾�Ʈ ����: IP �ּ�=%s, ��Ʈ ��ȣ=%d\n", inet_ntoa(clientaddr.sin_addr), ntohs(clientaddr.sin_port));	// ����
	}

	// closesocket(���� �Լ��� ������ ���ϰ�) : ���� �Լ��� ������ ������ �����ϴ� �뵵
	closesocket(listen_sock);

	// ���� ���� : ������ �׸� ���ڴ�. WSACleanup() <-> WSAStartup()
	WSACleanup();
	return 0;
}