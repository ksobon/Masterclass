using System;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Masterclass.Revit.Utilities;

namespace Masterclass.Revit.FourthButton
{
    public class FourthButtonCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var app = commandData.Application;
                DockablePanelUtils.ShowDockablePanel(app);
            }
            catch (Exception e)
            {
                return Result.Failed;
            }

            return Result.Succeeded;
        }

        public static void CreateButton(RibbonPanel panel)
        {
            var assembly = Assembly.GetExecutingAssembly();
            panel.AddItem(
                new PushButtonData(
                    MethodBase.GetCurrentMethod()?.DeclaringType?.Name,
                    "Fourth" + Environment.NewLine + "Button",
                    assembly.Location,
                    MethodBase.GetCurrentMethod()?.DeclaringType?.FullName
                )
                {
                    ToolTip = "Fourth button tooltip.",
                    LargeImage = ImageUtils.LoadImage(assembly, "_32x32.fourthButton.png")
                }
            );
        }
    }
}
