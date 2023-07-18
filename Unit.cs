
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PackML_v0
{

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

    public class Unit : PackMLStateModel
    {
        private readonly DBManager db = new DBManager();

        private readonly Dictionary<int, Alarm> alarms;
        private readonly Dictionary<Alarm, bool> dictAlarmState;

        private readonly Dictionary<int, StackLight> stackLights;
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

        public void AddAlarm(int alarmId, Command command, string message)
        {
            Alarm alarm = new Alarm(command, message);
            alarms[alarmId] = alarm;
        }

        public void RemoveAlarm(int alarmId)
        {
            if (alarms.ContainsKey(alarmId))
            {
                _ = alarms.Remove(alarmId);
            }
        }

        public Alarm GetAlarm(int alarmId)
        {
            return alarms[alarmId];
        }

        public void RemoveAllAlarms()
        {
            alarms.Clear();
            dictAlarmState.Clear();
        }

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

        public void AddStackLight(int stackLightId, string description)
        {
            StackLight stacklight = new StackLight(description);
            stackLights[stackLightId] = stacklight;
        }

        public void RemoveStackLight(int stacklightId)
        {
            if (stackLights.ContainsKey(stacklightId))
            {
                _ = stackLights.Remove(stacklightId);
            }
        }

        public void RemoveAllStackLight()
        {
            stackLights.Clear();
            dictStackLightState.Clear();
        }

        public StackLight GetStackLight(int stacklightId)
        {
            return stackLights[stacklightId];
        }

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

        public void AddCommandMachine(int commandID, string commandName, List<Command> commands)
        {
            CommandMachine commandMachine = new CommandMachine(commandName, commands);
            commandsMachine[commandID] = commandMachine;
        }

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

        public CommandMachine GetCommandMachine(int commandID)
        {
            return commandsMachine[commandID];
        }

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

        public void SaveDataToCSV(string csvFileName, char delimiter)
        {

            string path = @"Data/" + csvFileName;

            using (StreamWriter writer = new StreamWriter(path))
            {



                writer.WriteLine("Machine name" + delimiter + MachineName);
                writer.WriteLine("State" + delimiter + CurrentState);
                writer.WriteLine("");

                writer.Write("AlarmsID" + delimiter);
                foreach (int key in alarms.Keys)
                {
                    writer.Write(key);
                    writer.Write(delimiter);
                }

                writer.WriteLine("");
                writer.Write("Command" + delimiter);
                foreach (int key in alarms.Keys)
                {
                    writer.Write(alarms[key].Command.ToString());
                    writer.Write(delimiter);
                }

                writer.WriteLine("");
                writer.Write("Message" + delimiter);
                foreach (int key in alarms.Keys)
                {
                    writer.Write(alarms[key].Message.ToString());
                    writer.Write(delimiter);
                }

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

                writer.WriteLine("");
                writer.WriteLine("");
                writer.Write("StackLightID" + delimiter);
                foreach (int key in stackLights.Keys)
                {
                    writer.Write(key);
                    writer.Write(delimiter);
                }

                writer.WriteLine("");
                writer.Write("Description" + delimiter);
                foreach (int key in stackLights.Keys)
                {
                    writer.Write(stackLights[key].Description.ToString());
                    writer.Write(delimiter);
                }

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

            RemoveAllAlarms();
            RemoveAllStackLight();
            RemoveAllCommandMachine();

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
