#include <winsock2.h>
#include <stdlib.h>
#include <stdio.h>

#define BUFSIZE 512

// ���� �Լ� ���� ��� �� ����
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

// ����� ���� ������ ���� �Լ�
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

// ����(blocking, ������ �ö����� ��ٸ�), �񵿱�(non-blocking) : ���丸 �˾ƶ�

// 43p
// ������ �����ϱ� ���ؼ��� ���� �ʱ�ȭ �� ���� �Լ� ����
// TCP Ŭ���̾�Ʈ : socket - connect - send - recv

// closed port : 18, 19, 20, 22, 78, 79, 81, 82
// opened port : 21, 80

int main(int argc, char* argv[])
{
	int retval;

	// ����(������� ���� ���α׷���) �ʱ�ȭ
	WSADATA wsa;
	if(WSAStartup(MAKEWORD(2,2), &wsa) != 0)			// WSAStartup(������ ������, WSADATA ����ü�� ������ �ּ�) : ���� �ʱ�ȭ �Լ�, �Լ��� ���������� ����Ǹ� 0�� ��ȯ, �ƴϸ� 0�� �ƴ� �� ��ȯ
		return -1;

	// socket() : ���� ����, ����� ���� ���� �ڷᱸ��, ���� ��ũ���� Ȥ�� ����� ������ ����, �ü���� ����� ���� �����ϴ� �����͸� ���������� ������ �� �ֵ��� ���� ��
	// socket(�ּ� �йи�, ���� Ÿ��, ��������), �ּ� �йи� : ���ͳ��� ����Ѵٸ� AF_INET, ���� Ÿ�� : SOCK_STREAM(TCP ���), SOCK_DGRAM(UDP ���), �������� : 0���� �����ϸ� �׿� �´� �������� ���
	SOCKET sock = socket(AF_INET, SOCK_STREAM, 0);
	if(sock == INVALID_SOCKET) err_quit("socket()");	// ���� ������ ������ ��� INVALID_SOCKET ��ȯ
	
	//for (int i = atoi(argv[2]); i <= atoi(argv[3]); i++) {
		// connect(����, ���� ��û�� ���� ������ �ּ�, ũ��) : ���� ��û
		SOCKADDR_IN serveraddr;								// SOCKADDR_IN ����ü���� 4���� �μ��� ����
		ZeroMemory(&serveraddr, sizeof(serveraddr));		// ZeroMemory(0���� ä�� �޸� ���� �ּ�, 0���� ä�� ũ��), �Լ��� �ƴ϶� ��ũ��
		serveraddr.sin_family = AF_INET;					// sin_family : �ּ�ü��(�׻� AF_INET)
		serveraddr.sin_port = htons(82);					// sin_port : ��Ʈ ��ȣ(���� �� ���ο� ���� ��Ʈ ��ȣ�� ����), htons() : short integer �����͸� ��Ʈ��ũ byte order
		serveraddr.sin_addr.s_addr = inet_addr("203.241.228.120");				// argv[1], sin_addr : ȣ��Ʈ IP�ּ�, s_addr : IP�ּҸ� ������ ����ü, inet_addr() : ���ڿ� ���¸� ���� ���·� ��ȯ, 127.0.0.1 : �ڱ� ��Ʈ��ũ, 192.168.51.206 : ������ ��Ʈ��ũ
		retval = connect(sock, (SOCKADDR *)&serveraddr, sizeof(serveraddr));	// connect : �ּҷ� ����(�����͸� �ְ���� �غ� ����), serveraddr�� ������ ���� retval�� ����

		// ���������� �ٷ� opened port�� �������� ���������� �ð��� ���� �ɸ�
		if (retval == SOCKET_ERROR) 
			printf("%s\n", "closed port");
		else
			printf("%s\n", "opened port");
	//}
}