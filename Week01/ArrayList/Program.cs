using System;
using System.Collections;

//이것이 왜 안되는가 알아보기, object 클래스는 최상위 클래스인데 왜 최상위 클래스로 왔을까? 숙제!!!!!!!!!!
//int a[10];
//a[12] = 2; // 긴급확장

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

		foreach (object o in ar)                // ar 안에 있는 원소를 o로 빼내옴
			Console.WriteLine(o.ToString());    // javascript

		Console.WriteLine("개수 : " + ar.Count);
		Console.WriteLine("용량 : " + ar.Capacity);
	}
}
