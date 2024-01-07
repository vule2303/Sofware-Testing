using System.Windows;
using TestBuilder.Data;
using TestBuilder.Models;

namespace TestBuilder.View;

public partial class SubjectView
{
    private readonly TestDbContext _context = new();
    private  List<Subject>? _subjects;
    public SubjectView()
    {
        InitializeComponent();
        InitData();
        LoadData();
    }
    
    
    private void AddSubject(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(TxtAddSubject.Text))
        {
            _context.Subjects.Add(new Subject(){Name = TxtAddSubject.Text});
            _context.SaveChanges();
            LoadData();
            TxtAddSubject.Text="";
        }
        else
            MessageBox.Show("Vui lòng nhập tên môn học");
    }
    private void EditClick(object sender, RoutedEventArgs e)
    {
        AddSubjectButton.Visibility = Visibility.Hidden;
        UpdateSubjectButton.Visibility = Visibility.Visible;
        VisibleAddButton.Visibility = Visibility.Visible;
        var item = (SubjectDataGrid.SelectedItem as Subject)?.Name;
        if (item != null) TxtAddSubject.Text = item;
    }
    
    private void RemoveClick(object sender, RoutedEventArgs e)
    {
        var item = (SubjectDataGrid.SelectedItem as Subject)?.SubjectId;
        var subject = _context.Subjects.Find(item);
        if (subject != null)
        {
            _context.Subjects.Remove(subject);
            _context.SaveChanges();
            LoadData();
        }
    }

    private void LoadData()
    {
        _subjects = new List<Subject>();
        _subjects = _context.Subjects.ToList();
        SubjectDataGrid.ItemsSource = _subjects;

    }
    private void InitData()
    {
        _context.Add(new Subject(){Name = "Toán Cao Cấp"});
        _context.Add(new Subject(){Name = "Lập Trình Window"});
        _context.Add(new Subject(){Name = "Lập Trình Android"});
        _context.Add(new Subject(){Name = "Lập Trình Web"});
        _context.SaveChanges();
    }

    private void UpdateSubject(object sender, RoutedEventArgs e)
    {
        var item = (SubjectDataGrid.SelectedItem as Subject)?.SubjectId;
        var subject = _context.Subjects.Find(item);
        if (subject == null) return;
        if (!string.IsNullOrEmpty(TxtAddSubject.Text))
        {
            subject.Name = TxtAddSubject.Text;
            _context.SaveChanges();
            LoadData();
            TxtAddSubject.Text = "";
            ClickVisible(sender, e);
        }
        else
            MessageBox.Show("Vui lòng điền tên môn học");
    }

    private void ClickVisible(object sender, RoutedEventArgs e)
    {
        AddSubjectButton.Visibility = Visibility.Visible;
        UpdateSubjectButton.Visibility = Visibility.Hidden;
        VisibleAddButton.Visibility = Visibility.Hidden;
    }
}