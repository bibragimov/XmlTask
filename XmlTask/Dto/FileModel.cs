using XmlTask.Utils;

namespace XmlTask.Dto
{
    public class FileModel : Notifier
    {
        private string _dateTime;

        private string _fileVersion;

        private string _name;
        public long Id { get; set; }

        public string FileVersion
        {
            get { return _fileVersion; }
            set
            {
                _fileVersion = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string DateTime
        {
            get { return _dateTime; }
            set
            {
                _dateTime = value;
                OnPropertyChanged();
            }
        }
    }
}