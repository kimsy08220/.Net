#include <stdio.h>
#include <iostream>
using namespace std;

class A {
    int i;
public:
    A() {
        i=0;
    }
    void Print() { cout << i << endl; }		// virtual 선언하면 1이 출력, 아니면 0을 출력
};

class B : public A {
    int i;
public:
    B() {
        i=1;
    }
    void Print() { cout << i << endl; }
};

void main() {
    A* ap;			// ap : A class의 instance pointer를 담는 변수
    B b;			// b : B class의 instance(object)를 생성
    
    ap=&b;			// 부모 클래스(A)는 자식 클래스(B)를 품을 수 있음, b를 선언하면 B class는 A class 상속 받았기 때문에 생성자를 호출
    ap->Print();	// A class의 Print() 호출
 
}