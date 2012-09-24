namespace ProjectMercury.EffectEditor
{
    using System;
    using System.Diagnostics;

    public sealed class TraceIndenter : IDisposable
    {
        public TraceIndenter()
        {
            Trace.Indent();
        }

        public void Dispose()
        {
            Trace.Unindent();
        }
    }
}