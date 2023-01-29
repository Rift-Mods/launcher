﻿using launcher.LauncherUI;

namespace launcher.MVVM.VModel
{
    class MainViewModel : ObservableObject
    {

        public RCommand ModCommand { get; set; }
        public RCommand MainCommand { get; set; }
        public RCommand SettingsCommand { get; set; }
        public ModViewModel ModVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }
        public PlayViewModel PlayVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            PlayVM = new PlayViewModel();
            ModVM = new ModViewModel();
            SettingsVM = new SettingsViewModel();
            CurrentView = PlayVM;

            MainCommand = new RCommand(o =>
            {
                CurrentView = PlayVM;
            });

            ModCommand = new RCommand(o =>
            {
                CurrentView = ModVM;
            });

            SettingsCommand = new RCommand(o =>
            {
                CurrentView = SettingsVM;
            });
        }
    }
}
