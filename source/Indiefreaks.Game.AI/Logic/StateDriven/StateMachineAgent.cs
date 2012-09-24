using System;
using Indiefreaks.Xna.Core;

namespace Indiefreaks.Xna.Logic.StateDriven
{
    /// <summary>
    /// Add this component to your SceneEntity or SceneObject instances to provide Finite State Machine AI.
    /// </summary>
    /// <typeparam name="T">A custom Enum inheriting from ushort used to differentiate states</typeparam>
    public class StateMachineAgent<T> : NonPlayerAgent where T : struct, IConvertible
    {
        private T _currentState;

        /// <summary>
        /// Gets or sets the current global state instance for this agent. Used to give general objectives to your agents
        /// </summary>
        public T GlobalState { get; set; }

        /// <summary>
        /// Gets or sets the current active state
        /// </summary>
        public T CurrentState
        {
            get { return _currentState; }
            set
            {
                PreviousState = _currentState;
                _currentState = value;
            }
        }

        /// <summary>
        /// Gets or sets the previously active state.
        /// </summary>
        public T PreviousState { get; private set; }

        /// <summary>
        /// Event called when the component's parent object is assigned or reassigned.
        /// </summary>
        public override void OnInitialize()
        {
            base.OnInitialize();

            if (!typeof (T).IsEnum)
                throw new CoreException("StateMachineAgent generic accepts only Enum types");
            else if (typeof (T) != typeof (ushort))
                throw new CoreException("StateMachineAgent generic accepts only Enum types with an underlying type of ushort");
        }
    }
}