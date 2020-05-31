using System.Windows.Controls;
using MediaCatalog.Model.DTO;
using System.Windows;
using System.Windows.Media;

namespace MediaCatalog.View.Controls
{
    public partial class TV_Card : UserControl
    {
        public TV_ProgramDTO TVProgram
        {
            get
            {
                return (TV_ProgramDTO)GetValue(TVProgramProperty);
            }
            set
            {
                SetValue(TVProgramProperty, value);
            }
        }
        public static DependencyProperty TVProgramProperty = 
            DependencyProperty.Register(
                name:"TVProgram", 
                propertyType: typeof(TV_ProgramDTO), 
                ownerType: typeof(TV_Card), 
                typeMetadata: new PropertyMetadata(defaultValue: new TV_ProgramDTO()));

        public delegate void ClickEventHandler(object sender);
        public event ClickEventHandler OnClick;

        public TV_Card(TV_ProgramDTO program)
        {
            InitializeComponent();
            TVProgram = program;
            ContextDelete.CommandParameter = TVProgram;
            ContextEdit.CommandParameter = TVProgram;
        }

        public void Select()
        {
            CardBorder.BorderBrush = Brushes.Red;
        }

        public void Unselect()
        {
            CardBorder.BorderBrush = Brushes.Black;
        }

        private void CardBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OnClick != null)
            {
                OnClick.Invoke(this);
            }
        }
    }
}
