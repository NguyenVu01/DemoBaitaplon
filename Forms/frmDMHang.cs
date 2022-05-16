using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using QlyBanHang.Forms;

namespace QlyBanHang.Forms
{
    public partial class frmDMHang : Form
    {
        // Khai báo biến toàn cục
        DataTable tblH;

        public frmDMHang()
        {
            InitializeComponent();
        }

        private void frmDMHang_Load(object sender, EventArgs e)
        {
            txtMahang.Enabled = false;
            btnLuu.Enabled = false;
            btnBoqua.Enabled = false;
            Load_DataGridView();
            Functions.FillCombo("SELECT Machatlieu, Tenchatlieu FROM Chatlieu",cboMachatlieu, "Machatlieu", "Tenchatlieu");
            cboMachatlieu.SelectedIndex = -1;
            ResetValues();
        }

        private void ResetValues()
        {
            txtMahang.Text = "";
            txtTenhang.Text = "";
            cboMachatlieu.Text = "";
            txtSoluong.Text = "0";
            txtDongianhap.Text = "0";
            txtDongiaban.Text = "0";
            txtSoluong.Enabled = false;
            txtDongianhap.Enabled = false;
            txtDongiaban.Enabled = false;
            txtAnh.Text = "";
            picAnh.Image = null;
            txtGhichu.Text = "";
        }

