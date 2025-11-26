using Common.Core.DTOs;
using System;
using System.Collections.Generic;

namespace Common.Core.Annotations
{
    /// <summary>
    /// Attribute for model classes designed to provide custom importing logic when importing model state.
    /// Designed for custom targeting of invalid model state to make custom adjustments to the outgoing model.
    /// Used in POST-REDIRECT-GET workflows.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public abstract class ImportModelStateAttribute : Attribute
    {
        public abstract void OnModelStateImport(object model, IEnumerable<ModelStatePropertyResult> modelStateValues);
    }
}
