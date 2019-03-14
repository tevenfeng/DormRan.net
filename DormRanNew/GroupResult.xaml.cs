using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;

namespace DormRanNew
{
    /// <summary>
    /// GroupResult.xaml 的交互逻辑
    /// </summary>
    public partial class GroupResult : Window
    {
        private ObservableCollection<officer> checkedOfficers;
        private ObservableCollection<officer> GroupResultsWomen;
        private ObservableCollection<officer> GroupResultsMen;
        private ObservableCollection<officer> GroupResultsOne;
        private ObservableCollection<officer> GroupResultsTwo;
        private ObservableCollection<officer> GroupResultsThree;
        public GroupResult()
        {
            InitializeComponent();
        }

        public GroupResult(ObservableCollection<officer> checkedOfficer)
        {
            InitializeComponent();
            this.checkedOfficers = checkedOfficer;
            this.GroupResultsWomen= new ObservableCollection<officer>();
            this.GroupResultsMen = new ObservableCollection<officer>();
            this.GroupResultsOne = new ObservableCollection<officer>();
            this.GroupResultsTwo = new ObservableCollection<officer>();
            this.GroupResultsThree = new ObservableCollection<officer>();
            checkedOfficer_Divided();
        }

        private void checkedOfficer_Divided()
        {
            Random r = new Random();
            int numberOfPerson = this.checkedOfficers.Count();
            int numberOfWomen = 0;
            foreach (var tmpOfficer in checkedOfficers)
            {
                if (tmpOfficer.officer_gender.Equals("女"))
                {
                    numberOfWomen++;
                    this.GroupResultsWomen.Add(tmpOfficer);
                }
                else
                {
                    this.GroupResultsMen.Add(tmpOfficer);
                }
            }
            int numberOfMen = numberOfPerson - numberOfWomen;
            int secondGroupMenNumber = (int)(numberOfMen * 1.0 / 2);
            int thirdGroupMenNumber = numberOfMen - secondGroupMenNumber;
            HashSet<int> GroupMen = new HashSet<int>();
            HashSet<int> GroupWomen = new HashSet<int>();
            int middleSecondNumber = secondGroupMenNumber;
            while (middleSecondNumber > 0)
            {
                int index = r.Next(0, numberOfMen);
                if(!GroupMen.Contains(index))
                {
                    GroupResultsTwo.Add(GroupResultsMen[index]);
                    GroupMen.Add(index);
                    middleSecondNumber--;
                }
            }
            for(int i=0;i<numberOfMen;i++)
            {
                if(!GroupMen.Contains(i))
                {
                    GroupResultsThree.Add(GroupResultsMen[i]);
                    GroupMen.Add(i);
                }
            }
            if(numberOfWomen<=secondGroupMenNumber)
            {
                this.GroupResultsOne = this.GroupResultsWomen;
            }
            else
            {
                int middleFirstNumber = secondGroupMenNumber;
                while(middleFirstNumber>0)
                {
                    int index = r.Next(0, numberOfWomen);
                    if (!GroupWomen.Contains(index))
                    {
                        GroupResultsOne.Add(GroupResultsWomen[index]);
                        GroupWomen.Add(index);
                        middleFirstNumber--;
                    }
                }
                int restNumber = numberOfWomen - secondGroupMenNumber;
                int mod = 0;
                while(restNumber>0)
                {
                    int index = r.Next(0, numberOfWomen);
                    if (!GroupWomen.Contains(index))
                    {
                        if(mod==0)
                        {
                            GroupResultsOne.Add(GroupResultsWomen[index]);
                            mod = 1;
                        }
                        else if(mod==1)
                        {
                            GroupResultsTwo.Add(GroupResultsWomen[index]);
                            mod = 2;
                        }
                        else
                        {
                            GroupResultsThree.Add(GroupResultsWomen[index]);
                            mod = 0;
                        }
                        GroupWomen.Add(index);
                        restNumber--;
                    }
                }
            }
            this.dataGridGroupOne.ItemsSource = null;
            this.dataGridGroupOne.ItemsSource = this.GroupResultsOne;
            this.dataGridGroupTwo.ItemsSource = null;
            this.dataGridGroupTwo.ItemsSource = this.GroupResultsTwo;
            this.dataGridGroupThree.ItemsSource = null;
            this.dataGridGroupThree.ItemsSource = this.GroupResultsThree;
        }
    }
}
