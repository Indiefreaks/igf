/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Emitters
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using ProjectMercury.Controllers;
    using ProjectMercury.Modifiers;
    using Microsoft.Xna.Framework.Content;

    /// <summary>
    /// Defines the abstract base class for a particle emitter.
    /// </summary>
    public abstract class AbstractEmitter : ISupportDeepCopy<AbstractEmitter>
    {
        private String _name;

        /// <summary>
        /// Gets or sets the name of the particle emitter.
        /// </summary>
        public String Name
        {
            get { return this._name; }
            set
            {
                this._name = value ?? String.Empty;

                this.OnNameChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// DO NOT USE: Short term hack to allow per particle Axis. Will be replaced in future version
        /// </summary>
        public bool UseVelocityAsBillboardAxis;

        /// <summary>
        /// Event raised when the name of the particle emitter has been changed.
        /// </summary>
        public event EventHandler NameChanged;

        /// <summary>
        /// Raises the NameChanged event for the particle emitter.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnNameChanged(EventArgs e)
        {
            if (this.NameChanged != null)
                this.NameChanged(this, e);
        }

        /// <summary>
        /// Gets a value indicating wether or not the particle emitter has been initialised.
        /// </summary>
        public Boolean Initialised { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating wether or not the particle emitter is enabled.
        /// </summary>
        public Boolean Enabled { get; set; }

        /// <summary>
        /// Records the elapsed time in whole and fractional seconds.
        /// </summary>
        private Single TotalSeconds;

        /// <summary>
        /// An array of particles belonging to the emitter.
        /// </summary>
        internal Particle[] Particles;

        /// <summary>
        /// The index of the next idle particle in the ring buffer.
        /// </summary>
        private Int32 IdleIndex;

        /// <summary>
        /// The index of the first active particle in the ring buffer.
        /// </summary>
        internal Int32 ActiveIndex;

        private Int32 _budget;

        /// <summary>
        /// Gets or sets the total number of particles which will be available to the emitter.
        /// </summary>
        public Int32 Budget
        {
            get { return this._budget; }
            set
            {
                Check.ArgumentGreaterThan("Budget", value, 0);
                Check.False(this.Initialised, "Cannot change budget after emitter has been initialised!");

                this._budget = value;
            }
        }

        private Single _term;

        /// <summary>
        /// Gets or sets the amount of time particles will remain active in whole and fractional seconds.
        /// </summary>
        public Single Term
        {
            get { return this._term; }
            set
            {
                Check.ArgumentFinite("Term", value);
                Check.ArgumentGreaterThan("Term", value, 0f);
                Check.False(this.Initialised, "Cannot change term after emitter has been initialised!");

                this._term = value;
            }
        }

        private Int32 _releaseQuantity;

        /// <summary>
        /// Gets or sets the number of particles which will be released each time the emitter is triggered.
        /// </summary>
        public Int32 ReleaseQuantity
        {
            get { return this._releaseQuantity; }
            set
            {
                Check.ArgumentGreaterThan("ReleaseQuantity", value, 0);

                this._releaseQuantity = value;
            }
        }

        /// <summary>
        /// Gets or sets the style of the billboard rendering.
        /// </summary>
        public BillboardStyle BillboardStyle { get; set; }

        /// <summary>
        /// Gets or sets the rotational axis for cylindrical billboarding.
        /// </summary>
        public Vector3 BillboardRotationalAxis { get; set; }

        /// <summary>
        /// Gets or sets the release speed of particles.
        /// </summary>
        public Range ReleaseSpeed { get; set; }

        /// <summary>
        /// Gets or sets the release opacity of particles.
        /// </summary>
        public Range ReleaseOpacity { get; set; }

        /// <summary>
        /// Gets or sets the release scale of particles.
        /// </summary>
        public Range ReleaseScale { get; set; }

        /// <summary>
        /// Gets or sets the release colour of particles.
        /// </summary>
        public ColourRange ReleaseColour { get; set; }

        /// <summary>
        /// Gets or sets the release rotation of particles.
        /// </summary>
        public RotationRange ReleaseRotation { get; set; }

        /// <summary>
        /// Gets or sets the blending mode of the particle emitter.
        /// </summary>
        public EmitterBlendMode BlendMode { get; set; }

        /// <summary>
        /// Gets or sets the texture asset to be used when rendering particles.
        /// </summary>
        [ContentSerializerIgnore]
        public Texture2D ParticleTexture { get; set; }

        /// <summary>
        /// Gets or sets the path to the texture asset.
        /// </summary>
        public String ParticleTextureAssetPath { get; set; }

        /// <summary>
        /// Gets or sets the number of currently active particles.
        /// </summary>
        public Int32 ActiveParticlesCount { get; private set; }

        /// <summary>
        /// Gets or sets the collection of modifiers which are applied to the particle emitter.
        /// </summary>
        public ModifierCollection Modifiers { get; set; }

        /// <summary>
        /// Gets or sets the collection of modifiers which are applied to the particle emitter.
        /// </summary>
        //public ModifierCollection TriggerModifiers { get; set; }

        /// <summary>
        /// Gets or sets the controllers which have been applied to the emitter.
        /// </summary>
        public ControllerPipeline Controllers { get; set; }

        /// <summary>
        /// Initialises a new instance of the AbstractEmitter class.
        /// </summary>
        protected AbstractEmitter()
        {
            this.Enabled = true;

            this.Modifiers = new ModifierCollection();
            //this.TriggerModifiers = new ModifierCollection();
            this.Controllers = new ControllerPipeline();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of AbstractEmitter which is a copy of this instance.</returns>
        public AbstractEmitter DeepCopy()
        {
            return this.DeepCopy(null);
        }

        /// <summary>
        /// Copies the properties of this instance into the specified existing instance.
        /// </summary>
        /// <param name="exisitingInstance">An existing emitter instance.</param>
        protected virtual AbstractEmitter DeepCopy(AbstractEmitter exisitingInstance)
        {
            var value = exisitingInstance ?? default(AbstractEmitter);

            value.BlendMode       = this.BlendMode;
            value.Budget          = this.Budget;
            value.Enabled         = this.Enabled;
            value.Modifiers       = this.Modifiers.DeepCopy();
            //value.TriggerModifiers = this.TriggerModifiers.DeepCopy();
            value.Controllers     = this.Controllers.DeepCopy();
            value.Name            = this.Name;
            value.ParticleTexture = this.ParticleTexture;
            value.ParticleTextureAssetPath = this.ParticleTextureAssetPath;
            value.ReleaseColour   = this.ReleaseColour;
            value.ReleaseOpacity  = this.ReleaseOpacity;
            value.ReleaseQuantity = this.ReleaseQuantity;
            value.ReleaseRotation = this.ReleaseRotation;
            value.ReleaseScale    = this.ReleaseScale;
            value.ReleaseSpeed    = this.ReleaseSpeed;
            value.Term            = this.Term;
            value.BillboardRotationalAxis = this.BillboardRotationalAxis;
            value.BillboardStyle = this.BillboardStyle;

            return value;
        }

        /// <summary>
        /// Initialises the particle emitter.
        /// </summary>
        public void Initialise()
        {
            Check.False(this.Initialised, "Emitter is already initialised!");

            this.Particles = new Particle[this.Budget];

            this.IdleIndex = 0;
            this.ActiveIndex = 0;
            this.ActiveParticlesCount = 0;

            this.TotalSeconds = 0f;

            this.Initialised = true;
        }

        /// <summary>
        /// Initialises the particle emitter.
        /// </summary>
        /// <param name="budget">The total number of particles which will be available to the emitter.</param>
        /// <param name="term">The amount of time particles will be active in whole and fractional seconds.</param>
        public void Initialise(Int32 budget, Single term)
        {
            this.Initialised = false;

            this.Budget = budget;
            this.Term = term;

            this.Initialise();
        }

        /// <summary>
        /// Terminates the particle emitter immediately, retiring all active particles.
        /// </summary>
        public void Terminate()
        {
            this.IdleIndex = 0;
            this.ActiveIndex = 0;
            this.ActiveParticlesCount = 0;
        }

        /// <summary>
        /// Loads content required by the emitter.
        /// </summary>
        /// <param name="contentManager">A content manager instance.</param>
        protected virtual void LoadContent(ContentManager contentManager)
        {
            Check.ArgumentNotNull("contentManager", contentManager);

            if (String.IsNullOrEmpty(this.ParticleTextureAssetPath) != true)
                this.ParticleTexture = contentManager.Load<Texture2D>(this.ParticleTextureAssetPath);
        }

        /// <summary>
        /// Updates the particle emitter.
        /// </summary>
        /// <param name="deltaSeconds">Elapsed time in whole and fractional seconds.</param>
        public void Update(Single deltaSeconds)
        {
            Check.True(this.Initialised, "Emitter has not yet been initialised!");

            this.TotalSeconds += deltaSeconds;

            this.Controllers.Update(deltaSeconds);

            if (this.ActiveParticlesCount < 1)
                return;
#if UNSAFE
            unsafe
#endif
            {
#if UNSAFE
                fixed (Particle* particleArray = this.Particles)
#endif
                {
                    // Copy the state of the ring buffer as we will modify it as particles die...
                    var currentIndex         = this.ActiveIndex;
                    var currentParticleCount = this.ActiveParticlesCount;

                    for (var i = 0; i < currentParticleCount; i++)
                    {
#if UNSAFE
                        // Get a pointer to the particle in the buffer...
                        Particle* particle = particleArray + currentIndex;

                        // Calculate the age of the particle in seconds...
                        var actualAge = this.TotalSeconds - particle->Inception;
#else
                        // Extract the particle from the buffer...
                        Particle particle = this.Particles[currentIndex];

                        // Calculate the age of the particle in seconds...
                        var actualAge = this.TotalSeconds - particle.Inception;
#endif
                        // Check to see if the particle has expired...
                        if (actualAge > this.Term)
                        {
                            // Increment the index of the first active particle...
                            this.ActiveIndex = (this.ActiveIndex + 1) % this.Budget;

                            // Decrement the active particles count...
                            this.ActiveParticlesCount = (this.ActiveParticlesCount - 1);
                        }
                        else
                        {
#if UNSAFE
                            // Calculate the normalized age of the particle...
                            particle->Age = actualAge / this.Term;

                            //HACK: For progenitor DBP use the velocity as the axis for a per particle axis
                            if (!UseVelocityAsBillboardAxis)
                            {
                                // Apply particle movement...
                                particle->Position.X += (particle->Velocity.X * deltaSeconds);
                                particle->Position.Y += (particle->Velocity.Y*deltaSeconds);
                                particle->Position.Z += (particle->Velocity.Z*deltaSeconds);
                            }
                            else
                            {
                                particle->Velocity.Normalize();
                            }
#else
                            // Calculate the normalized age of the particle...
                            particle.Age = actualAge / this.Term;

                            // Apply particle movement...
                            particle.Position.X += (particle.Velocity.X * deltaSeconds);
                            particle.Position.Y += (particle.Velocity.Y * deltaSeconds);
                            particle.Position.Z += (particle.Velocity.Z * deltaSeconds);

                            // Put the mutated particle back in the buffer...
                            this.Particles[currentIndex] = particle;
#endif
                        }

                        currentIndex = (currentIndex + 1) % this.Budget;
                    }

                    if (this.ActiveParticlesCount > 0)
                    {
#if UNSAFE
                        var buffer = particleArray;
#else
                        var buffer = this.Particles;
#endif
                        var iterator = new ParticleIterator(buffer, this.Budget, this.ActiveIndex, this.ActiveParticlesCount);

                        this.Modifiers.RunProcessors(deltaSeconds, ref iterator);
                    }
                }
            }
        }

        /// <summary>
        /// Triggers the emitter using the specified trigger context.
        /// </summary>
        /// <param name="context">The trigger context.</param>
        internal void Trigger(ref TriggerContext context)
        {
            Check.True(this.Initialised, "Emitter has not yet been initialized!");

            var rotation = Matrix.Identity;

            var isRotated = false;
            
            if (context.Rotation != Vector3.Zero)
            {
                rotation = Matrix.CreateFromYawPitchRoll(context.Rotation.Z, context.Rotation.X, context.Rotation.Y);
                
                isRotated = true;
            }

            if (this.Enabled == false)
                return;
#if UNSAFE
            unsafe
#endif
            {
#if UNSAFE
                fixed (Particle* particleArray = this.Particles)
#endif
                {
                    var currentIndex = this.IdleIndex;
                    var startIndex = currentIndex;
                    var particlesAdded = 0;

                    // Calculate the number of particles to release - the lesser of [ReleaseQuantity] and
                    // [remaining idle particles]...
                    var releaseCount = Math.Min(context.ReleaseQuantity, this.Budget - this.ActiveParticlesCount);

                    for (var i = 0; i < releaseCount; i++)
                    {
#if UNSAFE
                        Particle* particle = particleArray + currentIndex;
#else
                        Particle particle = this.Particles[currentIndex];
#endif
                        Vector3 offset, force;

                        this.GenerateOffsetAndForce(out offset, out force);
                        if (isRotated)
                        {
                            Vector3.Transform(ref offset, ref rotation, out offset);
                            Vector3.Transform(ref force, ref rotation, out force);
                        }

                        var releaseSpeed = RandomUtil.NextSingle(this.ReleaseSpeed);
#if UNSAFE
                        particle->Age = 0f;
                        particle->Inception = this.TotalSeconds;

                        particle->Position.X = context.Position.X + offset.X;
                        particle->Position.Y = context.Position.Y + offset.Y;
                        particle->Position.Z = context.Position.Z + offset.Z;

                        particle->Velocity.X = force.X * releaseSpeed;
                        particle->Velocity.Y = force.Y * releaseSpeed;
                        particle->Velocity.Z = force.Z * releaseSpeed;

                        particle->Scale = RandomUtil.NextSingle(this.ReleaseScale);

                        particle->Colour = new Vector4(RandomUtil.NextColour(this.ReleaseColour),
                                                       RandomUtil.NextSingle(this.ReleaseOpacity));

                        particle->Rotation = RandomUtil.NextRotation(this.ReleaseRotation);

                        particle->EffectProxyIndex = context.ProxyIndex;
#else
                        particle.Age = 0f;
                        particle.Inception = this.TotalSeconds;

                        particle.Position.X = context.Position.X + offset.X;
                        particle.Position.Y = context.Position.Y + offset.Y;
                        particle.Position.Z = context.Position.Z + offset.Z;

                        particle.Velocity.X = force.X * releaseSpeed;
                        particle.Velocity.Y = force.Y * releaseSpeed;
                        particle.Velocity.Z = force.Z * releaseSpeed;

                        particle.Scale = RandomUtil.NextSingle(this.ReleaseScale);

                        particle.Colour = new Vector4(RandomUtil.NextColour(this.ReleaseColour),
                                                      RandomUtil.NextSingle(this.ReleaseOpacity));

                        particle.Rotation = RandomUtil.NextRotation(this.ReleaseRotation);

                        particle.EffectProxyIndex = context.ProxyIndex;

                        this.Particles[currentIndex] = particle;

#endif

                        this.IdleIndex = (this.IdleIndex + 1) % this.Budget;

                        this.ActiveParticlesCount++;

                        currentIndex = (currentIndex + 1) % this.Budget;

                        particlesAdded++;
                    }


//                    if (particlesAdded > 0)
//                    {
//#if UNSAFE
//                        var buffer = particleArray;
//#else
//                        var buffer = this.Particles;
//#endif
//                        ParticleIterator iterator = new ParticleIterator(buffer, this.Budget, startIndex, particlesAdded);

//                        this.TriggerModifiers.RunProcessors(0f, ref iterator);
//                    }
                }
            }
        }

        /// <summary>
        /// Generates offset and force vectors for a newly released particle.
        /// </summary>
        /// <param name="offset">Defines an offset vector from the trigger position.</param>
        /// <param name="force">A unit vector defining the inital force applied to the particle.</param>
        protected virtual void GenerateOffsetAndForce(out Vector3 offset, out Vector3 force)
        {
            offset = Vector3.Zero;

            force = RandomUtil.NextUnitVector3();
        }
    }
}