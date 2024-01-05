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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TestBuilder.Data;
using TestBuilder.Models;

namespace TestBuilder.Screens.Exam
{
    /// <summary>
    /// Interaction logic for ManageExam.xaml
    /// </summary>
    public partial class ManageExam : Window
    {
        private readonly TestDbContext _context = new ();
        private List<Models.Subject> _subjects;
        private List<Items> _items = new ();

        private class Items
        {
            public int ExamId { get; set; }
            public string ExamTitle { get; set; }
            public int SubjectId { get; set; }
            public string SubjectName { get; set; }
            public int Amount { get; set; }
        }
        
        public ManageExam()
        {
            InitializeComponent();
            InitData();
            _subjects = new List<Models.Subject>();
            _subjects = _context.Subjects.ToList();
            ListSubject.ItemsSource = _subjects;
            LoadGrid();
            
        }
        
        private void Insert(object sender, RoutedEventArgs e)
        {
            _context.Exams.Add(new Models.Exam(){Title = TitleExam.Text});
            _context.SaveChanges();
            
        }


        private void LoadGrid()
        {
            var listItem = _context.ExamsSubjects
                
                .ToList();
            foreach (var item in listItem)
            {
                _items.Add(new Items()
                {
                    ExamId = item.ExamId,
                    ExamTitle = item.Exam.Title,
                    SubjectId = item.SubjectId,
                    SubjectName = item.Subject.Name,
                    });                
            }
            GridItems.ItemsSource = _items;
        }
        private void InitData()
        {
            _context.Subjects.Add(new Models.Subject(){Name = "Toán"});
            _context.Subjects.Add(new Models.Subject(){Name = "Lí"});
            _context.Subjects.Add(new Models.Subject(){Name = "Anh"});
            _context.Exams.Add(new Models.Exam(){Title = "Thi học kì 1"});
            _context.Exams.Add(new Models.Exam(){Title = "Thi học kì 2"});
            _context.ExamsSubjects.Add(new Models.ExamsSubjects(){ExamId = 1, SubjectId = 1});
            _context.ExamsSubjects.Add(new Models.ExamsSubjects(){ExamId = 1, SubjectId = 2});
            _context.ExamsSubjects.Add(new Models.ExamsSubjects(){ExamId = 1, SubjectId = 3});
            _context.ExamsSubjects.Add(new Models.ExamsSubjects(){ExamId = 2, SubjectId = 1});
            _context.ExamsSubjects.Add(new Models.ExamsSubjects(){ExamId = 2, SubjectId = 2});
            _context.ExamsSubjects.Add(new Models.ExamsSubjects(){ExamId = 2, SubjectId = 3});
            _context.Tests.Add(new Models.Test(){ Title = "Test 1", SubjectId = 1});
            _context.Tests.Add(new Models.Test(){ Title = "Test 2" , SubjectId = 2});
            _context.TestExams.Add(new TestExams(){TestId = 1, ExamId = 1});
            _context.TestExams.Add(new TestExams(){TestId = 2, ExamId = 1});
            _context.TestExams.Add(new TestExams(){TestId = 1, ExamId = 2});
            _context.TestExams.Add(new TestExams(){TestId = 2, ExamId = 2});
            _context.SaveChanges();
        }
    }
}
