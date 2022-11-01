using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Masterclass.Revit.SecondButton;

namespace Masterclass.Revit.ThirdButton
{
    public class ThirdButtonModel
    {
        public UIApplication UiApp { get; }
        public Document Doc { get; }

        public ThirdButtonModel(UIApplication uiApp)
        {
            UiApp = uiApp;
            Doc = uiApp.ActiveUIDocument.Document;
        }

        public ObservableCollection<SpatialElementWrapper> CollectSpatialObjects()
        {
            var spatialObjects = new FilteredElementCollector(Doc)
                .OfClass(typeof(SpatialElement))
                .WhereElementIsNotElementType()
                .Cast<SpatialElement>()
                .Where(x => x is Room)
                .Select(x => new SpatialElementWrapper(x));

            return new ObservableCollection<SpatialElementWrapper>(spatialObjects);
        }

        public void Delete(List<SpatialElementWrapper> selected)
        {
            AppCommand.ThirdButtonHandler.Arg1 = selected;
            AppCommand.ThirdButtonHandler.Request = RequestId.Delete;
            AppCommand.ThirdButtonEvent.Raise();
        }
    }
}
