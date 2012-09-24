using System;
using System.Collections.Generic;
using System.Linq;
using Indiefreaks.Xna.Core;
using Indiefreaks.Xna.Sessions;

namespace Indiefreaks.Xna.Logic
{
    /// <summary>
    /// The Behavior class is responsible of maintaining a list of Conditions and Commands.
    /// Conditions are tested every frame and if their aggregated result returns true, the Behavior Commands are executed sequentially.
    /// </summary>
    public class Behavior : IProcess,IDisposable
    {
        internal List<Command> Commands = new List<Command>();
        internal List<Condition> Conditions = new List<Condition>();

        public Behavior()
        {
            Enabled = true;
            Frequency = ExecutionFrequency.FullUpdate60Hz;
        }

        /// <summary>
        /// Returns the Agent this Behavior is attached to
        /// </summary>
        public Agent Agent { get; set; }

        /// <summary>
        /// Gets or et if the current Behavior is active or not
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the frequency at which the current command will be considered for execution
        /// </summary>
        public ExecutionFrequency Frequency
        {
            get { return _executionFrequency; }
            set
            {
                if (_executionFrequency != value)
                {
                    _executionFrequency = value;
                    switch (_executionFrequency)
                    {
                        case ExecutionFrequency.PartialUpdate1Hz:
                            _frequencyValue = 1f;
                            break;
                        case ExecutionFrequency.PartialUpdate5Hz:
                            _frequencyValue = 5f;
                            break;
                        case ExecutionFrequency.PartialUpdate10Hz:
                            _frequencyValue = 10f;
                            break;
                        case ExecutionFrequency.PartialUpdate15Hz:
                            _frequencyValue = 15f;
                            break;
                        case ExecutionFrequency.HalfUpdate30Hz:
                            _frequencyValue = 30f;
                            break;
                        default:
                        case ExecutionFrequency.FullUpdate60Hz:
                            _frequencyValue = 60f;
                            break;
                    }
                }
            }
        }
        private ExecutionFrequency _executionFrequency;
        private float _frequencyValue;

        ~Behavior()
        {
            Dispose(false);
        }

        /// <summary>
        /// Initializes the Behavior
        /// </summary>
        /// <remarks>This method is called once the Behavior is added to the Agent's behaviors and associated with the Agent</remarks>
        public virtual void Initialize()
        {
            AddCondition(() => Enabled);
        }

        /// <summary>
        /// Adds a new condition to the Behavior
        /// </summary>
        /// <param name="condition">The Func<bool> method that will be tested</bool></param>
        protected void AddCondition(Condition condition)
        {
            Conditions.Add(condition);
        }

        /// <summary>
        /// Adds a new Command to the Behavior
        /// </summary>
        private void AddCommand(Command command)
        {
            command.Behavior = this;

            Commands.Add(command);

            // We register the command so that it can be executed in all player machines part of the current session
            SessionManager.CurrentSession.RegisterCommand(command);
        }

        /// <summary>
        /// Adds a Command to this behavior that will be executed solely on the client
        /// </summary>
        /// <param name="clientCommand">The delegate to the code that will be executed by the client</param>
        protected void AddLocalCommand(Command.ClientCommand clientCommand)
        {
            AddCommand(Command.CreateLocalCommand(clientCommand));
        }

        /// <summary>
        /// Adds a Command to this behavior that will be executed solely on the client
        /// </summary>
        /// <param name="clientCommand">The delegate to the code that will be executed by the client</param>
        protected void AddLocalCommand(Command.ClientCommand clientCommand, ExecutionFrequency frequency)
        {
            AddCommand(Command.CreateLocalCommand(clientCommand, frequency));
        }

        /// <summary>
        /// Adds a Command to this behavior that will be executed solely on the client
        /// </summary>
        /// <param name="clientCommand">The delegate to the code that will be executed by the client</param>
        /// <param name="condition"></param>
        protected void AddLocalCommand(Condition condition, Command.ClientCommand clientCommand)
        {
            AddCommand(Command.CreateLocalCommand(condition, clientCommand));
        }

        /// <summary>
        /// Adds a Command to this behavior that will be executed solely on the client
        /// </summary>
        /// <param name="clientCommand">The delegate to the code that will be executed by the client</param>
        /// <param name="condition"></param>
        /// <param name="frequency"></param>
        protected void AddLocalCommand(Condition condition, Command.ClientCommand clientCommand, ExecutionFrequency frequency)
        {
            AddCommand(Command.CreateLocalCommand(condition, clientCommand, frequency));
        }

