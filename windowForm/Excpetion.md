# Excpetion

- 예외처리가 필요한 이유
  - 프로그램이 다운될수도 있는 문제가 발생했을 때 핸들링
  - 문제가 발생했을 때 어떤 위치에서 발생했는지 추적하기 위해



- try-catch-finally 를 맹신하지 말고 if-else 통해 사전 작업도 필요하다.



- 프로젝트 자체에서 Exception이 중요하게 적용되지는 않는다.
  - 메소드 설계시 오류가 날 수 있는 부분을 예상하고 설계하는 습관을 길러야 한다.



- 프로젝트 개요
  - 컬러를 listbox에 추가하고 컬러를 선택한 채로 panel을 클릭하면 색 적용시킴
  - panel 적용시키는 방법에 대한 숙련도 필요
  - 사실, 색과 관련된 컨트롤을 많이 다뤄보지 않아 어색할 뿐 크게 어려운 사항은 없음



![Exception](https://user-images.githubusercontent.com/72305146/135970444-5495d62e-7c55-490d-8110-229fd733902e.png)



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

namespace _28_Exception
{
    public partial class Form1 : Form
    {

        Dictionary<string, Color> dColor = new Dictionary<string, Color>();
        Color oSelectedColor = new Color();

        public Form1()
        {
            InitializeComponent();

        }
		
        // 색 고르기
        private void pColor_MouseClick(object sender, MouseEventArgs e)
        {
            
            // 색상 테이블
            DialogResult dRet =  cDialogColor.ShowDialog();

            if (dRet == DialogResult.OK)
            {
                pColor.BackColor = cDialogColor.Color;
            }

            lblColorInfo.Text = pColor.BackColor.ToString();
        }
		
        // 투명도 적용
        private void tbarAlpha_Scroll(object sender, EventArgs e)
        {
            pColor.BackColor = Color.FromArgb(tbarAlpha.Value, pColor.BackColor);
            lblColorInfo.Text = pColor.BackColor.ToString();
        }
		
        // listbox에 색 추가
        private void btnColorSave_Click(object sender, EventArgs e)
        {
            Color oColor = pColor.BackColor;
            dColor.Add(oColor.ToString(), oColor);

            LBoxRefresh();
        }
		
        // listbox 초기화 메소드
        private void LBoxRefresh()
        {
            lboxColor.Items.Clear();

            foreach (string okey in dColor.Keys)
            {
                lboxColor.Items.Add(okey);
            }
        }
		
        // listbox item 삭제
        private void btnColorDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string key = lboxColor.SelectedItem.ToString();
                if (key != null && dColor.ContainsKey(key))
                {
                    dColor.Remove(key);
                }
                else
                {
                    MessageBox.Show("지정된 아이템이 없습니다");
                }
                LBoxRefresh();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
		
        
        // listbox에서 selecteditem 바뀔 때 마다 색 저장 값 바꾸는 메소드
        private void lboxColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            oSelectedColor = dColor[lboxColor.SelectedItem.ToString()];
        }
		
        // 색을 적용할 Panel 클릭
        private void Panel_Click(object sender, MouseEventArgs e)
        {
            try
            {
                if (sender is Panel)
                {
                    Panel oPanel = sender as Panel;
                    oPanel.BackColor = oSelectedColor;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

```

