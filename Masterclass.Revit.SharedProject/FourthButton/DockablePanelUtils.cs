using System;
using Autodesk.Revit.UI;

namespace Masterclass.Revit.FourthButton
{
    public static class DockablePanelUtils
    {
        public static void RegisterDockablePanel(UIControlledApplication app)
        {
            var vm = new DockablePanelViewModel();
            var v = new DockablePanelPage
            {
                DataContext = vm
            };

            var panelId = new DockablePaneId(new Guid("7A8F5162-67F6-4DFB-A3C7-E397292A7B38"));

            try
            {
                app.RegisterDockablePane(panelId, "Masterclass", v);
            }
            catch (Exception e)
            {
                // something magical happened
            }
        }

        public static void ShowDockablePanel(UIApplication app)
        {
            var panelId = new DockablePaneId(new Guid("7A8F5162-67F6-4DFB-A3C7-E397292A7B38"));
            var dp = app.GetDockablePane(panelId);
            if (dp != null && !dp.IsShown())
                dp.Show();
        }
    }
}
