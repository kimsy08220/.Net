#include <winsock2.h>
#include <stdio.h>
// ��Ʈ ��ȣ�� ��Ÿ�� �� ���
// �迭�� �ִ� ������ 0�������� ���� �Ǵµ� �� �ٽ� �迭 ������ �� ���� 0�������� ���� �ȴ�. 
// 78			12345678		78x	12
// 56   ->					->	56x	34
// 34							34x	56
// 12							12x	78
// ���� ȣ��Ʈ�� ������ ����� �ϱ� ���Ѵٸ� ���� ��(send) ��Ʈ��ũ byte order��, ���� ��(receive) ȣ��Ʈ byte order�� ����

int main(int argc, char* argv[])
{
    WSADATA wsa;
    if(WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
        return -1;

    u_short x = 0x1234;
    u_long y = 0x12345678;							// ������ �� ���� ����Ʈ�� ��

    u_short x2;
    u_long y2;

    // ȣ��Ʈ ����Ʈ -> ��Ʈ��ũ ����Ʈ
    printf("ȣ��Ʈ ����Ʈ -> ��Ʈ��ũ ����Ʈ\n");
    printf("0x%x -> 0x%x\n", x, x2 = htons(x));		// htons() : short integer �����͸� ��Ʈ��ũ byte order�� ����
    printf("0x%x -> 0x%x\n\n", y, y2 = htonl(y));	// htonl() : long integer �����͸� ��Ʈ��ũ byte order�� ����

    // ��Ʈ��ũ ����Ʈ -> ȣ��Ʈ ����Ʈ
    printf("��Ʈ��ũ ����Ʈ -> ȣ��Ʈ ����Ʈ\n");
    printf("0x%x -> 0x%x\n", x2, ntohs(x2));		// ntohs() : short intger �����͸� ȣ��Ʈ byte order�� ����
    printf("0x%x -> 0x%x\n\n", y2, ntohl(y2));		// ntohl() : long intger �����͸� ȣ��Ʈ byte order�� ����

    // �߸��� ��� ��
    printf("�߸��� ��� ��\n");
    printf("0x%x -> 0x%x\n", x, htonl(x));			// htonl() -> htons()

    WSACleanup();
    return 0;
}