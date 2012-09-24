namespace ProjectMercury.Design.Emitters
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using ProjectMercury.Emitters;

    public sealed class BoxEmitterTypeDescriptor : AbstractEmitterTypeDescriptor<BoxEmitter>
    {
        protected override IEnumerable<PropertyDescriptor> GetProperties()
        {
            var properties = new List<PropertyDescriptor>
            {
                PropertyDescriptorFactory.Create(EmitterType.GetProperty("Width"),
                    new CategoryAttribute("Box Emitter"),
                    new DisplayNameAttribute("Width"),
                    new DescriptionAttribute("Gets or sets the width of the box.")),
                
                PropertyDescriptorFactory.Create(EmitterType.GetProperty("Height"),
                    new CategoryAttribute("Box Emitter"),
                    new DisplayNameAttribute("Height"),
                    new DescriptionAttribute("Gets or sets the height of the box.")),
                     
                PropertyDescriptorFactory.Create(EmitterType.GetProperty("Depth"),
                    new CategoryAttribute("Box Emitter"),
                    new DisplayNameAttribute("Depth"),
                    new DescriptionAttribute("Gets or sets the depth of the box.")),
                          
                PropertyDescriptorFactory.Create(EmitterType.GetProperty("Rotation"),
                    new CategoryAttribute("Box Emitter"),
                    new DisplayNameAttribute("Rotation"),
                    new DescriptionAttribute("Gets or sets the rotation vector of the box.")),
            };

            return base.GetProperties().Concat(properties);
        }
    }
}