        private void Load_DataGridView()
        {
            string sql;
            sql = "SELECT Mahang, Tenhang, Machatlieu, Soluong, Dongianhap, Dongiaban FROM Hang";
            tblH = Functions.GetDataToTable(sql);
            DataGridView.DataSource = tblH;
            DataGridView.Columns[0].HeaderText = "Mã hàng";
            DataGridView.Columns[1].HeaderText = "Tên hàng";
            DataGridView.Columns[2].HeaderText = "Chất liệu";
            DataGridView.Columns[3].HeaderText = "Số lượng";
            DataGridView.Columns[4].HeaderText = "Đơn giá nhập";
            DataGridView.Columns[5].HeaderText = "Đơn giá bán";
            DataGridView.Columns[0].Width = 80;
            DataGridView.Columns[1].Width = 140;
            DataGridView.Columns[2].Width = 80;
            DataGridView.Columns[3].Width = 80;
            DataGridView.Columns[4].Width = 100;
            DataGridView.Columns[5].Width = 100;
            DataGridView.AllowUserToAddRows = false;
            DataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void DataGridView_Click(object sender, EventArgs e)
        {
            string ma;
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMahang.Focus();
                return;
            }
            if (tblH.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            txtMahang.Text = DataGridView.CurrentRow.Cells["Mahang"].Value.ToString();
            txtTenhang.Text = DataGridView.CurrentRow.Cells["Tenhang"].Value.ToString();
            ma = DataGridView.CurrentRow.Cells["Machatlieu"].Value.ToString();
            cboMachatlieu.Text = Functions.GetFieldValues("SELECT Tenchatlieu FROM Chatlieu WHERE Machatlieu = N'" + ma + "'");
            txtSoluong.Text = DataGridView.CurrentRow.Cells["Soluong"].Value.ToString();
            txtDongianhap.Text = DataGridView.CurrentRow.Cells["Dongianhap"].Value.ToString();
            txtDongiaban.Text = DataGridView.CurrentRow.Cells["Dongiaban"].Value.ToString();
            txtAnh.Text = Functions.GetFieldValues("SELECT Anh FROM Hang WHERE Mahang = N'" + txtMahang.Text + "'");
            picAnh.Image = Image.FromFile(txtAnh.Text);
            txtGhichu.Text = Functions.GetFieldValues("SELECT Ghichu FROM Hang WHERE Mahang = N'" + txtMahang.Text + "'");
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
            txtMahang.Enabled = true;
            txtMahang.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtMahang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã hàng", "Thông báo", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtMahang.Focus();
                return;
            }
            if (txtTenhang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên hàng", "Thông báo", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtTenhang.Focus();
                return;
            }
            if (cboMachatlieu.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập chất liệu", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMachatlieu.Focus();
                return;
            }
            if (txtAnh.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn ảnh minh họa cho hàng", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAnh.Focus();
                return;
            }
            sql = "SELECT Mahang FROM Hang WHERE Mahang=N'" + txtMahang.Text.Trim() + "'";
            if (Functions.CheckKey(sql))
            {
                MessageBox.Show("Mã hàng này đã có, bạn phải nhập mã khác", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMahang.Focus();
                txtMahang.Text = "";
                return;
            }
            sql = "INSERT INTO Hang(Mahang,Tenhang,Machatlieu,Soluong,Dongianhap,Dongiaban,Anh,Ghichu) VALUES(N'" + txtMahang.Text.Trim() +
                  "',N'" + txtTenhang.Text.Trim() + "',N'" + cboMachatlieu.SelectedValue.ToString() + "'," + txtSoluong.Text.Trim() +
                    "," + txtDongianhap.Text + "," + txtDongiaban.Text + ",'" + txtAnh.Text + "',N'" + txtGhichu.Text.Trim() + "')";
            Functions.RunSql(sql);
            Load_DataGridView();
            ResetValues();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnBoqua.Enabled = false;
            btnLuu.Enabled = false;
            txtMahang.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblH.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            if (txtMahang.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtTenhang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên hàng", "Thông báo", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtTenhang.Focus();
                return;
            }
            if (cboMachatlieu.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập chất liệu", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMachatlieu.Focus();
                return;
            }
            if (txtAnh.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn ảnh minh họa cho hàng", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAnh.Focus();
                return;
            }
            sql = "UPDATE Hang SET  Tenhang=N'" + txtTenhang.Text.Trim().ToString() +
                "',Machatlieu=N'" + cboMachatlieu.SelectedValue.ToString() +
                "',Anh='" + txtAnh.Text +
                "',Ghichu=N'" + txtGhichu.Text + "' WHERE Mahang=N'" + txtMahang.Text + "'";
            Functions.RunSql(sql);
            Load_DataGridView();
            ResetValues();
            btnBoqua.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblH.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            if (txtMahang.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo",MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                sql = "DELETE Hang WHERE Mahang=N'" + txtMahang.Text + "'";
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
            txtMahang.Enabled = false;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "PNG(*.png)|*.png|JPG File(*.jpg)|*.jpg|All files(*.*)|*.*";
            dlgOpen.InitialDirectory = "D:\\Downloads";
            dlgOpen.FilterIndex = 2;
            dlgOpen.Title = "Chọn hình ảnh để hiển thị!";
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                picAnh.Image = Image.FromFile(dlgOpen.FileName);
                txtAnh.Text = dlgOpen.FileName;
            }
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            string sql;
            if ((txtMahang.Text == "") && (txtTenhang.Text == "") && (cboMachatlieu.Text ==""))
            {
                MessageBox.Show("Hãy nhập một điều kiện tìm kiếm!!!", "Yêu cầu ...",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sql = "SELECT * FROM tblHang WHERE 1=1";
            if (txtMahang.Text != "")
                sql = sql + " AND Mahang Like N'%" + txtMahang.Text + "%'";
            if (txtTenhang.Text != "")
                sql = sql + " AND Tenhang Like N'%" + txtTenhang.Text + "%'";
            if (cboMachatlieu.Text != "")
                sql = sql + " AND Machatlieu Like N'%" + cboMachatlieu.SelectedValue + "%'";
            tblH = Functions.GetDataToTable(sql);
            if (tblH.Rows.Count == 0)
                MessageBox.Show("Không có bản ghi thỏa mãn điều kiện!!!", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show("Có " + tblH.Rows.Count + " bản ghi thỏa mãn điều kiện!!!","Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            DataGridView.DataSource = tblH;
            ResetValues();
        }

        private void btnHienthi_Click(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT Mahang, Tenhang, Machatlieu, Soluong, Dongianhap, Dongiaban, Anh, Ghichu FROM Hang";
            tblH = Functions.GetDataToTable(sql);
            DataGridView.DataSource = tblH;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
