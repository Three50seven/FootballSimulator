using System;

namespace Common.Core.Interfaces
{
    /// <summary>
    /// Profile blocks of code by registering steps. Designed with MiniProfiler in mind.
    /// </summary>
    public interface IProfiler : IDisposable
    {
        /// <summary>
        /// Create disposable step. Intended around a using statement to profile the performance of a block of code.
        /// </summary>
        /// <param name="name">Name for the step.</param>
        /// <returns></returns>
        IDisposable Step(string name);
    }
}
