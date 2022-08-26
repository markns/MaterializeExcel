using AddinX.Bootstrap.Contract;
using Autofac;
using MaterializeExcelWPF;

// using MaterializeExcelWPF.View;

// using MaterializeExcel.WPF;

namespace MaterializeExcelAddIn.Startup
{
    internal class RunnerWpfObjects : IRunner
    {
        public void Execute(IRunnerMain bootstrap)
        {
            var bootstrapper = (Bootstrapper)bootstrap;

            bootstrapper?.Builder.RegisterType<MainWindow>();
            // bootstrapper?.Builder.RegisterType<MainWindowViewModel>();
            //
            // bootstrapper?.Builder.RegisterType<MeetingWizardContainerViewModel>();
            // bootstrapper?.Builder.RegisterType<MeetingWizardContainerView>();
            //
            // bootstrapper?.Builder.RegisterType<MeetingController>();
            //
            //
            // bootstrapper?.Builder.RegisterType<MeetingWizardFirstView>();
            // bootstrapper?.Builder.RegisterType<MeetingWizardSecondView>();
            // bootstrapper?.Builder.RegisterType<MeetingWizardLastView>();
            //
            // bootstrapper?.Builder.RegisterType<MeetingWizardFirstViewModel>();
            // bootstrapper?.Builder.RegisterType<MeetingWizardSecondViewModel>();
            // bootstrapper?.Builder.RegisterType<MeetingWizardLastViewModel>();
        }
    }
}