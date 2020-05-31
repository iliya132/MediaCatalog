using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using MediaCatalog.Model.Interfaces;
using MediaCatalog.Model.Implementations;

namespace MediaCatalog.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<IProgramsProvider, SQLiteDataProvider>();
            SimpleIoc.Default.Register<IMediaInfoProvider, MediaInfo>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
    }
}