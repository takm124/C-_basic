# Array (배열)

- winForm에서 배열을 이용한 DataGridView 활용 방안



![Array1](https://user-images.githubusercontent.com/72305146/135372930-c691c200-37c1-4e59-9273-acf7c31315ad.png)
![Array2](https://user-images.githubusercontent.com/72305146/135372931-0511c120-5f66-4d0a-961f-49e6c0ff1c19.png)






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

namespace _07Array
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 초기화를 해주지 않으면 중첩됨
            dgDay.Rows.Clear();

            int[] iTest = { 10, 9, 18, 21, 4, 12, 17 };

            dgDay["colDay1", 0].Value = iTest[0];
            dgDay["colDay2", 0].Value = iTest[1];
            dgDay["colDay3", 0].Value = iTest[2];
            dgDay["colDay4", 0].Value = iTest[3];
            dgDay["colDay5", 0].Value = iTest[4];
            dgDay["colDay6", 0].Value = iTest[5];
            dgDay["colDay7", 0].Value = iTest[6];
           
            lbArrayCount.Text = "전체 자료 수 : " + iTest.Length;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 초기화를 해주지 않으면 중첩됨
            dgDay.Rows.Clear();

            int[,] iTest = { { 10, 9, 18, 21, 4, 12, 17 }, { 6, 21, 8, 14, 20, 11, 6 } };

            // 새로운 행을 추가
            dgDay.Rows.Add();

            dgDay["colDay1", 0].Value = iTest[0,0];
            dgDay["colDay2", 0].Value = iTest[0,1];
            dgDay["colDay3", 0].Value = iTest[0,2];
            dgDay["colDay4", 0].Value = iTest[0,3];
            dgDay["colDay5", 0].Value = iTest[0,4];
            dgDay["colDay6", 0].Value = iTest[0,5];
            dgDay["colDay7", 0].Value = iTest[0,6];

            dgDay["colDay1", 1].Value = iTest[1, 0];
            dgDay["colDay2", 1].Value = iTest[1, 1];
            dgDay["colDay3", 1].Value = iTest[1, 2];
            dgDay["colDay4", 1].Value = iTest[1, 3];
            dgDay["colDay5", 1].Value = iTest[1, 4];
            dgDay["colDay6", 1].Value = iTest[1, 5];
            dgDay["colDay7", 1].Value = iTest[1, 6];

            lbArrayCount.Text = "전체 자료 수 : " + iTest.Length;
        }

        
    }
}

```





### 정리

- 오직 배열만을 사용하여 DataGridView를 표현했기 때문에 반복 작업이 많다.
- 추후에는 Table을 한번에 만들어 삽입하는 형태로 설계할 수도 있다.



