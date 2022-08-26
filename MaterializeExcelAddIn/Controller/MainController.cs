using System;

namespace MaterializeExcelAddIn.Controller
{
    public class MainController :IDisposable
    {
        public MainController(SampleController sample
            , WpfInteractionController wpfInteraction)
        {
            Sample = sample;
            WpfInteraction = wpfInteraction;
        }

        public SampleController Sample { get; private set; }

        public WpfInteractionController WpfInteraction { get; private set; }

        public void Dispose()
        {
            Sample.Dispose();
            WpfInteraction.Dispose();
        }
    }
}