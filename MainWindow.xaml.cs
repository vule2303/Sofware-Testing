using System.Windows;
using System.Windows.Input;
using TestBuilder.Data;
using TestBuilder.Models;
using TestBuilder.Screens.Chapter;
using TestBuilder.Screens.Exam;
using TestBuilder.Screens.Question;
using TestBuilder.Screens.Test;
using TestBuilder.View;

namespace TestBuilder;

public partial class MainWindow
{
    private readonly TestDbContext _context = new();
    private bool _isMaximized;

    public MainWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private void HomeView(object sender, RoutedEventArgs e)
    {
        var view = new HomeView();
        ContentControl.Content = view;
    }

    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
    }

    private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount != 2) return;

        if (_isMaximized)
        {
            Width = 1080;
            Height = 720;

            _isMaximized = false;
        }
        else
        {
            _isMaximized = true;
        }
    }

    private void ButtonTest(object sender, RoutedEventArgs e)
    {
        var _ = new ManagerTest();
        ContentControl.Content = _;
    }

    private void ButtonQuestion(object sender, RoutedEventArgs e)
    {
        var _ = new ManagerQuestion();
        ContentControl.Content = _;
    }

    private void ChapterScreenClick(object sender, RoutedEventArgs e)
    {
        var _ = new ChapterView();
        ContentControl.Content = _;
    }

    private void GoToSubjectScreen(object sender, RoutedEventArgs e)
    {
        var _ = new SubjectView();
        ContentControl.Content = _;
    }

    private void GoToExamScreen(object sender, RoutedEventArgs e)
    {
        var _ = new ExamView();
        ContentControl.Content = _;
    }

    private void LoadData()
    {
        // Môn học
        _context.Subjects.Add(new Subject { Name = "Toán Cao Cấp" });
        _context.Subjects.Add(new Subject { Name = "Lập Trình Window" });
        _context.Subjects.Add(new Subject { Name = "Lập Trình Android" });
        _context.Subjects.Add(new Subject { Name = "Lập Trình Web" });

        _context.Chapters.Add(new Chapters { SubjectId = 1, Name = "Đạo hàm tích phân" });
        _context.Chapters.Add(new Chapters { SubjectId = 2, Name = "Window Form" });
        _context.Chapters.Add(new Chapters { SubjectId = 2, Name = "WPF" });
        _context.Chapters.Add(new Chapters { SubjectId = 3, Name = "Kotlin" });
        _context.Chapters.Add(new Chapters { SubjectId = 3, Name = "Android" });
        _context.Chapters.Add(new Chapters { SubjectId = 4, Name = "HTML" });
        _context.Chapters.Add(new Chapters { SubjectId = 4, Name = "CSS" });
        _context.Chapters.Add(new Chapters { SubjectId = 4, Name = "Javascript" });

        // Kỳ thi
        _context.Exams.Add(new Exam { Title = "Kiểm tra giữa môn" });
        _context.Exams.Add(new Exam { Title = "Báo cáo cuối môn" });

        _context.ExamsSubjects.Add(new ExamsSubjects { ExamId = 1, SubjectId = 1 });
        _context.ExamsSubjects.Add(new ExamsSubjects { ExamId = 1, SubjectId = 2 });
        _context.ExamsSubjects.Add(new ExamsSubjects { ExamId = 1, SubjectId = 3 });
        _context.ExamsSubjects.Add(new ExamsSubjects { ExamId = 1, SubjectId = 4 });
        _context.ExamsSubjects.Add(new ExamsSubjects { ExamId = 2, SubjectId = 1 });
        _context.ExamsSubjects.Add(new ExamsSubjects { ExamId = 2, SubjectId = 2 });
        _context.ExamsSubjects.Add(new ExamsSubjects { ExamId = 2, SubjectId = 3 });
        _context.ExamsSubjects.Add(new ExamsSubjects { ExamId = 2, SubjectId = 4 });

        _context.Tests.Add(new Test { Title = "Đề thi 1", SubjectId = 1 });
        _context.Tests.Add(new Test { Title = "Đề thi 1", SubjectId = 2 });
        _context.Tests.Add(new Test { Title = "Đề thi 2", SubjectId = 2 });

        _context.TestExams.Add(new TestExams { TestId = 1, ExamId = 1 });
        _context.TestExams.Add(new TestExams { TestId = 2, ExamId = 1 });
        _context.TestExams.Add(new TestExams { TestId = 1, ExamId = 2 });
        _context.TestExams.Add(new TestExams { TestId = 2, ExamId = 2 });

        _context.SaveChanges();
    }
}