using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using De02.BLL;
using De02.Entities;

namespace De02
{
    public partial class FrmSanPham : Form
    {
        public FrmSanPham()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.frmSanpham_Load);

        }


        private SanPham_BLL spBLL = new SanPham_BLL();
        private LoaiSP_BLL loaiBLL = new LoaiSP_BLL();
        private bool isAdding = false; // phân biệt thêm hay sửa


        private void frmSanpham_Load(object sender, EventArgs e)
        {
            LoadLoaiSPToCombo();
            LoadListView();
            SetControlMode(true);
        }

        private void LoadLoaiSPToCombo()
        {
            cboLoaiSP.Items.Clear();
            var ds = loaiBLL.GetAll();
            foreach (var l in ds)
            {
                cboLoaiSP.Items.Add(l.TenLoai);
            }
        }

        private void LoadListView()
        {
            lvSanpham.Items.Clear();
            var ds = spBLL.GetAll();
            foreach (var sp in ds)
            {
                // lấy tên loại để hiển thị
                var tenLoai = loaiBLL.GetAll().Find(x => x.MaLoai == sp.MaLoai)?.TenLoai ?? "";
                ListViewItem item = new ListViewItem(sp.MaSP);
                item.SubItems.Add(sp.TenSP);
                item.SubItems.Add(sp.Ngaynhap.ToString("dd/MM/yyyy"));
                item.SubItems.Add(tenLoai);
                lvSanpham.Items.Add(item);
            }
        }

        private void SetControlMode(bool standby)
        {
            // standby=true: chờ thao tác (không đang thêm/sửa)
            txtMaSP.Enabled = !standby && isAdding; // nếu đang add cho phép chỉnh MaSP, sửa thì khóa MaSP
            txtTenSP.Enabled = !standby;
            dtNgaynhap.Enabled = !standby;
            cboLoaiSP.Enabled = !standby;

            btnThem.Enabled = standby;
            btnSua.Enabled = standby;
            btnXoa.Enabled = standby;
            btnLuu.Enabled = !standby;
            btnKLuu.Enabled = !standby;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            isAdding = true;
            txtMaSP.Clear(); txtTenSP.Clear(); cboLoaiSP.SelectedIndex = -1; dtNgaynhap.Value = DateTime.Today;
            SetControlMode(false);
            txtMaSP.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSP.Text)) { MessageBox.Show("Chọn sản phẩm để sửa."); return; }
            isAdding = false;
            SetControlMode(false);
        }

        private void btnKLuu_Click(object sender, EventArgs e)
        {
            isAdding = false;
            SetControlMode(true);
            // reset nếu cần
        }


        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // map gui -> DTO
                SanPham sp = new SanPham
                {
                    MaSP = txtMaSP.Text.Trim(),
                    TenSP = txtTenSP.Text.Trim(),
                    Ngaynhap = dtNgaynhap.Value,
                    MaLoai = loaiBLL.GetMaLoaiByTen(cboLoaiSP.Text)
                };

                if (isAdding)
                    spBLL.Add(sp);
                else
                    spBLL.Update(sp);

                MessageBox.Show("Lưu thành công.");
                isAdding = false;
                LoadListView();
                SetControlMode(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSP.Text)) { MessageBox.Show("Chọn sản phẩm để xóa."); return; }
            if (MessageBox.Show("Bạn có muốn xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    spBLL.Delete(txtMaSP.Text.Trim());
                    LoadListView();
                    MessageBox.Show("Đã xóa.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string key = txtTim.Text.Trim();
            if (string.IsNullOrEmpty(key))
            {
                LoadListView();
                return;
            }
            lvSanpham.Items.Clear();
            var ds = spBLL.Search(key);
            foreach (var sp in ds)
            {
                var tenLoai = loaiBLL.GetAll().Find(x => x.MaLoai == sp.MaLoai)?.TenLoai ?? "";
                ListViewItem item = new ListViewItem(sp.MaSP);
                item.SubItems.Add(sp.TenSP);
                item.SubItems.Add(sp.Ngaynhap.ToString("dd/MM/yyyy"));
                item.SubItems.Add(tenLoai);
                lvSanpham.Items.Add(item);
            }
        }


        private void btnThoat_Click(object sender, EventArgs e)
        {
            // Hiện hộp thoại hỏi người dùng trước khi thoát
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn thoát chương trình không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            // Nếu người dùng chọn "Yes" thì đóng form
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }





        private void lvSanpham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSanpham.SelectedItems.Count == 0) return;
            var it = lvSanpham.SelectedItems[0];
            txtMaSP.Text = it.SubItems[0].Text;
            txtTenSP.Text = it.SubItems[1].Text;
            dtNgaynhap.Value = DateTime.ParseExact(it.SubItems[2].Text, "dd/MM/yyyy", null);
            cboLoaiSP.Text = it.SubItems[3].Text;
        }

        
    }
}
