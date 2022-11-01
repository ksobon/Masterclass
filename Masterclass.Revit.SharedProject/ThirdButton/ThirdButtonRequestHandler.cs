using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using GalaSoft.MvvmLight.Messaging;
using Masterclass.Revit.SecondButton;

namespace Masterclass.Revit.ThirdButton
{
    public enum RequestId
    {
        None,
        Delete,
        Select
    }

    public class ThirdButtonRequestHandler : IExternalEventHandler
    {
        public RequestId Request { get; set; }
        public object Arg1 { get; set; }

        public void Execute(UIApplication app)
        {
            try
            {
                switch (Request)
                {
                    case RequestId.None:
                        return;
                    case RequestId.Delete:
                        Delete(app);
                        break;
                    case RequestId.Select:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                // ignore
            }
        }

        public void Delete(UIApplication app)
        {
            if (!(Arg1 is List<SpatialElementWrapper> selected))
                return;

            var doc = app.ActiveUIDocument.Document;
            var ids = selected.Select(x => x.Id).ToList();
            using (var trans = new Transaction(doc, "Delete Rooms"))
            {
                trans.Start();
                doc.Delete(ids);
                trans.Commit();
            }

            Messenger.Default.Send(new SpatialObjectDeletedMessage(ids));
        }

        public string GetName()
        {
            return "Third Button Request Handler";
        }
    }
}