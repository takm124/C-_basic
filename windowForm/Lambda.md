# Lambda

- 사용 목적
  - 간단하고 재활용이 필요 없는 함수를 만들 때 사용 ( 무명 메소드 처럼 )
  - 함수의 인자 값이 메소드 형태일 경우 주로 사용



- 형태 종류

  - 식형식 람다식

  - 문형식 람다식

  - 제네릭 형태의 무명 메소드 Func

  - 제네릭 형태의 무명 메소드 Action

    

- 프로젝트
  - UI의 특이점은 없고 코드만 확인하면된다.
  - 람다식을 활용할 수 있는 각종 방법들에 대해 기술



![lambda](https://user-images.githubusercontent.com/72305146/136123621-c0725918-d6d3-4de5-9292-5df096698185.png)



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

namespace _30_Lambda
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Action _aStepCheck = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            _aStepCheck = () => lblStepCheck.Text = string.Format(" - 다음 Step은 {0}.{1}", iNowStep, ((enumLambdaCase)iNowStep).ToString());  // 람다식으로 등록
            _aStepCheck();  // 다음 Step을 표시하기 위해 Action을 호출

            ButtonColorChange();
        }

        private void ButtonColorChange()
        {
            // Event 함수에서 색상 호출
            btnColorChange_1.Click += BtnColorChange_1_Click;

            // 무명 메소드 호출
            btnColorChange_2.Click += delegate (object sender, EventArgs e)
            {
                btnColorChange_2.BackColor = Color.Brown;
            };

            // 람다식
            btnColorChange_3.Click += (sender, e) => btnColorChange_3.BackColor = Color.Coral;
            
        }

        private void BtnColorChange_1_Click(object sender, EventArgs e)
        {
            btnColorChange_1.BackColor = Color.Aqua;
        }

        int iNowStep = 0;
        delegate int delIntFunc(int a, int b);
        delegate string delStringFunc();

        private void btnNext_Click(object sender, EventArgs e)
        {
            Lambda(iNowStep);
            iNowStep++;
            _aStepCheck();
        }

        private void Lambda(int iCase)
        {
            switch (iCase)
            {
                case (int)enumLambdaCase.식형식_람다식:
                    // 식형식 람다식
                    delIntFunc dint = (a, b) => a + b;
                    int iRet = dint(4, 5);
                    lboxResult.Items.Add(iRet.ToString());

                    delStringFunc dString = () => string.Format("Lambda Sample 식형식");
                    string strRet = dString();
                    lboxResult.Items.Add(strRet);

                    break;
                case (int)enumLambdaCase.문형식_람다식:
                    // 문형식 람다식
                    delStringFunc dstrSegment = () =>
                     {
                         return string.Format("Lambda Sample 문형식");
                     };
                    string strSegRet = dstrSegment();
                    lboxResult.Items.Add(strSegRet);

                    break;
                case (int)enumLambdaCase.제네릭_형태의_무명메소드_Func:
                    // 제네릭 형태의 무명 메소드

                    // func : 반환 값이 있는 형태
                    Func<int, int, int> fint = (a, b) => a * b;
                    int fintRet = fint(4, 6);
                    lboxResult.Items.Add(fintRet.ToString());

                    break;
                case (int)enumLambdaCase.제네릭_형태의_무명메소드_Action:
                    // Action

                    Action<string, string> aString = (a, b) =>
                    {
                        string strText = string.Format("인자값 {0}, {1}", a, b);
                        lboxResult.Items.Add(strText);
                    };

                    aString("잠이", "옵니다");
                    break;
                case (int)enumLambdaCase.제네릭_형태의인자_반환_예제:
                    int[] iGroup = { 1, 3, 5, 7, 9 };
                    int iCount = iGroup.Sum(x => x);
                    lboxResult.Items.Add(iCount.ToString());
                    break;
                default:
                    break;
            }
        }

        enum enumLambdaCase
        {
            식형식_람다식 = 0,
            문형식_람다식 = 1,
            제네릭_형태의_무명메소드_Func = 2,
            제네릭_형태의_무명메소드_Action = 3,
            제네릭_형태의인자_반환_예제 = 4,
        }
    }
}

```

