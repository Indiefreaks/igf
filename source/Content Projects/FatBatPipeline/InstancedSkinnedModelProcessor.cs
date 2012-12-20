using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

using Indiefreaks.Xna.Rendering.Instancing.Skinned;
using SynapseGaming.LightingSystem.Processors;
using System.Diagnostics;

namespace FatBatPipeline
{
    /// <summary>
    /// Custom processor extends the ContentProcessor and wraps the ModelProcessor functionality
    /// </summary>
    [ContentProcessor(DisplayName="IGF - Instanced Skinned Model")]
    public class InstancedSkinnedModelProcessor : ContentProcessor<NodeContent, InstancedSkinnedModelContent>
    {

        private int sampleRate = 60; //default sample rate is 60 frames
        private List<Matrix> bindPose = new List<Matrix>();
        private List<Matrix> inverseBindPose = new List<Matrix>();
        private List<int> skeletonHierarchy = new List<int>();

        /// <summary>
        /// Get/set the sample rate that this processor will use. The reason this is important, is for
        /// recording the keyframes to the texture, we need to decide how often to sample the animation data
        /// </summary>
        public int SampleRate
        {
            get { return this.sampleRate; }
            set { this.sampleRate = value; }
        }


        /// <summary>
        /// The main Process method converts an intermediate format content pipeline
        /// NodeContent tree to a ModelContent object with embedded animation data.
        /// </summary>
        public override InstancedSkinnedModelContent Process(NodeContent input,
                                             ContentProcessorContext context)
        {
            //System.Diagnostics.Debugger.Launch();

            ValidateMesh(input, context, null);

            // Find the skeleton.
            BoneContent skeleton = MeshHelper.FindSkeleton(input);

            if (skeleton == null)
                throw new InvalidContentException("Input skeleton not found.");

            //Bake the transforms so everything is in the same coordinate system
            FlattenTransforms(input, skeleton);

            // This is a helper function that wasn't in the SkinnedModelSample, 
            // but was implemented here. Go through and remove meshes that don't have bone weights
            RemoveInvalidGeometry(input, context);

            // Read the bind pose and skeleton hierarchy data.
            IList<BoneContent> bones = MeshHelper.FlattenSkeleton(skeleton);

            // Collect bone information
            foreach (BoneContent bone in bones)
            {
                bindPose.Add(bone.Transform);
                inverseBindPose.Add(Matrix.Invert(bone.AbsoluteTransform));
                skeletonHierarchy.Add(bones.IndexOf(bone.Parent as BoneContent));
            }

            // We're going to keep a list of all the rows of animation matrices
            // We'll eventually turn this list into a texture
            List<Matrix[]> keyFrameMatrices = new List<Matrix[]>();

            // Get a list of animation clips, and at the same time, populate our keyFrameMatrices list
            Dictionary<string, InstancedAnimationClip> animationClips;
            animationClips = ProcessAnimations(skeleton.Animations, bones, keyFrameMatrices);

            // Create a content object that will hold the animation texture data
            PixelBitmapContent<Vector4> animationContent = GetEncodedTexture(keyFrameMatrices, bones.Count);

            // Create a texture 2D content object, and populate it with our data
            TextureContent animationTexture = new Texture2DContent();
            animationTexture.Faces[0].Add(animationContent);

            // We're going to create an instance of the original ModelProcessor,
            // with our minor material modification. The reason we do this is that 
            // the ModelProcessor does the heavy lifting for us of doing some BoneWeight
            // stuff, and we don't want to reimplement that when ModelProcessor does
            // such a fine job!
            //ModelProcessor processor = new ModifiedModelProcessor();

            DeferredLightingSystemModelProcessor processor = new DeferredLightingSystemModelProcessor();
            ModelContent model = processor.Process(input, context);  
          
            InstancedSkinningDataContent data = new InstancedSkinningDataContent(animationClips, animationTexture);
            InstancedSkinnedModelContent instancedModel = new InstancedSkinnedModelContent(model, data);

            return instancedModel;
        }


