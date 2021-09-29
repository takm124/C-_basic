# 자료구조(Stack, Queue)

- 큐 : 선입선출 (FIFO)
  - Enqueue, Dequeue
- 스택 : 후입선출(LIFO)
  - Push, Pop





- 프로젝트 개요
  - 이번 프로젝트에서 스택과 큐를 시각적으로 이해할 수 있다.
  - 내용은 크게 어려운게 없지만 TablelayoutPanel 사용법을 익혀보자
  - In을 누르면 데이터 삽입, Out은 데이터 출력이다.




![자료구조 스택 큐](https://github.com/takm124/csharp_basic/blob/main/windowForm/images/%EC%9E%90%EB%A3%8C%EA%B5%AC%EC%A1%B0(Stack,%20Queue).png?raw=true)




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

namespace _20_Stack_Queue
{
    public partial class Form1 : Form
    {
        Stack<int> _Stack = new Stack<int>(6);
        Queue<int> _Queue = new Queue<int>(6);

        public Form1()
        {
            InitializeComponent();
        }

        private void btnDataIn_Click(object sender, EventArgs e)
        {
            Random rd = new Random();
            int iData = rd.Next(1, 101); // 랜덤 값은 1부터 100까지

            // Queue에 데이터 삽입
            if (_Queue.Count < 6)
            {
                _Queue.Enqueue(iData);
                fQueueDataDisplay();
            }

            // Stack에 데이터 삽입
            if (_Stack.Count < 6)
            {
                _Stack.Push(iData);
                fStackDataDisplay();
            }
        }

        private void btnDataOut_Click(object sender, EventArgs e)
        {
            fDataOut();
        }

        private void fDataOut()
        {
            // Queue 데이터 Out
            if (_Queue.Count > 0)
            {
                _Queue.Dequeue();
                fQueueDataDisplay();
            }

            // Stack 데이터 Out
            if (_Stack.Count > 0)
            {
                _Stack.Pop();
                fStackDataDisplay();
            }
        }

        Timer _oTimer = new Timer();
        bool _bTimer = false; // 타이머 스위치

        private void btnAutoDataOut_Click(object sender, EventArgs e)
        {
            if (_bTimer)
            {
                _oTimer.Stop();
                _bTimer = true;
            }
            else
            {
                _oTimer.Interval = 2000; // 2초마다 한번씩 out 시킴
                _oTimer.Tick += _oTimer_Tick;
                _oTimer.Start();

                _bTimer = true;
            }
        }

        private void _oTimer_Tick(object sender, EventArgs e)
        {
            fDataOut();
        }

        private void fQueueDataDisplay()
        {
            int[] iArray = _Queue.ToArray();

            // 배열 크기 고정
            Array.Resize(ref iArray, 6);

            lbQueue1.Text = (iArray[0] == 0) ? "-" : iArray[0].ToString();
            lbQueue2.Text = (iArray[1] == 0) ? "-" : iArray[1].ToString();
            lbQueue3.Text = (iArray[2] == 0) ? "-" : iArray[2].ToString();
            lbQueue4.Text = (iArray[3] == 0) ? "-" : iArray[3].ToString();
            lbQueue5.Text = (iArray[4] == 0) ? "-" : iArray[4].ToString();
            lbQueue6.Text = (iArray[5] == 0) ? "-" : iArray[5].ToString();
        }

        private void fStackDataDisplay()
        {
            int[] iArray = _Stack.ToArray();

            // 배열 크기 고정
            Array.Resize(ref iArray, 6);

            lbStack1.Text = (iArray[0] == 0) ? "-" : iArray[0].ToString();
            lbStack2.Text = (iArray[1] == 0) ? "-" : iArray[1].ToString();
            lbStack3.Text = (iArray[2] == 0) ? "-" : iArray[2].ToString();
            lbStack4.Text = (iArray[3] == 0) ? "-" : iArray[3].ToString();
            lbStack5.Text = (iArray[4] == 0) ? "-" : iArray[4].ToString();
            lbStack6.Text = (iArray[5] == 0) ? "-" : iArray[5].ToString();
        }
    }
  
}

```

