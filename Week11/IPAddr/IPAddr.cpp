#include <winsock2.h>
#include <stdio.h>

// IP 주소가 컴퓨터 내부로 들어갈 때(문자열 -> 정수) : inet_addr()
// 우리가 아는 IP 주소를 나타낼 때(정수 -> 문자열) : inet_ntoa()

// IP 주소를 나타낼 때 사용
int main(int argc, char* argv[])
{
	WSADATA wsa;
	if(WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
		return -1;

	// 원래의 IP 주소 출력
	char *ipaddr = "147.46.114.70";			// 현재 문자열 형태지만 컴퓨터 내부로 들어갈 땐 정수 형태로 변환하여 들어가야 함
	printf("IP 주소 = %s\n", ipaddr);

	// inet_addr() 함수 연습
	printf("IP 주소(변환 후) = 0x%x\n", inet_addr(ipaddr));		// inet_addr() : 문자열 형태를 정수 형태로 변환

	// inet_ntoa() 함수 연습
	IN_ADDR temp;
	temp.s_addr = inet_addr(ipaddr);		
	printf("IP 주소(변환 후) = %s\n", inet_ntoa(temp));			// inet_ntoa() : 정수 형태를 문자열 형태로 변환

	WSACleanup();
	return 0;
}