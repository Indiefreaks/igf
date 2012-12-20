using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

using SynapseGaming.LightingSystem.Rendering;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Rendering.Instancing.Skinned
{
    public struct InstancedVertexPositionNormalTextureBumpSkin : IVertexType
    {

        public Vector3 Position;
        public Byte4 BoneIndices;
        public Vector4 BoneWeights;
        public Vector3 Normal;
        public Vector2 TextureCoordinate1;
        public Vector2 TextureCoordinate2;
        public Vector3 Tangent;
        public Vector3 Binormal;

        public InstancedVertexPositionNormalTextureBumpSkin(VertexPositionNormalTextureBumpSkin source, Matrix meshToObject, byte index)
        {
            Position = source.Position;
            Normal = source.Normal;
            TextureCoordinate1 = source.TextureCoordinate;
            TextureCoordinate2 = source.TextureCoordinate;
            Tangent = source.Tangent;
            Binormal = source.Binormal;

            BoneIndices = new Byte4(source.BoneIndex0, source.BoneIndex1, source.BoneIndex2, source.BoneIndex3);
            BoneWeights = source.BoneWeights;     
        }

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Byte4, VertexElementUsage.BlendIndices, 0),
            new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0),
            new VertexElement(32, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(44, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(52, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1),   
            new VertexElement(60, VertexElementFormat.Vector3, VertexElementUsage.Tangent, 0),
            new VertexElement(72, VertexElementFormat.Vector3, VertexElementUsage.Binormal, 0)
        );

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }

        public static int SizeInBytes
        {
            get
            {
                return 84;
            }
        }
    }
}
