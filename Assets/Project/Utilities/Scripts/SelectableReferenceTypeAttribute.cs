using System;
using UnityEngine;

namespace Utilities
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SelectableReferenceTypeAttribute : PropertyAttribute
    {
        private Type type;
        public Type Type => type;
        public SelectableReferenceTypeAttribute(Type type) { this.type = type; }
    }
}