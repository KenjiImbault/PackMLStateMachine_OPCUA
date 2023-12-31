﻿using System;
using System.Collections.Generic;

namespace PackML_v0
{
    /// <summary>
    /// Represent the different possible states of a machine. The values are taken from the PackML documentation.
    /// </summary>
    public enum State
    {
        IDLE = 1,
        STOPPED = 2,
        STARTING = 3,
        CLEARING = 4,
        SUSPENDED = 5,
        EXECUTE = 6,
        STOPPING = 7,
        ABORTING = 8,
        ABORTED = 9,
        HOLDING = 10,
        HELD = 11,
        UNHOLDING = 12,
        SUSPENDING = 13,
        UNSUSPENDING = 14,
        RESETTING = 15,
        COMPLETING = 16,
        COMPLETED = 17
    }

    /// <summary>
    /// Represent the different possible commands that can be applied to a machine. The values are taken from the PackML documentation.
    /// </summary>
    public enum Command
    {
        StateCompleted = 11,
        Reset = 1,
        Start = 2,
        Stop = 3,
        Hold = 4,
        Unhold = 5,
        Suspend = 6,
        Unsuspend = 7,
        Abort = 8,
        Clear = 9,
        Complete = 10
    }

    /// <summary>
    /// The class <c>PackMLStateModel</c> models a PackML State Machine.
    /// </summary>
    public class PackMLStateModel
    {
        /// <summary>
        /// The class <c>StateTransition</c> models a command that can be applied to a certain state.
        /// </summary>
        public class StateTransition
        {
            private readonly State CurrentState;
            private readonly Command Command;

            public StateTransition(State currentState, Command command)
            {
                CurrentState = currentState;
                Command = command;
            }

            public override int GetHashCode()
            {
                return 17 + (31 * CurrentState.GetHashCode()) + (31 * Command.GetHashCode());
            }

            public override bool Equals(object obj)
            {
                return obj is StateTransition other && CurrentState == other.CurrentState && Command == other.Command;
            }

        }

        private readonly Dictionary<StateTransition, State> Transitions;
        public State CurrentState { get; private set; }

        public PackMLStateModel(State InitialState)
        {
            CurrentState = InitialState;
            Transitions = new Dictionary<StateTransition, State>
            {
                {new StateTransition(State.STARTING, Command.StateCompleted), State.EXECUTE},
                {new StateTransition(State.CLEARING, Command.StateCompleted), State.STOPPED},
                {new StateTransition(State.STOPPING, Command.StateCompleted), State.STOPPED},
                {new StateTransition(State.ABORTING, Command.StateCompleted), State.ABORTED},
                {new StateTransition(State.HOLDING, Command.StateCompleted), State.HELD},
                {new StateTransition(State.UNHOLDING, Command.StateCompleted), State.EXECUTE},
                {new StateTransition(State.SUSPENDING, Command.StateCompleted), State.SUSPENDED},
                {new StateTransition(State.UNSUSPENDING, Command.StateCompleted), State.EXECUTE},
                {new StateTransition(State.RESETTING, Command.StateCompleted), State.IDLE},
                {new StateTransition(State.COMPLETING, Command.StateCompleted), State.COMPLETED},

                {new StateTransition(State.COMPLETED, Command.Reset), State.RESETTING },
                {new StateTransition(State.STOPPED, Command.Reset), State.RESETTING },

                {new StateTransition(State.IDLE, Command.Start), State.STARTING },

                {new StateTransition(State.IDLE, Command.Stop), State.STOPPING },
                {new StateTransition(State.STARTING, Command.Stop), State.STOPPING },
                {new StateTransition(State.SUSPENDED, Command.Stop), State.STOPPING },
                {new StateTransition(State.EXECUTE, Command.Stop), State.STOPPING },
                {new StateTransition(State.HOLDING, Command.Stop), State.STOPPING },
                {new StateTransition(State.HELD, Command.Stop), State.STOPPING },
                {new StateTransition(State.UNHOLDING, Command.Stop), State.STOPPING },
                {new StateTransition(State.SUSPENDING, Command.Stop), State.STOPPING },
                {new StateTransition(State.UNSUSPENDING, Command.Stop), State.STOPPING },
                {new StateTransition(State.RESETTING, Command.Stop), State.STOPPING },
                {new StateTransition(State.COMPLETING, Command.Stop), State.STOPPING },
                {new StateTransition(State.COMPLETED, Command.Stop), State.STOPPING },

                {new StateTransition(State.SUSPENDED, Command.Hold), State.HOLDING },
                {new StateTransition(State.EXECUTE, Command.Hold), State.HOLDING },

                {new StateTransition(State.HELD, Command.Unhold), State.UNHOLDING },

                {new StateTransition(State.EXECUTE, Command.Suspend), State.SUSPENDING },

                {new StateTransition(State.SUSPENDED, Command.Unsuspend), State.UNSUSPENDING },

                {new StateTransition(State.IDLE, Command.Abort), State.ABORTING },
                {new StateTransition(State.STARTING, Command.Abort), State.ABORTING },
                {new StateTransition(State.SUSPENDED, Command.Abort), State.ABORTING },
                {new StateTransition(State.EXECUTE, Command.Abort), State.ABORTING },
                {new StateTransition(State.HOLDING, Command.Abort), State.ABORTING },
                {new StateTransition(State.HELD, Command.Abort), State.ABORTING },
                {new StateTransition(State.UNHOLDING, Command.Abort), State.ABORTING },
                {new StateTransition(State.SUSPENDING, Command.Abort), State.ABORTING },
                {new StateTransition(State.UNSUSPENDING, Command.Abort), State.ABORTING },
                {new StateTransition(State.RESETTING, Command.Abort), State.ABORTING },
                {new StateTransition(State.COMPLETING, Command.Abort), State.ABORTING },
                {new StateTransition(State.COMPLETED, Command.Abort), State.ABORTING },

                {new StateTransition(State.ABORTED, Command.Clear), State.CLEARING },

                {new StateTransition(State.HELD, Command.Complete), State.COMPLETING },
                {new StateTransition(State.EXECUTE, Command.Complete), State.COMPLETING },
                {new StateTransition(State.SUSPENDED, Command.Complete), State.COMPLETING }
            };
        }

        public Dictionary<StateTransition, State> GetTransitions()
        {
            return Transitions;
        }

        public void SetState(State state)
        {
            CurrentState = state;
        }

        /// <summary>
        /// Returns what would be the next state if we applied the given command. Throws an exception if the command is not applicable.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public State GetNext(Command command)
        {
            StateTransition transition = new StateTransition(CurrentState, command);
            return Transitions.TryGetValue(transition, out State nextState)
                ? nextState
                : throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
        }

        /// <summary>
        /// Applies a command to a State Machine. If a command is not appliable to a State Machine in it's current state, throws an exception.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public State MoveNext(Command command)
        {
            CurrentState = GetNext(command);
            return CurrentState;
        }

        /// <summary>
        /// Get the available commands from the current state of the machine as a list.
        /// </summary>
        /// <returns></returns>
        public List<Command> GetAvailableCommands()
        {
            List<Command> commands = new List<Command>();
            foreach (int command in Enum.GetValues(typeof(Command)))
            {
                if (Transitions.ContainsKey(new StateTransition(CurrentState, (Command)command)))
                {
                    commands.Add((Command)command);
                }
            }
            return commands;
        }

        public bool IsCommandAvailable(Command command)
        {
            return GetAvailableCommands().Contains(command);
        }


    }
}
