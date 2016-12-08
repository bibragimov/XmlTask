using System.Windows;
using XmlTask.ViewModels;

namespace XmlTask
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Model;
        }

        public MainViewModel Model
        {
            get { return new MainViewModel(); }
        }
    }
}