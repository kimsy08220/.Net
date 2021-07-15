using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;   // 배열, 배열 리스트, 해시 테이블, 큐 등을 담고있음, ArrayList를 사용하기 위해 선언, 373p 참고

namespace First
{
    public partial class Form1 : Form   // :는 상속의 의미, Form(닷넷에 있는 클래스 라이브러리)이라는 클래스를 상속
    {
        public ArrayList ar;    // 373p
        public Random Random;   // 388p

        public Form1()
        {
            InitializeComponent();
            ar = new ArrayList();
            Random = new Random();
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)   // MouseEventArgs : Event handler, MouseButtons 정의로 이동하면 사용할 수 있는 메소드가 나옴
        {
            if (e.Button == (MouseButtons)1048576)      // (MouseButtons)1048576 == MouseButtons.Left
            {
                CMyData c = new CMyData();
                c.Shape = (int)Random.Next(2);          // 0,1 난수 생성
                c.Size = (int)Random.Next(50, 200);     // 50~199 난수 생성
                c.Point = new Point(e.X, e.Y);          // 213p(property), c.Point : set 메소드를 사용해 터치한 좌표 값 설정, e.X, e.Y는 필드같지만 Point 안에 get,set 메소드가 있다.
                c.bColor = Color.FromArgb(Random.Next(256), Random.Next(256), Random.Next(256));    // FromArgb : RGB를 만드는 역할
                c.pColor = Color.FromArgb(Random.Next(256), Random.Next(256), Random.Next(256));
                ar.Add(c);
            }
            //Invalidate();   // 무효화 시킴, Form1_Paint 호출
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (CMyData c in ar)   // ar 배열에 있는 내용을 c에 저장
            {
                SolidBrush brc = new SolidBrush(c.bColor);  // SolidBrush : 내부 색칠
                Pen p = new Pen(c.pColor);                  // Pen : 테두리 색칠

                if (c.Shape == 1) 
                {
                    e.Graphics.DrawEllipse(p, c.Point.X, c.Point.Y, c.Size, c.Size);        // c.Point.X : get 메소드를 사용해 좌표 값 얻음
                    e.Graphics.FillEllipse(brc, c.Point.X, c.Point.Y, c.Size, c.Size);
                }
                else
                {
                    e.Graphics.DrawRectangle(p, c.Point.X, c.Point.Y, c.Size, c.Size);      // DrawEllipse : 테두리 색칠
                    e.Graphics.FillRectangle(brc, c.Point.X, c.Point.Y, c.Size, c.Size);    // FillEllipse : 내부 색칠
                }
            }
        }

        public class CMyData
        {
            public Point Point { get; set; }    // property 최신 문법으로 교체
            //public Point Point                // property 정의, 위 문장과 동일
            //{
            //    get { return point; }
            //    set { point = value; }
            //}
            public Color pColor { get; set; }   // Color 정의로 이동하면 색깔 명이 나옴
            public Color bColor { get; set; }
            public int Size { get; set; }
            public int Shape { get; set; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
