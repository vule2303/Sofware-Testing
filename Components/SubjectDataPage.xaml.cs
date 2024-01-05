using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestBuilder.Components
{
    /// <summary>
    /// Interaction logic for SubjectDataPage.xaml
    /// </summary>
    public partial class SubjectDataPage : UserControl
    {
        public SubjectDataPage()
        {
            InitializeComponent();
            var subjects = new ObservableCollection<TestBuilder.Models.Subject>
         {
             new()
             {
                 SubjectId = 1200068,
                 Name = "Toán Cao Cấp",
                 Chapters = [],
                 ExamsSubjects = []
             },
             new()
             {
                 SubjectId = 1200668,
                 Name = "Lập Trình Window",
                 Chapters = [],
                 ExamsSubjects = []
             },
             new()
             {
                 SubjectId = 1232068,
                 Name = "Lập Trình Android",
                 Chapters = [],
                 ExamsSubjects = []
             },
             new()
             {
                 SubjectId = 1340068,
                 Name = "Lập Trình Web",
                 Chapters = [],
                 ExamsSubjects = []
             },
             new()
             {
                 SubjectId = 1202368,
                 Name = "Quản Lý Dự Án",
                 Chapters = [],
                 ExamsSubjects = []
             },
             new()
             {
                 SubjectId = 1234305,
                 Name = "Phát Triển Website",
                 Chapters = [],
                 ExamsSubjects = []
             },
             new()
             {
                 SubjectId = 1460068,
                 Name = "Công Nghệ Phần Mềm",
                 Chapters = [],
                 ExamsSubjects = []
             }
         };
            subjectDataGrid.ItemsSource = subjects;
            
        }
    }
}
