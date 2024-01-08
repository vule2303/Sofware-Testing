using System.Windows;
using System.Windows.Controls;
using TestBuilder.Data;

namespace TestBuilder.Screens.Exam;

public partial class ExamView : UserControl
{
    private readonly TestDbContext _context = new ();
    private List<Items>? _items;

    public class Items
    {
        public int ExamId { get; init; }
        public  required string? ExamTitle {  set; get; }
        public  int Amount { set; get; }
    }
    public ExamView()
    {
        InitializeComponent();
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
            /*var item = GridItems.SelectedItem as Items;
            Window examSubject = new ExamSubject(item, _context);
            examSubject.Show();*/
        }
        
        private void Update(object sender, RoutedEventArgs e)
        {
            TitleExam.Visibility = Visibility.Visible;
            /*ButtonUpdate.Visibility = Visibility.Visible;
            AddButton.Visibility = Visibility.Visible;
            Default.Visibility = Visibility.Hidden;*/
            TitleExam.Visibility = Visibility.Hidden;
            TitleExam.Text = (GridItems.SelectedItem as Items)?.ExamTitle ?? string.Empty;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            TitleExam.Visibility = Visibility.Hidden;
            /*ButtonUpdate.Visibility = Visibility.Hidden;
            AddButton.Visibility = Visibility.Hidden;
            Default.Visibility = Visibility.Visible;*/
            TitleExam.Visibility = Visibility.Visible;
        }
        
        private void UpdateTitle(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TitleExam.Text))
            {
                var item = (GridItems.SelectedItem as Items)?.ExamId;
                var update = _context.Exams.Find(item);
                if (update != null)
                    update.Title = TitleExam.Text;
                _context.SaveChanges();
                LoadGrid();
                TitleExam.Text = "";
                TitleExam.Visibility = Visibility.Hidden;
                /*ButtonUpdate.Visibility = Visibility.Hidden;
                AddButton.Visibility = Visibility.Hidden;
                Default.Visibility = Visibility.Visible;*/
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
}