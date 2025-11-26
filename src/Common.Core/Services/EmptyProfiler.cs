using Common.Core.Interfaces;
using System;

namespace Common.Core.Services
{
    /// <summary>
    /// Empty profiler implementation that can be used for default implmementation 
    /// or removing profiling per an environment.
    /// </summary>
    public class EmptyProfiler : IProfiler
    {
        public IDisposable Step(string name)
        {
            return new EmptyProfilerStep();
        }

        public void Dispose() { }

        private class EmptyProfilerStep : IDisposable
        {
            public void Dispose() { }
        }
    }
}
