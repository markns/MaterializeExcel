using System.Runtime.InteropServices;
using AddinX.Ribbon.Contract;
using AddinX.Ribbon.Contract.Command;
using AddinX.Ribbon.ExcelDna;

namespace MaterializeExcel.AddIn
{
    [ComVisible(true)]
    public class AddInRibbon : RibbonFluent
    {
        protected override void CreateFluentRibbon(IRibbonBuilder build)
        {
            build.CustomUi.Ribbon.Tabs(tab =>
                tab.AddTab("Materialize").SetId("CustomTab")
                    .Groups(g =>
                    {
                        g.AddGroup("Materialize").SetId("MaterializeGroup")
                            .Items(i =>
                            {
                                i.AddButton("Data Catalog").SetId("DataCatalogIdCmd")
                                    .LargeSize().ImageMso("TableOpenInBrowser").ShowLabel()
                                    .Screentip("");
                            });
                    }));
        }

        protected override void CreateRibbonCommand(IRibbonCommands cmds)
        {
            cmds.AddButtonCommand("DataCatalogIdCmd")
                // .IsEnabled(() => AddInContext.ExcelApp.Worksheets.Any()).IsVisible(() => true)
                .Action(() => AddInContext.MainController.TaskPane.OpenForm());
        }

        public override void OnClosing()
        {
            AddInContext.TokenCancellationSource.Cancel();

            AddInContext.MainController.Dispose();

            AddInContext.ExcelApp = null;

            AddInContext.Container.Dispose();
            AddInContext.Container = null;
        }

        public override void OnOpening()
        {
            // AddInContext.ExcelApp.SheetSelectionChangeEvent += (a, e) => Ribbon?.Invalidate();
            // AddInContext.ExcelApp.SheetActivateEvent += (e) => Ribbon?.Invalidate();
            // AddInContext.ExcelApp.SheetChangeEvent += (a, e) => Ribbon?.Invalidate();
        }
    }
}