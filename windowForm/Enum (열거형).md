# Enum (열거형)

- 미리 정해 놓은 값중 하나의 값만 가질 수 있는 변수 선언
- 코드가 단순해지며 가독성이 좋아짐
- 구현의 의도가 열거임을 명확히 알 수 있음





### windForm

- 열거형을 이용해 Listbox를 활용해보기


![Enum image](https://user-images.githubusercontent.com/72305146/135372976-173121c5-a5c5-4f15-92c9-06dc5cd7484e.jpg)



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

namespace csharp_lesson
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
		
        // 요일에 대한 열거형
        enum enumDay
        {
            Monday,
            TuesDay,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }
		
        // 시간대에 대한 열거형
        enum enumTime
        {
            Morning,
            Afternoon,
            Evening
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lboxDay.Items.Add(enumDay.Monday);
            lboxDay.Items.Add(enumDay.TuesDay);
            lboxDay.Items.Add(enumDay.Wednesday);
            lboxDay.Items.Add(enumDay.Thursday);
            lboxDay.Items.Add(enumDay.Friday);
            lboxDay.Items.Add(enumDay.Saturday);
            lboxDay.Items.Add(enumDay.Sunday);

            lboxTime.Items.Add(enumTime.Morning);
            lboxTime.Items.Add(enumTime.Afternoon);
            lboxTime.Items.Add(enumTime.Evening);
        }

        private void btnResult_Click(object sender, EventArgs e)
        {
            // 준호와 Monday(요일) Afternoon에 보기로 했습니다.
            string strResult = tboxName.Text + "와 " + lboxDay.SelectedItem + "(요일) "
                                    + lboxTime.SelectedItem + "에 보기로 했습니다";

            tboxResult.Text = strResult;
        }

        private void btnResultStringFormat_Click(object sender, EventArgs e)
        {
            string strResult = String.Format("{0}와 {1}(요일) {2}에 보기로 했습니다.",
                                        tboxName.Text, lboxDay.SelectedItem, lboxTime.SelectedItem);

            tboxResult.Text = strResult;
        }
    }
}

```





### 정리

- 특정 리스트를 만들 때 배열이나 컬렉션을 사용하기 전 열거형을 고민해 보자

- 상수형 대신 열거형을 사용해보자

  
