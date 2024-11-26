using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EmojiMemoryGameSystem
{
    public class EmojiCard : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string emoji { get; set; }
        private bool _isTextVisible { get; set; }
        private bool _isEnabled { get; set; }

        public EmojiCard(string emojistring)
        {
            emoji = emojistring;
            _isTextVisible = false;
            _isEnabled = false;
        }
        public bool IsTextVisible
        {
            get => _isTextVisible;
            set
            {
                _isTextVisible = value;
                this.InvokePropertyChanged("IsTextVisible");
                this.InvokePropertyChanged("DisplayedText");
            }
        }
        public string DisplayedText => IsTextVisible ? emoji : ""; // Conditional property for the text
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                this.InvokePropertyChanged("IsEnabled");
            }
        }
        private void InvokePropertyChanged([CallerMemberName] string propertyname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

    }
}
