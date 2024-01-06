using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TestBuilder.View;

public partial class SubjectView : UserControl
{

    public SubjectView()
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