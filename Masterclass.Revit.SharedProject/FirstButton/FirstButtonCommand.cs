using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Masterclass.Revit.Utilities;
using Nice3point.Revit.Toolkit.External;
using System;
using System.Reflection;
using System.Windows;

namespace Masterclass.Revit.FirstButton
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class FirstButtonCommand : ExternalCommand
    {
        public override void Execute()
        {
            try
            {
                MessageBox.Show("Hell world!", "AEC Tech", MessageBoxButton.OK);
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public static void CreateButton(RibbonPanel panel)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var type = typeof(FirstButtonCommand);

            panel.AddItem(new PushButtonData(
                type.Name,
                "First" + Environment.NewLine + "Button",
                assembly.Location,
                type.FullName
            )
            {
                ToolTip = "First button command.",
                LargeImage = ImageUtils.LoadImage(assembly, "_32x32.firstButton.png")
            });
        }
    }
}
