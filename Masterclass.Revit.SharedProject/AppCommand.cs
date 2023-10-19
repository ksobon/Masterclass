using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using GalaSoft.MvvmLight.Messaging;
using Masterclass.Revit.FirstButton;
using Masterclass.Revit.FourthButton;
using Masterclass.Revit.SecondButton;
using Masterclass.Revit.ThirdButton;
using Masterclass.Revit.Utilities;
using NLog;
using System;
using System.Linq;

namespace Masterclass.Revit
{
    public class AppCommand : IExternalApplication
    {
        private static readonly Logger Logger = NLogUtils.GetMasterclassLogger();
        public static ThirdButtonRequestHandler ThirdButtonHandler { get; set; }
        public static ExternalEvent ThirdButtonEvent { get; set; }

        public Result OnStartup(UIControlledApplication app)
        {
            // (Konrad) Log Levels
            // Logger.Fatal("Fatal message.");
            // Logger.Error("Error message.");
            // Logger.Warn("Warning message.");
            // Logger.Info("Info message.");
            // Logger.Debug("Debug message.");
            // Logger.Trace("Trace message.");
            //
            // // (Konrad) Logging message. Different constructors.
            // Logger.Debug("This is a debug message.");
            // Logger.Error(new Exception(), "This is an error message.");
            // Logger.Fatal("This is a fatal message.");
            //
            // // (Konrad) Logging with custom parameters.
            // var msg = new LogEventInfo(LogLevel.Info, "", "This is a message.");
            // msg.Properties.Add("User", "Ray Donovan");
            // Logger.Info(msg);
            //
            // // (Konrad) Structured messaging.
            // Logger.Info(string.Format("This is a message from {0}", "Mickey Donovan"));
            // Logger.Info("This is a message from {User}.", "Mickey Donovan");

            NLogManager.InfoLog("This is a info message with parameters.");

            try
            {
                app.CreateRibbonTab("Masterclass");
            }
            catch (Exception e)
            {
                // ignored
            }

#if Revit2021 || Revit2022
            var ribbonPanel = app.GetRibbonPanels("Masterclass").FirstOrDefault(x => x.Name == "Masterclass") ??
                              app.CreateRibbonPanel("Masterclass", "Masterclass");
#else
            var ribbonPanel = app.GetRibbonPanels("Masterclass").FirstOrDefault(x => x.Name == "Masterclass") ??
                              app.CreateRibbonPanel("Masterclass", "Masterclass");
#endif

            // (Konrad) Create buttons.
            FirstButtonCommand.CreateButton(ribbonPanel);
            ribbonPanel.AddSeparator();
            SecondButtonCommand.CreateButton(ribbonPanel);
            ribbonPanel.AddSeparator();
            ThirdButtonCommand.CreateButton(ribbonPanel);
            ribbonPanel.AddSeparator();
            FourthButtonCommand.CreateButton(ribbonPanel);

            DockablePanelUtils.RegisterDockablePanel(app);

            app.ControlledApplication.DocumentChanged += OnDocumentChanged;

            ThirdButtonHandler = new ThirdButtonRequestHandler();
            ThirdButtonEvent = ExternalEvent.Create(ThirdButtonHandler);

            return Result.Succeeded;
        }

        private void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
        {
            if (e.Operation != UndoOperation.TransactionCommitted)
                return;

            var doc = e.GetDocument();
            var addedElements = e.GetAddedElementIds().Select(x => new ElementWrapper(doc.GetElement(x))).ToList();
            Messenger.Default.Send(new AddedElementsMessage(addedElements));
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }
    }
}
