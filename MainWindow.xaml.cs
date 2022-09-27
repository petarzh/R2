using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ServiceStack.Text;
using System.IO;


namespace SirmaCSV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            // List<Empl> emplList = Load();

            // or dummy data
            List<Empl> emplList = new List<Empl>();
            emplList.Add(new Empl() { EmpID = 143, ProjectId = 12, DateFrom = DateTime.Now.AddDays(-10), DateTo = DateTime.Now });
            emplList.Add(new Empl() { EmpID = 218, ProjectId = 10, DateFrom = DateTime.Now.AddDays(-20), DateTo = DateTime.Now });
            emplList.Add(new Empl() { EmpID = 143, ProjectId = 10, DateFrom = DateTime.Now.AddDays(-2), DateTo = DateTime.Now });
            emplList.Add(new Empl() { EmpID = 155, ProjectId = 10, DateFrom = DateTime.Now.AddDays(-3), DateTo = DateTime.Now });


            var results = emplList
                    .OrderByDescending(o => o.Days)
                    .GroupBy(n => n.ProjectId)                  
                    .ToList();

            int maxDays = 0;
            EmplGridBinding empGridMax = new EmplGridBinding();
            foreach (var pairItem in results)
            {

                if (pairItem.Count() <= 1)
                    continue;

                foreach (var emplItem in pairItem)
                {                     
                    maxDays = emplItem.Days;
                    empGridMax.ProjectId = emplItem.ProjectId;
                     
                    if (empGridMax.Employee1 == 0)
                    {
                        empGridMax.Days = emplItem.Days;
                        empGridMax.Employee1 = emplItem.EmpID;
                    }
                    else
                    {
                        empGridMax.Employee2 = emplItem.EmpID;
                        break;
                    }


                }


            }

            MessageBox.Show("Lazzy grid binding ;) The longest pair of employess -> Employee1: " + empGridMax.Employee1 + ", Employee2: " + empGridMax.Employee2 + ", Max Days: " + empGridMax.Days);

        }

        private List<Empl> Load()
        {
            var fileName = @"E:\cv tasks\sirmaCSV\SirmaCSV\SirmaCSV\emp.csv";
            string csv = File.ReadAllText(fileName);
            return CsvSerializer.DeserializeFromString<List<Empl>>(csv);
        }


    }


    public class Empl
    {
        private DateTime dateFrom;
        private DateTime dateTo;

        public int EmpID { get; set; }

        public int ProjectId { get; set; }

        public DateTime? DateFrom
        {
            get
            {
                return dateFrom;
            }
            set
            {
                if (value == null)
                    dateFrom = DateTime.Now;
                else
                    dateFrom = (DateTime)value;
            }
        }

        public DateTime? DateTo
        {
            get
            {
                return dateTo;
            }
            set
            {
                if (value == null)
                    dateTo = DateTime.Now;
                else
                    dateTo = (DateTime)value;
            }
        }

        public int Days { get { return ((DateTime)DateTo - (DateTime)DateFrom).Days; } }


    }

    public class EmplGridBinding
    {
        // public List<int> EmployeeIds { get; set; }

        public int Employee1 { get; set; }

        public int Employee2 { get; set; }

        public int ProjectId { get; set; }

        public int Days { get; set; }


    }






}
