namespace ProjectMercury.Design.Emitters
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel;
    using ProjectMercury.Emitters;

    public abstract class AbstractEmitterTypeDescriptor<T> : AbstractTypeDescriptor<T> where T : AbstractEmitter
    {
        protected readonly Type EmitterType = typeof(T);

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return new PropertyDescriptorCollection(this.GetProperties().ToArray());
        }

        protected virtual IEnumerable<PropertyDescriptor> GetProperties()
        {
            return new List<PropertyDescriptor>
            {
                PropertyDescriptorFactory.Create(EmitterType.GetProperty("Name"),
                    new CategoryAttribute("General"),
                    new DisplayNameAttribute("Name"),
                    new DescriptionAttribute("Gets or sets the name of the emitter.")),

                PropertyDescriptorFactory.Create(EmitterType.GetProperty("Enabled"),
                new CategoryAttribute("General"),
                    new DisplayNameAttribute("Enabled"),
                    new DescriptionAttribute("Gets or sets a value indicating whether or not the emitter is enabled.")),

                PropertyDescriptorFactory.Create(EmitterType.GetProperty("ReleaseQuantity"),
                    new CategoryAttribute("Particles"),
                    new DisplayNameAttribute("Release Quantity"),
                    new DescriptionAttribute("Gets or sets the number of particles which will be released each time the emitter is triggered.")),

                PropertyDescriptorFactory.Create(EmitterType.GetProperty("BillboardStyle"),
                    new CategoryAttribute("Rendering"),
                    new DisplayNameAttribute("Billboard Style"),
                    new DescriptionAttribute("Gets or sets the style of the billboard rendering.")),

                PropertyDescriptorFactory.Create(EmitterType.GetProperty("BillboardRotationalAxis"),
                    new CategoryAttribute("Rendering"),
                    new DisplayNameAttribute("Billboard Rotational Axis"),
                    new DescriptionAttribute("Gets or sets the rotational axis for cylindrical billboarding.")),

                PropertyDescriptorFactory.Create(EmitterType.GetProperty("ReleaseSpeed"),
                    new CategoryAttribute("Particles"),
                    new DisplayNameAttribute("Release Speed"),
                    new DescriptionAttribute("Gets or sets the release speed of particles.")),

                PropertyDescriptorFactory.Create(EmitterType.GetProperty("ReleaseOpacity"),
                    new CategoryAttribute("Particles"),
                    new DisplayNameAttribute("Release Opacity"),
                    new DescriptionAttribute("Gets or sets the release opacity of particles.")),

                PropertyDescriptorFactory.Create(EmitterType.GetProperty("ReleaseScale"),
                    new CategoryAttribute("Particles"),
                    new DisplayNameAttribute("Release Scale"),
                    new DescriptionAttribute("Gets or sets the release scale of particles.")),

                PropertyDescriptorFactory.Create(EmitterType.GetProperty("ReleaseColour"),
                    new CategoryAttribute("Particles"),
                    new DisplayNameAttribute("Release Colour"),
                    new DescriptionAttribute("Gets or sets the release colour of particles.")),

                PropertyDescriptorFactory.Create(EmitterType.GetProperty("ReleaseRotation"),
                    new CategoryAttribute("Particles"),
                    new DisplayNameAttribute("Release Rotation"),
                    new DescriptionAttribute("Gets or sets the release rotation of particles.")),

                PropertyDescriptorFactory.Create(EmitterType.GetProperty("BlendMode"),
                    new CategoryAttribute("Rendering"),
                    new DisplayNameAttribute("Blend Mode"),
                    new DescriptionAttribute("Gets or sets the blending mode of the particle emitter.")),

                PropertyDescriptorFactory.Create(EmitterType.GetProperty("ParticleTextureAssetPath"),
                    new CategoryAttribute("Rendering"),
                    new DisplayNameAttribute("Particle Texture Asset Path"),
                    new DescriptionAttribute("The path to the texture asset to use when rendering particles.")),
            };
        }
    }
}