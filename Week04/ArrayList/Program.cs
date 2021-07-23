using System;
using System.Collections;   // ArrayList 사용하기 위해 선언

//int a[10];    // 배열의 이름(a) 시작주소를 가짐
//a[12] = 2;    // 긴급확장 X
//a[0] == *(a+0)
//a[9] == *(a+9)

// 배열은 Linked List처럼 거쳐 들어가서 값을 찾지 않고 가상 공간에 연속적으로 주소가 잡혀있기 때문에 긴급확장을 하게 되면 연속적 주소를 못 잡음 따라서 긴급확장 불가
// PID : Process ID

class CSTest
{
	static void Main()
	{
        ArrayList ar = new ArrayList(10);        // 10개의 ArrayList를 잡아놓음, but 넘어가도 상관없음
		ar.Add(1);
		ar.Add(2.34);
		ar.Add("string");
		ar.Add(new DateTime(2005, 3, 1));
        ar.Add(666);

        ar.Insert(1, 1234);
        ar.RemoveAt(0);

		foreach (object o in ar)                // ar 안에 있는 원소를 o로 빼내옴, 부모 클래스(object)는 자식 클래스(ArrayList)를 품을 수 있음
            Console.WriteLine(o.ToString());    // o.ToString() : object의 ToString()이 아닌 int, double, string 등의 ToString()을 호출, javascript 문법
         
		Console.WriteLine("개수 : " + ar.Count);
		Console.WriteLine("용량 : " + ar.Capacity);
	}
}
