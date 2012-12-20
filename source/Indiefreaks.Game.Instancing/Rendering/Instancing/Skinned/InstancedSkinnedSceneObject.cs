using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Rendering;
using SynapseGaming.LightingSystem.Effects.Deferred;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;

namespace Indiefreaks.Xna.Rendering.Instancing.Skinned
{
    public sealed class InstancedSkinnedSceneObject : SceneObject
    {
        private GraphicsDevice _graphicsDevice;
        private Matrix[] _instanceTransforms;
        private int[] _instanceAnimations;
        private DeferredSasEffect _sourceEffect;
        private Texture2D _animationTexture;
        private ISkinnedInstanceSource _model;
        private BoundingSphere _boundingSphere;
        private BoundingBox _boundingBox;

        private int _instancesCount;
        private int _maxInstances;

        public InstancedSkinnedSceneObject(GraphicsDevice graphicsDevice, ISkinnedInstanceSource source, DeferredSasEffect sourceEffect)
            : base()
        {
            _maxInstances = 15;
            _instancesCount = 0;

            _graphicsDevice = graphicsDevice;
            _sourceEffect = sourceEffect;
            _boundingSphere = new BoundingSphere(Vector3.Zero, float.MaxValue);
            _boundingBox = BoundingBox.CreateFromSphere(_boundingSphere);
            _model = source;
            _animationTexture = _model.InstancedSkinningData.AnimationTexture;         
            base.Visibility = ObjectVisibility.RenderedAndCastShadows;
            base.UpdateType = UpdateType.Automatic;
            BuildRenderableMeshes(_model.Model);
            InitializeInstanceData();            
            InitializeEffect();
        }

        public int InstancesCount
        {
            get { return _instancesCount; }
        }

        public int MaxInstances
        {
            get { return _maxInstances; }
        }

        public SkinnedInstanceEntity CreateInstance(string name, Matrix transform)
        {
            SkinnedInstanceEntity result = new SkinnedInstanceEntity(name, _instancesCount, this, transform);
            _instancesCount++;
            return result;
        }

        private void BuildRenderableMeshes(Model model)
        {
            List<short> indices = new List<short>();
            List<InstancedVertexPositionNormalTextureBumpSkin> vertices = new List<InstancedVertexPositionNormalTextureBumpSkin>();
            Dictionary<int, int> indexRemap = new Dictionary<int, int>();

            // Get the model transforms for baking down vertices into object space (XNA Models are stored in mesh space).
            Matrix[] boneArray = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(boneArray);


            foreach (ModelMesh modelMesh in model.Meshes)
            {
                Debug.WriteLine("Building " + modelMesh.Name);
                Matrix meshToObject = boneArray[modelMesh.ParentBone.Index];

                for(int p=0;p<modelMesh.MeshParts.Count;p++)
                {
                    ModelMeshPart modelMeshPart = modelMesh.MeshParts[p];

                    int vertexCount = modelMeshPart.VertexBuffer.VertexCount;
                    int indexCount = modelMeshPart.IndexBuffer.IndexCount;

                    indexRemap.Clear();

                    vertices.Clear();
                    indices.Clear();

                    // Create a material for this meshpart
                    // TODO: Group data by effects 
                    DeferredSasEffect partEffect = (DeferredSasEffect)_sourceEffect.Clone();
                    partEffect.Parameters["AnimationTexture"].SetValue(_animationTexture);
                    partEffect.Parameters["BoneDelta"].SetValue(1f / _animationTexture.Width);
                    partEffect.Parameters["RowDelta"].SetValue(1f / _animationTexture.Height);
                    DeferredObjectEffect effect = (DeferredObjectEffect) modelMeshPart.Effect;
                    partEffect.Parameters["DiffuseTexture"].SetValue(effect.DiffuseMapTexture);                    

                    short[] sourceIndices = new short[modelMeshPart.PrimitiveCount*3];
                    modelMeshPart.IndexBuffer.GetData<short>(modelMeshPart.StartIndex*2,sourceIndices,0,modelMeshPart.PrimitiveCount*3);

                    InstancedVertexPositionNormalTextureBumpSkin[] sourceVertices = new InstancedVertexPositionNormalTextureBumpSkin[modelMeshPart.NumVertices];
                    modelMeshPart.VertexBuffer.GetData<InstancedVertexPositionNormalTextureBumpSkin>(modelMeshPart.VertexOffset * modelMeshPart.VertexBuffer.VertexDeclaration.VertexStride, sourceVertices, 0, modelMeshPart.NumVertices, modelMeshPart.VertexBuffer.VertexDeclaration.VertexStride);

                    for (int instance = 0; instance < _maxInstances; instance++)
                    {
                        for (int i = 0; i < sourceIndices.Length; i++)
                        {
                            indices.Add((short) ((sourceIndices[i]) + (instance * sourceVertices.Length))) ;
                        }

                        for (int i = 0; i < sourceVertices.Length; i++)
                        {
                            sourceVertices[i].TextureCoordinate2 = new Vector2(instance, 0);
                            vertices.Add(sourceVertices[i]);
                        }
                    }

                    VertexBuffer vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(InstancedVertexPositionNormalTextureBumpSkin), vertices.Count, BufferUsage.None);
                    vertexBuffer.SetData<InstancedVertexPositionNormalTextureBumpSkin>(vertices.ToArray());
                    IndexBuffer indexBuffer = new IndexBuffer(_graphicsDevice, IndexElementSize.SixteenBits, indices.Count, BufferUsage.None);
                    indexBuffer.SetData<short>(indices.ToArray());

                    RenderableMesh renderableMesh = new RenderableMesh();
                    renderableMesh.Build(this, partEffect, Matrix.Identity, _boundingSphere, _boundingBox,
                        indexBuffer, vertexBuffer, 0, PrimitiveType.TriangleList, indexBuffer.IndexCount / 3, 0, vertexBuffer.VertexCount, 0, true);
                    base.Add(renderableMesh);

                }
            }
        }

        private void InitializeInstanceData()
        {
            _instanceTransforms = new Matrix[_maxInstances];
            _instanceAnimations = new int[_maxInstances];
            for (int i = 0; i < _maxInstances; i++)
            {                
                _instanceTransforms[i] = Matrix.CreateScale(0);
                _instanceAnimations[i] = 0;
            }
        }

        private void InitializeEffect() {

            SetAnimationData();
        }

        public Matrix[] InstanceTransforms
        {
            get { return _instanceTransforms; }
        }

        public int[] InstanceAnimationFrames
        {
            get { return _instanceAnimations; }
        }

       
        private void SetAnimationData() {
            foreach (RenderableMesh mesh in base.RenderableMeshes)
            {
                mesh.Effect.Parameters["InstanceTransforms"].SetValue(_instanceTransforms);
                mesh.Effect.Parameters["InstanceAnimations"].SetValue(_instanceAnimations);
                //mesh.Effect.CurrentTechnique.Passes[0].Apply();
            }
        }

        public IDictionary<string, InstancedAnimationClip> AnimationClips
        {
            get { return _model.InstancedSkinningData.Animations; }
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
            SetAnimationData();
        }
    }
}