        /// <summary>
        /// Adds a Command to this behavior that will be executed on the client and on the server
        /// </summary>
        /// <param name="clientExecution">The delegate to the code that will be executed by the client</param>
        /// <param name="serverExecution">The delegate to the code that will be executed by the server</param>
        /// <param name="applyServerResult">The delegate to the code that will be executed by all clients in the session when server returns</param>
        /// <param name="dataType">The Type of the data exchanged between the clients and server</param>
        /// <param name="dataTransferOptions">Tells how the command orders are transfered throught the network</param>
        protected void AddLocalAndServerCommand(Command.ClientCommand clientExecution,
                                             Command.ServerCommand serverExecution,
                                             Command.ApplyServerCommand applyServerResult, Type dataType,
                                             DataTransferOptions dataTransferOptions)
        {
            AddCommand(Command.CreateLocalAndServerCommand(clientExecution, serverExecution, applyServerResult, dataType, dataTransferOptions));
        }

        /// <summary>
        /// Adds a Command to this behavior that will be executed on the client and on the server
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="clientExecution">The delegate to the code that will be executed by the client</param>
        /// <param name="serverExecution">The delegate to the code that will be executed by the server</param>
        /// <param name="applyServerResult">The delegate to the code that will be executed by all clients in the session when server returns</param>
        /// <param name="dataType">The Type of the data exchanged between the clients and server</param>
        /// <param name="dataTransferOptions">Tells how the command orders are transfered throught the network</param>
        protected void AddLocalAndServerCommand(Condition condition, Command.ClientCommand clientExecution,
                                             Command.ServerCommand serverExecution,
                                             Command.ApplyServerCommand applyServerResult, Type dataType,
                                             DataTransferOptions dataTransferOptions)
        {
            AddCommand(Command.CreateLocalAndServerCommand(condition, clientExecution, serverExecution, applyServerResult, dataType, dataTransferOptions));
        }

        /// <summary>
        /// Adds a Command to this behavior that will be executed on the client and on the server
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="clientExecution">The delegate to the code that will be executed by the client</param>
        /// <param name="serverExecution">The delegate to the code that will be executed by the server</param>
        /// <param name="applyServerResult">The delegate to the code that will be executed by all clients in the session when server returns</param>
        /// <param name="dataType">The Type of the data exchanged between the clients and server</param>
        /// <param name="dataTransferOptions">Tells how the command orders are transfered throught the network</param>
        /// <param name="frequency"></param>
        protected void AddLocalAndServerCommand(Condition condition, Command.ClientCommand clientExecution,
                                             Command.ServerCommand serverExecution,
                                             Command.ApplyServerCommand applyServerResult, Type dataType,
                                             DataTransferOptions dataTransferOptions, ExecutionFrequency frequency)
        {
            AddCommand(Command.CreateLocalAndServerCommand(condition, clientExecution, serverExecution, applyServerResult, dataType, dataTransferOptions, frequency));
        }

        /// <summary>
        /// Adds a Command to this behavior that will be executed solely on the server and returned to all clients in the session
        /// </summary>
        /// <param name="serverExecution">The delegate to the code that will be executed by the server</param>
        protected void AddServerCommand(Command.ServerCommand serverExecution)
        {
            AddCommand(Command.CreateServerCommand(serverExecution));
        }

        /// <summary>
        /// Adds a Command to this behavior that will be executed solely on the server and returned to all clients in the session
        /// </summary>
        /// <param name="serverExecution">The delegate to the code that will be executed by the server</param>
        protected void AddServerCommand(Command.ServerCommand serverExecution, ExecutionFrequency frequency)
        {
            AddCommand(Command.CreateServerCommand(serverExecution, frequency));
        }

        /// <summary>
        /// Adds a Command to this behavior that will be executed solely on the server and returned to all clients in the session
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="serverExecution">The delegate to the code that will be executed by the server</param>
        protected void AddServerCommand(Condition condition, Command.ServerCommand serverExecution)
        {
            AddCommand(Command.CreateServerCommand(condition, serverExecution));
        }

        /// <summary>
        /// Adds a Command to this behavior that will be executed solely on the server and returned to all clients in the session
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="serverExecution">The delegate to the code that will be executed by the server</param>
        /// <param name="frequency"></param>
        protected void AddServerCommand(Condition condition, Command.ServerCommand serverExecution, ExecutionFrequency frequency)
        {
            AddCommand(Command.CreateServerCommand(condition, serverExecution, frequency));
        }

