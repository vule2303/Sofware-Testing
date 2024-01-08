using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestBuilder.Data;
using TestBuilder.Models;
using TestBuilder.Screens.Question;
using TestBuilder.Screens.Test;
using TestBuilder.Screens.Chapter;
using TestBuilder.Screens.Exam;

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
        Window chapter = new MainChapter();
        chapter.Show();
    }

    private void GoToExamScreen(object sender, RoutedEventArgs e)
    {
        Window examScreen = new ManageExam();
        examScreen.Show();
    }

    private void LoadData()
    {
        _context.Subjects.Add(new Subject() { Name = "Toán" });
        _context.Subjects.Add(new Subject() { Name = "Anh" });
        _context.Subjects.Add(new Subject() { Name = "Lí" });
        
        _context.Chapters.Add(new Chapters() { SubjectId = 1, Name = "Chương 1" });
        _context.Chapters.Add(new Chapters() { SubjectId = 1, Name = "Chương 2" });
        _context.Chapters.Add(new Chapters() { SubjectId = 2, Name = "Chương 1" });
        _context.Chapters.Add(new Chapters() { SubjectId = 2, Name = "Chương 2" });
        _context.Chapters.Add(new Chapters() { SubjectId = 3, Name = "Chương 1" });
        _context.Chapters.Add(new Chapters() { SubjectId = 3, Name = "Chương 2" });
        _context.SaveChanges();
    }
}