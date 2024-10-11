using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Masterclass.Revit.Utilities;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;

namespace Masterclass.Revit.ThirdButton
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class ThirdButtonCommand : IExternalCommand
    {
        public static ThirdButtonView View { get; set; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                if (View != null)
                {
                    if (View.WindowState == WindowState.Minimized)
                        View.WindowState = WindowState.Normal;

                    View.Activate();
                    return Result.Succeeded;
                }

                var uiApp = commandData.Application;
                var m = new ThirdButtonModel(uiApp);
                var vm = new ThirdButtonViewModel(m);
                var v = new ThirdButtonView
                {
                    DataContext = vm
                };

                _ = new WindowInteropHelper(v)
                {
                    Owner = Process.GetCurrentProcess().MainWindowHandle
                };

                View = v;
                View.Closing += OnViewClosing;

                v.Show();

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                return Result.Failed;
            }
        }

        private static void OnViewClosing(object sender, CancelEventArgs e)
        {
            View = null;
            if (sender is ThirdButtonView view)
            {
                if (view.DataContext is ThirdButtonViewModel vm)
                    vm.Clear();

                view.Closing -= OnViewClosing;
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
