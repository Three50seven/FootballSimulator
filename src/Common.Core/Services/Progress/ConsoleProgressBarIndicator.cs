using System;

namespace Common.Core.Services
{
    /// <summary>
    /// Display progress indicator (bar) in console window.
    /// </summary>
    public class ConsoleProgressBarIndicator : TextProgressBarIndicatorBase
    {
        public ConsoleProgressBarIndicator()
            : base()
        {
            // A progress bar is only for temporary display in a console window.
            // If the console output is redirected to a file, draw nothing.
            // Otherwise, we'll end up with a lot of garbage in the target file.
            if (!Console.IsOutputRedirected)
                ResetTimer();
        }

        protected override void OnReportedResult(string result)
        {
            Console.Write(result);
        }
    }
}
