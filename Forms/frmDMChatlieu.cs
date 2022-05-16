using System;
using System.Data;
using System.Windows.Forms;


namespace QlyBanHang
{
    public partial class frmDMChatlieu : Form
    {
        // Khai báo biến toàn cục
        DataTable tblCL;

        public frmDMChatlieu()
        {
            InitializeComponent();
        }

        private void frmDMChatlieu_Load(object sender, EventArgs e)
        {
            txtMachatlieu.Enabled = false;
            btnLuu.Enabled = false;
            btnBoqua.Enabled = false;
            Load_DataGridView();
        }

        //Với Load_DataGridView là thủ tục cục bộ được viết trong phần code của form
        //frmDM Chatlieu có tác dụng lấy dữ liệu từ bảng tblChatlieu đổ vào lưới DataGrid View.
        private void Load_DataGridView()
        {
            string sql;
            sql = "Select Machatlieu, Tenchatlieu from ChatLieu";
            tblCL = Functions.GetDataToTable(sql); //Return a data table 
            DataGridView.DataSource = tblCL;
            DataGridView.Columns[0].HeaderText = "Mã chất liệu";
            DataGridView.Columns[1].HeaderText = "Tên chất liệu";
            DataGridView.Columns[0].Width = 100;
            DataGridView.Columns[1].Width = 300;
            // Không cho phép thêm mới dữ liệu trực tiếp trên lưới
            DataGridView.AllowUserToAddRows = false;
            // Không cho phép sửa dữ liệu trực tiếp trên lưới, mà chỉ sửa bằng lập trình
            DataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void DataGridView_Click(object sender, EventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMachatlieu.Focus();
                return;
            }
            if (tblCL.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataGridView.CurrentRow.Cells["Machatlieu"].Value.ToString();
            DataGridView.CurrentRow.Cells["Tenchatlieu"].Value.ToString();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoqua.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnBoqua.Enabled = true;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            ResetValues();
            txtMachatlieu.Enabled = true;
            txtMachatlieu.Focus();
        }

        private void ResetValues()
        {
            txtMachatlieu.Text = "";
            txtTenchatlieu.Text = "";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtMachatlieu.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã chất liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMachatlieu.Focus();
                return;
            }
            if (txtTenchatlieu.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên chất liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenchatlieu.Focus();
                return;
            }
            sql = "SELECT Machatlieu FROM ChatLieu WHERE Machatlieu=N'" + txtMachatlieu.Text.Trim() + "'";
            if (Functions.CheckKey(sql))
            {
                MessageBox.Show("Mã chất liệu này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMachatlieu.Focus();
                txtMachatlieu.Text = "";
                return;
            }
            sql = "INSERT INTO Chatlieu(Machatlieu,Tenchatlieu) VALUES(N'" + txtMachatlieu.Text + "',N'" + txtTenchatlieu.Text + "')";
            Functions.RunSql(sql);
            Load_DataGridView();
            ResetValues();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnBoqua.Enabled = false;
            btnLuu.Enabled = false;
            txtMachatlieu.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblCL.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMachatlieu.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                sql = "DELETE tblChatlieu WHERE Machatlieu=N'" + txtMachatlieu.Text + "'";
                Functions.RunSqlDel(sql);
                Load_DataGridView();
                ResetValues();
            }


        }

        private void btnBoqua_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnBoqua.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtMachatlieu.Enabled = false;
        }
        private void txtMachatlieu_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

        private void txtTenchatlieu_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
