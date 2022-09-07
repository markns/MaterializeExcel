namespace MaterializeExcel.AddIn.Startup.Contract
{
    public interface IRunner
    {
        void Execute(IRunnerMain bootstrap);
    }
}