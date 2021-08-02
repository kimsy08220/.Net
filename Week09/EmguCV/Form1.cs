using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;                  // using을 시켜줘야 한다
using Emgu.CV.Structure;        // using을 시켜줘야 한다

// 프로젝트 - Nuget 패키지 관리 - 검색창(emgucv3.1.0.1) 설치
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Image<Bgr, byte> _imgInput;
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                _imgInput = new Image<Bgr, byte>(ofd.FileName);
                imageBox1.Image = _imgInput;        // 오픈한 이미지(_imgInput)를 이미지박스1로 염
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("종료 하시겠습니까?", "System Message", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void cannyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyCanny();
            return;
        }

        public void ApplyCanny(double thresh = 50.0, double threshLink = 20.0)
        {
            if (_imgInput == null)
            {
                return;
            }
            Image<Gray, byte> _imgCanny = new Image<Gray, byte>(_imgInput.Width, _imgInput.Height, new Gray(0));

            _imgCanny = _imgInput.Canny(thresh, threshLink);
            imageBox1.Image = _imgCanny;
            return;
        }
        private void sobelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_imgInput == null)
            {
                return;
            }
            Image<Gray, byte> _imgGray = _imgInput.Convert<Gray, byte>();   // _imgInput 이미지를 흑백으로 바꿈
            Image<Gray, float> _imgSobel = new Image<Gray, float>(_imgInput.Width, _imgInput.Height, new Gray(0));  // 새로운 이미지 생성, float로 선언한 이유 : 계산상의 정확도를 높이기 위해
            
            _imgSobel = _imgGray.Sobel(1,0,3).Add(_imgGray.Sobel(0,1,3)).AbsDiff(new Gray(0.0));                    // 0을 기점으로 양의 정수의 값만 받아옴
            imageBox1.Image = _imgSobel.Convert<Gray, byte>();              // 다시 변환해서 이미지박스로 넣음
            return;
        }


        private void laplacianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_imgInput == null)
            {
                return;
            }
            Image<Gray, byte> _imgGray = _imgInput.Convert<Gray, byte>();
            Image<Gray, float> _imgLaplacian = new Image<Gray, float>(_imgInput.Width, _imgInput.Height, new Gray(0));

            _imgLaplacian = _imgGray.Laplace(1).AbsDiff(new Gray(0.0));     // Laplace(1) : 메소드 호출
            imageBox1.Image = _imgLaplacian.Convert<Gray, byte>();
            return;
        }

        private void cannyParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CannyParameters cp = new WindowsFormsApplication1.CannyParameters(this);
            cp.StartPosition = FormStartPosition.CenterParent;
            cp.Show();
        }
    }
}
