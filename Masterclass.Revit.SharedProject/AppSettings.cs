using System;
using System.ComponentModel;

namespace Masterclass.Revit
{
    public class AppSettings : INotifyPropertyChanged
    {
        private static readonly Lazy<AppSettings> Lazy = new Lazy<AppSettings>(() => new AppSettings());

        public static AppSettings Instance
        {
            get { return Lazy.Value; }
        }

        private StoredSettings _storedSettings = new StoredSettings();
        public StoredSettings StoredSettings
        {
            get { return _storedSettings; }
            set { _storedSettings = value; RaisePropertyChanged(nameof(StoredSettings)); }
        }

        private AppSettings()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
