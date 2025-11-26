using System;

namespace Common.Core
{
    public interface IProgressIndicator : IProgress<double>, IDisposable
    {
    }
}
