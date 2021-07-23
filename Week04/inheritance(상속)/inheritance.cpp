#include <stdio.h>
#include <iostream>
using namespace std;

class A {
    int i;
public:
    A() {
        i=0;
    }
    void Print() { cout << i << endl; }		// virtual �����ϸ� 1�� ���, �ƴϸ� 0�� ���
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
    A* ap;			// ap : A class�� instance pointer�� ��� ����
    B b;			// b : B class�� instance(object)�� ����
    
    ap=&b;			// �θ� Ŭ����(A)�� �ڽ� Ŭ����(B)�� ǰ�� �� ����, b�� �����ϸ� B class�� A class ��� �޾ұ� ������ �����ڸ� ȣ��
    ap->Print();	// A class�� Print() ȣ��
 
}