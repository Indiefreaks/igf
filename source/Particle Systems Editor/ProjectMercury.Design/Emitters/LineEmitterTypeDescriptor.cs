namespace ProjectMercury.Design.Emitters
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using ProjectMercury.Emitters;

    public sealed class LineEmitterTypeDescriptor : PlaneEmitterTypeDescriptor<LineEmitter>
    {
        protected override IEnumerable<PropertyDescriptor> GetProperties()
        {
            var properties = new List<PropertyDescriptor>
            {
                PropertyDescriptorFactory.Create(EmitterType.GetProperty("Length"),
                    new CategoryAttribute("Line Emitter"),
                    new DisplayNameAttribute("Length"),
                    new DescriptionAttribute("Gets or sets the length of the line.")),
                
                PropertyDescriptorFactory.Create(EmitterType.GetField("Rectilinear"),
                    new CategoryAttribute("Line Emitter"),
                    new DisplayNameAttribute("Rectilinear"),
                    new DescriptionAttribute("If true, will emit particles perpendicular to the angle of the line.")),
                     
                PropertyDescriptorFactory.Create(EmitterType.GetField("EmitBothWays"),
                    new CategoryAttribute("Line Emitter"),
                    new DisplayNameAttribute("EmitBothWays"),
                    new DescriptionAttribute("If true, will emit particles both ways. Only work when Rectilinear is enabled.")),
            };

            return base.GetProperties().Concat(properties);
        }
    }
}