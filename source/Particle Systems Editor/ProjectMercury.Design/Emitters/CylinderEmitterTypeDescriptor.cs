namespace ProjectMercury.Design.Emitters
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using ProjectMercury.Emitters;

    public sealed class CylinderEmitterTypeDescriptor : PlaneEmitterTypeDescriptor<CylinderEmitter>
    {
        protected override IEnumerable<PropertyDescriptor> GetProperties()
        {
            var properties = new List<PropertyDescriptor>
            {
                PropertyDescriptorFactory.Create(EmitterType.GetProperty("Radius"),
                    new CategoryAttribute("Cylinder Emitter"),
                    new DisplayNameAttribute("Radius"),
                    new DescriptionAttribute("Gets or sets the radius of the cylinder.")),

                PropertyDescriptorFactory.Create(EmitterType.GetProperty("Height"),
                    new CategoryAttribute("Cylinder Emitter"),
                    new DisplayNameAttribute("Height"),
                    new DescriptionAttribute("Gets or sets the height of the cylinder.")),
                
                PropertyDescriptorFactory.Create(EmitterType.GetProperty("Shell"),
                    new CategoryAttribute("Cylinder Emitter"),
                    new DisplayNameAttribute("Shell"),
                    new DescriptionAttribute("Gets or sets a value indicating whether particles should be released only on the edge of the cylinder.")),
                     
                PropertyDescriptorFactory.Create(EmitterType.GetProperty("Radiate"),
                    new CategoryAttribute("Cylinder Emitter"),
                    new DisplayNameAttribute("Radiate"),
                    new DescriptionAttribute("Gets or sets a value indicating whether particles should radiate out from the centre column of the cylinder.")),
            };

            return base.GetProperties().Concat(properties);
        }
    }
}