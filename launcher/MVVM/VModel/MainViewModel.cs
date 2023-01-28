using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using launcher.LauncherUI;

namespace launcher.MVVM.VModel
{
    class MainViewModel : ObservableObject
    {
        public PlayViewModel PlayVM { get; set; }

        private object _currentView;

        public object CurrentView
        { 
            get { return _currentView; }
            set
            { 
                _currentView= value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            PlayVM = new PlayViewModel();
            CurrentView = PlayVM;
        }  
    }
}
