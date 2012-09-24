using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Rendering.Instancing
{
    /// <summary>
    /// Extract mesh data from Xna Model to be used as instance reference by an InstanceFactory instance
    /// </summary>
    public class InstanceModel : IInstanceSource
    {
        /// <summary>
        /// Creates a new InstanceModel used by InstanceFactory to create new instances
        /// </summary>
        /// <param name="model">The Xna Model to be used</param>
        public InstanceModel(Model model)
        {
            // Temporary buffers for building the data.
            List<VertexPositionNormalTextureBump> vertices = new List<VertexPositionNormalTextureBump>(256);
            List<short> indices = new List<short>(256);
            Dictionary<int, int> indexremap = new Dictionary<int, int>(256);

            // Temporary buffers for extracting model data.
            VertexPositionNormalTextureBump[] sourcevertices = new VertexPositionNormalTextureBump[1];
            short[] sourceindices = new short[1];

            // Get the model transforms for baking down vertices into object space (XNA Models are stored in mesh space).
            Matrix[] bonearray = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(bonearray);

            for (int m = 0; m < model.Meshes.Count; m++)
            {
                ModelMesh mesh = model.Meshes[m];

                // Test for correct vertex format.
                VerifyVertexFormat(mesh);

                // Get the mesh-to-object space transform.
                Matrix meshtoobject = bonearray[mesh.ParentBone.Index];

                indexremap.Clear();

                for (int p = 0; p < mesh.MeshParts.Count; p++)
                {
                    ModelMeshPart part = mesh.MeshParts[p];

                    // Get the number of verts and indices (only VertexPositionNormalTextureBump and short are supported - these are used by SunBurn).
                    int vertcount = part.VertexBuffer.VertexCount;
                    int indcount = part.IndexBuffer.IndexCount;

                    // Readjust the buffer sizes as necessary.
                    if (sourcevertices.Length < vertcount)
                        sourcevertices = new VertexPositionNormalTextureBump[vertcount];

                    if (sourceindices.Length < indcount)
                        sourceindices = new short[indcount];

                    // Get the mesh data.
                    part.VertexBuffer.GetData(sourcevertices, 0, vertcount);
                    part.IndexBuffer.GetData(sourceindices, 0, indcount);

                    // Loop through all of the vertices.
                    for (int i = 0; i < (part.PrimitiveCount*3); i++)
                    {
                        int index = sourceindices[i + part.StartIndex] + part.VertexOffset;

                        // Did we already store the data in the vertex buffer?
                        if (indexremap.ContainsKey(index))
                        {
                            indices.Add((short) indexremap[index]);
                            continue;
                        }

                        // Copy the vertex and convert to object space.
                        VertexPositionNormalTextureBump vert = sourcevertices[index];

                        vert.Position = Vector3.Transform(vert.Position, meshtoobject);
                        vert.Normal = Vector3.TransformNormal(vert.Normal, meshtoobject);
                        vert.Tangent = Vector3.TransformNormal(vert.Tangent, meshtoobject);
                        vert.Binormal = Vector3.TransformNormal(vert.Binormal, meshtoobject);

                        vert.Normal.Normalize();
                        vert.Tangent.Normalize();
                        vert.Binormal.Normalize();

                        // Remap the source index (from the model) to the destination index (in the buffers).
                        int destindex = vertices.Count;
                        indexremap.Add(index, destindex);

                        // Store the data.
                        indices.Add((short) destindex);
                        vertices.Add(vert);
                    }
                }
            }

            // Convert the buffers to the final arrays.
            Vertices = vertices.ToArray();
            Indices = indices.ToArray();
        }

        private void VerifyVertexFormat(ModelMesh mesh)
        {
            VertexElement[] validelements = VertexPositionNormalTextureBump.VertexElements;

            for (int p = 0; p < mesh.MeshParts.Count; p++)
            {
                ModelMeshPart part = mesh.MeshParts[p];
                VertexElement[] partelements = part.VertexBuffer.VertexDeclaration.GetVertexElements();

                foreach (VertexElement partelement in partelements)
                {
                    VertexElement validelement = FindElementByUsage(validelements, partelement.VertexElementUsage);
                    if (validelement.Equals(partelement))
                        continue;

                    throw new Exception("Model mesh '" + mesh.Name + "' contains an incorrect vertex format, " +
                                        "example is written to use the VertexPositionNormalTextureBump format.");
                }
            }
        }

        private VertexElement FindElementByUsage(VertexElement[] elements, VertexElementUsage usage)
        {
            foreach (VertexElement element in elements)
            {
                if (element.VertexElementUsage == usage)
                    return element;
            }

            return new VertexElement();
        }

        #region Implementation of IInstanceSource

        /// <summary>
        ///   Array of vertices describing the instance mesh.
        /// </summary>
        public VertexPositionNormalTextureBump[] Vertices { get; private set; }

        /// <summary>
        ///   Array of indices used to render the instance mesh.  Mesh primitives are assumed to be of the type TriangleList.
        /// </summary>
        public short[] Indices { get; private set; }

        #endregion
    }
}