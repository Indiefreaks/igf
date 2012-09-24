namespace ProjectMercury.Design.Emitters
{
    using System;
    using System.ComponentModel;
    using ProjectMercury.Emitters;

    public sealed class PointEmitterTypeDescriptor : AbstractEmitterTypeDescriptor<PointEmitter>
    {
        private readonly Type EmitterType = typeof(PointEmitter);
    }
}