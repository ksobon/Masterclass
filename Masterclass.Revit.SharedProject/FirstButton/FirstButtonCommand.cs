using System;
using System.Reflection;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Masterclass.Revit.Utilities;

namespace Masterclass.Revit.FirstButton
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class FirstButtonCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                MessageBox.Show("Hell world!", "AEC Tech", MessageBoxButton.OK);

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                return Result.Failed;
            }
        }

        public static void CreateButton(RibbonPanel panel)
        {
            var assembly = Assembly.GetExecutingAssembly();
            panel.AddItem(new PushButtonData(
                MethodBase.GetCurrentMethod().DeclaringType?.Name,
                "First" + Environment.NewLine + "Button",
                assembly.Location,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName
            )
            {
                ToolTip = "First button command.",
                LargeImage = ImageUtils.LoadImage(assembly, "_32x32.firstButton.png")
            });
        }
    }
}
