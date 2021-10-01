# Thread

- 이번장은 전체적인 프로세스 진행 순서에 대해 이해를 해야할 필요가 있다.
  - invoke 개념의 등장으로 Cross Thread가 발생하는 상황을 이해야한다.
  - 한 스레드가 이미 작동하고 있는데 다른 스레드에서 접근하려는 상황을 이야기한다.
  - 이럴 경우 Cross Thread (작동이 겹친다는 것)인 경우에 잠깐 순서를 양보해야하는데 invoke를 통해 구현한다.
  - invokeRequired를 통해 다른 스레드에 있는 컨트롤을 호출해야 하는 상황인지 확인한다.



- 프로젝트에서 cross thread가 일어나는 경우를 알아보자
  - 첫 번째, 플레이어 수를 결정한다 = 플레이어 수 만큼의 스레드 발생
  - 플레이어 스레드는 자식 창에서 실행된다.
  - 부모창의 스레드까지 있다는 것을 간과하면 안된다.
  - 먼저 자식창에 발생하는 자식 스레드들 간의 간섭이 발생하는 경우다.
    - progress bar가 진행되기위해서 스레드에서 계속 value값을 더해주고 있는 상황이다.
    - invokeRequired, invoke로 제어를 해준다.



![thread](https://user-images.githubusercontent.com/72305146/135581840-f1d62a49-bcb9-4d82-bb69-2bf0b2962e3e.png)









## 부모 Form

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

namespace _26_Thread
{
    public partial class Form1 : Form
    {
        private enum enumPlayer
        {
            아이린,
            슬기,
            웬디,
            조이,
            예리,

        }

        int _locationX = 0;
        int _locationY = 0;

        public Form1()
        {
            InitializeComponent();

            _locationX = this.Location.X;
            _locationY = this.Location.Y;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _locationX = this.Location.X + this.Size.Width;
            _locationY = this.Location.Y;

            for (int i = 0; i < numPlayerCount.Value; i++)
            {
                Play pl = new Play(((enumPlayer)i).ToString());
                // 자식 창을 부모 창 옆에 띄우기 (위치 값 조정)
                pl.Location = new Point(_locationX, _locationY + pl.Height * i);
                pl.eventdelMessage += Pl_eventdelMessage;

                pl.Show();
                pl.fThreadStart();
            }

            
        }

        private int Pl_eventdelMessage(object sender, string strResult)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(delegate ()
               {
                   Play oPlayerForm = sender as Play;
                   lboxResult.Items.Add(string.Format("Player : {0}, 결과 : {1}", oPlayerForm.StrPlayerName, strResult));
               }));
            }
            return 0;
        }
    }
}

```





## 자식 Form

```c#
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _26_Thread
{   
    public partial class Play : Form
    {
        public delegate int delMessage(object sender, string strResult); //delegate 선언
        public event delMessage eventdelMessage;

        string _strPlayerName = string.Empty;
        public string StrPlayerName
        {
            get
            {
                return _strPlayerName;
            }

            set
            {
                _strPlayerName = value;
            }
        }

        Thread _thread = null;


        public Play()
        {
            InitializeComponent();
        }

        public Play(string strPlayerName)
        {
            InitializeComponent();

            lbPlayerName.Text = StrPlayerName = strPlayerName;
        }

        public void fThreadStart()
        {
            //_thread = new Thread(new ThreadStart(run)); // ThreadStart 델리게이트 타입 객체

            _thread = new Thread(run); // 컴파일러에서 델리게이트 객체를 추론 후 생성하고 함수 넘김

            //_thread = new Thread(delegate () { run(); });

            _thread.Start();
        }

        private void run()
        {
            // UI Control이 자신이 만들어진 Thread가 아닌 다른 Thread에서 접근할 경우 Corss-Thread 발생

            int ivar = 0;
            Random rd = new Random();
            while (pbarPlayer.Value < 100)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(delegate ()
                    { // 함수값
                       ivar = rd.Next(1, 11);
                       if (pbarPlayer.Value + ivar > 100)
                       {
                           pbarPlayer.Value = 100;
                       }
                       else
                       {
                           pbarPlayer.Value += ivar;
                       }

                       lbProcess.Text = string.Format("진행 상황 표시 : {0}%", pbarPlayer.Value);

                       this.Refresh();
                    }));
                } 
            }
            eventdelMessage(this, "완주!! (thread completed");
        }
    }
}

```



