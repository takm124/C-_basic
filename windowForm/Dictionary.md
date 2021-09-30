# Dictionary

- Dictionary와 Hashtable의 차이
  - List, ArrayList 때와 마찬가지로 제네릭을 사용하느냐의 차이
  - 제네릭을 사용하지 않으면 박싱-언박싱이 계속 일어나게 된다.
  - Dictionary< key, Value>
  - Hahstable





- 프로젝트 개요
  - Key를 이용하여 데이터 값을 컨트롤 할 수 있음을 보여줌





![Dictionary](https://user-images.githubusercontent.com/72305146/135372452-a86d7c7d-0451-4a42-9170-7282e6597993.png)



```c#
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _22_Dictionary
{
    public partial class Form1 : Form
    {

        // 후보자들
        enum enBossName
        {
            보검,
            신혜,
            혜인,
            보영,
        }

        // 유권자들
        enum enClassName
        {
            진,
            정국,
            RM,
            지민,
            제이홉,
            뷔,
            슈가,

            은비,
            사쿠라,
            혜원,
            예나,
            채연,
            채원,
            민주,
            히토미,
            유리,
            유진,
            원영,
            나코,
        }

        int iPlayerCount = 0;

        Hashtable _ht = new Hashtable();
        Dictionary<string, string> _dic = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void pbox_Click(object sender, EventArgs e)
        {
            PictureBox pbox = sender as PictureBox;

            string strSelectText = string.Empty;

            switch (pbox.Name)
            {
                case "pbox1":
                    strSelectText = enBossName.보검.ToString();
                    break;
                case "pbox2":
                    strSelectText = enBossName.신혜.ToString();
                    break;
                case "pbox3":
                    strSelectText = enBossName.혜인.ToString();
                    break;
                case "pbox4":
                    strSelectText = enBossName.보영.ToString();
                    break;
            }

            // 투표자 인원 수 
            int iTotalCount = Enum.GetValues(typeof(enClassName)).Length;

            if (iTotalCount > iPlayerCount)
            {
                enClassName enName = (enClassName)iPlayerCount;

                _dic.Add(enName.ToString(), strSelectText);

                fUIDisplay(iTotalCount, enName.ToString());
                fDataGridViewDisplay();

                iPlayerCount++;
            }
            else
            {
                lbPlayerName.Text = "투표가 끝났습니다";
            }
            //_ht.Add("??", strSelectText);
        }

        private void fUIDisplay(int iTotalCount, string strPlayerName)
        {
            int i보검 = 0;
            int i신혜 = 0;
            int i해인 = 0;
            int i보영 = 0;

            foreach (string oitem in _dic.Values)
            {
                // string 값을 enum으로 형변환 하는 부분

                switch (oitem)
                {
                    case "보검":
                        i보검++;
                        break;
                    case "신혜":
                        i신혜++;
                        break;
                    case "해인":
                        i해인++;
                        break;
                    case "보영":
                        i보영++;
                        break;
                }
            }

            lbPick1.Text = i보검.ToString();
            lbPick2.Text = i신혜.ToString();
            lbPick3.Text = i해인.ToString();
            lbPick4.Text = i보영.ToString();


            // 0 / 0
            lbTotalCount.Text = string.Format("{0} / {1}", iPlayerCount + 1, iTotalCount);
            lbPlayerName.Text = strPlayerName;
        }


        private void fDataGridViewDisplay()
        {

            dgViewList.DataSource = _dic.ToArray();

            foreach (DataGridViewRow oRow in dgViewList.Rows)
            {
                oRow.HeaderCell.Value = oRow.Index.ToString();
            }

            dgViewList.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }
    }
}

```



