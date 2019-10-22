namespace Node.Net.Diagnostics
{
    public interface ICommand
    {
        ICommand Execute();
        int ExitCode { get; }
        string Name { get; }
        string Output { get; set; }
        string Error { get; set; }
        int Timeout { get; }
        string Summary { get; }
    }
}
