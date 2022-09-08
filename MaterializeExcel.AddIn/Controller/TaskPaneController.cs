using System;
using System.Collections.Generic;
using ExcelDna.Integration.CustomUI;
using MaterializeExcel.View;
using MaterializeExcel.ViewModel;
using ReactiveUI;
using Splat;

namespace MaterializeExcel.AddIn.Controller
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TaskPaneController : IDisposable  
    {
        public static CustomTaskPane MyPanel;

        private static readonly Dictionary<string, CustomTaskPane> MyTaskPanes =
            new Dictionary<string, CustomTaskPane>();

        public void OpenForm()
        {
            // TODO: handle opening and closing task pane

            // var excel = (Application) ExcelDnaUtil.Application;
            // var key = $"materializeExcel-({excel.Hwnd})";
            //
            // if (!MyTaskPanes.ContainsKey(key))
            // {

            var view = (MainControl)Locator.Current.GetService(typeof(IViewFor<MainControlViewModel>));

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
            // todo: dispose task pane
        }
    }
}