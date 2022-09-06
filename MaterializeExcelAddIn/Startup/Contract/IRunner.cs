namespace MaterializeExcelAddIn.Startup.Contract
{
    public interface IRunner
    {
        void Execute(IRunnerMain bootstrap);
    }
}