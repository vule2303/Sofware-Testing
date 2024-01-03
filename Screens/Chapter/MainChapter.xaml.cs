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
        private readonly List<Subject> _subjects;
        private readonly List<Items> _items;
        public partial class Items
        {
            public int IdChapter { get; set; }
            public int IdSubject { get; set; }
            public required string NameChapter { get; set; }
            public required string NameSubject { get; set; }
        }

        private void Load()
        {
            var items = _context.Chapters!.Include(chapters => chapters.Subject).ToList();
            foreach (var item in items)
            {
                _items.Add(new Items
                {
                    IdChapter = item.ChapterId,
                    IdSubject = item.SubjectId,
                    NameChapter = item.Name,
                    NameSubject = item.Subject.Name
                });
            }
        }
        
        public MainChapter()
        {
            InitializeComponent();
            _subjects = new List<Subject>();
            _subjects = _context.Subjects!.ToList();
            SelectedSubject.ItemsSource = _subjects;
            
            _items = new List<Items>(); 
            Load();
            GridItems.ItemsSource = _items;
        }
        
        private void Button_Create(object sender, RoutedEventArgs e)
        {
            try
            {
                var chapter = new Chapters
                {
                    SubjectId = _subjects[SelectedSubject.SelectedIndex].SubjectId,
                    Name = NameChapter.Text ?? "",
                    Subject = _subjects[SelectedSubject.SelectedIndex],
                };
                _context.Chapters!.Add(chapter);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void UpdateItem(object sender, RoutedEventArgs e)
        {
            var item = (GridItems.SelectedItem as Items)!.IdChapter;
            var subject = _subjects;
            Window insert = new UpdateChapter(item);
            insert.Show();
        }

        private void RemoveItem(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = (GridItems.SelectedItem as Items)!.IdChapter;
                var chapter = _context.Chapters!.FindAsync(item);
                _context.Chapters!.Remove((chapter.Result!));
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
    }
}
