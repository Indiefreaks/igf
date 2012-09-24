using System;
using Indiefreaks.Xna.Core;
using Indiefreaks.Xna.Sessions;

namespace Indiefreaks.Xna.Logic.StateDriven
{
    /// <summary>
    /// This abstract class provides the basic members to define a state
    /// </summary>
    /// <typeparam name="T">A custom Enum inheriting from ushort used to differentiate states</typeparam>
    public abstract class BaseState<T> : Behavior where T : struct, IConvertible
    {
        protected BaseState()
        {
            if (!typeof (T).IsEnum)
                throw new CoreException("StateMachineAgent generic accepts only Enum types");
            else if (typeof (T) != typeof (ushort))
                throw new CoreException("StateMachineAgent generic accepts only Enum types with an underlying type of ushort");

            AddServerCommand(CheckStatusOnServer, ApplyStatusOnClients, typeof (byte[]), DataTransferOptions.ReliableInOrder);
        }

        /// <summary>
        /// Returns the current state expressed with the provided Enum value
        /// </summary>
        protected abstract T CurrentStateEnumValue { get; }

        /// <summary>
        /// Returns if the current state is active
        /// </summary>
        public bool IsActive { get; protected set; }

        /// <summary>
        /// Handles the state status changes on the server and sends new status to all clients
        /// </summary>
        /// <param name="command"></param>
        /// <param name="networkvalue"></param>
        /// <returns></returns>
        protected object CheckStatusOnServer(Command command, object networkvalue)
        {
            var orders = new StateNetworkMessage<T>();

            if (!IsActive)
            {
                IsActive = true;
            }

            orders.IsActive = IsActive;

            T newState = CurrentStateEnumValue;
            if (IsActive)
                newState = CheckStateShouldChange();

            if (!newState.Equals(CurrentStateEnumValue))
            {
                orders.IsTerminating = true;

                orders.CurrentState = newState;

                IsActive = false;
            }
            else
            {
                orders.IsTerminating = false;

                orders.CurrentState = CurrentStateEnumValue;
            }

            return orders.ToBytes();
        }

        /// <summary>
        /// Applies the server computed state status on clients
        /// </summary>
        /// <param name="command"></param>
        /// <param name="networkvalue"></param>
        protected abstract void ApplyStatusOnClients(Command command, object networkvalue);

        /// <summary>
        /// Override this method to define what happens when the state activates
        /// </summary>
        protected abstract void Activate();

        /// <summary>
        /// Override this method to define when and how the current state changes
        /// </summary>
        /// <returns></returns>
        protected abstract T CheckStateShouldChange();

        /// <summary>
        /// Override this method to define what happens when the state terminates
        /// </summary>
        protected abstract void Terminate();
    }

    /// <summary>
    /// A Global state is a state that lives as long as the agent is alive. It is used to give agents general objectives
    /// </summary>
    /// <typeparam name="T">A custom Enum inheriting from ushort used to differentiate states</typeparam>
    public abstract class GlobalState<T> : BaseState<T> where T : struct, IConvertible
    {
        protected GlobalState()
        {
            AddCondition(() => ((StateMachineAgent<T>) Agent).GlobalState.Equals(CurrentStateEnumValue));
        }

        /// <summary>
        /// Applies the server computed state status on clients
        /// </summary>
        /// <param name="command"></param>
        /// <param name="networkvalue"></param>
        protected override void ApplyStatusOnClients(Command command, object networkvalue)
        {
            if (networkvalue == null)
                return;

            var orders = new StateNetworkMessage<T>((byte[])networkvalue);

            if (orders.IsActive)
            {
                IsActive = true;
                Activate();
            }

            if (orders.IsTerminating)
            {
                Terminate();
                IsActive = false;
            }

            if (!orders.CurrentState.Equals(((StateMachineAgent<T>)Agent).GlobalState))
                ((StateMachineAgent<T>)Agent).GlobalState = orders.CurrentState;
        }
    }

    /// <summary>
    /// The state class used to define states for your agents
    /// </summary>
    /// <typeparam name="T">A custom Enum inheriting from ushort used to differentiate states</typeparam>
    public abstract class State<T> : BaseState<T> where T : struct, IConvertible
    {
        protected State()
        {
            AddCondition(() => ((StateMachineAgent<T>) Agent).CurrentState.Equals(CurrentStateEnumValue));
        }

        /// <summary>
        /// Applies the server computed state status on clients
        /// </summary>
        /// <param name="command"></param>
        /// <param name="networkvalue"></param>
        protected override void ApplyStatusOnClients(Command command, object networkvalue)
        {
            if (networkvalue == null)
                return;

            var orders = new StateNetworkMessage<T>((byte[])networkvalue);

            if (orders.IsActive)
            {
                IsActive = true;
                Activate();
            }

            if (orders.IsTerminating)
            {
                Terminate();
                IsActive = false;
            }

            if (!orders.CurrentState.Equals(((StateMachineAgent<T>)Agent).CurrentState))
                ((StateMachineAgent<T>)Agent).CurrentState = orders.CurrentState;
        }
    }
}