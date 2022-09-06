
using System;
using System.Collections.Generic;
using AddinX.Wpf.Contract;
using ExcelDna.Integration.CustomUI;
using MaterializeExcel.View;
using MaterializeExcelViewModel;
using ReactiveUI;
using Splat;

namespace MaterializeExcelAddIn.Controller
{
    public class TaskPaneController : IDisposable
    {
        public static CustomTaskPane MyPanel;

        private static readonly Dictionary<string, CustomTaskPane> MyTaskPanes =
            new Dictionary<string, CustomTaskPane>();

        private readonly ILogger logger;
        private IWpfHelper wpfHelper;

        // Autofac.Core.DependencyResolutionException:
        // An exception was thrown while activating
        // MaterializeExcelAddIn.Controller.MainController -> MaterializeExcelAddIn.Controller.TaskPaneController.
        // ---> Autofac.Core.DependencyResolutionException: None of the constructors found with 'Autofac.Core.Activators.Reflection.DefaultConstructorFinder' on type 'MaterializeExcelAddIn.Controller.TaskPaneController' can be invoked with the available services and parameters:
        // Cannot resolve parameter 'Splat.ILogger logger' of constructor 'Void .ctor(Splat.ILogger, AddinX.Wpf.Contract.IWpfHelper)'.
        
        public TaskPaneController(IWpfHelper wpfHelper)
        {
            // this.logger = logger;
            this.wpfHelper = wpfHelper;
        }

        public void OpenForm()
        {
            // var excel = (Application) ExcelDnaUtil.Application;
            // var key = $"materializeExcel-({excel.Hwnd})";
            //
            // if (!MyTaskPanes.ContainsKey(key))
            // {
            
            var view = (MainControl)Locator.Current.GetService(typeof (IViewFor<MainControlViewModel>));
            
            var mainViewHost = new MainViewHost(view);

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