using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Masterclass.Revit.Utilities;
using Nice3point.Revit.Toolkit.External;
using System;
using System.Reflection;

namespace Masterclass.Revit.FourthButton
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class FourthButtonCommand : ExternalCommand
    {
        public override void Execute()
        {
            try
            {
                DockablePanelUtils.ShowDockablePanel(UiApplication);
            }
            catch (Exception e)
            {
                // ignored
            }
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
