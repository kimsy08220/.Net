#include <winsock2.h>
#include <stdlib.h>
#include <stdio.h>

#define BUFSIZE 512

// ���� �Լ� ���� ��� �� ����
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

// ���� �Լ� ���� ���
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

// ����� ���� ������ ���� �Լ�
int recvn(SOCKET s, char *buf, int len, int flags)
{
	int received;
	char *ptr = buf;
	int left = len;

	while (left > 0){
		received = recv(s, ptr, left, flags);
		if (received == SOCKET_ERROR)
			return SOCKET_ERROR;
		else if (received == 0)
			break;
		left -= received;
		ptr += received;
	}

	return (len - left);
}

// ������ �����ϱ� ���ؼ��� ���� �ʱ�ȭ �� ���� �Լ� ����
// TCP Ŭ���̾�Ʈ : socket - connect - send - send - recv

int main(int argc, char* argv[])
{
	int retval;
	int totretval;

	// ����(������� ���� ���α׷���) �ʱ�ȭ
	WSADATA wsa;
	if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0)			// WSAStartup(������ ������, WSADATA ����ü�� ������ �ּ�) : ���� �ʱ�ȭ �Լ�, �Լ��� ���������� ����Ǹ� 0�� ��ȯ, �ƴϸ� 0�� �ƴ� �� ��ȯ
		return -1;

	// socket() : ���� ����, ����� ���� ���� �ڷᱸ��, ���� ��ũ���� Ȥ�� ����� ������ ����, �ü���� ����� ���� �����ϴ� �����͸� ���������� ������ �� �ֵ��� ���� ��
	// socket(�ּ� �йи�, ���� Ÿ��, ��������), �ּ� �йи� : ���ͳ��� ����Ѵٸ� AF_INET, ���� Ÿ�� : SOCK_STREAM(TCP ���), SOCK_DGRAM(UDP ���), �������� : 0���� �����ϸ� �׿� �´� �������� ���
	SOCKET sock = socket(AF_INET, SOCK_STREAM, 0);
	if (sock == INVALID_SOCKET) err_quit("socket()");	// ���� ������ ������ ��� INVALID_SOCKET ��ȯ

	// connect(����, ���� ��û�� ���� ������ �ּ�, ũ��) : ���� ��û
	SOCKADDR_IN serveraddr;									// SOCKADDR_IN ����ü���� 4���� �μ��� ����
	ZeroMemory(&serveraddr, sizeof(serveraddr));			// ZeroMemory(0���� ä�� �޸� ���� �ּ�, 0���� ä�� ũ��), �Լ��� �ƴ϶� ��ũ��
	serveraddr.sin_family = AF_INET;						// sin_family : �ּ�ü��(�׻� AF_INET)
	serveraddr.sin_port = htons(9000);						// sin_port : ��Ʈ ��ȣ
	serveraddr.sin_addr.s_addr = inet_addr("127.0.0.1");	// sin_addr : ȣ��Ʈ IP�ּ�, s_addr : IP�ּҸ� ������ ����ü, 127.0.0.1 : �ڱ� ��Ʈ��ũ, 192.168.51.206 : ������ ��Ʈ��ũ, ���� Ŭ���̾�Ʈ loop back �ּ�
	retval = connect(sock, (SOCKADDR *)&serveraddr, sizeof(serveraddr));	// connect : �ּҷ� ����(�����͸� �ְ���� �غ� ����), serveraddr�� ������ ���� retval�� ����
	if (retval == SOCKET_ERROR) err_quit("connect()");		// connect ������ ��� SOCKET_ERROR(-1) ��ȯ, ������ 0 ��ȯ

	// ������ ��ſ� ����� ����
	char buf[BUFSIZE + 1];
	int len;
	int sendCnt = 1;

	// ������ ������ ���
	while (1){
		// ������ �Է�
		ZeroMemory(buf, sizeof(buf));					// ZeroMemory : �޸𸮸� 0���� ����
		printf("\n[���� ������] ");
		if (fgets(buf, BUFSIZE + 1, stdin) == NULL)		// fgets(���ڿ��� �ִ� ����, ����, ������(stdin : Ű���� �Է�)) : ���ڿ� �޾ƿ��� �Լ�
			break;

		// '\n' ���� ����
		len = strlen(buf);
		if (buf[len - 1] == '\n')
			buf[len - 1] = '\0';							// \0 : ���ڿ��� ���� ��Ÿ��
		if (strlen(buf) == 0)
			break;

		// send(����, ������ ������, ũ��, flags) : ������ ������, flags : MSG_DONTWAIT(���� �����Ͱ� ���ٸ� -1 ��ȯ), MSG_NOSIGNAL(����� ������ ������ �� �ñ׳��� ���� ����)
		retval = send(sock, buf, strlen(buf), 0);
		if (retval == SOCKET_ERROR){						// ������ �����⸦ ������ ��� SOCKET_ERROR(-1) ��ȯ, ���� �� ������ ����Ʈ �� ��ȯ
			err_display("send()");
			break;
		}
		printf("[TCP Ŭ���̾�Ʈ] %d����Ʈ�� ���½��ϴ�.\n", retval);

		if (sendCnt != 1)
		{
			totretval += retval;
			sendCnt = 1;

			// recv(����, ������ ������, ũ��, flags) : ������ �ޱ�, flags : MSG_DONTWAIT(���� �����Ͱ� ���ٸ� -1 ��ȯ), MSG_NOSIGNAL(����� ������ ������ �� �ñ׳��� ���� ����)
			retval = recvn(sock, buf, totretval, 0);	// recvn���� n���� �ȴ�
			if (retval == SOCKET_ERROR){				// ������ �����⸦ ������ ��� SOCKET_ERROR(-1) ��ȯ, ���� �� ������ ����Ʈ �� ��ȯ
				err_display("recv()");
				break;
			}
			else if (retval == 0)						// �ƹ��͵� �޾ƿ��� �ʾ��� ���
				break;

			// ���� ������ ���
			buf[retval] = '\0';
			printf("[TCP Ŭ���̾�Ʈ] %d����Ʈ�� �޾ҽ��ϴ�.\n", retval);
			printf("[���� ������] %s\n", buf);
		}
		else
		{
			totretval = retval;
			sendCnt++;
		}
	}

	// closesocket(���� �Լ��� ������ ���ϰ�) : ���� �Լ��� ������ ������ �����ϴ� �뵵
	closesocket(sock);

	// ���� ���� : ������ �׸� ���ڴ�. WSACleanup() <-> WSAStartup()
	WSACleanup();
	return 0;
}