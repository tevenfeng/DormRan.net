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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace DormRanNew
{
    public enum Management { 人员, 楼栋, 历史, 签到 };

    public class Result
    {
        public string dormName { get; set; }
        public string gender { get; set; }
        public string floorsOfDorm { get; set; }
    }

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region 变量

        /// <summary>
        /// 数据库连接
        /// </summary>
        private check_dorm_oldEntities db;

        /// <summary>
        /// 经过筛选以后的本次可被选择的区域
        /// </summary>
        private List<int> areas;

        /// <summary>
        /// 提取所有宿舍名称，进行动画抽选
        /// </summary>
        private List<string> allDorms;

        /// <summary>
        /// 用于动画显示的floors
        /// </summary>
        private List<string> allFloors;

        /// <summary>
        /// 计时器是否开始，辅助变量
        /// </summary>
        private bool isTimer1Started = false;

        /// <summary>
        /// 动画效果计时器
        /// </summary>
        private System.Windows.Threading.DispatcherTimer timer1;

        /// <summary>
        /// 抽选宿舍显示计时器
        /// </summary>
        private System.Windows.Threading.DispatcherTimer timer2;

        /// <summary>
        /// 存储宿舍相关信息，加载到内存中，加快速度
        /// </summary>
        private List<dorm> myDorms;

        /// <summary>
        /// 存储选定区域的宿舍楼
        /// </summary>
        //private List<dorm> dormsSelected;

        /// <summary>
        /// 存储选定区域组1的宿舍楼
        /// </summary>
        private List<dorm> dormsGroup1;

        /// <summary>
        /// 显示选定区域组1的宿舍楼
        /// </summary>
        private ObservableCollection<Result> dormsShow1;

        /// <summary>
        /// 存储选定区域组2的宿舍楼
        /// </summary>
        private List<dorm> dormsGroup2;

        /// <summary>
        /// 显示选定区域组2的宿舍楼
        /// </summary>
        private ObservableCollection<Result> dormsShow2;

        /// <summary>
        /// 存储历史相关信息，加载到内存中，加快速度
        /// </summary>
        private List<history> myHistories;

        /// <summary>
        /// 若当前学期为第二学期，则存储上一学期的历史记录，可能为空
        /// </summary>
        private List<history> lastTermHistories;

        /// <summary>
        /// 存储新增条目
        /// </summary>
        private List<history> newHistories;

        /// <summary>
        /// 存储当前学期
        /// </summary>
        public static string term;

        /// <summary>
        /// 存储记录中最新的学期
        /// </summary>
        private string lastestTerm;

        /// <summary>
        /// 存储当前查询次数
        /// </summary>
        private int checkId;

        /// <summary>
        /// 随机数发生器
        /// </summary>
        private Random rd;

        /// <summary>
        /// 每学期抽查最大次数
        /// </summary>
        private const int checkTimes = 4;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载计时器，查询一些基本的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.db = new check_dorm_oldEntities();
            this.timer1 = new System.Windows.Threading.DispatcherTimer();
            this.timer1.Tick += new EventHandler(OnTimer1Event);
            this.timer1.Interval = TimeSpan.FromSeconds(1.0 / 200);

            this.timer2 = new System.Windows.Threading.DispatcherTimer();
            this.timer2.Tick += new EventHandler(OnTimer2Event);
            this.timer2.Interval = TimeSpan.FromSeconds(1);

            this.myDorms = this.db.dorm.ToList();  // 把宿舍信息拿出来缓存着，减少数据库的读取，
                                                   // 同时激活数据库连接，加速后续读取历史信息的速度
            this.allDorms = this.myDorms.Select(p => p.dorm_name).ToList();
            this.allFloors = this.InitFloors();
            this.myHistories = this.db.history.ToList();
            this.newHistories = new List<history>();
            this.dormsShow1 = new ObservableCollection<Result>();
            this.dormsShow2 = new ObservableCollection<Result>();

            term = getTerm();

            if (myHistories.Count == 0)
            {
                this.checkId = 1;
            }
            else
            {
                this.lastestTerm = myHistories.Last().term;
                this.checkId = myHistories.Last().check_id;

                if (term.Equals(this.lastestTerm))
                {
                    if (this.checkId == checkTimes)
                    {
                        this.btnStartSampling.IsEnabled = false;
                        this.samplingDormLabel.Content = "本学期";
                        this.samplingFloorLabel.Content = "抽选次数已满";
                    }
                    else
                    {
                        this.checkId++;
                    }
                }
                else
                {
                    this.checkId = 1;
                }
            }

            this.checkIdText.Text = "第 " + checkId + " 次抽选";
            this.rd = new Random();
        }

        /// <summary>
        /// 宿舍抽选按钮响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartSampling_Click(object sender, RoutedEventArgs e)
        {
            if (this.btnSaveRecord.IsEnabled)
            {
                this.btnSaveRecord.IsEnabled = false;
            }

            this.myHistories = this.db.history.ToList();
            this.lastTermHistories = this.getLastTermHistories();
            this.newHistories.Clear();
            this.areas = getAreasAvailable(term);
            getDormsSelected(this.areas[this.rd.Next(0, this.areas.Count)], out this.dormsGroup1,
                out this.dormsGroup2);

            this.dataGridGroupOne.ItemsSource = null;
            this.dataGridGroupTwo.ItemsSource = null;

            this.dormsShow1.Clear();
            this.dormsShow2.Clear();

            this.btnStartSampling.IsEnabled = false;
            this.timer1.Start();
            this.timer2.Start();
            this.isTimer1Started = true;
        }

        /// <summary>
        /// 抽选记录保存按钮响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSaveRecord_Click(object sender, RoutedEventArgs e)
        {
            this.checkId = (this.checkId % checkTimes) + 1;
            this.insertHistories(newHistories);
            this.btnSaveRecord.IsEnabled = false;
            if (this.checkId == 1)
            {
                this.btnStartSampling.IsEnabled = false;
                this.samplingDormLabel.Content = "本学期";
                this.samplingFloorLabel.Content = "抽选次数已满";
            }
            else
            {
                this.checkIdText.Text = "第 " + checkId + " 次抽选";
            }

            await this.ShowMessageAsync("", "保存成功！");
        }

        /// <summary>
        /// 初始化动画效果的楼层List
        /// </summary>
        /// <returns></returns>
        private List<string> InitFloors()
        {
            List<string> floors = new List<string>();
            floors.Add("A座");
            floors.Add("B座");
            floors.Add("1层 2层");
            floors.Add("1层 3层");
            floors.Add("1层 4层");
            floors.Add("1层 6层");
            floors.Add("2层 3层");
            floors.Add("2层 5层");
            floors.Add("3层 4层");
            floors.Add("3层 6层");
            floors.Add("1层 2层 4层");
            floors.Add("1层 3层 5层");
            floors.Add("2层 3层 6层");
            floors.Add("2层 3层 4层");
            floors.Add("3层 4层 6层");
            return floors;
        }

        /// <summary>
        /// 获取当前学期，去年9月~今年1月为第一学期，今年3~8月为第二学期
        /// </summary>
        /// <returns></returns>
        public string getTerm()
        {
            string nowTerm = "";
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            if (month >= 3 && month <= 8)
            {
                int pre = year % 100 - 1;
                int suffix = year % 100;
                nowTerm += "" + pre + suffix + "2";
            }
            else if (month >= 9)
            {
                int pre = year % 100;
                int suffix = year % 100 + 1;
                nowTerm += "" + pre + suffix + "1";
            }
            else
            {
                int pre = year % 100 - 1;
                int suffix = year % 100;
                nowTerm += "" + pre + suffix + "1";
            }

            this.termText.Text = "当前学期：" + nowTerm;
            return nowTerm;
        }

        /// <summary>
        /// 若当前学期为第二学期，获取上一学期所有历史记录，可能为空，也可能不足4次
        /// </summary>
        /// <returns></returns>
        private List<history> getLastTermHistories()
        {
            List<history> histories = new List<history>();

            if (term.Last() == '2')
            {
                string lastTerm = (int.Parse(term) - 1).ToString();
                histories = this.myHistories
                    .Where(p => p.term.Equals(lastTerm))
                    .ToList();
                Console.WriteLine(lastTerm);
            }

            return histories;
        }

        /// <summary>
        /// 计时器委托函数，用于随机变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimer1Event(object sender, EventArgs e)
        {
            //Random rd = new Random();
            int next = this.rd.Next(0, this.allDorms.Count);
            this.samplingDormLabel.Content = this.allDorms[next];
            next = this.rd.Next(0, this.allFloors.Count);
            this.samplingFloorLabel.Content = this.allFloors[next];
        }

        /// <summary>
        /// 计数器委托函数，用于停止timer1并显示抽选到的宿舍及楼层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimer2Event(object sender, EventArgs e)
        {
            if (this.isTimer1Started)
            {
                this.timer1.Stop();

                dorm firstDorm = new dorm();

                if (this.dormsGroup1.Count != 0)
                {
                    firstDorm = this.dormsGroup1.First();
                }
                else if (this.dormsGroup2.Count != 0)
                {
                    firstDorm = this.dormsGroup2.First();
                }

                List<string> floors = this.getFloors(firstDorm);
                string floorsSelected = "";
                string floorsOfDorm = "";

                for (int i = 0; i < floors.Count - 1; i++)
                {
                    floorsOfDorm += floors[i] + "层  ";
                    floorsSelected += floors[i] + " ";
                }

                floorsOfDorm += floors[floors.Count - 1] + "层";
                floorsSelected += floors[floors.Count - 1];

                this.samplingDormLabel.Content = firstDorm.dorm_name;
                this.samplingFloorLabel.Content = floorsOfDorm;
                Result result = new Result();
                result.dormName = firstDorm.dorm_name;
                result.gender = firstDorm.gender;
                result.floorsOfDorm = floorsOfDorm;
                AddNewHistory(firstDorm, floorsSelected);

                if (this.dormsGroup1.Count != 0)
                {
                    this.dormsShow1.Add(result);
                    this.dataGridGroupOne.ItemsSource = null;
                    this.dataGridGroupOne.ItemsSource = dormsShow1;
                    this.dormsGroup1.RemoveAt(0);
                }
                else if (this.dormsGroup2.Count != 0)
                {
                    this.dormsShow2.Add(result);
                    this.dataGridGroupTwo.ItemsSource = null;
                    this.dataGridGroupTwo.ItemsSource = dormsShow2;
                    this.dormsGroup2.RemoveAt(0);
                }

                if (this.dormsGroup2.Count == 0)
                {
                    this.timer2.Stop();
                    this.btnStartSampling.IsEnabled = true;
                    this.btnSaveRecord.IsEnabled = true;
                }

                this.isTimer1Started = false;
            }
            else
            {
                this.timer1.Start();
                this.isTimer1Started = true;
            }
        }

        /// <summary>
        /// 根据选择的学期得出本次还可以抽查的片区
        /// </summary>
        /// <param name="term">选择的学期号</param>
        /// <returns></returns>
        private List<int> getAreasAvailable(string term)
        {
            List<int> result = new List<int>();

            List<int> AllAreas = new List<int> { 1, 2, 3, 4 };

            List<int> areasSelected = getAreasSelected(term);

            foreach (int area in AllAreas)
            {
                if (!areasSelected.Exists((int s) => s.Equals(area)))
                {
                    result.Add(area);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取已经被选取的片区号
        /// </summary>
        /// <param name="term">选择的学期号</param>
        /// <returns></returns>
        private List<int> getAreasSelected(string term)
        {
            List<int> result;
            result = this.myHistories
                .Where(q => q.term.Equals(term))
                .Select(x => x.area)
                .Distinct().ToList();

            return result;
        }

        /// <summary>
        /// 获取选择区域的宿舍，并安男女分组顺序排列
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        private void getDormsSelected(int area, out List<dorm> dromsGroup1,
            out List<dorm> dromsGroup2)
        {
            List<dorm> dorms;
            dorms = this.myDorms
                .Where(q => q.area.Equals(area))
                .ToList();
            dromsGroup1 = dorms
                .Where(q => q.group_id.Equals(1))
                .ToList();
            dromsGroup2 = dorms
                .Where(q => q.group_id.Equals(2))
                .ToList();
        }

        /// <summary>
        /// 获取选定对应宿舍楼的抽查楼层
        /// </summary>
        /// <param name="dormsSelected"></param>
        /// <returns></returns>
        private List<string> getFloors(dorm dormSelected)
        {
            List<string> floorsSelected = new List<string>();
            List<int> floors = new List<int>();
            List<history> lastHistories = this.lastTermHistories
               .Where(p => p.dorm_name.Equals(dormSelected.dorm_name))
               .ToList();
            //Random rd = new Random();
            if (lastHistories.Count == 0)
            {
                int number = (int)dormSelected.floor_number;
                //当宿舍楼层数为奇数且不为1时，随机选择向上或向下选择
                if (number % 2 != 0)
                {
                    int next = this.rd.Next(0, 2);
                    number /= 2;
                    number += next;
                }
                else
                {
                    number /= 2;
                }

                //考虑多学期情况，应修改为如果当前学期为奇数，抽选楼层；
                //当前学期为偶数，选取上学期未选取的楼层
                for (int i = 0; i < number; i++)
                {
                    int floor = this.rd.Next(1, (int)dormSelected.floor_number + 1);
                    while (floors.Contains(floor))
                    {
                        floor = this.rd.Next(1, (int)dormSelected.floor_number + 1);
                    }
                    floors.Add(floor);
                }
            }
            else
            {
                for (int i = 1; i <= dormSelected.floor_number; i++)
                {
                    floors.Add(i);
                }

                string[] tmp = lastHistories[0].floor_id.Split(' ');
                List<int> lastTermFloors = new List<int>(Array.ConvertAll(tmp, int.Parse));

                floors = floors.Except(lastTermFloors).ToList();
            }

            floors.Sort();
            foreach (int floor in floors)
            {
                floorsSelected.Add(floor.ToString());
            }
            return floorsSelected;
        }

        private void AddNewHistory(dorm dormSelected, string floorsSelected)
        {
            history newHistory = new history();
            newHistory.term = term;
            newHistory.area = dormSelected.area;
            newHistory.dorm_name = dormSelected.dorm_name;
            newHistory.check_id = this.checkId;
            newHistory.floor_id = floorsSelected;
            newHistory.insert_date = DateTime.Now;
            newHistories.Add(newHistory);
        }

        /// <summary>
        /// 插入新增的抽查记录到history表中
        /// </summary>
        /// <param name="newHistoryies"></param>
        private void insertHistories(List<history> newHistoryies)
        {
            foreach (history newHistory in newHistoryies)
            {
                this.db.history.Add(newHistory);
            }
            this.db.SaveChanges();
        }

        private void btnStartSamplingGroup_Click(object sender, RoutedEventArgs e)
        {
            Checkin checkinWindow = new Checkin();
            checkinWindow.Owner = this;
            checkinWindow.ShowDialog();
        }

        private void btnDatabaseManagement_Click(object sender, RoutedEventArgs e)
        {
            Manager managerWindow = new Manager();
            managerWindow.Owner = this;
            managerWindow.ShowDialog();
        }
    }
}
