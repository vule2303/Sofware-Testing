using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TestBuilder.Data;
using TestBuilder.Models;

namespace TestBuilder.Screens.Chapter
{
    /// <summary>
    /// Interaction logic for Update.xaml
    /// </summary>
    public partial class UpdateChapter : Window
    {
        private readonly TestDbContext _context = new();
        private readonly List<Subject> _subjects;
        private readonly int _id;

        public UpdateChapter(int item)
        {
            InitializeComponent();
            _id = item;
            var chapters = _context.Chapters!
                .Where(x => x.ChapterId == item)
                .FirstOrDefault();
            NameChapter.Text = chapters!.Name;

            _subjects = new List<Subject>();
            _subjects = _context.Subjects!.ToList();
            SelectedSubject.ItemsSource = _subjects;
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            try
            {
                var chapter = _context.Chapters!.Find(_id);
                if (chapter != null)
                {
                    if (NameChapter.Text != "")
                        chapter.Name = NameChapter.Text;
                    if (SelectedSubject.SelectedItem != null)
                        chapter.SubjectId = (SelectedSubject.SelectedItem as Subject)!.SubjectId;

                }
                _context.Chapters!.Update(chapter!);
                _context.SaveChanges();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
