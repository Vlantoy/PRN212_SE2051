using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AppManager.Views
{
    /// <summary>
    /// Interaction logic for StudentRegistrationView.xaml
    /// </summary>
    public partial class StudentRegistrationView : UserControl
    {
        public StudentRegistrationView()
        {
            InitializeComponent();
        }

        private void btnDangKy_Click(object sender, RoutedEventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            if (string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Vui lòng nhập Họ tên!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtHoTen.Focus();
                return;
            }

            string maSV = txtMaSV.Text.Trim();
            if (string.IsNullOrEmpty(maSV))
            {
                MessageBox.Show("Vui lòng nhập Mã sinh viên!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtMaSV.Focus();
                return;
            }

            if (rbNam.IsChecked != true && rbNu.IsChecked != true)
            {
                MessageBox.Show("Vui lòng chọn Giới tính!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!dpNgaySinh.SelectedDate.HasValue)
            {
                MessageBox.Show("Vui lòng chọn Ngày sinh!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                dpNgaySinh.Focus();
                return;
            }

            if (cbKhoa.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Khoa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbKhoa.Focus();
                return;
            }

            string gioiTinh = rbNam.IsChecked == true ? "Nam" : "Nữ";
            string ngaySinh = dpNgaySinh.SelectedDate.Value.ToString("dd/MM/yyyy");
            string khoa = (cbKhoa.SelectedItem as ComboBoxItem)?.Content.ToString() ?? cbKhoa.Text;

            List<string> listSoThich = new List<string>();
            if (chkLapTrinh.IsChecked == true) listSoThich.Add("Lập trình");
            if (chkDocSach.IsChecked == true) listSoThich.Add("Đọc sách");
            if (chkTheThao.IsChecked == true) listSoThich.Add("Thể thao");
            string soThich = listSoThich.Count > 0 ? string.Join(", ", listSoThich) : "Không có";

            int mucDo = (int)sliderMucDo.Value;

            lblHoTenResult.Text = $"Họ tên: {hoTen}";
            lblMaSVResult.Text = $"Mã sinh viên: {maSV}";
            lblGioiTinhResult.Text = $"Giới tính: {gioiTinh}";
            lblNgaySinhResult.Text = $"Ngày sinh: {ngaySinh}";
            lblKhoaResult.Text = $"Khoa: {khoa}";
            lblSoThichResult.Text = $"Sở thích: {soThich}";
            lblMucDoResult.Text = $"Mức độ yêu thích: {mucDo}%";

            borderResult.Visibility = Visibility.Visible;
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            txtHoTen.Clear();
            txtMaSV.Clear();
            rbNam.IsChecked = false;
            rbNu.IsChecked = false;
            dpNgaySinh.SelectedDate = null;
            cbKhoa.SelectedIndex = 0;
            chkLapTrinh.IsChecked = false;
            chkDocSach.IsChecked = false;
            chkTheThao.IsChecked = false;
            sliderMucDo.Value = 50;

            borderResult.Visibility = Visibility.Collapsed;
            txtHoTen.Focus();
        }
    }
}
