# Recursion

- 재귀는 함수 스스로를 다시 호출하는 것을 의미함
- winForm에서 재귀를 사용하는 경우는 계층간의 처리를 할 때



- 프로젝트 개요
  - GroupBox 1단계
  - GroupBox 1단계, 2단계
  - GroupBox 1단계, 2단계, 3단계 로 총 3개의 그룹박스가 구성
  - 단계와 텍스트, 컨트롤을 선택하면 해당하는 컨트롤에 텍스트 삽입



![recursion](https://user-images.githubusercontent.com/72305146/136338684-0cc52f27-0d9f-450b-bff3-c8a677d74672.png)



## Main

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

namespace _37_Recursion
{
    public partial class Form1 : Form
    {
        enum eControlType
        {
            Unknown,
            Label,
            TextBox,
            Button,
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnExe_Click(object sender, EventArgs e)
        {
            // Control Level, Control Type, Control Text
            int iLevel = (int)numLevel.Value;

            eControlType eCType = eControlType.Unknown;
            if (rdoLabel.Checked) eCType = eControlType.Label;
            else if (rdoTextBox.Checked) eCType = eControlType.TextBox;
            else if (rdoButton.Checked) eCType = eControlType.Button;

            string strChageText = tboxChangeText.Text;
			
            ControlSearch(gboxCheckList, iLevel, eCType, strChageText);
        }

        private void ControlSearch(GroupBox CheckList, int level, eControlType eType, string strChangeText)
        {
            foreach (var item in CheckList.Controls)
            {
                if (CheckList.Text.Equals("Level " + level))
                {
                    switch (eType)
                    {
                        case eControlType.Label:
                            if (item is Label)
                            {
                                ((Label)item).Text = strChangeText;
                                lboxResult.Items.Add(string.Format("현재 GroupBox : {0}, Label Text : {1} 로 변경", CheckList.Text, strChangeText));
                            }
                            break;
                        case eControlType.TextBox:
                            if (item is TextBox)
                            {
                                ((TextBox)item).Text = strChangeText;
                                lboxResult.Items.Add(string.Format("현재 GroupBox : {0}, Textbox Text : {1} 로 변경", CheckList.Text, strChangeText));
                            }
                            break;
                        case eControlType.Button:
                            if (item is Button)
                            {
                                ((Button)item).Text = strChangeText;
                                lboxResult.Items.Add(string.Format("현재 GroupBox : {0}, Button Text : {1} 로 변경", CheckList.Text, strChangeText));
                            }
                            break;

                    }
                }

                if (item is GroupBox)
                {
                    lboxResult.Items.Add(string.Format("현재 GroupBox : {0}, 다음 GroupBox : {1} 로 이동", CheckList.Text, ((GroupBox)item).Text));
                    ControlSearch((GroupBox)item, level, eType, strChangeText);    // 현재 호출된 함수 내에서 동일한 함수를 다시 호출 함!! (재귀 함수)
                }
            }

            if (CheckList == gboxCheckList)
            {
                lboxResult.Items.Add(string.Format("END"));
            }

        }
    }
}

```

