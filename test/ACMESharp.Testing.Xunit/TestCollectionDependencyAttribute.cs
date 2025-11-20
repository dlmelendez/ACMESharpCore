using System;

namespace ACMESharp.Testing.Xunit
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TestCollectionDependencyAttribute(Type @class) : Attribute
    {

        /// <summary>
        /// The test class (Collection) that is a dependency.
        /// </summary>
        public Type Class { get; } = @class;
    }
}
