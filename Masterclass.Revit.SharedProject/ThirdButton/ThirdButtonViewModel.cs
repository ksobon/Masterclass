using Masterclass.Revit.SecondButton;
using Masterclass.Revit.Utilities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Masterclass.Revit.ThirdButton
{
    public class ThirdButtonViewModel : INotifyPropertyChanged
    {
        public ThirdButtonModel Model { get; set; }
        public RelayCommand<Window> Close { get; set; }
        public RelayCommand<Window> Delete { get; set; }

        private ObservableCollection<SpatialElementWrapper> _spatialObjects;
        public ObservableCollection<SpatialElementWrapper> SpatialObjects
        {
            get { return _spatialObjects; }
            set { _spatialObjects = value; RaisePropertyChanged(nameof(SpatialObjects)); }
        }

        public ThirdButtonViewModel(ThirdButtonModel model)
        {
            Model = model;
            SpatialObjects = Model.CollectSpatialObjects();
            Close = new RelayCommand<Window>(OnClose);
            Delete = new RelayCommand<Window>(OnDelete);

            AppCommand.Messages.SpatialObjectsDeleted += OnSpatialObjectsDeleted;
        }

        public void Clear()
        {
            AppCommand.Messages.SpatialObjectsDeleted -= OnSpatialObjectsDeleted;
        }

        private void OnSpatialObjectsDeleted(object sender, EventArgs e)
        {
            var obj = (Messages.SpatialObjectsDeletedEventArgs)e;

            var spatialObjects = SpatialObjects.Where(x => !obj.Ids.Contains(x.Id));
            SpatialObjects = new ObservableCollection<SpatialElementWrapper>(spatialObjects);
        }

        private void OnDelete(Window win)
        {
            var selected = SpatialObjects.Where(x => x.IsSelected).ToList();
            Model.Delete(selected);
        }

        private void OnClose(Window win)
        {
            win.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
