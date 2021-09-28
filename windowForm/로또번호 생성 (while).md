# 로또번호 생성 (while)

- while 문을 이용해서 로또번호를 생성하는 프로젝트
- 주요 포인트
  - 난수 발생시키기
  - 난수를 중복시키지 않고 배열에 추가하기
  - 배열을 스트링으로 반환하기



![로또번호](https://github.com/takm124/csharp_basic/blob/main/windowForm/images/%EB%A1%9C%EB%98%90%EB%B2%88%ED%98%B8.png?raw=true)



```c#
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 로또번호생성
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnwhileResult_Click(object sender, EventArgs e)
        {
            // 1~45 중 6개의 숫자
            int[] iArray = new int[6];
            int iCount = 0; // 배열 위치 확인

            StringBuilder sb = new StringBuilder();
            Random rd = new Random(); // 난수 발생

            while (Array.IndexOf(iArray, 0) != -1) // 0이라는 값이 없을 때까지
            {
                int iNumber = rd.Next(1, 46);

                if (Array.IndexOf(iArray, iNumber) == -1) // 발생한 난수가 배열에 없을 때 값 추가
                {
                    iArray[iCount] = iNumber;
                    // sb.Append(string.Format("{0}, ", iNumber)); 
                    iCount++;
                }
            }

            // 배열 오름차순 정리
            Array.Sort(iArray);

            // 배열 스트링 변환
            string result = string.Join(", ", iArray);


            lbwhileResult.Text = result;
            lboxwhileResult.Items.Add(result);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rd = new Random();

            int iNumber = int.Parse(tboxNumber.Text);

            int iCheckNumber = 0;
            int iCount = 0;

            do
            {
                iCheckNumber = rd.Next(1, 101);
                iCount++;

            } while (iNumber != iCheckNumber);

            lbdowhileResult.Text = string.Format("- 뽑은 숫자 {0}을 {1}회 만에 뽑았습니다", iCheckNumber, iCount);
        }
    }
}

```