        /// <summary>
        /// This function processes the animation content of the model, and stores the data in the Matrix[] List
        /// </summary>
        /// <param name="animations"></param>
        /// <param name="bones"></param>
        /// <param name="outKeyFrames"></param>
        /// <returns></returns>
        private Dictionary<string, InstancedAnimationClip> ProcessAnimations(
                                                            AnimationContentDictionary animations,
                                                            IList<BoneContent> bones,
                                                            List<Matrix[]> outKeyFrames)
        {

            // Create a map of bones - just like in the SkinnedModel Smaple
            Dictionary<string, int> boneMap = new Dictionary<string, int>();
            for (int i = 0; i < bones.Count; i++)
            {
                string boneName = bones[i].Name;

                if (!string.IsNullOrEmpty(boneName))
                    boneMap.Add(boneName, i);
            }

            // This is a dictionary of all the animations in the model
            Dictionary<string, InstancedAnimationClip> animationDictionary = new Dictionary<string, InstancedAnimationClip>();

            // Loop through each animation, and add that stuff to our animation texture, as well as to our dictionary we are going to output
            foreach (KeyValuePair<string, AnimationContent> data in animations)
            {
                string animationName = data.Key;
                AnimationContent animation = data.Value;

                InstancedAnimationClip clip = ProcessAnimation(animation, outKeyFrames, boneMap);
                animationDictionary.Add(animationName, clip);
            }

            return animationDictionary;

        }

        /// <summary>
        /// This does the actual heavy lifting of going through the animation keyframe data, calculating the final world transforms,
        /// and storing them in our list of matrices which will eventually become our animation texture
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="animationRows"></param>
        /// <param name="boneMap"></param>
        /// <returns></returns>
        private InstancedAnimationClip ProcessAnimation(AnimationContent animation, List<Matrix[]> animationRows, Dictionary<string, int> boneMap)
        {
            // Get a list of all the key frames in the animation, sorted by time
            List<Keyframe> keyframes = GetKeyFrameList(animation, boneMap);

            // Now, we're going to sort of copy the functionality of AnimationPlayer in the original skinned model sample
            // We're going to cycle through the animation, at a specified framerate, and take snapshots of the matrices
            // We'll then store these snapshots in the list, and record the start of the snapshot
            int startRow = animationRows.Count;

            float timeDelta = 1.0f / (float)this.sampleRate;
            float currentTime = 0f;
            int currentKeyFrame = 0;


            //Copy over the bindpose matrices
            Matrix[] boneTransforms = this.bindPose.ToArray();

            while (currentTime < animation.Duration.TotalSeconds)
            {

                //Update bone transforms based on the keyframes
                for (int i = currentKeyFrame; i < keyframes.Count; i++)
                {
                    Keyframe keyframe = keyframes[i];
                    if (currentTime < keyframe.Time.TotalSeconds)
                        break;

                    boneTransforms[keyframe.Bone] = keyframe.Transform;
                    currentKeyFrame = i;
                }

                //Create the world transforms
                Matrix[] worldTransforms = new Matrix[boneTransforms.Length];
                worldTransforms[0] = boneTransforms[0];
                for (int i = 1; i < worldTransforms.Length; i++)
                {
                    int parentBone = this.skeletonHierarchy[i];
                    worldTransforms[i] = boneTransforms[i] * worldTransforms[parentBone];
                }

                //Create the skinning transforms
                Matrix[] skinningTransforms = new Matrix[worldTransforms.Length];
                for (int i = 0; i < skinningTransforms.Length; i++)
                {
                    skinningTransforms[i] = this.inverseBindPose[i] * worldTransforms[i];
                }

                //Add the final skinning animation to our list of rows
                animationRows.Add(skinningTransforms);

                currentTime += timeDelta;
            }

            return new InstancedAnimationClip(animation.Duration, startRow, animationRows.Count - 1, this.sampleRate);

        }

