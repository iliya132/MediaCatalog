using System.Windows;
using System.Windows.Media;

namespace MediaCatalog
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ScrollViewer_DragEnter(object sender, DragEventArgs e)
        {
            ContentBorder.BorderThickness = new Thickness(2);
            ContentBorder.BorderBrush = Brushes.OrangeRed;
        }

        private void ScrollViewer_DragLeave(object sender, DragEventArgs e)
        {
            ContentBorder.BorderThickness = new Thickness(0,2,0,0);
            ContentBorder.BorderBrush = Brushes.DarkGray;
        }
    }
}
