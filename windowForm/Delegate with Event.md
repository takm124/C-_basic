# Delegate with Event

- delegate는 주로 event와 함께 사용한다.

- 이번 프로젝트는 부모 폼 - 자식 폼 사이에서 이벤트 이동이 어떻게 일어나는지 이해하는 것이 중요하다
  - '자식'이 event를 생성한다.
  - '부모'는 '자식' 객체를 생성하고 event에 +=를 이용해 메소드를 중첩한다.
  - '자식'쪽의 데이터를 이용해 event를 발생시키면 '부모' 쪽으로 데이터가 넘어간다.
  - 즉, 이번 챕터에서는 이벤트를 언제 발생시키는지와 데이터를 어떻게 주고 받는지 이해하는 장이다.





- 프로젝트 개요
  - 이전의 delegate 챕터에서 했던 프로젝트를 조금 더 고도화 시킨 버전이다.
  - 피자 주문을 넣으면 자식창에서 피자를 만드는 과정을 보여준다.
  - 피자가 다 만들어지면 부모 창에서 시간이 얼마나 걸렸는지 출력해주면서 주문이 끝이난다.





![delegate event](https://user-images.githubusercontent.com/72305146/135548364-c5e0ecd0-5d90-48d0-938a-a519542d0f2f.png)





## 자식 form

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

namespace _24_Delegate
{
    public partial class frmPizza : Form
    {
        public delegate int delPizzaComplete(string strResult, int iTime);
        public event delPizzaComplete eventdelPizzaComplete; // delegate event 선언

        public frmPizza()
        {
            InitializeComponent();
        }

        // 창 닫기
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void fPizzaCheck(Dictionary<string, int> dPizzaOrder)
        {
            int iTotalTime = 0;

            foreach (KeyValuePair<string, int> order in dPizzaOrder)
            {
                int iNowTime = 0;
                string strType = string.Empty;
                int iTime = 0;
                int iCount = order.Value;

                switch (order.Key)
                {
                    // 도우
                    case "오리지널":
                        iNowTime = 3000;
                        strType = "도우";
                        
                        break;
                    case "씬":
                        iNowTime = 3500;
                        strType = "도우";
                        break;

                    // 엣지
                    case "리치골드":
                        iNowTime = 500;
                        strType = "엣지";
                        break;
                    case "치즈크러스트":
                        iNowTime = 400;
                        strType = "엣지";
                        break;

                    // 토핑
                    case "소세지":
                        iNowTime = 32;
                        strType = "토핑";
                        break;
                    case "감자":
                        iNowTime = 17;
                        strType = "토핑";
                        break;
                    case "치즈":
                        iNowTime = 48;
                        strType = "토핑";
                        break;

                    default:
                        break;
                }

                iTime = iNowTime * iCount;
                lboxMake.Items.Add(string.Format("{0} {1} : {2}초 ({3}초, {4}개 )", strType, order.Key, iTime, iNowTime, iCount));

                iTotalTime += iTime;

                Refresh();
                Thread.Sleep(1000);
            }

            int iRet = eventdelPizzaComplete("Pizza가 완성되었습니다." , iTotalTime);

            if (iRet == 0)
            {
                lboxMake.Items.Add("----------------------");
                lboxMake.Items.Add("주문 완료 확인");
            }
            else
            {
                lboxMake.Items.Add("----------------------");
                lboxMake.Items.Add("시간 초과");
            }
        }

    }
}

```





## 부모 form

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

namespace _24_Delegate
{
    public partial class Form1 : Form
    {
        public delegate int delFunDough_Edge(int i);
        public delegate int delFunTopping(string strOder, int Ea);

        

        // 전체 가격
        int _iTotalPrice = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            // 주문을 담을 dictionary
            Dictionary<string, int> dPizzaOrder = new Dictionary<string, int>();

            delFunDough_Edge delDough = new delFunDough_Edge(fDough);
            delFunDough_Edge delEdge = new delFunDough_Edge(fEdge);

            delFunTopping delTopping = null;

            int iDoughOrder = 0;
            int iEdgeOrder = 0;
            // 도우 선택
            if (rdbDough1.Checked)
            {
                iDoughOrder = 1;
                dPizzaOrder.Add("오리지널", 1);
            }
            else if (rdbDough2.Checked)
            {
                iDoughOrder = 2;
                dPizzaOrder.Add("씬", 1);
            }

            //delDough(iDoughOrder);


            // 엣지 선택
            if (rdbEdge1.Checked)
            {
                iEdgeOrder = 1;
                dPizzaOrder.Add("리치골드", 1);
            }
            else if (rdbEdge2.Checked)
            {
                iEdgeOrder = 2;
                dPizzaOrder.Add("치즈크러스트", 1);
            }

            //delEdge(iEdgeOrder);

            fCallBackDelegate(iDoughOrder, delDough);
            fCallBackDelegate(iEdgeOrder, delEdge);


            // 토핑 선택
            if (cboxTopping1.Checked)
            {
                delTopping += fTopping1;
                dPizzaOrder.Add("소세지", (int)numEa.Value);
            }

            if (cboxTopping2.Checked)
            {
                delTopping += fTopping2;
                dPizzaOrder.Add("감자", (int)numEa.Value);
            }

            if (cboxTopping3.Checked)
            {
                delTopping += fTopping3;
                dPizzaOrder.Add("치즈", (int)numEa.Value);
            }

            delTopping("토핑", (int)numEa.Value);

            

            //추가된 코드
            fmLoading(dPizzaOrder);
        }

        #region Function
        /// <summary>
        /// 0 : 선택안함 1 : 오리지널  2: 씬
        /// </summary>
        /// <param name="iOrder"></param>
        /// <returns></returns>
        private int fDough(int iOrder)
        {
            string strOrder = string.Empty;
            int iPrice = 0;

            if (iOrder == 1)
            {
                iPrice = 10000;
                strOrder = string.Format("도우는 오리지널을 선택 하셨습니다. ({0}원)", iPrice.ToString());
            }
            else if (iOrder == 2)
            {
                iPrice = 10000;
                strOrder = string.Format("도우는 오리지널을 선택 하셨습니다. ({0}원)", iPrice.ToString());
            }
            else
            {
                strOrder = "도우를 선택하지 않았습니다";
            }

            flboxOrderRed(strOrder);

            return _iTotalPrice += iPrice;
        }

        /// <summary>
        /// 0 : 선택안함 1 : 리치골드  2: 치즈크러스트
        /// </summary>
        /// <param name="iOrder"></param>
        /// <returns></returns>
        private int fEdge(int iOrder)
        {
            string strOrder = string.Empty;
            int iPrice = 0;

            if (iOrder == 1)
            {
                iPrice = 1000;
                strOrder = string.Format("엣지는 리치골드를 선택 하셨습니다. ({0}원)", iPrice.ToString());
            }
            else if (iOrder == 2)
            {
                iPrice = 2000;
                strOrder = string.Format("엣지는 치즈크러스트를 선택 하셨습니다. ({0}원)", iPrice.ToString());
            }
            else
            {
                strOrder = "엣지를 선택하지 않았습니다";
            }

            flboxOrderRed(strOrder);
            return _iTotalPrice += iPrice;
        }

        public int fCallBackDelegate(int i, delFunDough_Edge dFunction)
        {
            return dFunction(i);
        }

        private int fTopping1(string Order, int iEa)
        {
            string strOrder = string.Empty;
            int iPrice = iEa * 500;

            strOrder = string.Format("소세지 {0} {1} 개를 선택 하였습니다. : ({2}원 (1ea 500원)", Order, iEa, iPrice);

            flboxOrderRed(strOrder);

            return _iTotalPrice = _iTotalPrice + iPrice;
        }

        private int fTopping2(string Order, int iEa)
        {
            string strOrder = string.Empty;
            int iPrice = iEa * 200;

            strOrder = string.Format("감자 {0} {1} 개를 선택 하였습니다. : ({2}원 (1ea 200원)", Order, iEa, iPrice);

            flboxOrderRed(strOrder);

            return _iTotalPrice = _iTotalPrice + iPrice;
        }

        private int fTopping3(string Order, int iEa)
        {
            string strOrder = string.Empty;
            int iPrice = iEa * 300;

            strOrder = string.Format("치즈 {0} {1} 개를 선택 하였습니다. : ({2}원 (1ea 300원)", Order, iEa, iPrice);

            flboxOrderRed(strOrder);

            return _iTotalPrice = _iTotalPrice + iPrice;
        }

        private void flboxOrderRed(string strOrder)
        {
            lboxOrder.Items.Add(strOrder);
        }
        #endregion

        #region event 예제
        frmPizza fPizza;

        private void fmLoading(Dictionary<string, int> dPizzaOrder)
        {
            if (fPizza != null)
            {
                fPizza.Dispose();
                fPizza = null;
            }

            fPizza = new frmPizza();
            fPizza.eventdelPizzaComplete += FPizza_eventdelPizzaComplete;
            fPizza.Show();

            fPizza.fPizzaCheck(dPizzaOrder);
        }

        private int FPizza_eventdelPizzaComplete(string strResult, int iTime)
        {

            flboxOrderRed("-------------------");
            flboxOrderRed(string.Format("{0} / 걸린시간 : {1}", strResult, iTime));

            return 0;
        }

        #endregion

    }
}

```

