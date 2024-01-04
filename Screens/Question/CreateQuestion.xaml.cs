using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using TestBuilder.Data;
using TestBuilder.Models;

namespace TestBuilder.Screens.Question;

public partial class CreateQuestion
{
    private readonly TestDbContext _context = new();
    private readonly ObservableCollection<Answer> _listAnswer;
    private string _imagePath = "";

    public CreateQuestion()
    {
        InitializeComponent();
        _listAnswer = new ObservableCollection<Answer>
        {
            new()
            {
                Content = "Đáp án 1",
                Image = null
            }
        };
        AnswerDataGrid.ItemsSource = _listAnswer;
        DataContext = this;
    }

    private void ButtonClose(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void ButtonAddAnswer(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(ContentQuestion.TextBoxArea.Text))
        {
            MessageBox.Show("Vui lòng nhập dữ liệu");
            return;
        }

        var questionId = new Guid();

        var newQuestion = new Models.Question
        {
            QuestionId = questionId,
            Content = ContentQuestion.TextBoxArea.Text,
            Image = _imagePath,
            Options = [],
            TestQuestions = null
        };

        foreach (var answer in _listAnswer)
        {
            newQuestion.Options.Add(new Option
            {
                QuestionId = questionId,
                Text = answer.Content,
                Image = answer.Image,
                Question = newQuestion
            });
        }

        _context.Questions.Add(newQuestion);

        await _context.SaveChangesAsync();

        MessageBox.Show("Thêm thành công!");
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
}