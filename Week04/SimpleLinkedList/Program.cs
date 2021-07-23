using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SimpleLinkedList
{
    class Program
    {
        static void Main(string[] args)
        {
            LinkedList<string> list = new LinkedList<string>();                     // string 타입의 data를 가진 LinkedList
            list.AddLast("Apple");                                                  // 차례대로 추가
            list.AddLast("Banana");
            list.AddLast("Lemon");

            LinkedListNode<string> node = list.Find("Banana");                      // Banana가 들어있는 Node를 찾음
            LinkedListNode<string> newNode = new LinkedListNode<string>("Grape");   // string 타입의 Grape를 생성
            
            list.AddAfter(node, newNode);                                           // 새 Grape 노드를 Banana 노드 뒤에 추가

            //list.ToList().ForEach(p => Console.WriteLine(p));                     // 리스트 출력, 밑의 문장과 동일 lambda 표현법
            foreach (string str in list)  
                Console.WriteLine(str);
        }
    }
}
