using AppManager.Views;
using System.Windows;

namespace AppManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly StudentRegistrationView _studentRegView = new();
        private readonly StudentManagementView _studentMgmtView = new();
        private readonly EmployeeManagementView _employeeView = new();
        private readonly AttendanceManagementView _attendanceView = new();

        public MainWindow()
        {
            InitializeComponent();
            SwitchTab(1);
        }

        private void SwitchTab(int tabIndex)
        {
            btnTab1.Tag = null;
            btnTab2.Tag = null;
            btnTab3.Tag = null;
            btnTab4.Tag = null;

            switch (tabIndex)
            {
                case 1:
                    btnTab1.Tag = "Active";
                    mainContent.Content = _studentRegView;
                    break;
                case 2:
                    btnTab2.Tag = "Active";
                    mainContent.Content = _studentMgmtView;
                    break;
                case 3:
                    btnTab3.Tag = "Active";
                    mainContent.Content = _employeeView;
                    break;
                case 4:
                    btnTab4.Tag = "Active";
                    mainContent.Content = _attendanceView;
                    break;
            }
        }

        private void btnTab1_Click(object sender, RoutedEventArgs e) => SwitchTab(1);
        private void btnTab2_Click(object sender, RoutedEventArgs e) => SwitchTab(2);
        private void btnTab3_Click(object sender, RoutedEventArgs e) => SwitchTab(3);
        private void btnTab4_Click(object sender, RoutedEventArgs e) => SwitchTab(4);
    }
}
