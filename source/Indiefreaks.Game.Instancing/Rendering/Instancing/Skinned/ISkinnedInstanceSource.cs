using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace Indiefreaks.Xna.Rendering.Instancing.Skinned
{
    public interface ISkinnedInstanceSource
    {
        Model Model { get; }
        InstancedSkinningData InstancedSkinningData { get; }
    }
}
