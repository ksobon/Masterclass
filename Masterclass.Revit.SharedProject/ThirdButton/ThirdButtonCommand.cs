using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Masterclass.Revit.Utilities;
using Nice3point.Revit.Toolkit.External;
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
    public class ThirdButtonCommand : ExternalCommand
    {
        public static ThirdButtonView View { get; set; }

        public override void Execute()
        {
            try
            {
                if (View != null)
                {
                    if (View.WindowState == WindowState.Minimized)
                        View.WindowState = WindowState.Normal;

                    View.Activate();
                }

                var m = new ThirdButtonModel(UiApplication);
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
            }
            catch (Exception e)
            {
                // ignored
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
            var type = typeof(ThirdButtonCommand);

            panel.AddItem(new PushButtonData(
                type.Name,
                "Third" + Environment.NewLine + "Button",
                assembly.Location,
                type.FullName
            )
            {
                ToolTip = "Third button command.",
                LargeImage = ImageUtils.LoadImage(assembly, "_32x32.thirdButton.png")
            });
        }
    }
}
