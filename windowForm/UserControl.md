# UserControl

- windowForm이 제공하는 Control이 아닌 사용자가 커스터마이징한 Control
- 반복된 형태의 컨트롤이 필요하다면 만들어서 사용하면 장점이 많다.
  - 코드가 분리되어서 가독성이 좋아짐
  - 유지보수도 편하며 캡슐화가 이루어진다.



- UserControl을 만들어서 프로퍼티를 설정하는 방법, event를 연결하는 법에 대해 숙지하자
  - delegate와 event에 익숙해지는 것이 최우선이다.



- 프로젝트 개요
  - 현상수배지는 반복되는 control이기 때문에 usercontrol로 따로 만든다.
  - usercontrol 마다 프로퍼티를 설정하는 방법에 대해 학습한다.
  - usercontrol에서 event가 발생했을 때 main form에 적용시키는 방법에 대해 학습한다.



![UserControl](https://user-images.githubusercontent.com/72305146/135978608-6397ae93-8fdf-43a3-8827-50e994eb16c2.png)





## UserControl 부분

```c#
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _29_UserControl
{
    public partial class UCInfo : UserControl
    {

        // Control의 부모 쪽으로 전달할 Event Delegate 
        public delegate int delEvent(object Sender, string strText); // delegate 선언
        public event delEvent eventdelSender; // delegate event 선언

        [Category("UserProperty"), Description("Image")]
        public Image UserFace
        {
            get
            {
                return this.pboxFace.BackgroundImage;
            }
            set
            {
                this.pboxFace.BackgroundImage = value;
            }
        }

        [Category("UserProperty"), Description("No")]
        public string UserNo
        {
            get
            {
                return this.lblNo.Text;
            }
            set
            {
                this.lblNo.Text = value;
            }
        }

        [Category("UserProperty"), Description("현상범의 이름")]
        public string UserName
        {
            get
            {
                return this.lblName.Text;
            }
            set
            {
                this.lblName.Text = value;
            }
        }

        [Category("UserProperty"), Description("현상금")]
        public string UserGold
        {
            get
            {
                return this.lblGold.Text;
            }
            set
            {
                this.lblGold.Text = value;
            }
        }

        public UCInfo()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            string strText = string.Empty;

            Button oBtn = sender as Button;  // object 형태로 되어 있는 sender를 Button 형태로 형변환

            switch (oBtn.Name)
            {
                case "btnReg":
                    this.BackColor = Color.Red;
                    strText = string.Format("{0}은 금액 {1}으로 수배중 입니다.", lblName.Text, lblGold.Text);
                    break;
                case "btnIdle":
                    this.BackColor = Color.Yellow;
                    strText = string.Format("{0}은 수배 중지  상태 입니다.", lblName.Text);
                    break;
                case "btnCatch":
                    this.BackColor = Color.Green;
                    strText = string.Format("{0}은 잡혔습니다.", lblName.Text);
                    break;
                default:
                    break;
            }

            if (eventdelSender != null)  // 부모가 Event를 생성하지 않았을 수 있으므로 생성 했을 경우에만 Delegate를 호출
            {
                eventdelSender(this, strText);
            }
        }
    }
}

```



## Main Form 부분

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

namespace _29_UserControl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // pMain Control 안에서 생성한 UserControl을 찾아서 형변환 한 뒤 Event를 생성 함
            foreach (var oControl in pMain.Controls)
            {
                if (oControl is UCInfo)
                {
                    UCInfo oInfo = oControl as UCInfo;

                    oInfo.eventdelSender += OInfo_eventdelSender;
                }
            }
        }

        private int OInfo_eventdelSender(object Sender, string strText)
        {
            UCInfo oInfo = Sender as UCInfo;

            lboxList.Items.Add(string.Format("{0}) {1}", oInfo.UserNo, strText));

            return 0;
        }
    }
}

```

