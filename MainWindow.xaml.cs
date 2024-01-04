using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestBuilder.Models;

namespace TestBuilder;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        var subjects = new ObservableCollection<Subject>
        {
            //Create datagrid subjects
            new()
            {
                SubjectId = 1200068,
                Name = "Toán Cao Cấp",
                Chapters = [],
                ExamsSubjects = []
            },
            new()
            {
                SubjectId = 1200668,
                Name = "Lập Trình Window",
                Chapters = [],
                ExamsSubjects = []
            },
            new()
            {
                SubjectId = 1232068,
                Name = "Lập Trình Android",
                Chapters = [],
                ExamsSubjects = []
            },
            new()
            {
                SubjectId = 1340068,
                Name = "Lập Trình Web",
                Chapters = [],
                ExamsSubjects = []
            },
            new()
            {
                SubjectId = 1202368,
                Name = "Quản Lý Dự Án",
                Chapters = [],
                ExamsSubjects = []
            },
            new()
            {
                SubjectId = 1234305,
                Name = "Phát Triển Website",
                Chapters = [],
                ExamsSubjects = []
            },
            new()
            {
                SubjectId = 1460068,
                Name = "Công Nghệ Phần Mềm",
                Chapters = [],
                ExamsSubjects = []
            }
        };

        SubjectsDataGrid.ItemsSource = subjects;
    }


    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }

    private bool _isMaximized;

    private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount != 2) return;

        if (_isMaximized)
        {
            WindowState = WindowState.Normal;
            Width = 1080;
            Height = 720;

            _isMaximized = false;
        }
        else
        {
            WindowState = WindowState.Maximized;

            _isMaximized = true;
        }
    }
}