using System;

namespace ACMESharp.Testing.Xunit
{
    /// <param name="methodName">the name of the test method (Fact)
    ///     that is a dependency.</param>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TestDependencyAttribute(string methodName) : Attribute
    {

        /// <summary>
        /// The name of the test method (Fact) that is a dependency.
        /// </summary>
        public string MethodName { get; } = methodName;
    }
}