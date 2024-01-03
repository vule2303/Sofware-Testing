using System.Windows;
using TestBuilder.Data;
using TestBuilder.Models;

namespace TestBuilder.Screens.Chapter
{
    /// <summary>
    /// Interaction logic for MainChapter.xaml
    /// </summary>
    public partial class MainChapter 
    {
        private readonly TestDbContext _context = new();
        private readonly List<Subject> _subjects;
        public MainChapter()
        {
            InitializeComponent();
            _subjects = _context.Subjects!.ToList();
            SelectedSubject.ItemsSource = _subjects;
        }
        
        private void Button_Create(object sender, RoutedEventArgs e)
        {
            var chapter = new Chapters
            {
                SubjectId = _subjects[SelectedSubject.SelectedIndex].SubjectId,
                Name = NameChapter.Text ?? "",
                Subject = _subjects[SelectedSubject.SelectedIndex],
            };
            _context.Chapters!.Add(chapter);
        }
    }
}
