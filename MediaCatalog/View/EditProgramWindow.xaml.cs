using System.Windows;
using System.Windows.Media;

namespace MediaCatalog.View
{
    public partial class EditProgramWindow : Window
    {
        public EditProgramWindow()
        {
            InitializeComponent();
        }

        private void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateFields())
            {
                DialogResult = true;
                Close();
            }
        }

        private bool ValidateFields()
        {
            bool success = true;

            if (string.IsNullOrWhiteSpace(ProgramName.Text))
            {
                success = false;
                ProgramNameLabel.Text = "Поле не может быть пустым";
                ProgramNameLabel.Foreground = Brushes.Red;
            }

            if (string.IsNullOrWhiteSpace(ProgramDescription.Text))
            {
                success = false;
                ProgramDescriptionLabel.Text = "Поле не может быть пустым";
                ProgramDescriptionLabel.Foreground = Brushes.Red;
            }

            if (string.IsNullOrWhiteSpace(Actors.Text))
            {
                success = false;
                ActorsLabel.Text = "Поле не может быть пустым";
                ActorsLabel.Foreground = Brushes.Red;
            }

            if (string.IsNullOrWhiteSpace(YearEstablished.Text))
            {
                success = false;
                YearEstablishedLabel.Text = "Поле не может быть пустым";
                YearEstablishedLabel.Foreground = Brushes.Red;
            }
            return success;
        }
    }
}
