using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Masterclass.Revit.SecondButton;

namespace Masterclass.Revit.ThirdButton
{
    public class ThirdButtonViewModel : ViewModelBase
    {
        public ThirdButtonModel Model { get; set; }
        public RelayCommand<Window> Close { get; set; }
        public RelayCommand<Window> Delete { get; set; }

        private ObservableCollection<SpatialElementWrapper> _spatialObjects;
        public ObservableCollection<SpatialElementWrapper> SpatialObjects
        {
            get { return _spatialObjects; }
            set { _spatialObjects = value; RaisePropertyChanged(() => SpatialObjects); }
        }

        public ThirdButtonViewModel(ThirdButtonModel model)
        {
            Model = model;
            SpatialObjects = Model.CollectSpatialObjects();
            Close = new RelayCommand<Window>(OnClose);
            Delete = new RelayCommand<Window>(OnDelete);

            Messenger.Default.Register<SpatialObjectDeletedMessage>(this, OnSpatialObjectDeletedMessage);
        }

        private void OnSpatialObjectDeletedMessage(SpatialObjectDeletedMessage obj)
        {
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
    }
}
