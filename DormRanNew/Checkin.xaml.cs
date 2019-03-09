using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace DormRanNew
{
    /// <summary>
    /// Checkin.xaml 的交互逻辑
    /// </summary>
    public partial class Checkin : MetroWindow
    {
        private ObservableCollection<officer> officers;

        private HashSet<string> departments;

        private ObservableCollection<officer> checkedOfficers;

        public Checkin()
        {
            InitializeComponent();
        }

        private async void btnCheckin_Click(object sender, RoutedEventArgs e)
        {
            string selected = (string)this.checkinComboBox.SelectedValue;

            try
            {
                if (!selected.Trim().Equals(""))
                {
                    officer selectedOfficer = this.officers.Where(p => p.officer_id.Equals(selected)).First();

                    checkin_history tmpHistory = new checkin_history();
                    tmpHistory.officer_id = selected;
                    tmpHistory.officer_name = selectedOfficer.officer_name;
                    tmpHistory.term = MainWindow.term;
                    tmpHistory.insert_date = DateTime.Now;

                    using (check_dorm_newEntities db = new check_dorm_newEntities())
                    {
                        db.checkin_history.Add(tmpHistory);
                        db.SaveChanges();
                        this.checkedOfficers.Add(selectedOfficer);
                        this.checkinDataGrid.ItemsSource = null;
                        this.checkinDataGrid.ItemsSource = this.checkedOfficers;
                    }

                    await this.ShowMessageAsync("", "签到成功！");
                }
            }
            catch (Exception exp) { }
        }

        private void checkinDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected_department = (string)this.checkinDepartment.SelectedValue;
            try
            {
                if (!selected_department.Trim().Equals(""))
                {
                    using (check_dorm_newEntities db = new check_dorm_newEntities())
                    {
                        this.officers = new ObservableCollection<officer>(db.officer.Where(p=>p.officer_department.Equals(selected_department)).ToList());
                        this.checkinComboBox.ItemsSource = this.officers;
                    }
                }
            }
            catch (Exception exp) { }
        }

        private void CheckinWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (check_dorm_newEntities db = new check_dorm_newEntities())
            {
                List<officer> tmp = db.officer.ToList();
                this.departments = new HashSet<string>();
                foreach (var tmpOfficer in tmp)
                {
                    this.departments.Add(tmpOfficer.officer_department);
                }

                this.checkinDepartment.ItemsSource = this.departments;
                this.checkedOfficers = new ObservableCollection<officer>();
            }
        }

        private void btnGroupOfficers_Click(object sender, RoutedEventArgs e)
        {
            GroupResult groupResultWindow = new GroupResult(checkedOfficers);
            groupResultWindow.Owner = this;
            groupResultWindow.ShowDialog();
        }
    }
}
