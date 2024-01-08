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
        DragMove();
        LoadData();
         
    }
    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
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
        Window examScreen = new ManageExam();
        examScreen.Show();
    }

    private void LoadData()
    {
        _context.Add(new Subject(){Name = "Toán Cao Cấp"});
        _context.Add(new Subject(){Name = "Lập Trình Window"});
        _context.Add(new Subject(){Name = "Lập Trình Android"});
        _context.Add(new Subject(){Name = "Lập Trình Web"});
       
        _context.Chapters.Add(new Chapters() { SubjectId = 1, Name = "Đạo hàm tích phân" });
        _context.Chapters.Add(new Chapters() { SubjectId = 2, Name = "Window Form" });
        _context.Chapters.Add(new Chapters() { SubjectId = 2, Name = "WPF" });
        _context.Chapters.Add(new Chapters() { SubjectId = 3, Name = "Kotlin" });
        _context.Chapters.Add(new Chapters() { SubjectId = 3, Name = "Android" });
        _context.Chapters.Add(new Chapters() { SubjectId = 4, Name = "HTML" });
        _context.Chapters.Add(new Chapters() { SubjectId = 4, Name = "CSS" });
        _context.Chapters.Add(new Chapters() { SubjectId = 4, Name = "Javascript" });
        
        _context.SaveChanges();
    }


}