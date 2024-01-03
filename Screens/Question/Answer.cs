using System.ComponentModel;

namespace TestBuilder.Screens.Question;

public sealed class Answer : INotifyPropertyChanged
{
    private string _content = "";
    private string? _image;

    public string Content
    {
        get => _content;
        set
        {
            if (_content == value) return;
            _content = value;
            OnPropertyChanged(nameof(Content));
        }
    }

    public string? Image
    {
        get => _image;
        set
        {
            if (_image == value) return;
            _image = value;
            OnPropertyChanged(nameof(Image));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}