        /// <summary>
        /// This function encodes a new texture based on the list of key frame matrices
        /// </summary>
        /// <param name="keyFrameMatrices"></param>
        /// <returns></returns>
        static private PixelBitmapContent<Vector4> GetEncodedTexture(List<Matrix[]> keyFrameMatrices, int bones)
        {

            //We need 4 pixels per bone. We can store each row of the transform matrix in one pixel.
            int width = bones * 4;

            //Create a new bitmap content object
            PixelBitmapContent<Vector4> animationTexture = new PixelBitmapContent<Vector4>(width, keyFrameMatrices.Count);

            int y = 0;
            //Now, we'll write all the rows
            foreach (Matrix[] animationRow in keyFrameMatrices)
            {
                int x = 0;
                foreach (Matrix transform in animationRow)
                {
                    // Pretty trivial - we just right out the matrix data as pixels. Instead of R, G, B, and A, we write matrix values.

                    // However, as mentioned in the article, this could be optimized. We really only need 3 pixels to describe a matrix,
                    // because in a transform matrix, the fourth row is always (0, 0, 0, 1). Thus, we could use the RGB values to describe the 3x3
                    // matrix, and then use the A values to describe the translation values in the 4th column. During runtime, this would save
                    // us a vertex texture fetch per transform!

                    animationTexture.SetPixel(x + 0, y, new Vector4(transform.M11, transform.M12, transform.M13, transform.M14));
                    animationTexture.SetPixel(x + 1, y, new Vector4(transform.M21, transform.M22, transform.M23, transform.M24));
                    animationTexture.SetPixel(x + 2, y, new Vector4(transform.M31, transform.M32, transform.M33, transform.M34));
                    animationTexture.SetPixel(x + 3, y, new Vector4(transform.M41, transform.M42, transform.M43, transform.M44));
                    x += 4;
                }

                y++;
            }

            return animationTexture;
        }

        /// <summary>
        /// Return a list of keyframes, sorted by time, from an animation. This is basically unchanged from the SkinnedModel sample.
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="boneMap"></param>
        /// <returns></returns>
        static private List<Keyframe> GetKeyFrameList(AnimationContent animation, Dictionary<string, int> boneMap)
        {
            List<Keyframe> keyframes = new List<Keyframe>();

            // For each input animation channel.
            foreach (KeyValuePair<string, AnimationChannel> channel in
                animation.Channels)
            {
                // Look up what bone this channel is controlling.
                int boneIndex;

                if (!boneMap.TryGetValue(channel.Key, out boneIndex))
                {
                    throw new InvalidContentException(string.Format(
                        "Found animation for bone '{0}', " +
                        "which is not part of the skeleton.", channel.Key));
                }

                // Convert the keyframe data.
                foreach (AnimationKeyframe keyframe in channel.Value)
                {
                    keyframes.Add(new Keyframe(boneIndex, keyframe.Time,
                                               keyframe.Transform));
                }
            }

            // Sort the merged keyframes by time.
            keyframes.Sort(CompareKeyframeTimes);

            if (keyframes.Count == 0)
                throw new InvalidContentException("Animation has no keyframes.");

            if (animation.Duration <= TimeSpan.Zero)
                throw new InvalidContentException("Animation has a zero duration.");

            return keyframes;
        }

        /// <summary>
        /// Comparison function for sorting keyframes into ascending time order. Unchanged from skinned model sample.
        /// </summary>
        static int CompareKeyframeTimes(Keyframe a, Keyframe b)
        {
            return a.Time.CompareTo(b.Time);
        }


