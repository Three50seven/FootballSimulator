namespace Common.Core.Services
{
    public class ConsoleProgressIndicatorFactory : IFactory<IProgressIndicator>
    {
        public IProgressIndicator Create()
        {
            return new ConsoleProgressBarIndicator();
        }
    }
}
