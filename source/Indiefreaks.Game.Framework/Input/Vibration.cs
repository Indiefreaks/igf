using System;
using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Input
{
#if WINDOWS || XBOX
    public class Vibration
    {
        #region Motor enum

        /// <summary>
        /// Enumeration for vibration target
        /// </summary>
        public enum Motor
        {
            Left = 0,
            Right = 1
        }

        #endregion

        private readonly float _initialDuration;

        private readonly float _initialFrequency;
        private readonly Func<Vibration, float> _modifier;
        private readonly Motor _targetMotor;
        internal float _computedFrequency;
        private long _timeRemaining;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vibration"/> class.
        /// </summary>
        /// <param name="targetMotor">The target motor</param>
        /// <param name="frequency">The motor frequency.</param>
        /// <param name="duration">The duration of the vibration</param>
        public Vibration(Motor targetMotor, float frequency, float duration)
            : this(targetMotor, frequency, duration, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vibration"/> class.
        /// </summary>
        /// <param name="targetMotor">The target motor</param>
        /// <param name="frequency">The motor frequency.</param>
        /// <param name="duration">The duration of the vibration</param>
        /// <param name="modifier">Frequency modifier delegate</param>
        public Vibration(Motor targetMotor, float frequency, float duration, Func<Vibration, float> modifier)
        {
            _targetMotor = targetMotor;
            _computedFrequency = _initialFrequency = frequency;
            _initialDuration = duration;
            // duration input is specified in milliseconds - so convert it to ticks
            _timeRemaining = (long) duration*TimeSpan.TicksPerMillisecond;
            _modifier = modifier;
        }

        /// <summary>
        /// Gets the target motor.
        /// </summary>
        public Motor TargetMotor
        {
            get { return _targetMotor; }
        }

        /// <summary>
        /// Gets the frequency.
        /// </summary>
        public float Frequency
        {
            get { return _initialFrequency; }
        }

        /// <summary>
        /// Gets the computed frequency.
        /// </summary>
        public float ComputedFrequency
        {
            get { return _computedFrequency; }
        }

        /// <summary>
        /// Gets the duration.
        /// </summary>
        public float Duration
        {
            get { return _initialDuration; }
        }

        /// <summary>
        /// Gets the time remaining.
        /// </summary>
        public float TimeRemaining
        {
            // since user supplies duration in milliseconds convert it back for them
            get { return _timeRemaining/TimeSpan.TicksPerMillisecond; }
        }

        /// <summary>
        /// Delegate to apply a linear falloff to the vibration frequency
        /// </summary>
        /// <param name="vibration">The vibration.</param>
        /// <returns>Calculated frequency</returns>
        public static float LinearFalloff(Vibration vibration)
        {
            return MathHelper.Lerp(vibration.Frequency, 0, 1 - (vibration.TimeRemaining/vibration.Duration));
        }

        /// <summary>
        /// Delegate to apply a linear ramp to the vibration frequency
        /// </summary>
        /// <param name="vibration">The vibration.</param>
        /// <returns>Calculated frequency</returns>
        public static float LinearRamp(Vibration vibration)
        {
            return MathHelper.Lerp(0, vibration.Frequency, 1 - (vibration.TimeRemaining/vibration.Duration));
        }

        /// <summary>
        /// Updates the vibration.
        /// </summary>
        /// <param name="dt">Elapsed time in ticks</param>
        internal void Update(long dt)
        {
            _timeRemaining -= dt;
            // no point updating if we're only going to remove it next
            if (_timeRemaining > 0 && _modifier != null)
            {
                _computedFrequency = _modifier(this);
            }
        }
    }
#endif
}