using System;

namespace Common.Core.Domain
{
    public interface IEntityConcurrency
    {
        byte[] RowVersion { get; }
    }
}
