using System;
using System.Text;
using System.Threading;

namespace Common.Core.Services
{
    public abstract class TextProgressBarIndicatorBase : IProgressIndicator
    {
        private const int _blockCount = 10;
        private readonly TimeSpan _animationInterval = TimeSpan.FromSeconds(1.0 / 8);
        private const string _animation = @"|/-\";

        private readonly Timer _timer;

        private double _currentProgress = 0;
        private string _currentText = string.Empty;
        private bool _disposed = false;
        private int _animationIndex = 0;

        public TextProgressBarIndicatorBase()
        {
            _timer = new Timer(TimerHandler);
        }

        public virtual void Report(double value)
        {
            // Make sure value is in [0..1] range
            value = Math.Max(0, Math.Min(1, value));
            Interlocked.Exchange(ref _currentProgress, value);
        }

        protected virtual void TimerHandler(object state)
        {
            lock (_timer)
            {
                if (_disposed)
                    return;

                int progressBlockCount = (int)(_currentProgress * _blockCount);
                int percent = (int)(_currentProgress * 100);
                string text = string.Format("[{0}{1}] {2,3}% {3}",
                    new string('#', progressBlockCount), new string('-', _blockCount - progressBlockCount),
                    percent,
                    _animation[_animationIndex++ % _animation.Length]);

                UpdateText(text);
                ResetTimer();
            }
        }

        protected virtual void UpdateText(string text)
        {
            // Get length of common portion
            int commonPrefixLength = 0;
            int commonLength = Math.Min(_currentText.Length, text.Length);
            while (commonPrefixLength < commonLength && text[commonPrefixLength] == _currentText[commonPrefixLength])
            {
                commonPrefixLength++;
            }

            // Backtrack to the first differing character
            var outputBuilder = new StringBuilder();
            outputBuilder.Append('\b', _currentText.Length - commonPrefixLength);

            // Output new suffix
            outputBuilder.Append(text.Substring(commonPrefixLength));

            // If the new text is shorter than the old one: delete overlapping characters
            int overlapCount = _currentText.Length - text.Length;
            if (overlapCount > 0)
            {
                outputBuilder.Append(' ', overlapCount);
                outputBuilder.Append('\b', overlapCount);
            }

            OnReportedResult(outputBuilder.ToString());
            _currentText = text;
        }

        protected abstract void OnReportedResult(string result);

        protected virtual void ResetTimer()
        {
            _timer?.Change(_animationInterval, TimeSpan.FromMilliseconds(-1));
        }

        public virtual void Dispose()
        {
            lock (_timer)
            {
                _disposed = true;
                UpdateText(string.Empty);
            }
        }
    }
}
