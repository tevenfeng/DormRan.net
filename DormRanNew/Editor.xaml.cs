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
    public class OfficerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value,
            System.Globalization.CultureInfo cultureInfo)
        {
            officer tmpOfficer = (value as BindingGroup).Items[0] as officer;
            if (tmpOfficer.officer_id == null || tmpOfficer.officer_name == null)
            {
                return new ValidationResult(false,
                    "所有项目都是必填项！");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }

    public class DormValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value,
            System.Globalization.CultureInfo cultureInfo)
        {
            dorm tmpDorm = (value as BindingGroup).Items[0] as dorm;
            if (tmpDorm.area < 5 && tmpDorm.area > 0 && tmpDorm.dorm_name != null
                && tmpDorm.group_id > 0 && tmpDorm.group_id < 4
                && (tmpDorm.gender == "男" || tmpDorm.gender == "女"))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "所有项目都是必填项！");
            }
        }
    }

    /// <summary>
    /// Editor.xaml 的交互逻辑
    /// </summary>
    public partial class Editor : MetroWindow
    {
        /// <summary>
        /// 用于判断当前编辑器是管理人员还是楼栋的数据
        /// true代表人员，false代表楼栋
        /// </summary>
        private Management management;

        private ObservableCollection<officer> officers;

        private ObservableCollection<dorm> dorms;

        private ObservableCollection<history> histories;

        public Editor(Management management)
        {
            this.management = management;

            InitializeComponent();
        }

        private void editorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (check_dorm_newEntities db = new check_dorm_newEntities())
            {
                // 读取已有数据
                if (this.management == Management.人员)
                {
                    this.Title = "人员管理";
                    this.officeGrid.Visibility = Visibility.Visible;
                    this.officers = new ObservableCollection<officer>(db.officer.ToList());
                    this.officeGrid.DataContext = this.officers;
                }
                else if (this.management == Management.楼栋)
                {
                    this.Title = "楼栋管理";
                    this.dormGrid.Visibility = Visibility.Visible;
                    this.dorms = new ObservableCollection<dorm>(db.dorm.ToList());
                    this.dormGrid.DataContext = this.dorms;
                }
                else if (this.management == Management.历史)
                {
                    this.Title = "历史记录查询";
                    this.historyGrid.Visibility = Visibility.Visible;
                    this.btnRemoveRecord.IsEnabled = false;
                    this.btnRemoveRecord.Visibility = Visibility.Hidden;
                    this.histories = new ObservableCollection<history>(db.history.ToList());
                    this.historyGrid.DataContext = this.histories;
                }
            }
        }

        private async void btnRemoveRecord_Click(object sender, RoutedEventArgs e)
        {
            if (this.management == Management.人员)
            {
                // 删除的是人员数据
                officer tmpOfficer = (officer)this.officeGrid.SelectedItem;
                using (check_dorm_newEntities db = new check_dorm_newEntities())
                {
                    MessageDialogResult result = await this.ShowMessageAsync("人员管理", "您确定要删除该行数据吗", MessageDialogStyle.AffirmativeAndNegative);
                    if (result != MessageDialogResult.Negative)
                    {
                        // 删除
                        db.officer.Remove(db.officer.Where(p => p.row_id.Equals(tmpOfficer.row_id)).First());
                        db.SaveChanges();
                        this.officers.Remove(tmpOfficer);
                        this.officeGrid.ItemsSource = null;
                        this.officeGrid.ItemsSource = this.officers;
                    }
                }
            }
            else if (this.management == Management.楼栋)
            {
                // 删除的是楼栋数据
                dorm tmpDorm = (dorm)this.dormGrid.SelectedItem;
                using (check_dorm_newEntities db = new check_dorm_newEntities())
                {
                    MessageDialogResult result = await this.ShowMessageAsync("楼栋管理", "您确定要删除该行数据吗", MessageDialogStyle.AffirmativeAndNegative);
                    if (result != MessageDialogResult.Negative)
                    {
                        // 删除
                        db.dorm.Remove(db.dorm.Where(p => p.dorm_name.Equals(tmpDorm.dorm_name)).First());
                        db.SaveChanges();
                        this.dorms.Remove(tmpDorm);
                        this.dormGrid.ItemsSource = null;
                        this.dormGrid.ItemsSource = this.dorms;
                    }
                }
            }
        }

        private void officeGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            // 获取编辑后的数据
            officer tmpOfficer = (officer)e.Row.DataContext;
            try
            {
                using (check_dorm_newEntities db = new check_dorm_newEntities())
                {
                    if (tmpOfficer.row_id.Equals(0))
                    {
                        // row_id == 0说明是新加入的行
                        officer newOfficer = new officer();
                        newOfficer.officer_id = tmpOfficer.officer_id;
                        newOfficer.officer_department = tmpOfficer.officer_department;
                        newOfficer.officer_name = tmpOfficer.officer_name;
                        newOfficer.officer_gender = tmpOfficer.officer_gender;
                        db.officer.Add(newOfficer);
                        db.SaveChanges();

                        // 添加完毕以后需要重新读取数据库中新加入行的row_id用于更新内存中缓存的row_id
                        this.officers.Where(p => p.officer_id.Equals(newOfficer.officer_id)).First().row_id = db.officer.Where(p => p.officer_id.Equals(newOfficer.officer_id)).First().row_id;
                    }
                    else
                    {
                        // row_id != 0说明是原先存在的行，修改即可
                        db.officer.Where(p => p.row_id.Equals(tmpOfficer.row_id)).First().officer_id = tmpOfficer.officer_id;
                        db.officer.Where(p => p.row_id.Equals(tmpOfficer.row_id)).First().officer_department = tmpOfficer.officer_department;
                        db.officer.Where(p => p.row_id.Equals(tmpOfficer.row_id)).First().officer_name = tmpOfficer.officer_name;
                        db.officer.Where(p => p.row_id.Equals(tmpOfficer.row_id)).First().officer_gender = tmpOfficer.officer_gender;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception exp) { }
        }

        private void dormGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            dorm tmpDorm = (dorm)e.Row.DataContext;

            try
            {
                using (check_dorm_newEntities db = new check_dorm_newEntities())
                {
                    if (tmpDorm.row_id.Equals(0))
                    {
                        dorm newDorm = new dorm();
                        newDorm.area = tmpDorm.area;
                        newDorm.group_id = tmpDorm.group_id;
                        newDorm.dorm_name = tmpDorm.dorm_name;
                        newDorm.floor_number = tmpDorm.floor_number;
                        newDorm.gender = tmpDorm.gender;

                        db.dorm.Add(newDorm);
                        db.SaveChanges();

                        // 添加完毕以后需要重新读取数据库中新加入行的row_id用于更新内存中缓存的row_id
                        this.dorms.Where(p => p.dorm_name.Equals(newDorm.dorm_name)).First().row_id = db.dorm.Where(p => p.dorm_name.Equals(newDorm.dorm_name)).First().row_id;
                    }
                    else
                    {
                        db.dorm.Where(p => p.row_id.Equals(tmpDorm.row_id)).First().area = tmpDorm.area;
                        db.dorm.Where(p => p.row_id.Equals(tmpDorm.row_id)).First().group_id = tmpDorm.group_id;
                        db.dorm.Where(p => p.row_id.Equals(tmpDorm.row_id)).First().dorm_name = tmpDorm.dorm_name;
                        db.dorm.Where(p => p.row_id.Equals(tmpDorm.row_id)).First().floor_number = tmpDorm.floor_number;
                        db.dorm.Where(p => p.row_id.Equals(tmpDorm.row_id)).First().gender = tmpDorm.gender;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception exp) { }
        }
    }
}
