using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Masterclass.Revit.ThirdButton
{
    public class SpatialObjectDeletedMessage
    {
        public List<ElementId> Ids { get; set; }

        public SpatialObjectDeletedMessage(List<ElementId> ids)
        {
            Ids = ids;
        }
    }
}
