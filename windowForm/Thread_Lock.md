# Thread_Lock

- Thread의 동시 접근을 막기 위한 임계지점 설정



- 주요 메소드

  - ParameterizedThreadStart

    - thread 실행시 파라미터 넘기는 방법
    - 파라미터 설정 해주지 않으면 스레드 실행시에 전역변수가 변화할 수 있음

  - lock

    - object 객체 하나를 만들어서 임계 지점 설정

    

- 프로젝트 개요
  - 방이 2개가 있고 예약(스레드)를 걸 수 있음
  - 스레드는 하나씩 진행되며 진행사항은 listbox에 표시된다.
  - 스레드는 순차적으로 처리됨
  - lock, invoke의 활용이 필요한 부분



![Thread_Lock](https://user-images.githubusercontent.com/72305146/136483163-7b49b492-db40-4e86-ac7e-1d028037b325.png)





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

namespace _38_thread_lock
{


    public partial class Form1 : Form
    {
        Thread _T1;
        Thread _T2;
        object RoomLock = new object();

        int _iRoom1Count = 1;
        int _iRoom2Count = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnRoom1_Click(object sender, EventArgs e)
        {
            lboxRoom1.Items.Add(string.Format("Room 1 : {0} 예약", _iRoom1Count));

            _T1 = new Thread(new ParameterizedThreadStart(Run));
            _T1.Start(_iRoom1Count);

            _iRoom1Count++;

        }

        private void Run(object obj)
        {
            lock(RoomLock)
            {
                string str = string.Format("Room 1: Player {0} 사용중", obj);
                invokeFunction(lblLockStatus, str);

                for (int i = 1; i < 4; i++)
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(delegate ()
                        {
                            lboxResult.Items.Add(string.Format("Room 1 : Player {0} 진행중 . . . {1}", obj, i));
                            Refresh();
                        }));
                    }

                    Thread.Sleep(1000);
                }

                lblLockStatus.Text = string.Format("Room 1: Player {0} 사용완료", obj);
                Thread.Sleep(1000);
            }
        }

        private void invokeFunction(Label objlabel, string str)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(delegate ()
                {
                    objlabel.Text = str;
                }));
            }
        }

        private void btnRoom2_Click(object sender, EventArgs e)
        {
            lboxRoom2.Items.Add(string.Format("Room 2 : {0} 예약", _iRoom2Count));

            _T2 = new Thread(new ParameterizedThreadStart(Run2));
            _T2.Start(_iRoom2Count);

            _iRoom2Count++;
        }

        private void Run2(object obj)
        {
            lock(RoomLock)
            {
                string str = string.Format("Room 2: Player {0} 사용중", obj);
                invokeFunction(lblLockStatus, str);

                for (int i = 1; i < 4; i++)
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(delegate ()
                        {
                            lboxResult.Items.Add(string.Format("Room 2 : Player {0} 진행중 . . . {1}", obj, i));
                            Refresh();
                        }));
                    }

                    Thread.Sleep(1000);
                }

                lblLockStatus.Text = string.Format("Room 2: Player {0} 사용완료", obj);
                Thread.Sleep(1000);
            }
        }


    }
}

```



