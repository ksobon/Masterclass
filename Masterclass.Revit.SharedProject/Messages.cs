using Autodesk.Revit.DB;
using Masterclass.Revit.FourthButton;
using System;
using System.Collections.Generic;

namespace Masterclass.Revit
{
    public class Messages
    {
        #region Elements Added

        public event EventHandler ElementsAdded;

        public void RaiseElementsAdded(EventArgs e)
        {
            OnElementsAdded(e);
        }

        protected virtual void OnElementsAdded(EventArgs e)
        {
            ElementsAdded?.Invoke(this, e);
        }

        public class ElementsAddedEventArgs : EventArgs
        {
            public List<ElementWrapper> AddedElements { get; set; }

            public ElementsAddedEventArgs(List<ElementWrapper> elements)
            {
                AddedElements = elements;
            }
        }

        #endregion

        #region Spatial Objects Deleted

        public event EventHandler SpatialObjectsDeleted;

        public void RaiseSpatialObjectsDeleted(EventArgs e)
        {
            OnSpatialObjectsDeleted(e);
        }

        protected virtual void OnSpatialObjectsDeleted(EventArgs e)
        {
            SpatialObjectsDeleted?.Invoke(this, e);
        }

        public class SpatialObjectsDeletedEventArgs : EventArgs
        {
            public List<ElementId> Ids { get; set; }

            public SpatialObjectsDeletedEventArgs(List<ElementId> ids)
            {
                Ids = ids;
            }
        }

        #endregion

    }
}
