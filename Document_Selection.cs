using System;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;

namespace Document_Selection
{
    public class Document_Selection_button : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            // Add a new ribbon panel
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("NewRibbonPanel1");

            // Create a push button to trigger a command add it to the ribbon panel.
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            PushButtonData buttonData = new PushButtonData("cmdHelloWorld1",
               "Document_select", thisAssemblyPath, "Document_Selection.Document_Selection_action");

            PushButton? pushButton = ribbonPanel.AddItem(buttonData) as PushButton;

            if (pushButton == null)
            {
                TaskDialog.Show("Error", "Failed to create PushButton. Check your buttonData configuration.");
                return Result.Failed;
            }

            // Optionally, other properties may be assigned to the button
            pushButton.ToolTip = "Execute Document Selection.";

            // Use supported image formats (e.g., .png)
            Uri uriImage = new Uri(@"C:\Users\kentaro.yamasaki\source\repos\Document_Selection\Add.webp");
            BitmapImage largeImage = new BitmapImage(uriImage);
            pushButton.LargeImage = largeImage;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)]
    public class Document_Selection_action : IExternalCommand
    {
        [Obsolete]
        public Autodesk.Revit.UI.Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            try
            {
                // Get active document
                UIDocument uidoc = commandData.Application.ActiveUIDocument;

                // Get selected elements
                ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();

                if (selectedIds.Count == 0)
                {
                    TaskDialog.Show("Revit", "No elements selected.");
                }
                else
                {
                    string info = "Selected Element IDs:";
                    foreach (ElementId id in selectedIds)
                    {
                        info += $"\n{id.IntegerValue}";
                    }
                    TaskDialog.Show("Revit", info);
                }
            }
            catch (Exception e)
            {
                message = e.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
}
