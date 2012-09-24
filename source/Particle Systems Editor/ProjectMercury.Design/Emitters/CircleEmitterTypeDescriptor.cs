namespace ProjectMercury.Design.Emitters
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using ProjectMercury.Emitters;

    public sealed class CircleEmitterTypeDescriptor : PlaneEmitterTypeDescriptor<CircleEmitter>
    {
        protected override IEnumerable<PropertyDescriptor> GetProperties()
        {
            var properties = new List<PropertyDescriptor>
            {
                PropertyDescriptorFactory.Create(EmitterType.GetProperty("Radius"),
                    new CategoryAttribute("Circle Emitter"),
                    new DisplayNameAttribute("Radius"),
                    new DescriptionAttribute("Gets or sets the radius of the circle.")),
                
                PropertyDescriptorFactory.Create(EmitterType.GetProperty("Shell"),
                    new CategoryAttribute("Circle Emitter"),
                    new DisplayNameAttribute("Shell"),
                    new DescriptionAttribute("Gets or sets a value indicating whether particles should be released only on the edge of the circle.")),
                     
                PropertyDescriptorFactory.Create(EmitterType.GetProperty("Radiate"),
                    new CategoryAttribute("Circle Emitter"),
                    new DisplayNameAttribute("Radiate"),
                    new DescriptionAttribute("Gets or sets a value indicating whether particles should radiate out from the centre of the circle.")),
            };

            return base.GetProperties().Concat(properties);
        }
    }
}