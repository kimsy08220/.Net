#include <iostream>

using namespace std;

class A {	// �� ��ü���� �ʵ� ������ ���� �־������� �޼ҵ�� ����
public : 
	int num;
public:
    void print_This() {
        cout << "Class A�� this �ּ� : " << this << endl;	// this : instance pointer(��ü�� �ּ�)
    }

	void mul(int a) {										// a�� this(�ּ�)�� �Ѿ�� ���ڰ� ����
		cout << "num * a = " << this->num * a << endl;		// num == this->num
	}

    A* return_A() {
        return this;
    }
};

int main(void) {

    A a,b;  

	a.num = 2;
	b.num = 5;

	a.mul(3);
	b.mul(5);

	// a instance ponter : a�� �ּ�
    cout << "a�� �ּҰ� : " << &a << endl;  
    a.print_This();

	cout << "b�� �ּҰ� : " << &b << endl; 
	b.print_This();

    cout << "a.return_A() : " << a.return_A() << endl;

    return 0;
}
