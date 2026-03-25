#if NET35 || NET40
    namespace System.Runtime.CompilerServices
    {
        [AttributeUsage(AttributeTargets.Parameter)]
        internal sealed class CallerMemberNameAttribute : Attribute { }

        [AttributeUsage(AttributeTargets.Parameter)]
        internal sealed class CallerFilePathAttribute : Attribute { }

        [AttributeUsage(AttributeTargets.Parameter)]
        internal sealed class CallerLineNumberAttribute : Attribute { }
    }
#endif
