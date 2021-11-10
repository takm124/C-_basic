# SortedList

- SortedList (정렬 리스트)
  - 리스트라고 하지만 Key-value 형태임
  - 데이터 등록시 자동으로 key를 기준으로 sorting이 이루어짐
  - 시간복잡도는 O(n)



- 프로젝트 개요
  - SortedList를 이용한 스케쥴러 만들기



![Sorted List](https://user-images.githubusercontent.com/72305146/136327306-699b1415-0dba-4659-b282-69c2c5a7b720.png)



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

namespace _36_SortedList
{
    public partial class Form1 : Form
    {
        SortedList<DateTime, string> slScheduler = new SortedList<DateTime, string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnScheduler_Click(object sender, EventArgs e)
        {
            // SortedList

            DateTime dSetDate = mcScheduler.SelectionStart;
			
            // 중복을 허용하지 않기 때문에 list에 key가 있는지 확인하는 과정
            if (!slScheduler.ContainsKey(dSetDate))
            {
                slScheduler.Add(dSetDate, tboxScheduler.Text);
            }
            else // 중복 key가 있으면 value를 update
            {
                slScheduler[dSetDate] = tboxScheduler.Text;
            }

            lboxScheduler.Items.Clear();

            foreach (KeyValuePair<DateTime, string> oitem in slScheduler)
            {
                lboxScheduler.Items.Add(string.Format("{0} : {1}", oitem.Key.ToString("yyyy-MM-dd"), oitem.Value));
            }
        }
    }
}

```

