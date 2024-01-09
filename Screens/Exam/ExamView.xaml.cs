using System.Windows;
using MahApps.Metro.IconPacks;
using Microsoft.EntityFrameworkCore;
using TestBuilder.Data;

namespace TestBuilder.Screens.Exam;

public partial class ExamView
{
    private readonly TestDbContext _context = new ();
    private List<Items>? _items;
    private List<GridData>? _datas;

    public class GridData
    {
        public  string? SubjectName {  set; get; }
        public  int Amount { set; get; }
    }

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
            GridItems2.Visibility = Visibility.Visible;
            GridItems.Visibility = Visibility.Hidden;
            ButtonRollback.Visibility = Visibility.Visible;
            Group.Visibility = Visibility.Hidden;
            LoadGrid();
        }
        
        private void EditClick(object sender, RoutedEventArgs e)
        {
            var item = (GridItems.SelectedItem as Items)?.ExamTitle;
            if (item != null) TitleExam.Text = item;
            ButtonAction.Click -= Insert;
            ButtonAction.Click += Update;
            IconAction.Kind = PackIconMaterialKind.PencilOutline;
            ButtonName.Text = "Cập nhật";
            VisibleAddButton.Visibility = Visibility.Visible;
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            var item = (GridItems.SelectedItem as Items)?.ExamId;
            var exam = _context.Exams.Find(item);
            if (exam == null) return;
            exam.Title = TitleExam.Text;
            _context.SaveChanges();
            LoadGrid();
            TitleExam.Text = "";
            ButtonAction.Click -= Update;
            ButtonAction.Click += Insert;
            IconAction.Kind = PackIconMaterialKind.Plus;
            ButtonName.Text = "Thêm kì thi";
            VisibleAddButton.Visibility = Visibility.Hidden;
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
            if (GridItems.Visibility == Visibility.Visible)
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
            if (GridItems2.Visibility == Visibility.Visible)
            {
                var exam = (GridItems.SelectedItem as Items)?.ExamId;
                _datas = new List<GridData>();
                var listItem = _context.ExamsSubjects.Where(x => exam != null && x.ExamId == exam)
                    .Include(examsSubjects => examsSubjects.Subject).ToList();
                foreach (var item in listItem)
                {
                    var count = _context.Tests
                        .Include(s => s.Subject)
                        .Include(te => te.TestExams)
                        .Count(x => exam != null 
                                    && x.TestExams != null 
                                    && x.SubjectId == item.SubjectId 
                                    && exam == x.TestExams.Select(te => te.ExamId)
                                        .FirstOrDefault());
                    _datas.Add(new GridData()
                    {
                        SubjectName = item.Subject?.Name,
                        Amount = count
                    });
                }
                GridItems2.ItemsSource = _datas;
            }
        }

        private void ClickVisible(object sender, RoutedEventArgs e)
        {
            ButtonAction.Click -= Update;
            ButtonAction.Click += Insert;
            IconAction.Kind = PackIconMaterialKind.Plus;
            ButtonName.Text = "Thêm kì thi";
            VisibleAddButton.Visibility = Visibility.Hidden;
        }

        private void ClickUndo(object sender, RoutedEventArgs e)
        {
            ButtonRollback.Visibility = Visibility.Hidden;
            Group.Visibility = Visibility.Visible;
            GridItems.Visibility = Visibility.Visible;
            GridItems2.Visibility = Visibility.Hidden;
            LoadGrid();
        }
}