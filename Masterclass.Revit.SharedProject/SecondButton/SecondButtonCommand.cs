using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Masterclass.Revit.Utilities;
using Nice3point.Revit.Toolkit.External;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Interop;

namespace Masterclass.Revit.SecondButton
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class SecondButtonCommand : ExternalCommand
    {
        public override void Execute()
        {
            try
            {
                var m = new SecondButtonModel(UiApplication);
                var vm = new SecondButtonViewModel(m);
                var v = new SecondButtonView
                {
                    DataContext = vm
                };

                _ = new WindowInteropHelper(v)
                {
                    Owner = Process.GetCurrentProcess().MainWindowHandle
                };

                v.ShowDialog();
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public static void CreateButton(RibbonPanel panel)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var type = typeof(SecondButtonCommand);

            panel.AddItem(new PushButtonData(
                type.Name,
                "Second" + Environment.NewLine + "Button",
                assembly.Location,
                type.FullName
            )
            {
                ToolTip = "Second button command.",
                LargeImage = ImageUtils.LoadImage(assembly, "_32x32.secondButton.png")
            });
        }
    }
}
