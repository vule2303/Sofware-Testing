using System.Windows;
using TestBuilder.Data;
using TestBuilder.Models;

namespace TestBuilder.Screens.Exam
{
    /// <summary>
    /// Interaction logic for ManageExam.xaml
    /// </summary>
    public partial class ManageExam
    {
        private readonly TestDbContext _context = new ();
        private readonly List<Items> _items;

        private  class Items
        {
            public int ExamId { get; init; }
            public  required string ExamTitle {  set; get; }
            public  int Amount { set; get; }
        }
        
        public ManageExam()
        {
            InitializeComponent();
            _items = new List<Items>();
            InitData();
            LoadGrid();
            DataContext = this;
        }
        
        private void Insert(object sender, RoutedEventArgs e)
        {
            _context.Exams.Add(new Models.Exam{Title = TitleExam.Text});
            _context.SaveChanges();
            LoadGrid();
        }
        
        private async void Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = GridItems.SelectedItem as Items;
                var testExam = _context.TestExams.Where(exam => item != null && exam.ExamId == item.ExamId).ToList();
                _context.TestExams.RemoveRange(testExam);

                var examSubject = _context.ExamsSubjects.Where(exam => item != null && exam.ExamId == item.ExamId)
                    .ToList();
                _context.ExamsSubjects.RemoveRange(examSubject);

                if (item != null)
                {
                    var exam = _context.Exams.FindAsync(item.ExamId);
                    if (exam.Result != null) _context.Exams.Remove(exam.Result);
                }

                await _context.SaveChangesAsync();
                LoadGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void LoadGrid()
        {
            var listItem = _context.Exams.ToList();
            foreach (var item in listItem)
            {
                var count = _context.TestExams.Count(exam => exam.ExamId == item.ExamId);
                _items.Add(new Items()
                {
                    ExamId = item.ExamId,
                    ExamTitle = item.Title,
                    Amount = count
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
            _context.ExamsSubjects.Add(new ExamsSubjects(){ExamId = 1, SubjectId = 1});
            _context.ExamsSubjects.Add(new ExamsSubjects(){ExamId = 1, SubjectId = 2});
            _context.ExamsSubjects.Add(new ExamsSubjects(){ExamId = 1, SubjectId = 3});
            _context.ExamsSubjects.Add(new ExamsSubjects(){ExamId = 2, SubjectId = 1});
            _context.ExamsSubjects.Add(new ExamsSubjects(){ExamId = 2, SubjectId = 2});
            _context.ExamsSubjects.Add(new ExamsSubjects(){ExamId = 2, SubjectId = 3});
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
