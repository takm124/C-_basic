# DataRow, DataTable, DataSet

- DataRow : DataTable에서 한 줄에 대한 정보
- DataTable : Table 형태의 데이터
- DataSet : Data들의 집합 (DataTable 여러개)





- TableLayerPanel 의 한칸에는 하나의 도구만 들어간다
  - 남자 여자를 판넬하나에 넣기 위해서 Panel 도구를 넣어줘야함



- 프로젝트 개요
  - 반, 이름, 성별, 특이사항을 적어서 DataTable에 삽입한다.
  - 이름과 반이 일치하면 성별과 특이사항을 수정할 수 있다.
  - 선택한 row를 삭제할 수 있다.
  - 각각의 반이 DataTable이며 DataSet을 통해 테이블을 관리한다.
  - Set에서 Table을 찾고 거기서 또 데이터를 찾는 숙련도가 필요하다.





![DataRow DataTable DataSet](https://user-images.githubusercontent.com/72305146/135393583-dc721d04-aad7-4d5a-ad59-4568284d7fe3.png)





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

namespace _23_DataRow_DataTable_DataSet
{
    public partial class Form1 : Form
    {
        DataSet ds = new DataSet(); // 학급에 대한 정보 (1반, 2반 3반 ...)
        

        public Form1()
        {
            InitializeComponent();
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            // DataSet에 DataTable이 있는지 확인
            bool bCheckIsTable = false;

            if (ds.Tables.Contains(cboxRegClass.Text))
            {
                bCheckIsTable = true;
            }

            // 테이블 선언
            DataTable dt = null;


            // 기존 테이블이 있는지 확인
            if (!bCheckIsTable)
            {
                // 테이블 초기화, 매개변수는 테이블 이름
                dt = new DataTable(cboxRegClass.Text);

                // 열 생성
                DataColumn colName = new DataColumn("NAME", typeof(string));
                DataColumn colSex = new DataColumn("SEX", typeof(string));
                DataColumn colRef = new DataColumn("REF", typeof(string));

                dt.Columns.Add(colName);
                dt.Columns.Add(colSex);
                dt.Columns.Add(colRef);
            }
            else
            {
                dt = ds.Tables[cboxRegClass.Text];
            }

            // 행에 자료 삽입
            DataRow row = dt.NewRow();
            row["NAME"] = tboxRegName.Text;
            if (rdbRegSexMale.Checked)
            {
                row["SEX"] = "남자";
            }
            else if (rdbRegSexFeMale.Checked)
            {
                row["SEX"] = "여자";
            }
            row["REF"] = tboxRegRef.Text;

            // 테이블에 등록
            //dt.Rows.Add(row);

            if (bCheckIsTable)
            {
                ds.Tables[cboxRegClass.Text].Rows.Add(row);
            }
            else
            {
                dt.Rows.Add(row);
                ds.Tables.Add(dt);
            }

            viewRefresh();
        }

        // 수정
        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tboxRegName.Text))
            {
                foreach (DataRow item in ds.Tables[cboxRegClass.Text].Rows) // 선택한 반의 row들 스캔
                {
                    if (item["NAME"].Equals(tboxRegName.Text)) //row 중에 일치하는 이름이 있는지
                    {
                        if (rdbRegSexMale.Checked)
                        {
                            item["SEX"] = "남자";
                        }
                        else if (rdbRegSexFeMale.Checked)
                        {
                            item["SEX"] = "여자";
                        }
                        item["REF"] = tboxRegRef.Text;
                    }
                }
            }
            viewRefresh();
        }

        // 삭제
        private void btnViewDataDel_Click(object sender, EventArgs e)
        {
            int isSelectedRow = dgvViewInfo.SelectedRows[0].Index;

            ds.Tables[cboxViewClass.Text].Rows.RemoveAt(isSelectedRow);
            viewRefresh();
        }

        private void cboxViewClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            viewRefresh();
        }

        public void viewRefresh()
        {
            dgvViewInfo.DataSource = ds.Tables[cboxViewClass.Text];

            foreach (DataGridViewRow oRow in dgvViewInfo.Rows)
            {
                oRow.HeaderCell.Value = oRow.Index.ToString();
            }

            dgvViewInfo.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

        
    }
}

```

