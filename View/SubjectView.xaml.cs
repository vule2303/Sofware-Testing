using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.IconPacks;
using TestBuilder.Data;
using TestBuilder.Models;

namespace TestBuilder.View;

public partial class SubjectView
{
    private readonly TestDbContext _context = new();
    private List<Subject>? _subjects;

    public SubjectView()
    {
        InitializeComponent();
        LoadData();
    }


    private void AddSubject(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(TxtAddSubject.Text))
        {
            _context.Subjects.Add(new Subject() { Name = TxtAddSubject.Text });
            _context.SaveChanges();
            LoadData();
            TxtAddSubject.Text = "";
        }
        else
        {
            MessageBox.Show("Vui lòng nhập tên môn học");
        }
    }

    private void EditClick(object sender, RoutedEventArgs e)
    {
        ButtonAction.Click -= AddSubject;
        ButtonAction.Click += UpdateSubject;
        IconAction.Kind = PackIconMaterialKind.Pencil;
        TextAction.Text = "Câp nhật";
        ButtonAction.Margin = new Thickness(0, 0, 70, 0);
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
        SubjectLabel.Text = "Tổng môn học: ";
        SubjectCount.Text = _context.Subjects.Count().ToString();
        _subjects = new List<Subject>();
        _subjects = _context.Subjects.ToList();
        SubjectDataGrid.ItemsSource = _subjects;
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
        {
            MessageBox.Show("Vui lòng điền tên môn học");
        }
    }

    private void ClickVisible(object sender, RoutedEventArgs e)
    {
        VisibleAddButton.Visibility = Visibility.Hidden;
        TxtAddSubject.Text = "";
        ButtonAction.Click -= UpdateSubject;
        ButtonAction.Click += AddSubject;
        ButtonAction.Margin = new Thickness(0, 0, 0, 0);
        IconAction.Kind = PackIconMaterialKind.Plus;
        TextAction.Text = "Thêm môn học";
    }
}