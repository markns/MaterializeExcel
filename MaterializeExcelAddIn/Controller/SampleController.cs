using System;
using System.Collections.Generic;
using AddinX.Wpf.Contract;
using ExcelDna.Integration.CustomUI;
using MaterializeExcel.View;
using NLog;
using ReactiveUI;

namespace MaterializeExcelAddIn.Controller
{
    
    public class SampleController : IDisposable
    {
        public static CustomTaskPane MyPanel;
        private static readonly Dictionary<string, CustomTaskPane> MyTaskPanes =
            new Dictionary<string, CustomTaskPane>();
        
        private readonly ILogger logger;
        private IWpfHelper wpfHelper;

        public SampleController(ILogger logger, IWpfHelper wpfHelper)
        {
            this.logger = logger;
            this.wpfHelper = wpfHelper;
        }

        public void OpenForm()
        {
            // var excel = (Application) ExcelDnaUtil.Application;
            // var key = $"materializeExcel-({excel.Hwnd})";
            //
            // if (!MyTaskPanes.ContainsKey(key))
            // {
                var mainViewHost = new MainViewHost(new MainControl(RxApp.MainThreadScheduler));

                MyPanel =
                    CustomTaskPaneFactory.CreateCustomTaskPane(mainViewHost, "Materialize Catalog");
                MyPanel.Width = 250;
                // MyPanel.DockPosition = Settings.Default.ApplicationsViewDockPosition;
                // MyPanel.VisibleStateChange += MainViewTaskPaneOnVisibleStateChange;
                // MyPanel.DockPositionStateChange += MainViewTaskPaneOnDockPositionStateChange;
                MyPanel.Visible = true;
            //     MyTaskPanes.Add(key, MyPanel);
            // }
            // else
            // {
            //     MyPanel = MyTaskPanes[key];
            //     MyPanel.Visible = true;
            // }
            
            // var window = AddinContext.Container.Resolve<MainWindow>();
            // wpfHelper.Show(window);
        }

        public void Dispose()
        {
            wpfHelper = null;
        }
    }
}