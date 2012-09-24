namespace ProjectMercury.Design.Emitters
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using ProjectMercury.Emitters;

    public sealed class SphereEmitterTypeDescriptor : AbstractEmitterTypeDescriptor<SphereEmitter>
    {
        protected override IEnumerable<PropertyDescriptor> GetProperties()
        {
            var properties = new List<PropertyDescriptor>
            {
                PropertyDescriptorFactory.Create(EmitterType.GetProperty("Radius"),
                    new CategoryAttribute("Sphere Emitter"),
                    new DisplayNameAttribute("Radius"),
                    new DescriptionAttribute("Gets or sets the radius of the sphere.")),
                
                PropertyDescriptorFactory.Create(EmitterType.GetProperty("Shell"),
                    new CategoryAttribute("Sphere Emitter"),
                    new DisplayNameAttribute("Shell"),
                    new DescriptionAttribute("Gets or sets a value indicating whether particles should be released only on the edge of the sphere.")),
                     
                PropertyDescriptorFactory.Create(EmitterType.GetProperty("Radiate"),
                    new CategoryAttribute("Sphere Emitter"),
                    new DisplayNameAttribute("Radiate"),
                    new DescriptionAttribute("Gets or sets a value indicating whether particles should radiate out from the centre of the sphere.")),
            };

            return base.GetProperties().Concat(properties);
        }
    }
}