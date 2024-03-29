using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using TestBuilder.Data;
using TestBuilder.Models;

namespace TestBuilder.Screens.Question;

public partial class DetailsQuestion
{
    private readonly TestDbContext _context = new();
    private readonly ObservableCollection<Answer> _listAnswer = [];
    private readonly Guid? _questionId;
    private string _imagePath = "";

    public DetailsQuestion()
    {
        InitializeComponent();
        _listAnswer =
        [
            new Answer
            {
                Content = "Đáp án 1",
                Image = null
            }
        ];
        AnswerDataGrid.ItemsSource = _listAnswer;
        _questionId = null;
        DataContext = this;
    }

    public DetailsQuestion(Guid questionId)
    {
        InitializeComponent();

        _questionId = questionId;
        var question = _context.Questions
            .Include(q => q.Options)
            .FirstOrDefault(q => q.QuestionId == questionId);
        if (question is null)
        {
            MessageBox.Show("Không tìm thấy câu hỏi");
            return;
        }

        TxtBoxQuestionDetail.Text = question.Content;
        if (!string.IsNullOrEmpty(question.Image))
        {
            _imagePath = question.Image;
            ImageQuestion.Source = new BitmapImage(new Uri(question.Image!));
            ImageQuestion.Width = 270;
            ImageQuestion.Height = 200;
        }

        if (!string.IsNullOrEmpty(question.Formula))
        {
            RawFormula.Text = question.Formula;
            FormulaControl.Formula = question.Formula;
        }

        question.Options!.ForEach(a => _listAnswer.Add(new Answer
        {
            Content = a.Text,
            Image = a.Image
        }));
        AnswerDataGrid.ItemsSource = _listAnswer;
        DataContext = this;
    }

    private void ButtonClose(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void ButtonDoneAnswer(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(TxtBoxQuestionDetail.Text))
        {
            MessageBox.Show("Vui lòng nhập dữ liệu");
            return;
        }

        if (_questionId is not null)
        {
            var question = _context.Questions
                .Include(q => q.Options)
                .FirstOrDefault(q => q.QuestionId.Equals(_questionId));

            if (question is null)
            {
                MessageBox.Show("Không tìm thấy câu hỏi");
                return;
            }

            question.Content = TxtBoxQuestionDetail.Text;
            question.Formula = RawFormula.Text;
            question.Image = _imagePath;
            question.Options!.Clear();
            foreach (var answer in _listAnswer)
                question.Options.Add(new Option
                {
                    QuestionId = question.QuestionId,
                    Text = answer.Content,
                    Image = answer.Image,
                    Question = question
                });
        }
        else
        {
            var questionId = new Guid();

            var newQuestion = new Models.Question
            {
                QuestionId = questionId,
                Content = TxtBoxQuestionDetail.Text,
                Formula = RawFormula.Text,
                Image = _imagePath,
                Options = [],
                TestQuestions = null
            };

            foreach (var answer in _listAnswer)
                newQuestion.Options.Add(new Option
                {
                    QuestionId = questionId,
                    Text = answer.Content,
                    Image = answer.Image,
                    Question = newQuestion
                });

            _context.Questions.Add(newQuestion);
        }

        await _context.SaveChangesAsync();

        MessageBox.Show("Đã lưu thành công!");
        Close();
    }

    private void ButtonRemoveAnswer(object sender, RoutedEventArgs e)
    {
        _listAnswer.RemoveAt(AnswerDataGrid.SelectedIndex);
    }

    private void SelectAndUploadImage(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
        };

        if (openFileDialog.ShowDialog() != true) return;

        _imagePath = openFileDialog.FileName;
        ImageQuestion.Width = 270;
        ImageQuestion.Height = 200;
        var bitmap = new BitmapImage(new Uri(_imagePath));
        ImageQuestion.Source = bitmap;

        // UploadProgressBar.Visibility = Visibility.Visible;
        // ButtonAdd.IsEnabled = false;
        // var progressHandler = new Progress<(long uploaded, long total)>(progress =>
        // {
        //     UploadProgressBar.Value = (double)progress.uploaded / progress.total * 100;
        // });
        //
        // try
        // {
        //     _imagePath = await Image.UploadImage(filePath, progressHandler);
        // }
        // catch (Exception exception)
        // {
        //     // Console.WriteLine(exception.Message);
        //     MessageBox.Show(exception.Message);
        //     ImageQuestion.Source = null;
        // }
        // ButtonAdd.IsEnabled = true;
        // UploadProgressBar.Visibility = Visibility.Collapsed;
    }

    private void ButtonAddImageAnswer(object sender, RoutedEventArgs e)
    {
        var imagePath = SelectImage();
        if (imagePath is null) return;
        if (AnswerDataGrid.SelectedIndex == _listAnswer.Count)
        {
            _listAnswer.Add(new Answer
            {
                Content = "",
                Image = imagePath.FileName
            });
        }
        else
        {
            if (AnswerDataGrid.SelectedItem is not Answer answer)
            {
                MessageBox.Show("Có lỗi xảy ra!");
                return;
            }

            answer.Image = imagePath.FileName;
        }
    }

    private static OpenFileDialog? SelectImage()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
        };

        return openFileDialog.ShowDialog() != true ? null : openFileDialog;
    }

    private void UIElement_OnKeyUp(object sender, KeyEventArgs keyEventArgs)
    {
        FormulaControl.Formula = RawFormula.Text;
    }
}