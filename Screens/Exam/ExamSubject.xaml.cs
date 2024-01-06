using Microsoft.EntityFrameworkCore;
using TestBuilder.Data;

namespace TestBuilder.Screens.Exam;

public partial class ExamSubject 
{
    private readonly TestDbContext _context;
    private readonly ManageExam.Items? _item;
    private List<GridData>? _datas;

    public class GridData
    {
        public  string? SubjectName {  set; get; }
        public  int Amount { set; get; }
    }

    public ExamSubject(ManageExam.Items? item, TestDbContext context)
    {
        InitializeComponent();
        if (item != null)
            _item = new ManageExam.Items() { ExamId = item.ExamId, ExamTitle = item.ExamTitle, Amount = item.Amount };
        _context = context;
        LoadData();
        DataContext = this;
    }

    private void LoadData()
    {
        _datas = new List<GridData>();
        var listItem = _context.ExamsSubjects.Where(x => _item != null && x.ExamId == _item.ExamId)
            .Include(examsSubjects => examsSubjects.Subject).ToList();
        foreach (var item in listItem)
        {
            var count = _context.Tests
                .Include(s => s.Subject)
                .Include(te => te.TestExams)
                .Count(x => _item != null 
                            && x.TestExams != null 
                            && x.SubjectId == item.SubjectId 
                            && _item.ExamId == x.TestExams.Select(te => te.ExamId)
                                .FirstOrDefault());
            _datas.Add(new GridData()
            {
                SubjectName = item.Subject?.Name,
                Amount = count
            });
        }
        GridItems.ItemsSource = _datas;
    }
}