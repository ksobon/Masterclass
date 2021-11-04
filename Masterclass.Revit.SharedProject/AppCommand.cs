using System;
using System.Linq;
using Autodesk.Revit.UI;
using Masterclass.Revit.FirstButton;
using Masterclass.Revit.SecondButton;

namespace Masterclass.Revit
{
    public class AppCommand : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            try
            {
                app.CreateRibbonTab("Masterclass");
            }
            catch (Exception e)
            {
                // ignored
            }

            var ribbonPanel = app.GetRibbonPanels("Masterclass").FirstOrDefault(x => x.Name == "Masterclass") ??
                              app.CreateRibbonPanel("Masterclass", "Masterclass");

            // (Konrad) Create buttons.
            FirstButtonCommand.CreateButton(ribbonPanel);
            ribbonPanel.AddSeparator();
            SecondButtonCommand.CreateButton(ribbonPanel);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }
    }
}
