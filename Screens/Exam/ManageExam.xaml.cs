using System.Windows;
using System.Windows.Controls;
using TestBuilder.Data;
using TestBuilder.Models;
using TestBuilder.Screens.Chapter;

namespace TestBuilder.Screens.Exam
{
    /// <summary>
    /// Interaction logic for ManageExam.xaml
    /// </summary>
    public partial class ManageExam
    {
        private readonly TestDbContext _context = new ();
        private List<Items>? _items;

        public class Items
        {
            public int ExamId { get; init; }
            public  required string? ExamTitle {  set; get; }
            public  int Amount { set; get; }
        }
        
        public ManageExam()
        {
            InitializeComponent();
            InitData();
            LoadGrid();
            DataContext = this;
        }
        
        private void Insert(object sender, RoutedEventArgs e)
        {
            if (TitleExam.Text != "")
            {
                _context.Exams.Add(new Models.Exam{Title = TitleExam.Text});
                _context.SaveChanges();
                LoadGrid();
                TitleExam.Text = "";
            }
            else
                MessageBox.Show("Vui lòng nhập tên kì thi");
        }
        
        private void View(object sender, RoutedEventArgs e)
        {
            var item = GridItems.SelectedItem as Items;
            Window examSubject = new ExamSubject(item, _context);
            examSubject.Show();
        }
        
        private void Update(object sender, RoutedEventArgs e)
        {
            TitleUpdate.Visibility = Visibility.Visible;
            ButtonUpdate.Visibility = Visibility.Visible;
            AddButton.Visibility = Visibility.Visible;
            Default.Visibility = Visibility.Hidden;
            TitleExam.Visibility = Visibility.Hidden;
            TitleUpdate.Text = (GridItems.SelectedItem as Items)?.ExamTitle ?? string.Empty;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            TitleUpdate.Visibility = Visibility.Hidden;
            ButtonUpdate.Visibility = Visibility.Hidden;
            AddButton.Visibility = Visibility.Hidden;
            Default.Visibility = Visibility.Visible;
            TitleExam.Visibility = Visibility.Visible;
        }
        
        private void UpdateTitle(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TitleUpdate.Text))
            {
                var item = (GridItems.SelectedItem as Items)?.ExamId;
                var update = _context.Exams.Find(item);
                if (update != null)
                    update.Title = TitleUpdate.Text;
                _context.SaveChanges();
                LoadGrid();
                TitleUpdate.Text = "";
                TitleUpdate.Visibility = Visibility.Hidden;
                ButtonUpdate.Visibility = Visibility.Hidden;
                AddButton.Visibility = Visibility.Hidden;
                Default.Visibility = Visibility.Visible;
                TitleExam.Visibility = Visibility.Visible;
            }
            else
                MessageBox.Show("Vui lòng nhập tên kì thi");
        }
        
        private async void Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = (GridItems.SelectedItem as Items)?.ExamId;
                if (item != null)
                {
                    var exam = _context.Exams.FindAsync(item);
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
            _items = new List<Items>();
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
            _context.Tests.Add(new Models.Test(){ Title = "Test 3" , SubjectId = 2});
            _context.TestExams.Add(new TestExams(){TestId = 1, ExamId = 1});
            _context.TestExams.Add(new TestExams(){TestId = 2, ExamId = 1});
            _context.TestExams.Add(new TestExams(){TestId = 1, ExamId = 2});
            _context.TestExams.Add(new TestExams(){TestId = 2, ExamId = 2});
            _context.SaveChanges();
        }

      
    }
}
