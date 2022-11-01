using System.Collections.Generic;

namespace Masterclass.Revit.FourthButton
{
    public class AddedElementsMessage
    {
        public List<ElementWrapper> AddedElements { get; set; }

        public AddedElementsMessage(List<ElementWrapper> elements)
        {
            AddedElements = elements;
        }
    }
}
