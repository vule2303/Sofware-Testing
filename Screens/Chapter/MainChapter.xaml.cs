using System.Windows;
using Microsoft.EntityFrameworkCore;
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
        private List<Models.Subject>? _subjects;
        private List<Items>? _items;
        public class Items
        {
            public int IdChapter { get; init; }
            public int IdSubject { get; set; }
            public required string NameChapter { get; set; }
            public required string? NameSubject { get; set; }
        }
        public MainChapter()
        {
            InitializeComponent();
            InitDataSubject();
            InitDataChapter();
            LoadSupject();
            LoadChapter();
            DataContext = this;
        }
        
        private void Button_Create(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_subjects != null)
                {
                    var chapter = new Chapters
                    {
                        SubjectId = _subjects[SelectedSubject.SelectedIndex].SubjectId,
                        Name = NameChapter.Text,
                        Subject = _subjects[SelectedSubject.SelectedIndex],
                    };
                    _context.Chapters.Add(chapter);
                }

                _context.SaveChanges();
                LoadChapter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void UpdateItem(object sender, RoutedEventArgs e)
        {
            var item = GridItems.SelectedItem as Items;
            var subject = _subjects;
            Window insert = new UpdateChapter(item, subject, _context, this);
            insert.Show();           
        }

        private void RemoveItem(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = (GridItems.SelectedItem as Items)?.IdChapter;
                var chapter =  _context.Chapters.FindAsync(item);
                if (chapter.Result != null) _context.Chapters.Remove((chapter.Result));
                _context.SaveChanges();
                LoadChapter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void InitDataSubject()
        {
            _subjects = new List<Models.Subject>();
            _context.Subjects.Add(new Models.Subject(){ Name = "Toán"});
            _context.Subjects.Add(new Models.Subject(){ Name = "Anh"});
            _context.Subjects.Add(new Models.Subject(){ Name = "Lí"});
            _context.SaveChanges();
        }

        private void InitDataChapter()
        {
            _context.Chapters.Add(new Chapters(){SubjectId = 1, Name = "Chương 1"});
            _context.Chapters.Add(new Chapters(){SubjectId = 1, Name = "Chương 2"});
            _context.Chapters.Add(new Chapters(){SubjectId = 2, Name = "Chương 1"});
            _context.Chapters.Add(new Chapters(){SubjectId = 2, Name = "Chương 2"});
            _context.Chapters.Add(new Chapters(){SubjectId = 3, Name = "Chương 1"});
            _context.Chapters.Add(new Chapters(){SubjectId = 3, Name = "Chương 2"});
            _context.SaveChanges();
        }
        
        public void LoadChapter()
        {
            _items = new List<Items>(); 
            var items = _context.Chapters.Include(chapters => chapters.Subject).ToList();
                foreach (var item in items)
                {
                    _items.Add(new Items
                    {
                        IdChapter = item.ChapterId,
                        IdSubject = item.SubjectId,
                        NameChapter = item.Name,
                        NameSubject = item.Subject?.Name
                    });
                }
            GridItems.ItemsSource = _items;
        }

        private void LoadSupject()
        {
            var subjects = _context.Subjects.ToList();
            _subjects = subjects;
            SelectedSubject.ItemsSource = _subjects;
        }
    }
}
