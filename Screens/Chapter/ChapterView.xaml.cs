using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.IconPacks;
using Microsoft.EntityFrameworkCore;
using TestBuilder.Data;
using TestBuilder.Models;

namespace TestBuilder.Screens.Chapter;

public partial class ChapterView : UserControl
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


    public ChapterView()
    {
        InitializeComponent();
        LoadSubject();
        LoadChapter();
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

        private void EditClick(object sender, RoutedEventArgs e)
        {
            ButtonName.Text = "Cập nhật";
            IconAdd.Kind = PackIconMaterialKind.Pencil;
            ButtonAction.Click -= Button_Create;
            ButtonAction.Click += ButtonUpdate;
            RollBackAdd.Visibility = Visibility.Visible;
            
            if (GridItems.SelectedItem is Items item)
            {
                NameChapter.Text = item.NameChapter; 
                SelectedSubject.SelectedItem = _subjects?.Find(x => x.SubjectId == item.IdSubject);
            }
        }

        private void ButtonUpdate(object sender, RoutedEventArgs e)
        {
            var item = GridItems.SelectedItem as Items;
            var chapter = _context.Chapters.Find(item?.IdChapter);
            if (chapter != null)
            {
                chapter.Name = NameChapter.Text;
                chapter.SubjectId = ((Models.Subject)SelectedSubject.SelectedItem).SubjectId;
                _context.SaveChanges();
                LoadChapter();
            }
            
            ButtonName.Text = "Thêm chương";
            IconAdd.Visibility = Visibility.Visible;
            RollBackAdd.Visibility = Visibility.Hidden;
            ButtonAction.Click -= ButtonUpdate;
            ButtonAction.Click += Button_Create;

        }

        private void RemoveItem(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = (GridItems.SelectedItem as Items)?.IdChapter;
                var chapter = _context.Chapters.FindAsync(item);
                if (chapter.Result != null) _context.Chapters.Remove((chapter.Result));
                _context.SaveChanges();
                LoadChapter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
        private void LoadSubject()
        {
            var subjects = _context.Subjects.ToList();
            _subjects = subjects;
            SelectedSubject.ItemsSource = _subjects;
        }

        private void Button_RollBack(object sender, RoutedEventArgs e)
        {
            ButtonName.Text = "Thêm chương";
            IconAdd.Visibility = Visibility.Visible;
            ButtonAction.Click -= ButtonUpdate;
            ButtonAction.Click += Button_Create;
            RollBackAdd.Visibility = Visibility.Hidden;
        }
}
