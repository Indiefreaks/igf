/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Renderers
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Graphics.PackedVector;
    using ProjectMercury.Proxies;

    /// <summary>
    /// Defines a renderer which renders particles using billboarded quads.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class QuadRenderer : AbstractRenderer
    {
        private const Int32 MaxVertices         = 65536;
        private const Int32 VerticesPerParticle = 4;
        private const Int32 IndicesPerParticle  = 6;

        private readonly Int32 RenderBufferSize;
        private readonly Int32 MaxParticles;
        private readonly Int32 NumIndices;
        private readonly Int32 NumVertices;

        //Cached so we can pass into ref parameters..
        private static Vector3 Up      = Vector3.Up;
        private static Vector3 Forward = Vector3.Forward;

        /// <summary>
        /// Gets or sets the index buffer.
        /// </summary>
        private readonly Int16[] Indices;

        /// <summary>
        /// Gets or sets the vertex buffer.
        /// </summary>
        private readonly ParticleVertex[] Vertices;

        private DynamicVertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;

        /// <summary>
        /// Gets or sets the basic effect shader.
        /// </summary>
        private BasicEffect BasicEffect;

        //Create the untransformed corner vertices just once rather than per particle
        Vector3 inv1 = new Vector3(-.5f, -.5f, 0f);
        Vector3 inv2 = new Vector3(.5f, -.5f, 0f);
        Vector3 inv3 = new Vector3(.5f, .5f, 0f);
        Vector3 inv4 = new Vector3(-.5f, .5f, 0f);

        //Cache the texture coordinates
#if WINDOWS_PHONE
        private Vector2 uv1 = Vector2.Zero;
        private Vector2 uv2 = Vector2.UnitX;
        private Vector2 uv3 = Vector2.One;
        private Vector2 uv4 = Vector2.UnitY;
#else
        private HalfVector2 uv1 = new HalfVector2(Vector2.Zero);
        private HalfVector2 uv2 = new HalfVector2(Vector2.UnitX);
        private HalfVector2 uv3 = new HalfVector2(Vector2.One);
        private HalfVector2 uv4 = new HalfVector2(Vector2.UnitY);
#endif
        //private readonly RasterizerState wireframe = new RasterizerState { FillMode = FillMode.WireFrame, CullMode = CullMode.None };
        private int _vertexBufferPosition;

        public QuadRenderer()
            : this(MaxVertices)
        {
        }

        ///<summary>
        /// Creates a new QuadRenderer
        ///</summary>
        ///<param name="bufferSize"></param>
        public QuadRenderer(int bufferSize)
        {
            this.RenderBufferSize = bufferSize;
            this.MaxParticles = RenderBufferSize / VerticesPerParticle;
            this.NumIndices = MaxParticles * IndicesPerParticle;
            this.NumVertices = MaxParticles * VerticesPerParticle;

            this.Indices = new Int16[NumIndices];
            this.Vertices = new ParticleVertex[NumVertices];
        }

        /// <summary>
        /// Loads any content items needed by the renderer.
        /// </summary>
        /// <param name="content">A content manager instance.</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public override void LoadContent(ContentManager content)
        {
            Check.True(base.GraphicsDeviceService != null, "GraphicsDeviceService property has not been initialised with a valid value!");

            //See http://blogs.msdn.com/b/shawnhar/archive/2010/04/25/basiceffect-optimizations-in-xna-game-studio-4-0.aspx
            //For the optimal configurations of Basic effect
            this.BasicEffect = new BasicEffect(base.GraphicsDeviceService.GraphicsDevice)
                                   {
                                       TextureEnabled = true,
                                       World = Matrix.Identity,
                                       LightingEnabled = false,
                                       PreferPerPixelLighting = false,
                                       Projection = Matrix.Identity,
                                       VertexColorEnabled = true,
                                       FogEnabled = false,
                                   };

            Int16 currentVertex = 0;
            Int32 indexCount = 0;

            //Each  polygon has 4 vertices making up a 2 triangle quad - we can use this for any quad
            //Since IB always refers to quads with an indentical count we can simply make it once and reuse it
            //No need to buld each frame
            for (Int32 i = 0; i < MaxParticles; i++)
            {
                this.Indices[indexCount++] = (Int16)(currentVertex + 0);
                this.Indices[indexCount++] = (Int16)(currentVertex + 1);
                this.Indices[indexCount++] = (Int16)(currentVertex + 2);
                this.Indices[indexCount++] = (Int16)(currentVertex + 0);
                this.Indices[indexCount++] = (Int16)(currentVertex + 2);
                this.Indices[indexCount++] = (Int16)(currentVertex + 3);

                currentVertex += 4;
            }

            //Since we know each particle is a quad containing 4 vertices that span 0-1 across uvspace and we ALWAYS fill the vertices in the same order
	        //we can prepopulate the texture coordinates into the whole array
            for (Int32 i = 0; i < NumVertices; i+=4 )
            {
                Vertices[i].TextureCoordinate = uv1;
                Vertices[i + 1].TextureCoordinate = uv2;
                Vertices[i + 2].TextureCoordinate = uv3;
                Vertices[i + 3].TextureCoordinate = uv4;
            }

            _vertexBuffer = new DynamicVertexBuffer(base.GraphicsDeviceService.GraphicsDevice, typeof(ParticleVertex), NumVertices, BufferUsage.WriteOnly);
            _indexBuffer = new IndexBuffer(base.GraphicsDeviceService.GraphicsDevice, IndexElementSize.SixteenBits, NumIndices, BufferUsage.WriteOnly);
            _indexBuffer.SetData(Indices);
            _vertexBufferPosition = 0;
        }

        /// <summary>
        /// Allows the renderer class to perform one time set up for each particle effect.
        /// </summary>
        /// <remarks>At this stage the <ref name="renderContext"/> will have its world, view and
        /// projection matrices set, as well as the camera position. Other properties such as the particle
        /// pointer or blend state will <b>not</b> be set.</remarks>
        protected override void PreRender(ref Matrix worldMatrix, ref Matrix viewMatrix, ref Matrix projectionMatrix)
        {
            base.GraphicsDeviceService.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            base.GraphicsDeviceService.GraphicsDevice.RasterizerState   = RasterizerState.CullNone;
            //base.GraphicsDeviceService.GraphicsDevice.RasterizerState = wireframe;
            base.GraphicsDeviceService.GraphicsDevice.SamplerStates[0]  = SamplerState.AnisotropicClamp;

            //Note we dont set the world here - its multiplied into the vertex buffer during vertex setup 
            //so that the billboarding works properly
            //TODO: For non billboarded we could do it here and save lots of CPU multiplication
            this.BasicEffect.View = viewMatrix;
            this.BasicEffect.Projection = projectionMatrix;

            base.GraphicsDeviceService.GraphicsDevice.Indices = _indexBuffer;

        }

        /// <summary>
        /// Performs rendering of particles.
        /// </summary>
        /// <param name="iterator">The particle iterator object.</param>
        /// <param name="context">The render context containing rendering information.</param>
        protected override void Render(ref RenderContext context, ref ParticleIterator iterator)
        {
            Int32 vertexCount = 0;
            Vector3 cameraPos = context.CameraPosition;
            Vector3 rotationAxis = context.BillboardRotationalAxis;
            bool squareTexture = (context.Texture.Height == context.Texture.Width);
            float aspectRatio = context.Texture.Height / (float)context.Texture.Width ;

            SetDataOptions setDataOptions = SetDataOptions.NoOverwrite;

            //Use the SpriteBatch style of filling buffers
            //http://blogs.msdn.com/b/shawnhar/archive/2010/07/07/setdataoptions-nooverwrite-versus-discard.aspx
            if (_vertexBufferPosition + context.Count * VerticesPerParticle > NumVertices)
            {
                //Too much to fit in the remaining space - start at the beginning and discard
                _vertexBufferPosition = 0;
                setDataOptions = SetDataOptions.Discard;
            }

#if UNSAFE
            unsafe
            {
                fixed (ParticleVertex* vertexArray = Vertices)
                {
                    ParticleVertex* verts = vertexArray +_vertexBufferPosition;
#else
                    int vertexIndex = _vertexBufferPosition;
#endif
                    var particle = iterator.First;
                    do
                    {
#if UNSAFE
                        Single scale = particle->Scale;
                        Vector3 position = particle->Position;
                        Vector3 rotation = particle->Rotation;
                        Vector4 colour = particle->Colour;
                        //Work out our world transform - and set a flag to avoid some multiplies if world ends up as zero
                        bool worldIsNotIdentity = true;
                        Matrix effectWorld;
                        if (context.WorldIsIdentity)
                        {
                            if (particle->EffectProxyIndex > 0)
                            {
                                effectWorld = ParticleEffectProxy.Proxies[particle->EffectProxyIndex].World;
                            }
                            else
                            {
                                worldIsNotIdentity = false;
                                effectWorld = Matrix.Identity; //Makes the compiler happy though we will never actually use it.
                            }
                        }
                        else
                        {
                            effectWorld = particle->EffectProxyIndex > 0 ? ParticleEffectProxy.Proxies[particle->EffectProxyIndex].FinalWorld : context.World;
                        }

#else
                        Single scale = particle.Scale;
                        Vector3 position = particle.Position;
                        Vector3 rotation = particle.Rotation;
                        Vector4 colour = particle.Colour;
                        //If we have a proxy then multiply in the proxy world matrix
                        bool worldIsNotIdentity = true;
                        Matrix effectWorld;
                        if (context.WorldIsIdentity)
                        {
                            if (particle.EffectProxyIndex > 0)
                            {
                                effectWorld = ParticleEffectProxy.Proxies[particle.EffectProxyIndex].World;
                            }
                            else
                            {
                                worldIsNotIdentity = false;
                                effectWorld = Matrix.Identity;
                                    //Makes the compiler happy though we will never actually use it.
                            }
                        }
                        else
                        {
                            effectWorld = particle.EffectProxyIndex > 0
                                              ? ParticleEffectProxy.Proxies[particle.EffectProxyIndex].FinalWorld
                                              : context.World;
                        }
#endif

                        //Individual particle transformations - scale and rotation
                        //The Rotation setup will fill in the top 3x3 so we just need to initialise 44
                        var transform = new Matrix {M44 = 1};

                        float scaleX = scale;
                        float scaleY = squareTexture ? scale : scale * aspectRatio;
                        //ScaleZ is always 1 so no need multiple into M_3

                        //Inline the rotation and scale calculations and do each element in one go
                        //Fast rotation matrix - see http://en.wikipedia.org/wiki/Rotation_matrix#General_rotations
                        //This set matches
                        //Matrix temp2 = Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z); //Matches math od Rotx*RotY*RotZ
                        //var cosX = (float) Math.Cos(rotation.X);
                        //var cosY = (float) Math.Cos(rotation.Y);
                        //var cosZ = (float) Math.Cos(rotation.Z);
                        //var sinX = (float) Math.Sin(rotation.X);
                        //var sinY = (float) Math.Sin(rotation.Y);
                        //var sinZ = (float) Math.Sin(rotation.Z);
                        //transform.M11 = cosY*cosZ;
                        //transform.M12 = cosY * sinZ;
                        //transform.M13 = -sinY;
                        //transform.M21 = sinX*sinY*cosZ - cosX * sinZ;
                        //transform.M22 = sinX*sinY*sinZ + cosX*cosZ;
                        //transform.M23 = sinX*cosY;
                        //transform.M31 = cosX*sinY*cosZ + sinX * sinZ;
                        //transform.M32 = cosX*sinY*sinZ - sinX * cosZ;
                        //transform.M33 = cosX * cosY;

                        //This set matches
                        //Matrix temp2 = Matrix.CreateScale(new VEctor3(scaleX, scaleY,1) * Matrix.CreateRotationZ(rotation.Z) * Matrix.CreateRotationX(rotation.Y) * Matrix.CreateRotationY(rotation.X) ; //Matches YawPitchRoll order
                        //Matrix temp = Matrix.CreateScale(new VEctor3(scaleX, scaleY,1) * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);
                        //TODO - can we optimise out a rotation e.g.fast path if rotation.Y=0 etc
                        //TODO - can we optimise out rotation(s) if we know its a billboard? That overwrites much of the transform
                        var cosX = (float)Math.Cos(rotation.Y);
                        var cosY = (float)Math.Cos(rotation.X);
                        var cosZ = (float)Math.Cos(rotation.Z);
                        var sinX = (float)Math.Sin(rotation.Y);
                        var sinY = (float)Math.Sin(rotation.X);
                        var sinZ = (float)Math.Sin(rotation.Z);
                        var cosYcosZ = cosY*cosZ;
                        var cosYsinZ = cosY*sinZ;
                        var sinXsinY = sinX*sinY;
                        transform.M11 = (cosYcosZ + sinXsinY * sinZ) * scaleX;
                        transform.M12 = (cosX * sinZ) * scaleX;
                        transform.M13 = (sinX * cosYsinZ - sinY * cosZ) * scaleX;
                        transform.M21 = (sinXsinY * cosZ - cosYsinZ) * scaleY;
                        transform.M22 = (cosX * cosZ) *scaleY;
                        transform.M23 = (sinY * sinZ + sinX * cosYcosZ) * scaleY;
                        transform.M31 = cosX * sinY;
                        transform.M32 = -sinX;
                        transform.M33 = cosX * cosY;

                        switch (context.BillboardStyle)
                        {
                            case BillboardStyle.None:
                                //Position the particle without a multiplication!
                                transform.M41 = position.X;
                                transform.M42 = position.Y;
                                transform.M43 = position.Z;
                                //Just apply the world
                                //TODO - we can just do this in Basic effect instead of per vertex - only if there is no proxy, sort of a fast path!
                                if (worldIsNotIdentity)
                                {
                                    Matrix.Multiply(ref transform, ref effectWorld, out transform);
                                }
                                break;

                            default: //Its billboarded

                                Vector3 worldPos;
                                if (worldIsNotIdentity)
                                {
                                    Vector3.Transform(ref position, ref effectWorld, out worldPos);
                                }
                                else
                                {
                                    worldPos = position;
                                }


                                //Apply the billboard (which includes the world translation)
                                Matrix billboardMatrix;
                                if (context.BillboardStyle == BillboardStyle.Spherical)
                                {
                                    //Spherical billboards (always face the camera)
                                    Matrix.CreateBillboard(ref worldPos, ref cameraPos, ref Up, Forward,
                                                           out billboardMatrix);
                                }
                                else
                                {
                                    //HACK: For progenitor DBP use the velocity as the axis for a per particle axis
                                    if (context.UseVelocityAsBillboardAxis)
                                    {
#if UNSAFE
                                        Matrix.CreateConstrainedBillboard(ref worldPos, ref cameraPos, ref particle->Velocity,
                                                                          Forward, null, out billboardMatrix);
#else
                                        Matrix.CreateConstrainedBillboard(ref worldPos, ref cameraPos, ref particle.Velocity,
                                                                          Forward, null, out billboardMatrix);

#endif
                                    }
                                    else
                                    {
                                        //Cylindrical billboards have a vector they are allowed to rotate around
                                        Matrix.CreateConstrainedBillboard(ref worldPos, ref cameraPos, ref rotationAxis,
                                                                          Forward, null, out billboardMatrix);
                                    }


                                }

                                Matrix.Multiply(ref transform, ref billboardMatrix, out transform);
                                break;
                        }

                        Vector3 v1;
                        Vector3 v2;
                        Vector3 v3;
                        Vector3 v4;

                        Vector3.Transform(ref inv1, ref transform, out v1);
                        Vector3.Transform(ref inv2, ref transform, out v2);
                        Vector3.Transform(ref inv3, ref transform, out v3);
                        Vector3.Transform(ref inv4, ref transform, out v4);
#if UNSAFE

                        //inline particle value assignments - removes 4 calls with their parameters and its a struct anyway
                        verts->Position.X = v1.X;
                        verts->Position.Y = v1.Y;
                        verts->Position.Z = v1.Z;
                        verts->Colour.X = colour.X;
                        verts->Colour.Y = colour.Y;
                        verts->Colour.Z = colour.Z;
                        verts->Colour.W = colour.W;
                        verts++;

                        verts->Position.X = v2.X;
                        verts->Position.Y = v2.Y;
                        verts->Position.Z = v2.Z;
                        verts->Colour.X = colour.X;
                        verts->Colour.Y = colour.Y;
                        verts->Colour.Z = colour.Z;
                        verts->Colour.W = colour.W;
                        verts++;

                        verts->Position.X = v3.X;
                        verts->Position.Y = v3.Y;
                        verts->Position.Z = v3.Z;
                        verts->Colour.X = colour.X;
                        verts->Colour.Y = colour.Y;
                        verts->Colour.Z = colour.Z;
                        verts->Colour.W = colour.W;
                        verts++;

                        verts->Position.X = v4.X;
                        verts->Position.Y = v4.Y;
                        verts->Position.Z = v4.Z;
                        verts->Colour.X = colour.X;
                        verts->Colour.Y = colour.Y;
                        verts->Colour.Z = colour.Z;
                        verts->Colour.W = colour.W;
                        verts++;
#else
                        //inline particle value assignments - removes 4 calls with their parameters and its a struct anyway

                        this.Vertices[vertexIndex].Position = v1;
                        this.Vertices[vertexIndex].Colour = colour;
                        this.Vertices[vertexIndex++].TextureCoordinate = uv1;
                        this.Vertices[vertexIndex].Position = v2;
                        this.Vertices[vertexIndex].Colour = colour;
                        this.Vertices[vertexIndex++].TextureCoordinate = uv2;
                        this.Vertices[vertexIndex].Position = v3;
                        this.Vertices[vertexIndex].Colour = colour;
                        this.Vertices[vertexIndex++].TextureCoordinate = uv3;
                        this.Vertices[vertexIndex].Position = v4;
                        this.Vertices[vertexIndex].Colour = colour;
                        this.Vertices[vertexIndex++].TextureCoordinate = uv4;
#endif
                        vertexCount += 4;
                    }
#if UNSAFE
                    while (iterator.MoveNext(&particle));
#else
                    while (iterator.MoveNext(ref particle));
#endif

                    base.GraphicsDeviceService.GraphicsDevice.BlendState = context.BlendState;
                    this.BasicEffect.Texture = context.Texture;

                    //Xbox need the vertex buffer to be set to null before SetData is called
                    //Windows does not
                    //TODO: Is this a bug? see http://forums.create.msdn.com/forums/p/61885/399495.aspx#399495
#if XBOX
                    if (setDataOptions == SetDataOptions.Discard)
                    {
                        base.GraphicsDeviceService.GraphicsDevice.SetVertexBuffer(null);
                    }
#endif
                    _vertexBuffer.SetData(_vertexBufferPosition * ParticleVertex.Size, Vertices, _vertexBufferPosition, vertexCount, ParticleVertex.Size, setDataOptions);
                    Debug.WriteLine(String.Format("position: {0} Count: {1} Hint: {2}", _vertexBufferPosition, vertexCount, setDataOptions));
                    base.GraphicsDeviceService.GraphicsDevice.SetVertexBuffer(_vertexBuffer);

                    foreach (EffectPass pass in this.BasicEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        base.GraphicsDeviceService.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, _vertexBufferPosition, vertexCount,
                                                                                        _vertexBufferPosition/4*6, vertexCount/2);
                    }

                    //Move to the next free part of the array
                    _vertexBufferPosition += vertexCount;
#if UNSAFE
                }
            }
#endif
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.BasicEffect != null)
                {
                    this.BasicEffect.Dispose();

                    this.BasicEffect = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Defines the structure for a vertex used by the BillboardRenderer.
        /// </summary>
        [Serializable]
        [ImmutableObject(true)]
        [StructLayout(LayoutKind.Sequential)]
        struct ParticleVertex : IVertexType
        {

            /// <summary>
            /// Access the vertex declaration for this vertex structure.
            /// </summary>
            private static readonly VertexDeclaration VertexDeclaration;

            /// <summary>
            /// Initialises the ParticleVertext structure.
            /// </summary>
            [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
            [SuppressMessage("Microsoft.Usage", "CA2207:InitializeValueTypeStaticFieldsInline")]
            static ParticleVertex()
            {
                var elements = new[]
                {
                    new VertexElement( 0, VertexElementFormat.Vector3, VertexElementUsage.Position,          0),
                    new VertexElement(12, VertexElementFormat.Vector4, VertexElementUsage.Color,             0),
#if WINDOWS_PHONE
                    //Windows phone does not support HalfVector2
                    new VertexElement(28, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
#else
                    new VertexElement(28, VertexElementFormat.HalfVector2, VertexElementUsage.TextureCoordinate, 0)
#endif
                    //Note: Make sure to change the size below if you change this declaration
                };

                VertexDeclaration = new VertexDeclaration(elements)
                {
                    Name = "BillboardRenderer.ParticleVertex.VertexDeclaration"
                };
            }

#if WINDOWS_PHONE
            internal const int Size = 36; //NOTE: Make sure to change this if you change the vertex declaration
#else
            internal const int Size = 32; //NOTE: Make sure to change this if you change the vertex declaration
#endif

            /// <summary>
            /// Gets the position of the vertex in 3D space.
            /// </summary>
            internal Vector3 Position;

            /// <summary>
            /// Gets the colour of the vertex.
            /// </summary>
            internal Vector4 Colour;


#if WINDOWS_PHONE
            //Phone does not support HalfVector2
            /// <summary>
            /// Gets the texture coordinate of the vertex.
            /// </summary>
            internal Vector2 TextureCoordinate;
#else
            /// <summary>
            /// Gets the texture coordinate of the vertex.
            /// </summary>
            internal HalfVector2 TextureCoordinate;
#endif

            /// <summary>
            /// Gets a reference to the vertex declaration.
            /// </summary>
            VertexDeclaration IVertexType.VertexDeclaration
            {
                get { return ParticleVertex.VertexDeclaration; }
            }
        }
    }
}