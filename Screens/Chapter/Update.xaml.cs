using System.Windows;
using TestBuilder.Data;

namespace TestBuilder.Screens.Chapter;

/// <summary>
/// Interaction logic for Update.xaml
/// </summary>
public partial class UpdateChapter : Window
{
    private readonly TestDbContext _context;
    private readonly List<Models.Subject>? _subjects;
    private readonly Items? _items;
    private readonly MainChapter _mainChapter;
    public class Items
    {
        public int IdChapter { get; init; }
        public int IdSubject { get; set; }
        public required string NameChapter { get; set; }
    }

    public UpdateChapter(MainChapter.Items item, List<Models.Subject> subjects, TestDbContext context, MainChapter mainChapter)
    {
        InitializeComponent();
        _items = new Items(){ IdChapter = item.IdChapter, NameChapter = item.NameChapter, IdSubject = item.IdSubject };
        _subjects = subjects;
        _context = context;
        _mainChapter = mainChapter;
        NameChapter.Text = _items.NameChapter;
        SelectedSubject.ItemsSource = _subjects;
        SelectedSubject.SelectedIndex = _subjects.FindIndex(x => x.SubjectId == _items.IdSubject);
    }

    private void Update(object sender, RoutedEventArgs e)
    {
        try
        {
            var chapter = _context.Chapters.FirstOrDefault(c => c.ChapterId == _items.IdChapter);
            if (chapter != null)
            {
                if (NameChapter.Text != "")
                    chapter.Name = NameChapter.Text;
                if (SelectedSubject.SelectedItem != null)
                    chapter.SubjectId = (SelectedSubject.SelectedItem as Models.Subject).SubjectId;
                _context.Chapters.Update(chapter);
                _context.SaveChanges();
            }
            _mainChapter.GridItems.ItemsSource = _context.Chapters.ToList();
            _mainChapter.LoadChapter();
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}