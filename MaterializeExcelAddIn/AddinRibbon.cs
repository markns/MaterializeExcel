using System.Linq;
using System.Runtime.InteropServices;
using AddinX.Ribbon.Contract;
using AddinX.Ribbon.Contract.Command;
using AddinX.Ribbon.ExcelDna;

namespace MaterializeExcelAddIn
{
    [ComVisible(true)]
    public class AddinRibbon : RibbonFluent
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
                                https: //bert-toolkit.com/imagemso-list.html
                                i.AddButton("Config").SetId("ConfigIdCmd")
                                    .LargeSize().ImageMso("DataSourceCatalogServerScript").ShowLabel()
                                    .Screentip("");
                                i.AddButton("Data Catalog").SetId("DataCatalogIdCmd")
                                    .LargeSize().ImageMso("SharingOpenCalendarFolder").ShowLabel()
                                    .Screentip("");
                            });
                    }));
        }

        protected override void CreateRibbonCommand(IRibbonCommands cmds)
        {
            cmds.AddButtonCommand("ConfigIdCmd")
                .IsEnabled(() => AddinContext.ExcelApp.Worksheets.Any()).IsVisible(() => true)
                .Action(() => AddinContext.MainController.Sample.OpenForm());
        }

        public override void OnClosing()
        {
            AddinContext.TokenCancellationSource.Cancel();

            AddinContext.MainController.Dispose();

            AddinContext.ExcelApp.DisposeChildInstances(true);
            AddinContext.ExcelApp = null;

            AddinContext.Container.Dispose();
            AddinContext.Container = null;
        }

        public override void OnOpening()
        {
            AddinContext.ExcelApp.SheetSelectionChangeEvent += (a, e) => Ribbon?.Invalidate();
            AddinContext.ExcelApp.SheetActivateEvent += (e) => Ribbon?.Invalidate();
            AddinContext.ExcelApp.SheetChangeEvent += (a, e) => Ribbon?.Invalidate();
        }
    }
}