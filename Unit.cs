
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PackML_v0
{
    /// <summary>
    /// Class <c>Alarm</c> models an alarm.
    /// It contains the command to execute once the alarm is triggered (for example: "Abort")
    /// And the message associated to the alarm (for example: "Aborting: Error")
    /// </summary>
    public class Alarm
    {
        public Command Command { get; set; }
        public string Message { get; set; }

        public Alarm(Command command, string message)
        {
            Command = command;
            Message = message;
        }

        public override string ToString()
        {
            return this == null ? string.Empty : Message;
        }
    }
    /// <summary>
    /// Class <c>StackLight</c> models a stack light.
    /// It contains the description of the stack light (for example: "Blue light", or "Loud Horn")
    /// </summary>
    public class StackLight
    {
        public string Description { get; set; }

        public StackLight(string description)
        {
            Description = description;
        }

        public override string ToString()
        {
            return this == null ? string.Empty : Description;
        }
    }

    /// <summary>
    /// Class <c>CommandMachine</c> models a button that can interact with the machine. The button can be a physical button or a button on a GUI.
    /// It contains the name of the button (for example: "Start/Stop")
    /// And it contains the commands associated with the button (for example: {Start, Stop})
    /// </summary>
    public class CommandMachine
    {
        public string CommandMachineName { get; set; }
        public List<Command> Commands { get; set; }

        public CommandMachine(string commandMachineName, List<Command> commands)
        {
            CommandMachineName = commandMachineName;
            Commands = commands;
        }
    }

    /// <summary>
    /// Class <c>Unit</c> models a machine.
    /// This machine has an ID, a name, a state machine, and a state
    /// It may or may not contain alarms, stacklights and/or buttons
    /// </summary>
    public class Unit : PackMLStateModel
    {
        private readonly DBManager db = new DBManager();

        private readonly Dictionary<int, Alarm> alarms;

        // Represents the state of the alarms (on/off).
        private readonly Dictionary<Alarm, bool> dictAlarmState;

        private readonly Dictionary<int, StackLight> stackLights;

        // Represents the state of the stacklights (on/off).
        private readonly Dictionary<StackLight, bool> dictStackLightState;

        private readonly Dictionary<int, CommandMachine> commandsMachine;

        private static int Id = 1;
        public int MachineID { get; private set; }

        private string MachineName { get; set; }

        public Unit(State InitialState) : base(InitialState)
        {
            alarms = new Dictionary<int, Alarm>();
            dictAlarmState = new Dictionary<Alarm, bool>();

            stackLights = new Dictionary<int, StackLight>();
            dictStackLightState = new Dictionary<StackLight, bool>();

            commandsMachine = new Dictionary<int, CommandMachine>();
            MachineID = Id++;
            MachineName = "Machine";
        }

        public static void ResetId()
        {
            Id = 1;
        }

        public void SetMachineName(string machineName)
        {
            MachineName = machineName;
        }

        public string GetMachineName()
        {
            return MachineName;
        }

        /// <summary>
        /// Applies a command to a State Machine. If a command is not applicable to a State Machine in it's current state, do nothing.
        /// </summary>
        /// <param name="command">The command to apply</param>
        /// <returns>Returns true if successfuly applied the command. False otherwise.</returns>
        public bool SafeMoveNext(Command command)
        {
            StateTransition transition = new StateTransition(CurrentState, command);
            if (GetTransitions().TryGetValue(transition, out _))
            {
                _ = MoveNext(command);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Execute a process to the machine. The machine will execute the commands associated with the process one by one.
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public async Task ExecuteProcess(Process process)
        {
            int i = 0;
            while (i < process.GetCommands().Count)
            {
                if (IsCommandAvailable(Command.StateCompleted))
                {
                    await Task.Delay(process.GetSCTime());
                    _ = SafeMoveNext(Command.StateCompleted);
                }
                else
                {
                    await Task.Delay(process.GetCommandTime());
                    _ = SafeMoveNext(process.GetCommands()[i]);
                    i++;
                }
                if (i == process.GetCommands().Count)
                {
                    await Task.Delay(process.GetSCTime());
                    _ = SafeMoveNext(Command.StateCompleted);

                }
            }
        }

        /// <summary>
        /// Add an alarm to the machine. If the id of the alarm already exists, replace the existing one.
        /// </summary>
        /// <param name="alarmId">The id of the alarm to add or to replace</param>
        /// <param name="command">The command to apply to the machine if the alarm is triggered.</param>
        /// <param name="message">The message associated with the alarm</param>
        public void AddAlarm(int alarmId, Command command, string message)
        {
            Alarm alarm = new Alarm(command, message);
            alarms[alarmId] = alarm;
        }

        /// <summary>
        /// Remove an alarm from the machine.
        /// </summary>
        /// <param name="alarmId">The id of the alarm to remove.</param>
        public void RemoveAlarm(int alarmId)
        {
            if (alarms.ContainsKey(alarmId))
            {
                _ = alarms.Remove(alarmId);
            }
        }

        /// <summary>
        /// Returns the alarm with a given id
        /// </summary>
        /// <param name="alarmId">The id of the alarm to return</param>
        /// <returns></returns>
        public Alarm GetAlarm(int alarmId)
        {
            return alarms[alarmId];
        }

        /// <summary>
        /// Remove all the alarms from the machine.
        /// </summary>
        public void RemoveAllAlarms()
        {
            alarms.Clear();
            dictAlarmState.Clear();
        }

        /// <summary>
        /// Read a well formated .csv file and add the alarms to the machine. See "_Template_Alarms.csv" in the Data folder to see how to correctly format a .csv file
        /// </summary>
        /// <param name="csvFileName">The name of the .csv file. You need to include the full name, extension included. (for example "alarmfile.csv")</param>
        /// <param name="delimiter">The delimiter of the .csv file. Usually a comma</param>
        public void AddAlarmsFromCSV(string csvFileName, char delimiter)
        {
            string path = @"Data/" + csvFileName;
            List<int> alarmsID = new List<int>();
            List<int> commands = new List<int>();
            List<string> messages = new List<string>();
            int lineNumber = 1;
            using (StreamReader reader = new StreamReader(path))
            {

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line != null ? line.Split(delimiter) : Array.Empty<string>();
                    if (lineNumber != 1)
                    {
                        alarmsID.Add(int.Parse(values[0]));
                        commands.Add(int.Parse(values[1]));
                        messages.Add(values[2]);
                    }
                    lineNumber++;
                }
            }
            for (int i = 0; i < alarmsID.Count; i++)
            {
                AddAlarm(alarmsID[i], (Command)commands[i], messages[i]);
            }
        }

        /// <summary>
        /// Triggers an alarm. This will change the state of the alarm to ON, try to change the state of the machine, and return the message of the alarm
        /// </summary>
        /// <param name="alarmId">ID of the alarm to trigger</param>
        /// <returns>Returns the message of the alarm</returns>
        /// <exception cref="Exception">Throws when the given id does not have an associated alarm</exception>
        public string TriggerAlarm(int alarmId)
        {
            if (alarms.TryGetValue(alarmId, out Alarm alarm))
            {
                dictAlarmState[alarm] = true;
                _ = SafeMoveNext(alarm.Command);

                return alarm.Message;
            }
            else
            {
                throw new Exception("Can't trigger alarm, alarm with ID: " + alarmId + ", not found");
            }
        }

        /// <summary>
        /// Sets the state of the alarm to OFF
        /// </summary>
        /// <param name="alarmId">ID of the alarm to turn off</param>
        /// <exception cref="Exception">Throws when the given id does not have an associated alarm</exception>
        public void UntriggerAlarm(int alarmId)
        {
            _ = alarms.TryGetValue(alarmId, out Alarm alarm)
                ? dictAlarmState.Remove(alarm)
                : throw new Exception("Can't stop alarm, alarm with ID: " + alarmId + ", not found");
        }

        public List<Alarm> GetTriggeredAlarms()
        {
            List<Alarm> triggeredAlarms = new List<Alarm>();
            foreach (Alarm alarm in dictAlarmState.Keys)
            {
                if (dictAlarmState[alarm] == true)
                {
                    triggeredAlarms.Add(alarm);
                }
            }
            return triggeredAlarms;
        }


        public bool IsAlarmOn(Alarm alarm)
        {
            return GetTriggeredAlarms().Contains(alarm);
        }

        public Dictionary<int, Alarm> GetAllAlarms()
        {
            return alarms;
        }

        /// <summary>
        /// Add a stack light to the machine. if the id of the stack light already exists, replace the existing one.
        /// </summary>
        /// <param name="stackLightId">The id of the stack light to add or to replace</param>
        /// <param name="description">The description associated with the alarm</param>
        public void AddStackLight(int stackLightId, string description)
        {
            StackLight stacklight = new StackLight(description);
            stackLights[stackLightId] = stacklight;
        }

        /// <summary>
        /// Remove a stack light from the machine
        /// </summary>
        /// <param name="stacklightId">The id of the stack light to remove</param>
        public void RemoveStackLight(int stacklightId)
        {
            if (stackLights.ContainsKey(stacklightId))
            {
                _ = stackLights.Remove(stacklightId);
            }
        }

        /// <summary>
        /// Remove all the stack lights from the machine
        /// </summary>
        public void RemoveAllStackLight()
        {
            stackLights.Clear();
            dictStackLightState.Clear();
        }

        /// <summary>
        /// Returns the stack light with a given id
        /// </summary>
        /// <param name="stacklightId">The id of the stack light to return</param>
        /// <returns></returns>
        public StackLight GetStackLight(int stacklightId)
        {
            return stackLights[stacklightId];
        }

        /// <summary>
        /// Read a well formated .csv file and add the stack lights to the machine. See "_Template_Stacklights.csv" in the Data folder to see how to correctly format a .csv file
        /// </summary>
        /// <param name="csvFileName">The name of the .csv file. You need to include the full name, extension included. (for example "stacklightfile.csv")</param>
        /// <param name="delimiter">The delimiter of the .csv file. Usually a comma</param>
        public void AddStackLightsFromCSV(string csvFileName, char delimiter)
        {
            string path = @"Data/" + csvFileName;
            List<int> stacklightID = new List<int>();
            List<string> description = new List<string>();
            int lineNumber = 1;
            using (StreamReader reader = new StreamReader(path))
            {

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line != null ? line.Split(delimiter) : Array.Empty<string>();
                    if (lineNumber != 1)
                    {
                        stacklightID.Add(int.Parse(values[0]));
                        description.Add(values[1]);
                    }
                    lineNumber++;
                }
            }
            for (int i = 0; i < stacklightID.Count; i++)
            {
                AddStackLight(stacklightID[i], description[i]);
            }
        }

        /// <summary>
        /// Triggers a stack light. This will change the state of the stack light to ON and return the description of the stack light.
        /// </summary>
        /// <param name="stackLightId">The ID of the stack light to trigger</param>
        /// <returns>Returns the description of the stack light</returns>
        /// <exception cref="Exception">Throws when the given id does not have an associated stack light</exception>
        public string TriggerStackLight(int stackLightId)
        {
            if (stackLights.TryGetValue(stackLightId, out StackLight stackLight))
            {
                dictStackLightState[stackLight] = true;
                return stackLight.Description;
            }
            else
            {
                throw new Exception("Can't trigger stack light, stack Light with ID: " + stackLightId + ", not found");
            }
        }

        /// <summary>
        /// Sets the state of the stack light to OFF
        /// </summary>
        /// <param name="stackLightId">ID of the stack light to turn off</param>
        /// <exception cref="Exception">Throws when the given id ddoes not have an associated stack light</exception>
        public void UntriggerStackLight(int stackLightId)
        {
            _ = stackLights.TryGetValue(stackLightId, out StackLight stackLight)
                ? dictStackLightState.Remove(stackLight)
                : throw new Exception("Can't stop stack light, stack light with ID: " + stackLightId + ", not found");
        }

        public List<StackLight> GetTriggeredStackLights()
        {
            List<StackLight> triggeredStackLights = new List<StackLight>();
            foreach (StackLight stackLight in dictStackLightState.Keys)
            {
                if (dictStackLightState[stackLight] == true)
                {
                    triggeredStackLights.Add(stackLight);
                }
            }
            return triggeredStackLights;
        }

        public Dictionary<int, StackLight> GetAllStackLights()
        {
            return stackLights;
        }

        public int? GetStackLightId(StackLight stackLight)
        {
            foreach (int id in stackLights.Keys)
            {
                if (stackLights[id] == stackLight)
                {
                    return id;
                }
            }
            return null;
        }

        public bool IsStackLightOn(StackLight stacklight)
        {
            return GetTriggeredStackLights().Contains(stacklight);
        }

        /// <summary>
        /// Add a button to the machine. If the id of the button already exists, replace the existing one.
        /// </summary>
        /// <param name="commandID">The id of the button to add or to replace</param>
        /// <param name="commandName">The name of the button</param>
        /// <param name="commands">The commands associated with the button</param>
        public void AddCommandMachine(int commandID, string commandName, List<Command> commands)
        {
            CommandMachine commandMachine = new CommandMachine(commandName, commands);
            commandsMachine[commandID] = commandMachine;
        }

        /// <summary>
        /// Remove a button from the machine.
        /// </summary>
        /// <param name="commandID">The id of the button to remove.</param>
        public void RemoveCommandMachine(int commandID)
        {
            if (commandsMachine.ContainsKey(commandID))
            {
                _ = commandsMachine.Remove(commandID);
            }
        }

        public void RemoveAllCommandMachine()
        {
            commandsMachine.Clear();
        }

        /// <summary>
        /// Returns the button with a given id
        /// </summary>
        /// <param name="commandID">The id of the button to return.</param>
        /// <returns></returns>
        public CommandMachine GetCommandMachine(int commandID)
        {
            return commandsMachine[commandID];
        }

        /// <summary>
        /// Read a well formated .csv file and add the buttons to the machine. See "_Template_Commands.csv" in the Data folder to see how to correctly format a .csv file
        /// </summary>
        /// <param name="csvFileName">The name of the .csv file. You need to include the full name, extension included. (for example "buttonfile.csv")</param>
        /// <param name="delimiter">The delimiter of the .csv file. Usually a comma</param>
        public void AddCommandsMachineFromCSV(string csvFileName, char delimiter)
        {

            string path = @"Data/" + csvFileName;
            List<int> commandID = new List<int>();
            List<string> commandName = new List<string>();

            List<Command> actionsOfACommand = new List<Command>();
            List<List<Command>> actions = new List<List<Command>>();

            using (StreamReader reader = new StreamReader(path))
            {
                Stack<string> lines = new Stack<string>();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Push(line);
                }

                while (lines.Count > 1)
                {
                    string currentLine = lines.Pop();
                    string[] value = currentLine.Split(delimiter);
                    if (value[0] == "")
                    {
                        actionsOfACommand.Add((Command)int.Parse(value[2]));
                    }
                    else
                    {
                        actionsOfACommand.Add((Command)int.Parse(value[2]));
                        actionsOfACommand.Reverse();
                        commandID.Add(int.Parse(value[0]));
                        commandName.Add(value[1]);
                        actions.Add(actionsOfACommand);

                        actionsOfACommand = new List<Command>();
                    }
                }
            }

            for (int i = commandID.Count - 1; i >= 0; i--)
            {
                AddCommandMachine(commandID[i], commandName[i], actions[i]);
            }
        }

        /// <summary>
        /// Triggers a button.
        /// </summary>
        /// <param name="commandID">ID of the button to trigger</param>
        public void TriggerCommandMachine(int commandID)
        {
            foreach (Command command in commandsMachine[commandID].Commands)
            {
                if (SafeMoveNext(command))
                {
                    _ = SafeMoveNext(command);
                    break;
                }
            }
        }

        public Dictionary<int, CommandMachine> GetAllCommandsMachine()
        {
            return commandsMachine;
        }

        public string GetCommandMachineName(int commandID)
        {
            return commandsMachine[commandID].CommandMachineName;
        }

        /// <summary>
        /// Save the data of the machine to a .csv file.
        /// </summary>
        /// <param name="csvFileName">The name of the csv file to save as. (for example "data.csv")</param>
        /// <param name="delimiter">The delimiter of the .csv file. Usually a comma</param>
        public void SaveDataToCSV(string csvFileName, char delimiter)
        {
            //The path of the file. Located in the Data folder.
            string path = @"Data/" + csvFileName;

            using (StreamWriter writer = new StreamWriter(path))
            {
                //Will write the first few lines containing the name of the machine and the state of the machine.
                writer.WriteLine("Machine name" + delimiter + MachineName);
                writer.WriteLine("State" + delimiter + CurrentState);
                writer.WriteLine("");

                // Write the alarm ID on one single line
                writer.Write("AlarmsID" + delimiter);
                foreach (int key in alarms.Keys)
                {
                    writer.Write(key);
                    writer.Write(delimiter);
                }

                // Write the command of the alarms on one single line
                writer.WriteLine("");
                writer.Write("Command" + delimiter);
                foreach (int key in alarms.Keys)
                {
                    writer.Write(alarms[key].Command.ToString());
                    writer.Write(delimiter);
                }

                // Write the message of the alamrs on one single line
                writer.WriteLine("");
                writer.Write("Message" + delimiter);
                foreach (int key in alarms.Keys)
                {
                    writer.Write(alarms[key].Message.ToString());
                    writer.Write(delimiter);
                }
                
                // Write the state of the alarm on one single line
                writer.WriteLine("");
                writer.Write("AlarmState" + delimiter);
                foreach (Alarm alarm in alarms.Values)
                {
                    if (IsAlarmOn(alarm))
                    {
                        writer.Write("TRUE");
                    }
                    else
                    {
                        writer.Write("FALSE");
                    }
                    writer.Write(delimiter);
                }

                // Write the ID of the stack lights on one single line
                writer.WriteLine("");
                writer.WriteLine("");
                writer.Write("StackLightID" + delimiter);
                foreach (int key in stackLights.Keys)
                {
                    writer.Write(key);
                    writer.Write(delimiter);
                }

                // Write the description of the stack lights on one single line
                writer.WriteLine("");
                writer.Write("Description" + delimiter);
                foreach (int key in stackLights.Keys)
                {
                    writer.Write(stackLights[key].Description.ToString());
                    writer.Write(delimiter);
                }

                // Write the state of the stack lights on one single line
                writer.WriteLine("");
                writer.Write("StackLightState" + delimiter);
                foreach (StackLight stacklight in stackLights.Values)
                {
                    if (IsStackLightOn(stacklight))
                    {
                        writer.Write("TRUE");
                    }
                    else
                    {
                        writer.Write("FALSE");
                    }
                    writer.Write(delimiter);
                }

                writer.WriteLine("");

                // Write the ID of the buttons on one single line
                writer.WriteLine("");
                writer.Write("CommandID" + delimiter);
                foreach (int key in commandsMachine.Keys)
                {
                    for (int i = 0; i < commandsMachine[key].Commands.Count; i++)
                    {
                        writer.Write(key);
                        writer.Write(delimiter);
                    }
                }

                // Write the name of the buttons on one single line
                writer.WriteLine("");
                writer.Write("CommandName" + delimiter);
                foreach (int key in commandsMachine.Keys)
                {
                    for (int i = 0; i < commandsMachine[key].Commands.Count; i++)
                    {
                        writer.Write(commandsMachine[key].CommandMachineName);
                        writer.Write(delimiter);
                    }
                }

                // Write the actions of the buttons on one single line
                writer.WriteLine("");
                writer.Write("Actions" + delimiter);
                foreach (int key in commandsMachine.Keys)
                {
                    foreach (Command c in commandsMachine[key].Commands)
                    {
                        writer.Write(c);
                        writer.Write(delimiter);
                    }
                }
            }
        }

        /// <summary>
        /// Load the data from a .csv file to the machine. See "__Template_Machine.csv" in the Data folder to see how to format the file correctly.
        /// </summary>
        /// <param name="csvFileName">The name of the .csv file. You need to include the full name, extension included. (for example "data.csv")</param>
        /// <param name="delimiter">The delimiter of the .csv file. Usually a comma</param>
        public void ReadDataFromCSV(string csvFileName, char delimiter)
        {
            string path = @"Data/" + csvFileName;

            List<int> alarmsID = new List<int>();
            List<Command> commands = new List<Command>();
            List<string> messages = new List<string>();

            List<int> stacklightID = new List<int>();
            List<string> description = new List<string>();

            List<int> commandID = new List<int>();
            List<string> commandName = new List<string>();
            List<Command> actionsOfACommand = new List<Command>();
            List<List<Command>> actions = new List<List<Command>>();

            Dictionary<int, bool> alarmsState = new Dictionary<int, bool>();
            Dictionary<int, bool> stackLightsState = new Dictionary<int, bool>();

            State stateMachine = new State();

            int numberOfLine = 0;

            Dictionary<int, int> howManyActionsInThisID = new Dictionary<int, int>();
            int actionsNum = 0;


            using (StreamReader reader = new StreamReader(path))
            {

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line != null ? line.Split(delimiter) : Array.Empty<string>();
                    numberOfLine++;
                    
                    // Read the number of the line given.
                    switch (numberOfLine)
                    {
                        case 1:
                            SetMachineName(values[1]);
                            break;
                        case 2:
                            stateMachine = (State)Enum.Parse(typeof(State), values[1]);
                            break;
                        case 4:
                            for (int i = 1; i < values.Length; i++)
                            {
                                if (values[i] != "")
                                {
                                    alarmsID.Add(int.Parse(values[i]));
                                }
                            }
                            break;
                        case 5:
                            for (int i = 1; i < values.Length; i++)
                            {
                                if (values[i] != "")
                                {
                                    commands.Add((Command)Enum.Parse(typeof(Command), values[i]));
                                }
                            }
                            break;
                        case 6:
                            for (int i = 1; i < values.Length; i++)
                            {
                                if (values[i] != "")
                                {
                                    messages.Add(values[i]);
                                }
                            }
                            break;
                        case 7:
                            for (int i = 1; i < values.Length; i++)
                            {
                                if (values[i] != "")
                                {
                                    alarmsState[alarmsID[i - 1]] = bool.Parse(values[i]);
                                }
                            }
                            break;
                        case 9:
                            for (int i = 1; i < values.Length; i++)
                            {
                                if (values[i] != "")
                                {
                                    stacklightID.Add(int.Parse(values[i]));
                                }
                            }
                            break;
                        case 10:
                            for (int i = 1; i < values.Length; i++)
                            {
                                if (values[i] != "")
                                {
                                    description.Add(values[i]);
                                }
                            }
                            break;
                        case 11:
                            for (int i = 1; i < values.Length; i++)
                            {
                                if (values[i] != "")
                                {
                                    stackLightsState[stacklightID[i - 1]] = bool.Parse(values[i]);
                                }
                            }
                            break;

                        case 13:
                            for (int i = 1; i < values.Length; i++)
                            {

                                if (values[i] != values[i - 1])
                                {
                                    actionsNum = 0;
                                    if (values[i] != "")
                                    {
                                        commandID.Add(int.Parse(values[i]));
                                    }
                                }
                                else
                                {
                                    actionsNum++;
                                }
                                if (values[i] != "")
                                {
                                    howManyActionsInThisID[int.Parse(values[i])] = actionsNum + 1;
                                }
                            }
                            break;

                        case 14:
                            for (int i = 1; i < values.Length; i++)
                            {
                                if (values[i] != values[i - 1])
                                {
                                    if (values[i] != "")
                                    {
                                        commandName.Add(values[i]);
                                    }
                                }
                            }
                            break;

                        case 15:
                            int j = 1;
                            foreach (int id in commandID)
                            {
                                for (int i = 1; i <= howManyActionsInThisID[id]; i++)
                                {

                                    if (j < values.Length && values[j] != "")
                                    {
                                        actionsOfACommand.Add((Command)Enum.Parse(typeof(Command), values[j]));
                                    }
                                    j++;
                                }
                                actions.Add(actionsOfACommand);
                                actionsOfACommand = new List<Command>();
                            }
                            break;

                    }
                }
            }

            // Remove all specifications from the machine
            RemoveAllAlarms();
            RemoveAllStackLight();
            RemoveAllCommandMachine();

            // Add the new specifiations to the machine
            for (int i = 0; i < alarmsID.Count; i++)
            {
                AddAlarm(alarmsID[i], commands[i], messages[i]);
            }

            for (int i = 0; i < stacklightID.Count; i++)
            {
                AddStackLight(stacklightID[i], description[i]);
            }

            for (int i = 0; i < commandID.Count; i++)
            {
                AddCommandMachine(commandID[i], commandName[i], actions[i]);
            }

            foreach (int id in alarmsState.Keys)
            {
                if (alarmsState[id])
                {
                    _ = TriggerAlarm(id);
                }
            }

            foreach (int id in stackLightsState.Keys)
            {
                if (stackLightsState[id])
                {
                    _ = TriggerStackLight(id);
                }
            }

            SetState(stateMachine);

        }

        /// <summary>
        /// Save the data of the machine to a database file. (This may be broken)
        /// </summary>
        public void SaveDataToDB()
        {

            List<string> idAlarm = db.SelectMultipleCommand("SELECT idAlarm FROM Alarms WHERE idMachine = " + MachineID, "idAlarm");
            List<string> idButton = db.SelectMultipleCommand("SELECT idButton FROM Buttons WHERE idMachine = " + MachineID, "idButton");
            List<string> idStackLight = db.SelectMultipleCommand("SELECT idStackLight FROM StackLights WHERE idMachine = " + MachineID, "idStackLight");
            db.DeleteMachine(this.MachineID);
            if (idAlarm.Count > 0)
            {
                db.AddMachine(this, idAlarm, idButton, idStackLight);
            }
            else
            {
                db.AddMachine(this);
            }
        }

        /// <summary>
        /// Load the data from the database file into the machine. (This is broken)
        /// </summary>
        public void ReadDataFromDB()
        {
            SetMachineName(db.SelectSingleCommand("SELECT NameMachine FROM Machines WHERE idMachine = " + MachineID));

            List<string> idAlarmMachineDB = db.SelectMultipleCommand("SELECT idAlarmMachine FROM Alarms WHERE idMachine = " + MachineID, "idAlarmMachine");
            List<string> messageAlarmDB = db.SelectMultipleCommand("SELECT MessageAlarm FROM Alarms WHERE idMachine = " + MachineID, "MessageAlarm");
            List<string> isAlarmTriggeredDB = db.SelectMultipleCommand("SELECT IsAlarmTriggered FROM Alarms WHERE idMachine = " + MachineID, "isAlarmTriggered");
            List<string> idCommandDB = db.SelectMultipleCommand("SELECT idCommand FROM Alarms WHERE idMachine = " + MachineID, "idCommand");

            RemoveAllAlarms();

            for (int i = 0; i < idAlarmMachineDB.Count; i++)
            {
                AddAlarm(int.Parse(idAlarmMachineDB[i]), (Command)int.Parse(idCommandDB[i]), messageAlarmDB[i]);
                if (int.Parse(isAlarmTriggeredDB[i]) == 1)
                {
                    _ = TriggerAlarm(int.Parse(idAlarmMachineDB[i]));
                }
            }

            List<string> idStackLightDB = db.SelectMultipleCommand("SELECT idStackLightMachine FROM StackLights WHERE idMachine = " + MachineID, "idStackLightMachine");
            List<string> stackLightDescriptionDB = db.SelectMultipleCommand("SELECT StackLightDescription FROM StackLights WHERE idMachine = " + MachineID, "StackLightDescription");
            List<string> isStackLightTriggeredDB = db.SelectMultipleCommand("SELECT IsStackLightTriggered FROM StackLights WHERE idMachine = " + MachineID, "IsStackLightTriggered");

            RemoveAllStackLight();

            for (int i = 0; i < idStackLightDB.Count; i++)
            {
                AddStackLight(int.Parse(idStackLightDB[i]), stackLightDescriptionDB[i]);
                if (int.Parse(isStackLightTriggeredDB[i]) == 1)
                {
                    _ = TriggerStackLight(int.Parse(idStackLightDB[i]));
                }
            }

            List<string> idButtonsMachineDB = db.SelectMultipleCommand("SELECT idButtonsMachine FROM Buttons WHERE idMachine = " + MachineID, "idButtonsMachine");
            List<string> buttonNameDB = db.SelectMultipleCommand("SELECT ButtonName FROM Buttons WHERE idMachine = " + MachineID, "ButtonName");

            List<string> idButtonDB = db.SelectMultipleCommand("SELECT Triggers.idButton FROM Triggers INNER JOIN Buttons WHERE Triggers.idButton = Buttons.idButton AND Buttons.idMachine =  " + MachineID, "idButton");
            List<string> idCommandTriggerDB = db.SelectMultipleCommand("SELECT Triggers.idCommand FROM Triggers INNER JOIN Buttons WHERE Triggers.idButton = Buttons.idButton AND Buttons.idMachine =  " + MachineID, "idCommand");

            List<Command> commandDB = new List<Command>();
            List<List<Command>> commandsDB = new List<List<Command>>();

            for (int j = 0; j < idButtonDB.Count; j++)
            {
                commandDB.Add((Command)int.Parse(idCommandTriggerDB[j]));

                if (j + 1 >= idButtonDB.Count)
                {
                    commandsDB.Add(commandDB);
                    commandDB = new List<Command>();
                }
                else if (idButtonDB[j] != idButtonDB[j + 1])
                {
                    commandsDB.Add(commandDB);
                    commandDB = new List<Command>();
                }

            }

            RemoveAllCommandMachine();

            for (int i = 0; i < idButtonsMachineDB.Count; i++)
            {
                AddCommandMachine(int.Parse(idButtonsMachineDB[i]), buttonNameDB[i], commandsDB[i]);
            }

            string commandVal = db.SelectSingleCommand("SELECT idState FROM Machines WHERE idMachine = " + MachineID);
            if(commandVal == "")
            {
                Console.WriteLine("Error reading the Database: No state defined. Defaulting to IDLE state.");
                commandVal = "1";
            }
            SetState((State)int.Parse(commandVal));

            GC.Collect();

        }
    }
}
