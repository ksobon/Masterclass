using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows.Interop;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Masterclass.Revit.SecondButton;
using Masterclass.Revit.Utilities;

namespace Masterclass.Revit.ThirdButton
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class ThirdButtonCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var uiApp = commandData.Application;
                var m = new ThirdButtonModel(uiApp);
                var vm = new ThirdButtonViewModel(m);
                var v = new ThirdButtonView
                {
                    DataContext = vm
                };

                var unused = new WindowInteropHelper(v)
                {
                    Owner = Process.GetCurrentProcess().MainWindowHandle
                };

                v.Show();

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
                MethodBase.GetCurrentMethod()?.DeclaringType?.Name,
                "Third" + Environment.NewLine + "Button",
                assembly.Location,
                MethodBase.GetCurrentMethod()?.DeclaringType?.FullName
            )
            {
                ToolTip = "Third button command.",
                LargeImage = ImageUtils.LoadImage(assembly, "_32x32.thirdButton.png")
            });
        }
    }
}
