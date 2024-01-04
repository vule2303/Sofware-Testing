using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Media;
using TestBuilder.Models;
namespace TestBuilder;
public partial class MainWindow : Window
{
    private int sequenceNumber = 1;
    public MainWindow()
    {
        InitializeComponent();
        var converter = new BrushConverter();
        ObservableCollection<Subject> subjects = new ObservableCollection<Subject>();


        //Create datagrid subjects
        subjects.Add(new Subject
        {
            SubjectId = 1200068,
            Name = "Toán Cao Cấp",
            Chapters = [],
            ExamsSubjects = []
        });
        subjects.Add(new Subject
        {
            SubjectId = 1200668,
            Name = "Lập Trình Window",
            Chapters = [],
            ExamsSubjects = []
        });
        subjects.Add(new Subject
        {
            SubjectId = 1232068,
            Name = "Lập Trình Android",
            Chapters = [],
            ExamsSubjects = []
        });
        subjects.Add(new Subject
        {
            SubjectId = 1340068,
            Name = "Lập Trình Web",
            Chapters = [],
            ExamsSubjects = []
        });
        subjects.Add(new Subject
        {
            SubjectId = 1202368,
            Name = "Quản Lý Dự Án",
            Chapters = [],
            ExamsSubjects = []
        });
        subjects.Add(new Subject
        {
            SubjectId = 1234305,
            Name = "Phát Triển Website",
            Chapters = [],
            ExamsSubjects = []
        });
        subjects.Add(new Subject
        {
            SubjectId = 1460068,
            Name = "Công Nghệ Phần Mềm",
            Chapters = [],
            ExamsSubjects = []
        });

        subjectsDataGrid.ItemsSource = subjects;
    }

    private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if(e.ChangedButton == MouseButton.Left)
        {
            this.DragMove();
        }
    }
    private bool isMaximized = false;
    private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if(e.ClickCount == 2)
        {
            if (isMaximized)
            {
                this.WindowState = WindowState.Normal;
                this.Width = 1080;
                this.Height = 720;

                isMaximized = false;
            }
            else{
                this.WindowState = WindowState.Maximized;      

                isMaximized = true;
            }
        }
    }
}

