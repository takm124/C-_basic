# LINQ

- Language Integrated Query
- 데이터 뭉치에서 Data를 검색하는 방법이라고 생각하면 편하다
  - 그 방법이 SQL의 쿼리 구문과 비슷하다.
  - from, where, orderby, select로 구성됨



- 프로젝트 개요
  - 데이터 테이블을 등록하고 정렬, 필터링 하는 법을 숙지하기



![LINQ](https://user-images.githubusercontent.com/72305146/136131237-3120742d-756a-45d2-b1be-df0c16d73bd1.png)



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

namespace _31_LINQ
{
    public partial class Form1 : Form
    {
        DataTable dt;

        const string sLevel = "LEVEL";
        const string sName = "NAME";
        const string sAttribute = "ATTRIBUTE";

        enum EnumName
        {
            슬라임,
            가고일,
            골렘,
            코볼트,
            고블린,
            고스트,
            언데드,
            노움,
            드래곤,
            웜,
            서큐버스,
            데빌,
            만티코어,
            바실리스트,
        }

        enum EnumAttribute
        {
            불,
            물,
            바람,
            번개,
            어둠,
            빛,
            땅,
            나무,
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // DataTable 생성
            DataTableCreate();

            // 정보 생성
            EnemeyCreate();

            // 콤보 박스
            cBoxCreate();
        }

        private void DataTableCreate()
        {
            dt = new DataTable("Enemy");

            // DataColumn 생성
            DataColumn colLevel = new DataColumn(sLevel, typeof(int));
            DataColumn colName = new DataColumn(sName, typeof(string));
            DataColumn colAttribute = new DataColumn(sAttribute, typeof(string));

            dt.Columns.Add(colLevel);
            dt.Columns.Add(colName);
            dt.Columns.Add(colAttribute);
        }

        private void EnemeyCreate()
        {
            Random rd = new Random();

            foreach (EnumName oName in Enum.GetValues(typeof(EnumName)))
            {
                DataRow dr = dt.NewRow();
                dr[sLevel] = rd.Next(1, 11); // 레벨은 1부터 10까지
                dr[sName] = oName.ToString();

                int iEnumLength = Enum.GetValues(typeof(EnumAttribute)).Length; // EnumAttribute의 개수
                int iAttribute = rd.Next(iEnumLength);
                dr[sAttribute] = ((EnumAttribute)iAttribute).ToString();

                dt.Rows.Add(dr);
            }

            dgEnemyTable.DataSource = dt;
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            Button oBtn = sender as Button;
            DataTable dtCopy = dgEnemyTable.DataSource as DataTable;

            IEnumerable<DataRow> vSortTable = null;

            switch (oBtn.Name)
            {
                case "btnLevel":
                    vSortTable = from oRow in dtCopy.AsEnumerable()
                                 orderby oRow.Field<int>(sLevel)
                                 select oRow;
                    break;
                case "btnName":
                    vSortTable = from oRow in dtCopy.AsEnumerable()
                                 orderby oRow.Field<string>(sName)
                                 select oRow;
                    break;
                case "btnAttribute":
                    vSortTable = from oRow in dtCopy.AsEnumerable()
                                 orderby oRow.Field<string>(sAttribute)
                                 select oRow;
                    break;
            }
            dtCopy = vSortTable.CopyToDataTable();
            dgEnemyTable.DataSource = dtCopy;
        }

        private void cBoxCreate()
        {
            foreach (EnumAttribute oAttribute in Enum.GetValues(typeof(EnumAttribute)))
            {
                cboxAttribute.Items.Add(oAttribute);
            }

            cboxAttribute.SelectedIndex = 0;
        }


        private void btnFilter_Click(object sender, EventArgs e)
        {
            DataTable dtCopy = dgEnemyTable.DataSource as DataTable;
            IEnumerable<DataRow> vSortTable = from oRow in dtCopy.AsEnumerable()
                                              where oRow.Field<string>(sAttribute) == cboxAttribute.Text &&
                                                    (oRow.Field<int>(sLevel) >= nLevelMin.Value && oRow.Field<int>(sLevel) <= nLevelMax.Value)
                                              select oRow;

            if (vSortTable.Count() > 0)
            {
                dtCopy = vSortTable.CopyToDataTable();
                dgEnemyTable.DataSource = dtCopy;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dgEnemyTable.DataSource = dt;
        }
    }
}

```





