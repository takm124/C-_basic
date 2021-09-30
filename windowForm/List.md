# List

- List에 데이터를 저장해서 컨트롤 하는 방법
- List는 제네릭이 필요하지만 ArrayList는 필요하지 않다.
  - ArrayList는 여러가지 데이터 형을 보관할 수 있음



- 프로젝트 개요
  - 이벤트가 일어날 때 마다 아이템을 리스트에 저장한다.
  - 리스트에 저장한 데이터를 DataGridView에 뿌려준다.
    - 뿌리는 방식은 여러가지이다.



![List](https://user-images.githubusercontent.com/72305146/135368236-9f7cb93a-c897-4da8-87de-94edba0a072a.png)



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

namespace _21_List
{
    public partial class Form1 : Form
    {
        List<string> _strList = new List<string>();


        public Form1()
        {
            InitializeComponent();
            //dgViewList.Columns.Add("dgValue", "Value");
        }

        private void pbox_Click(object sender, EventArgs e)
        {
            PictureBox pbox = sender as PictureBox;

            string strSelectText = string.Empty;

            switch (pbox.Name)
            {
                case "pbox1":
                    strSelectText = "cake";
                    break;
                case "pbox2":
                    strSelectText = "burger";
                    break;
                case "pbox3":
                    strSelectText = "pizza";
                    break;
                case "pbox4":
                    strSelectText = "ice";
                    break;
            }

            _strList.Add(strSelectText);
            fUIDisplay();
            fDataGridViewDisplay();
        }

        private void fUIDisplay()
        {
            int iCake = 0;
            int iBurger = 0;
            int iPizza = 0;
            int iIce = 0;

            foreach (string item in _strList)
            {
                // 선택한 아이템이 무엇인지
                switch (item)
                {
                    case "cake":
                        iCake++;
                        break;
                    case "burger":
                        iBurger++;
                        break;
                    case "pizza":
                        iPizza++;
                        break;
                    case "ice":
                        iIce++;
                        break;
                }
            }

            lbPick1.Text = iCake.ToString();
            lbPick2.Text = iBurger.ToString();
            lbPick3.Text = iPizza.ToString();
            lbPick4.Text = iIce.ToString();

            lbTotalCount.Text = _strList.Count.ToString();
        }


        // DataGriveView Display
        private void fDataGridViewDisplay()
        {
            //dgViewList.Rows.Clear();

            //// 아이템 출력
            //foreach (string item in _strList)
            //{
            //    dgViewList.Rows.Add(item);
            //}

            dgViewList.DataSource = _strList.Select(x => new { Value = x }).ToList();


            // 행 머리에 인덱스 출력
            foreach (DataGridViewRow row in dgViewList.Rows)
            {
                row.HeaderCell.Value = row.Index.ToString();
            }

            dgViewList.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }
    }
}

```

