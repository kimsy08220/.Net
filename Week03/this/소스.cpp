#include <iostream>

using namespace std;

class A {	// 각 객체마다 필드 공간이 따로 주어지지만 메소드는 공유
public : 
	int num;
public:
    void print_This() {
        cout << "Class A의 this 주소 : " << this << endl;	// this : instance pointer(객체의 주소)
    }

	void mul(int a) {										// a의 this(주소)가 넘어가서 인자가 전달
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

	// a instance ponter : a의 주소
    cout << "a의 주소값 : " << &a << endl;  
    a.print_This();

	cout << "b의 주소값 : " << &b << endl; 
	b.print_This();

    cout << "a.return_A() : " << a.return_A() << endl;

    return 0;
}