        /// <summary>
        /// Adds a Command to this behavior that will be executed solely on the server and returned to all clients in the session
        /// </summary>
        /// <param name="serverExecution">The delegate to the code that will be executed by the server</param>
        /// <param name="applyServerResult">The delegate to the code that will be executed by all clients in the session when server returns</param>
        /// <param name="dataType">The Type of the data exchanged between the clients and server</param>
        /// <param name="dataTransferOptions">Tells how the command orders are transfered throught the network</param>
        protected void AddServerCommand(Command.ServerCommand serverExecution,
                                     Command.ApplyServerCommand applyServerResult, Type dataType,
                                     DataTransferOptions dataTransferOptions)
        {
            AddCommand(Command.CreateServerCommand(serverExecution, applyServerResult, dataType, dataTransferOptions));
        }

        /// <summary>
        /// Adds a Command to this behavior that will be executed solely on the server and returned to all clients in the session
        /// </summary>
        /// <param name="serverExecution">The delegate to the code that will be executed by the server</param>
        /// <param name="applyServerResult">The delegate to the code that will be executed by all clients in the session when server returns</param>
        /// <param name="dataType">The Type of the data exchanged between the clients and server</param>
        /// <param name="dataTransferOptions">Tells how the command orders are transfered throught the network</param>
        protected void AddServerCommand(Command.ServerCommand serverExecution,
                                     Command.ApplyServerCommand applyServerResult, Type dataType,
                                     DataTransferOptions dataTransferOptions, ExecutionFrequency frequency)
        {
            AddCommand(Command.CreateServerCommand(serverExecution, applyServerResult, dataType, dataTransferOptions, frequency));
        }

        /// <summary>
        /// Adds a Command to this behavior that will be executed solely on the server and returned to all clients in the session
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="serverExecution">The delegate to the code that will be executed by the server</param>
        /// <param name="applyServerResult">The delegate to the code that will be executed by all clients in the session when server returns</param>
        /// <param name="dataType">The Type of the data exchanged between the clients and server</param>
        /// <param name="dataTransferOptions">Tells how the command orders are transfered throught the network</param>
        protected void AddServerCommand(Condition condition, Command.ServerCommand serverExecution,
                                     Command.ApplyServerCommand applyServerResult, Type dataType,
                                     DataTransferOptions dataTransferOptions)
        {
            AddCommand(Command.CreateServerCommand(condition, serverExecution, applyServerResult, dataType, dataTransferOptions));
        }

        /// <summary>
        /// Adds a Command to this behavior that will be executed solely on the server and returned to all clients in the session
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="serverExecution">The delegate to the code that will be executed by the server</param>
        /// <param name="applyServerResult">The delegate to the code that will be executed by all clients in the session when server returns</param>
        /// <param name="dataType">The Type of the data exchanged between the clients and server</param>
        /// <param name="dataTransferOptions">Tells how the command orders are transfered throught the network</param>
        /// <param name="frequency"></param>
        protected void AddServerCommand(Condition condition, Command.ServerCommand serverExecution,
                                     Command.ApplyServerCommand applyServerResult, Type dataType,
                                     DataTransferOptions dataTransferOptions, ExecutionFrequency frequency)
        {
            AddCommand(Command.CreateServerCommand(condition, serverExecution, applyServerResult, dataType, dataTransferOptions, frequency));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Conditions.Clear();
                Conditions = null;

                Commands.Clear();
                Commands = null;
            }
        }

        private bool CanProcess()
        {
            return Conditions.Aggregate(true, (current, condition) => current && condition.Invoke());
        }

        private float _tick;

        void IProcess.Process(float elapsed)
        {
            _tick += elapsed;

            if(Enabled)
                UpdateCommandsTick(elapsed);

            if (_tick > 1f / _frequencyValue)
            {
                _tick = 0f;

                if(CanProcess())
                    Process(elapsed);
            }
        }

        private void UpdateCommandsTick(float elapsed)
        {
            for (int i = 0; i < Commands.Count; i++)
            {
                var command = Commands[i];
                command.UpdateTick(elapsed);
            }
        }

        protected virtual void Process(float elapsed)
        {
            for (int i = 0; i < Commands.Count; i++)
            {
                Command command = Commands[i];
                command.Process(elapsed);
            }
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Exécute les tâches définies par l'application associées à la libération ou à la redéfinition des ressources non managées.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }

    public interface IProcess
    {
        void Process(float elapsed);
    }
}