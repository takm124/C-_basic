# XML

- 이전에는 단순히 Text 파일의 입출력을 다뤄봤다.
- 조금더 복잡한 형태인 XML을 저장하고 불러와보자

- 포인트
  - static : 프로그램 실행시 메모리에 할당, 종료시 해제
  - using : 특정 함수를 영역내에서만 사용, 영역에서 나가면 자동으로 Dispose
  - Dictionary : key-value의 해시테이블 형태의 컬렉션



- 프로그램 개요
  - XML을 컨트롤할 클래스를 따로 생성
  - UI는 스트림에서 사용했던 것 그대로 사용하되 버튼 이벤트만 살짝 바꿔줌



- 컨트롤 할 XML 형태

```xml
<?xml version="1.0" encoding="UTF-8"?>

-<SETTING ID="0001">
	<TEXT_DATA>test</TEXT_DATA>
	<CBOX_DATA>True</CBOX_DATA>
	<NUMBER_DATA>2</NUMBER_DATA>
</SETTING>
```





## XML 컨트롤 클래스

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace _16_스트림
{
    class CXMLControl
    {
        public static string _TEXT_DATA = "TEXT_DATA";
        public static string _CBOX_DATA = "CBOX_DATA";
        public static string _NUMBER_DATA = "NUMBER_DATA";

        public Dictionary<string, string> fXML_Reader(string strXMLPath)
        {
            Dictionary<string, string> DXMLConfig = new Dictionary<string, string>();
            using (XmlReader rd = XmlReader.Create(strXMLPath))
            {
                while (rd.Read())
                {
                    if (rd.IsStartElement())
                    {
                        if (rd.Name.Equals("SETTING"))
                        {
                            string strID = rd["ID"];
                            rd.Read();

                            string strTEXT = rd.ReadElementContentAsString(_TEXT_DATA, "");
                            DXMLConfig.Add(_TEXT_DATA, strTEXT);

                            string strCBOX = rd.ReadElementContentAsString(_CBOX_DATA, "");
                            DXMLConfig.Add(_CBOX_DATA, strCBOX);

                            string strNUMBER = rd.ReadElementContentAsString(_NUMBER_DATA, "");
                            DXMLConfig.Add(_NUMBER_DATA, strNUMBER);
                        }
                    }
                }
            }

            return DXMLConfig;
        }

        // XML 파일 생성
        public void fXML_Writer(string strXMLPath, Dictionary<string, string> DXMLConfig)
        {
            using (XmlWriter wr = XmlWriter.Create(strXMLPath))
            {
                wr.WriteStartDocument();

                // SETTING
                wr.WriteStartElement("SETTING");
                wr.WriteAttributeString("ID", "0001");

                wr.WriteElementString(_TEXT_DATA, DXMLConfig[_TEXT_DATA]);
                wr.WriteElementString(_CBOX_DATA, DXMLConfig[_CBOX_DATA]);
                wr.WriteElementString(_NUMBER_DATA, DXMLConfig[_NUMBER_DATA]);

                wr.WriteEndElement();
                wr.WriteEndDocument();
            }
        }
    }
}

```





## Form

```c#
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _16_스트림
{
    public partial class Form1 : Form
    {
        CXMLControl _XML = new CXMLControl();
        Dictionary<string, string> _dData = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();
        }

        // 데이터 세팅
        private void btnConfigSet_Click(object sender, EventArgs e)
        {
            string strEnter = "\r\n"; // \r\n = 줄바꿈

            string strText = tboxData.Text;
            bool bChecked = cboxData.Checked;
            int iNumber = (int)numData.Value;

            StringBuilder sb = new StringBuilder();
            sb.Append(strText); 
            sb.Append(strEnter);
            sb.Append(bChecked.ToString());
            sb.Append(strEnter);
            sb.Append(iNumber.ToString());

            tboxConfigData.Text = sb.ToString();

            _dData.Clear();
            _dData.Add(CXMLControl._TEXT_DATA, strText);
            _dData.Add(CXMLControl._CBOX_DATA, bChecked.ToString());
            _dData.Add(CXMLControl._NUMBER_DATA, iNumber.ToString());
        }



        // 저장 
        private void btnSave_Click(object sender, EventArgs e)
        {
            string strFilePath = string.Empty;
            SFDialog.InitialDirectory = Application.StartupPath; // 프로그램 시작경로 (exe가 있는 폴더)
            SFDialog.FileName = "*.xml"; // 기본 파일명
            SFDialog.Filter = "xml files(*.xml)|*.xml|All files(*.*)|*.*";

            if (SFDialog.ShowDialog() == DialogResult.OK)
            {
                strFilePath = SFDialog.FileName;

                //StreamWriter swSFDialog = new StreamWriter(strFilePath); // 경로 설정
                //swSFDialog.WriteLine(tboxConfigData.Text); // 데이터 파일에 저장
                //swSFDialog.Close();
                //File.WriteAllText(strFilePath, tboxConfigData.Text); // 이런 형태도 가능


                // XML 추가분
                _XML.fXML_Writer(strFilePath, _dData);
            }


        }

        // 불러오기
        private void btnLoad_Click(object sender, EventArgs e)
        {
            string strFilePath = string.Empty;
            OFDialog.InitialDirectory = Application.StartupPath;
            OFDialog.FileName = "*.xml";
            OFDialog.Filter = "xml files(*.xml)|*.xml|All files(*.*)|*.*";

            StringBuilder sb = new StringBuilder();

            if (OFDialog.ShowDialog() == DialogResult.OK)
            {
                strFilePath = OFDialog.FileName;

                //StreamReader srOFDialog = new StreamReader(strFilePath, Encoding.UTF8, true);
                //while (srOFDialog.EndOfStream == false) // 스트림이 끝날 때 까지
                //{
                //    sb.Append(srOFDialog.ReadLine()); // 한줄 씩 읽어옴
                //    sb.Append("\r\n"); // CRLF
                //}

                sb.Append(File.ReadAllText(strFilePath)); // 간단하게 텍스트 불러오기
                tboxConfigData.Text = sb.ToString();

                // XML 부분
                _dData.Clear();
                _dData = _XML.fXML_Reader(strFilePath);

            }
        }

        // 가져온 데이터 config에 적용
        private void btnConfigRead_Click(object sender, EventArgs e)
        {
            string[] strConfig = tboxConfigData.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            tboxData.Text = strConfig[0];
            cboxData.Checked = bool.Parse(strConfig[1]);
            numData.Value = int.Parse(strConfig[2]);
        }
    }
}

```

