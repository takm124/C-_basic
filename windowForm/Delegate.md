# Delegate

- Delegate = 대리자
  - 무엇은 대신하는가? -> 함수를 대신해서 호출함
  - C++의 포인터 역할



- 기본
  - method와 동일한 Type의 delegate 선언(반환 값, 파라미터)
  - 선언한 delegate 변수 생성
  - 생성한 delegate에 사용할 method 참조(연결)



- 콜백을 구현
  - 파라미터로 delegate 사용할 수 있음



![delegate1](https://user-images.githubusercontent.com/72305146/135401070-15508e23-2640-40f2-a895-0b82f29d2953.png)



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
            delFunDough_Edge delDough = new delFunDough_Edge(fDough);
            delFunDough_Edge delEdge = new delFunDough_Edge(fEdge);

            delFunTopping delTopping = null;

            int iDoughOrder = 0;
            int iEdgeOrder = 0;
            // 도우 선택
            if (rdbDough1.Checked)
            {
                iDoughOrder = 1;
                iEdgeOrder = 1;
            }
            else if (rdbDough2.Checked)
            {
                iDoughOrder = 2;
                iEdgeOrder = 2;
            }

            //delDough(iDoughOrder);


            // 엣지 선택
            if (rdbEdge1.Checked)
            {
                iEdgeOrder = 1;
            }
            else if (rdbEdge2.Checked)
            {
                iEdgeOrder = 2;
            }

            //delEdge(iEdgeOrder);

            fCallBackDelegate(iDoughOrder, delDough);
            fCallBackDelegate(iEdgeOrder, delEdge);


            // 토핑 선택
            if (cboxTopping1.Checked)
            {
                delTopping += fTopping1;
            }

            if (cboxTopping2.Checked)
            {
                delTopping += fTopping2;
            }

            if (cboxTopping3.Checked)
            {
                delTopping += fTopping3;
            }

            delTopping("토핑", (int)numEa.Value);

            flboxOrderRed("-------------------");
            flboxOrderRed(string.Format("전체 가격은 {0} 입니다", _iTotalPrice));
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
    }
}

```

