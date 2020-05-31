using MediaCatalog.Model.DTO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MediaCatalog.View.Controls
{
    public partial class TV_CardsWrap : UserControl
    {
        #region ItemsSource
        public IEnumerable<TV_ProgramDTO> ItemsSource
        {
            get { return (IEnumerable<TV_ProgramDTO>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource",
                typeof(IEnumerable<TV_ProgramDTO>),
                typeof(TV_CardsWrap),
                new PropertyMetadata(OnItemsSourcePropertyChanged));

        private static void OnItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TV_CardsWrap control = sender as TV_CardsWrap;
            if (control != null)
                control.OnItemsSourceChanged((IEnumerable<TV_ProgramDTO>)e.OldValue, (IEnumerable<TV_ProgramDTO>)e.NewValue);
        }

        private void OnItemsSourceChanged(IEnumerable<TV_ProgramDTO> oldValue, IEnumerable<TV_ProgramDTO> newValue)
        {
            INotifyCollectionChanged oldValueINotifyPropertyChanged = oldValue as INotifyCollectionChanged;
            if (oldValueINotifyPropertyChanged != null)
            {
                oldValueINotifyPropertyChanged.CollectionChanged -= 
                    new NotifyCollectionChangedEventHandler(ChangeItemSource);
            }
            else
            {
                UpdateWrapPanel();
            }
            INotifyCollectionChanged newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if (newValueINotifyCollectionChanged != null)
            {
                newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(ChangeItemSource);
            }
        }

        private void ChangeItemSource(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateWrapPanel();
        }
        #endregion

        #region SelectedItem
        public TV_ProgramDTO SelectedItem
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
                name: "SelectedItem",
                propertyType: typeof(TV_ProgramDTO),
                ownerType: typeof(TV_CardsWrap),
                typeMetadata: new PropertyMetadata(SelectedItemChanged));

        private static void SelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TV_CardsWrap control = sender as TV_CardsWrap;
            if (control != null)
                control.OnSelectedItemChanged((TV_ProgramDTO)e.OldValue, (TV_ProgramDTO)e.NewValue);
        }

        private void OnSelectedItemChanged(TV_ProgramDTO oldValue, TV_ProgramDTO newValue)
        {
            INotifyCollectionChanged oldValueINotifyPropertyChanged = oldValue as INotifyCollectionChanged;
            if (oldValueINotifyPropertyChanged != null)
            {
                oldValueINotifyPropertyChanged.CollectionChanged -= 
                    new NotifyCollectionChangedEventHandler(ChangeSelectedItem);
            }
            else
            {
                SelectItem(SelectedItem);
            }
            INotifyCollectionChanged newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if (newValueINotifyCollectionChanged != null)
            {
                newValueINotifyCollectionChanged.CollectionChanged += 
                    new NotifyCollectionChangedEventHandler(ChangeSelectedItem);
            }
        }

        private void ChangeSelectedItem(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectItem(SelectedItem);
        }
        #endregion

        public TV_CardsWrap()
        {
            InitializeComponent();
        }

        private void UpdateWrapPanel()
        {
            CardsWrapPanel.Children.Clear();
            foreach (TV_ProgramDTO task in ItemsSource)
            {
                TV_Card tvCard = new TV_Card(task);
                tvCard.OnClick += SelectCard;
                if (!IsSourceCorrect(tvCard.TV_Image))
                {
                    tvCard.TV_Image.Source = new BitmapImage(new Uri("/Resources/nopicture.jpg", UriKind.Relative));
                }
                
                CardsWrapPanel.Children.Add(tvCard);
            }
            if(ItemsSource.Count() > 0)
            {
                if (SelectedItem == null)
                {
                    SelectedItem = ItemsSource.First();
                }
                SelectItem(SelectedItem);
            }
        }

        private bool IsSourceCorrect(Image image)
        {
            try
            {
                double i = image.Source.Height;
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void SelectItem(TV_ProgramDTO program)
        {
            foreach(TV_Card card in CardsWrapPanel.Children)
            {
                if(card.TVProgram == program)
                {
                    SelectCard(card);
                    break;
                }
            }
        }

        private void SelectCard(object card)
        {
            UnselectAllCards();
            Select(card as TV_Card);
        }

        private void UnselectAllCards()
        {
            foreach (TV_Card card in CardsWrapPanel.Children)
            {
                card.Unselect();
            }
        }

        private void Select(TV_Card card)
        {
            card.Select();
        }
    }
}
