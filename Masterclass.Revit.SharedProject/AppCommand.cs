using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Masterclass.Revit.FirstButton;
using Masterclass.Revit.FourthButton;
using Masterclass.Revit.SecondButton;
using Masterclass.Revit.ThirdButton;
using Masterclass.Revit.Utilities;
using Nice3point.Revit.Toolkit.External;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Masterclass.Revit
{
    public class AppCommand : ExternalApplication
    {
        public static ThirdButtonRequestHandler ThirdButtonHandler { get; set; }
        public static ExternalEvent ThirdButtonEvent { get; set; }
        public static string AssemblyPath = string.Empty;
        public static Messages Messages { get; set; } = new Messages();

        static AppCommand()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                var names = new List<string>
                {
                    "EPPlus.dll"
                };
                var assemblyName = new AssemblyName(args.Name).Name;
                if (!names.Contains(assemblyName)) return null;

                try
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var dir = Path.Combine(Path.GetDirectoryName(AssemblyPath),
                        names[names.IndexOf(assemblyName)] + ".dll");
                    var result = Assembly.LoadFrom(dir);
                    return result;
                }
                catch (Exception e)
                {
                    return null;
                }
            };
        }

        public override void OnStartup()
        {
            var message = "Hello world";
//            AssemblyPath = Assembly.GetAssembly(GetType()).Location;

//            try
//            {
//                Application.CreateRibbonTab("Masterclass");
//            }
//            catch (Exception e)
//            {
//                // ignored
//            }

//#if Revit2021 || Revit2022
//            var ribbonPanel = Application.GetRibbonPanels("Masterclass").FirstOrDefault(x => x.Name == "Masterclass") ??
//                              Application.CreateRibbonPanel("Masterclass", "Masterclass");
//#else
//            var ribbonPanel = Application.GetRibbonPanels("Masterclass").FirstOrDefault(x => x.Name == "Masterclass") ??
//                              Application.CreateRibbonPanel("Masterclass", "Masterclass");
//#endif

//            // (Konrad) Create buttons.
//            FirstButtonCommand.CreateButton(ribbonPanel);
//            ribbonPanel.AddSeparator();
//            SecondButtonCommand.CreateButton(ribbonPanel);
//            ribbonPanel.AddSeparator();
//            ThirdButtonCommand.CreateButton(ribbonPanel);
//            ribbonPanel.AddSeparator();
//            FourthButtonCommand.CreateButton(ribbonPanel);

//            DockablePanelUtils.RegisterDockablePanel(Application);

//            Application.ControlledApplication.DocumentChanged += OnDocumentChanged;
//            Application.ControlledApplication.DocumentSaved += OnDocumentSaved;
//            Application.ControlledApplication.DocumentSynchronizingWithCentral += OnDocumentSynchronizingWithCentral;
//            Application.ControlledApplication.DocumentOpened += OnDocumentOpened;

//            ThirdButtonHandler = new ThirdButtonRequestHandler();
//            ThirdButtonEvent = ExternalEvent.Create(ThirdButtonHandler);
        }

        public override void OnShutdown()
        {
            Application.ControlledApplication.DocumentChanged -= OnDocumentChanged;
            Application.ControlledApplication.DocumentSaved -= OnDocumentSaved;
            Application.ControlledApplication.DocumentSynchronizingWithCentral -= OnDocumentSynchronizingWithCentral;
            Application.ControlledApplication.DocumentOpened -= OnDocumentOpened;
        }

        private void OnDocumentSaved(object sender, DocumentSavedEventArgs e)
        {
            var doc = e.Document;
            if (doc == null || doc.IsFamilyDocument)
                return;

            try
            {
                SchemaUtils.SaveSchema(doc);
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        private void OnDocumentOpened(object sender, DocumentOpenedEventArgs e)
        {
            try
            {
                var doc = e.Document;
                if (doc == null || e.IsCancelled() || doc.IsFamilyDocument)
                    return;

                CheckIn(doc);
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        private void CheckIn(Document doc)
        {
            var settings = SchemaUtils.LoadSchema(doc);
            AppSettings.Instance.StoredSettings = settings ?? new StoredSettings();
        }

        private static void OnDocumentSynchronizingWithCentral(object sender, DocumentSynchronizingWithCentralEventArgs e)
        {
            var doc = e.Document;
            if (doc == null || doc.IsFamilyDocument)
                return;

            try
            {
                SchemaUtils.SaveSchema(doc);
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        private void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
        {
            if (e.Operation != UndoOperation.TransactionCommitted)
                return;

            var doc = e.GetDocument();
            var addedElements = e.GetAddedElementIds().Select(x => new ElementWrapper(doc.GetElement(x))).ToList();
            Messages.RaiseElementsAdded(new Messages.ElementsAddedEventArgs(addedElements));
        }
    }
}
