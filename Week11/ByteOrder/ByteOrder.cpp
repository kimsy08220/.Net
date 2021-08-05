#include <winsock2.h>
#include <stdio.h>
// 포트 번호를 나타낼 때 사용
// 배열에 있던 값들이 0번지부터 들어가게 되는데 또 다시 배열 안으로 들어갈 때도 0번지부터 들어가게 된다. 
// 78			12345678		78x	12
// 56   ->					->	56x	34
// 34							34x	56
// 12							12x	78
// 원격 호스트와 데이터 통신을 하길 원한다면 보낼 때(send) 네트워크 byte order로, 받을 때(receive) 호스트 byte order로 변경

int main(int argc, char* argv[])
{
    WSADATA wsa;
    if(WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
        return -1;

    u_short x = 0x1234;
    u_long y = 0x12345678;							// 실제로 들어갈 때는 바이트로 들어감

    u_short x2;
    u_long y2;

    // 호스트 바이트 -> 네트워크 바이트
    printf("호스트 바이트 -> 네트워크 바이트\n");
    printf("0x%x -> 0x%x\n", x, x2 = htons(x));		// htons() : short integer 데이터를 네트워크 byte order로 변경
    printf("0x%x -> 0x%x\n\n", y, y2 = htonl(y));	// htonl() : long integer 데이터를 네트워크 byte order로 변경

    // 네트워크 바이트 -> 호스트 바이트
    printf("네트워크 바이트 -> 호스트 바이트\n");
    printf("0x%x -> 0x%x\n", x2, ntohs(x2));		// ntohs() : short intger 데이터를 호스트 byte order로 변경
    printf("0x%x -> 0x%x\n\n", y2, ntohl(y2));		// ntohl() : long intger 데이터를 호스트 byte order로 변경

    // 잘못된 사용 예
    printf("잘못된 사용 예\n");
    printf("0x%x -> 0x%x\n", x, htonl(x));			// htonl() -> htons()

    WSACleanup();
    return 0;
}