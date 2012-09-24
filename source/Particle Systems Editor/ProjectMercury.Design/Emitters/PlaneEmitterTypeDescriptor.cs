namespace ProjectMercury.Design.Emitters
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using ProjectMercury.Emitters;

    public abstract class PlaneEmitterTypeDescriptor<T> : AbstractEmitterTypeDescriptor<T> where T : PlaneEmitter
    {
        protected override IEnumerable<PropertyDescriptor> GetProperties()
        {
            var properties = new List<PropertyDescriptor>
            {
                PropertyDescriptorFactory.Create(EmitterType.GetProperty("ConstrainToPlane"),
                    new CategoryAttribute("Plane Emitter"),
                    new DisplayNameAttribute("Constrain To Plane"),
                    new DescriptionAttribute("Should random forces keep the particle in the XY plane or allow it to move in all 3. Can be used in 2d or 3d. In 2d this will keep particles in the same plane as the emitter. In 3d it keeps particles radiating in a planar fashion.")),
            };

            return base.GetProperties().Concat(properties);
        }
    }
}