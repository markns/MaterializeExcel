using System;

namespace MaterializeExcel.AddIn.Controller
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainController :IDisposable
    {
        public MainController(TaskPaneController taskPane
            , WpfInteractionController wpfInteraction)
        {
            TaskPane = taskPane;
            WpfInteraction = wpfInteraction;
        }

        public TaskPaneController TaskPane { get; private set; }

        public WpfInteractionController WpfInteraction { get; private set; }

        public void Dispose()
        {
            TaskPane.Dispose();
            WpfInteraction.Dispose();
        }
    }
}