        /// <summary>
        /// Makes sure this mesh contains the kind of data we know how to animate. Basically unchanged from SkinnedModelSample.
        /// </summary>
        static void ValidateMesh(NodeContent node, ContentProcessorContext context,
                                 string parentBoneName)
        {
            MeshContent mesh = node as MeshContent;

            if (mesh != null)
            {
                // Validate the mesh.
                if (parentBoneName != null)
                {
                    context.Logger.LogWarning(null, null,
                        "Mesh {0} is a child of bone {1}. InstancedSkinnedModelProcessor " +
                        "does not correctly handle meshes that are children of bones.",
                        mesh.Name, parentBoneName);
                }

                if (!MeshHasSkinning(mesh))
                {
                    context.Logger.LogWarning(null, null,
                        "Mesh {0} has no skinning information, so it has been deleted.",
                        mesh.Name);

                    mesh.Parent.Children.Remove(mesh);
                    return;
                }
            }
            else if (node is BoneContent)
            {
                // If this is a bone, remember that we are now looking inside it.
                parentBoneName = node.Name;
            }

            // Recurse (iterating over a copy of the child collection,
            // because validating children may delete some of them).
            foreach (NodeContent child in new List<NodeContent>(node.Children))
                ValidateMesh(child, context, parentBoneName);
        }


        /// <summary>
        /// Checks whether a mesh contains skininng information. Unchanged from SkinnedModelSample.
        /// </summary>
        static bool MeshHasSkinning(MeshContent mesh)
        {
            foreach (GeometryContent geometry in mesh.Geometry)
            {
                if (!geometry.Vertices.Channels.Contains(VertexChannelNames.Weights()))
                    return false;
            }

            return true;
        }


        /// <summary>
        /// Bakes unwanted transforms into the model geometry,
        /// so everything ends up in the same coordinate system.
        /// Unchanged from SkinnedModelSample
        /// </summary>
        static void FlattenTransforms(NodeContent node, BoneContent skeleton)
        {
            foreach (NodeContent child in node.Children)
            {

                MeshContent content;
                // Don't process the skeleton, because that is special.
                if (child == skeleton)
                    continue;

                // Bake the local transform into the actual geometry.
                MeshHelper.TransformScene(child, child.Transform);

                // Having baked it, we can now set the local
                // coordinate system back to identity.
                child.Transform = Matrix.Identity;

                // Recurse.
                FlattenTransforms(child, skeleton);
            }
        }


        /// <summary>
        /// This function removes geometry that contains no bone weights, because the ModelProcessor
        /// will throw an exception if we give it geometry content like that.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="context"></param>
        static void RemoveInvalidGeometry(NodeContent node, ContentProcessorContext context)
        {

            MeshContent meshContent = node as MeshContent;
            if (meshContent != null)
            {
                // Maintain a list of all the geometry that was invalid that we will be removing
                List<GeometryContent> removeGeometry = new List<GeometryContent>();
                foreach (GeometryContent geometry in meshContent.Geometry)
                {

                    VertexChannelCollection channels = geometry.Vertices.Channels;

                    // Does this geometry contain bone weight information?
                    if (geometry.Vertices.Channels.Contains(VertexChannelNames.Weights(0)))
                    {
                        bool removed = false;

                        VertexChannel<BoneWeightCollection> weights = geometry.Vertices.Channels.Get<BoneWeightCollection>(VertexChannelNames.Weights(0));
                        foreach (BoneWeightCollection collection in weights)
                        {
                            // If we don't have any weights, then this isn't going to be good. The geometry has no bone weights,
                            // so lets just remove it.
                            if (collection.Count <= 0)
                            {
                                removeGeometry.Add(geometry);
                                removed = true;
                                break;
                            }
                            else
                                // Otherwise, normalize the weights. This call is probably unnecessary.
                                collection.NormalizeWeights(4);
                        }

                        //If we removed something from this geometry, just remove the whole geometry - there's no point in going farther
                        if (removed)
                            break;
                    }

                }

                // Remove all the invalid geometry we found, and log a warning.
                foreach (GeometryContent geometry in removeGeometry)
                {
                    meshContent.Geometry.Remove(geometry);
                    context.Logger.LogWarning(null, null,
                    "Mesh part {0} has been removed because it has no bone weights associated with it.",
                    geometry.Name);

                }
            }

            // Recursively call this function for each child
            foreach (NodeContent child in node.Children)
            {
                RemoveInvalidGeometry(child, context);

            }
        }
    }
}
