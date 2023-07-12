
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace PackML_v0
{
    public class DBManager
    {
        private readonly string connectionString = "Data Source=PackMLSavedDB.db";
        private readonly SQLiteConnection connection;

        public DBManager()
        {
            connection = new SQLiteConnection(connectionString);
        }

        public SQLiteConnection GetSQLiteConnection()
        { return connection; }
        public void ExecuteCommand(string command)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            using (SQLiteCommand commandToSend = new SQLiteCommand(command, connection))
            {
                _ = commandToSend.ExecuteNonQuery();
                connection.Close();
            }
        }

        public string SelectSingleCommand(string query)
        {
            object result;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    result = command.ExecuteScalar();
                }
                connection.Close();
            }
#pragma warning disable CS8603 // Possible null reference return.
            return Convert.ToString(result) != null ? Convert.ToString(result) : "";
#pragma warning restore CS8603 // Possible null reference return.


        }

        public List<string> SelectMultipleCommand(string query, string column)
        {
            List<string> strings = new List<string>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {

                connection.Open();

                using (SQLiteCommand fmd = connection.CreateCommand())
                {
                    fmd.CommandText = query;
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        strings.Add(Convert.ToString(r[column]));
                    }
                }

                connection.Close();
            }
            return strings;
        }

        public void CreateDB()
        {
            ExecuteCommand(@"CREATE TABLE MachineStates (
   idState INTEGER PRIMARY KEY,
   StateName VARCHAR(14),
   UNIQUE(StateName)
);

CREATE TABLE Commands (
   idCommand INTEGER PRIMARY KEY,
   CommandName VARCHAR(15),
   UNIQUE(CommandName)
);

CREATE TABLE Machines (
   idMachine INTEGER PRIMARY KEY,
   NameMachine VARCHAR(50),
   idState INTEGER NOT NULL,
   FOREIGN KEY(idState) REFERENCES MachineStates(idState)
);

CREATE TABLE Alarms (
   idAlarm INTEGER PRIMARY KEY AUTOINCREMENT,
   idAlarmMachine INTEGER NOT NULL,
   MessageAlarm VARCHAR(50),
   IsAlarmTriggered INTEGER NOT NULL,
   idCommand INTEGER NOT NULL,
   idMachine INTEGER,
   FOREIGN KEY(idCommand) REFERENCES Commands(idCommand),
   FOREIGN KEY(idMachine) REFERENCES Machines(idMachine)
   
);

CREATE TABLE Buttons (
   idButton INTEGER PRIMARY KEY AUTOINCREMENT,
   idButtonsMachine INTEGER NOT NULL,
   ButtonName VARCHAR(50),
   idMachine INTEGER,
   FOREIGN KEY(idMachine) REFERENCES Machines(idMachine)
);

CREATE TABLE StackLights (
   idStackLight INTEGER PRIMARY KEY AUTOINCREMENT,
   idStackLightMachine INTEGER NOT NULL,
   StackLightDescription VARCHAR(50),
   IsStackLightTriggered INTEGER NOT NULL,
   idMachine INTEGER,
   FOREIGN KEY(idMachine) REFERENCES Machines(idMachine)
);

CREATE TABLE Triggers (
   idButton INTEGER,
   idCommand INTEGER,
   PRIMARY KEY(idButton, idCommand),
   FOREIGN KEY(idButton) REFERENCES Buttons(idButton),
   FOREIGN KEY(idCommand) REFERENCES Commands(idCommand)
);");
        }

        public void RemoveDB()
        {
            ExecuteCommand(@"PRAGMA writable_schema = 1;
delete from sqlite_master where type in ('table', 'index', 'trigger');
PRAGMA writable_schema = 0;
VACUUM;");
        }

        public void InsertButtons(int idButtonsMachine, string name, int idMachine)
        {
            connection.Open();
            string insertCommand = @"INSERT INTO Buttons (idButtonsMachine, ButtonName, idMachine) VALUES (@idButtonsMachine, @ButtonName, @idMachine);";
            using (SQLiteCommand insert = new SQLiteCommand(insertCommand, connection))
            {
                _ = insert.Parameters.AddWithValue("@idButtonsMachine", idButtonsMachine);
                _ = insert.Parameters.AddWithValue("@ButtonName", name);
                _ = insert.Parameters.AddWithValue("@idMachine", idMachine);
                _ = insert.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void InsertButtons(int idButton, int idButtonsMachine, string name, int idMachine)
        {
            connection.Open();
            string insertCommand = @"INSERT INTO Buttons (idButton, idButtonsMachine, ButtonName, idMachine) VALUES (@idButton, @idButtonsMachine, @ButtonName, @idMachine);";
            using (SQLiteCommand insert = new SQLiteCommand(insertCommand, connection))
            {
                _ = insert.Parameters.AddWithValue("@idButton", idButton);
                _ = insert.Parameters.AddWithValue("@idButtonsMachine", idButtonsMachine);
                _ = insert.Parameters.AddWithValue("@ButtonName", name);
                _ = insert.Parameters.AddWithValue("@idMachine", idMachine);
                _ = insert.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void UpdateButtons(int idButtonsMachine, string name, int idMachine)
        {
            connection.Open();
            string updateCommand = "UPDATE Buttons SET idButtonsMachine=@idButtonsMachine, ButtonName=@ButtonName WHERE idMachine=@idMachine;";
            using (SQLiteCommand update = new SQLiteCommand(updateCommand, connection))
            {
                _ = update.Parameters.AddWithValue("@idButtonsMachine", idButtonsMachine);
                _ = update.Parameters.AddWithValue("@ButtonName", name);
                _ = update.Parameters.AddWithValue("@idMachine", idMachine);
                _ = update.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void InsertStackLights(int idStackLightMachine, string description, int triggered, int idMachine)
        {
            connection.Open();
            string insertCommand = @"INSERT INTO StackLights (idStackLightMachine, StackLightDescription, IsStackLightTriggered, idMachine) VALUES (@idStackLightMachine, @StackLightDescription, @IsStackLightTriggered, @idMachine);";
            using (SQLiteCommand insert = new SQLiteCommand(insertCommand, connection))
            {
                _ = insert.Parameters.AddWithValue("@idStackLightMachine", idStackLightMachine);
                _ = insert.Parameters.AddWithValue("@StackLightDescription", description);
                _ = insert.Parameters.AddWithValue("@IsStackLightTriggered", triggered);
                _ = insert.Parameters.AddWithValue("@idMachine", idMachine);
                _ = insert.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void InsertStackLights(int idStackLight, int idStackLightMachine, string description, int triggered, int idMachine)
        {
            connection.Open();
            string insertCommand = @"INSERT INTO StackLights (idStackLight, idStackLightMachine, StackLightDescription, IsStackLightTriggered, idMachine) VALUES (@idStackLight, @idStackLightMachine, @StackLightDescription, @IsStackLightTriggered, @idMachine);";
            using (SQLiteCommand insert = new SQLiteCommand(insertCommand, connection))
            {
                _ = insert.Parameters.AddWithValue("@idStackLight", idStackLight);
                _ = insert.Parameters.AddWithValue("@idStackLightMachine", idStackLightMachine);
                _ = insert.Parameters.AddWithValue("@StackLightDescription", description);
                _ = insert.Parameters.AddWithValue("@IsStackLightTriggered", triggered);
                _ = insert.Parameters.AddWithValue("@idMachine", idMachine);
                _ = insert.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void UpdateStackLights(int idStackLightMachine, string description, int triggered, int idMachine)
        {
            connection.Open();
            string updateCommand = "UPDATE StackLights SET idStackLightMachine=@idStackLightMachine, StackLightDescription=@StackLightDescription, IsStackLightTriggered=@IsStackLightTriggered WHERE idMachine=@idMachine;";
            using (SQLiteCommand update = new SQLiteCommand(updateCommand, connection))
            {
                _ = update.Parameters.AddWithValue("@idStackLightMachine", idStackLightMachine);
            _ = update.Parameters.AddWithValue("@StackLightDescription", description);
            _ = update.Parameters.AddWithValue("@IsStackLightTriggered", triggered);
            _ = update.Parameters.AddWithValue("@idMachine", idMachine);
            _ = update.ExecuteNonQuery();
            connection.Close();
            }
        }

        public void InsertMachineStates()
        {
            connection.Open();
            foreach (State state in Enum.GetValues(typeof(State)))
            {
                string insertCommand = @"INSERT INTO MachineStates (idState, StateName) VALUES (@idState, @StateName);";
                SQLiteCommand insert = new SQLiteCommand(insertCommand, connection);
                _ = insert.Parameters.AddWithValue("@idState", (int)state);
                _ = insert.Parameters.AddWithValue("@StateName", state.ToString());
                _ = insert.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void InsertCommands()
        {
            connection.Open();
            string insertCommand = @"INSERT INTO Commands (idCommand, CommandName) VALUES (@idCommand, @CommandName);";
            SQLiteCommand insert = new SQLiteCommand(insertCommand, connection);

            foreach (Command command in Enum.GetValues(typeof(Command)))
            {
                _ = insert.Parameters.AddWithValue("@idCommand", (int)command);
                _ = insert.Parameters.AddWithValue("@CommandName", command.ToString());
                _ = insert.ExecuteNonQuery();
            }

            _ = insert.Parameters.AddWithValue("@idCommand", 0);
            _ = insert.Parameters.AddWithValue("@CommandName", "NoCommand");
            _ = insert.ExecuteNonQuery();

            connection.Close();
        }

        public void InsertMachines(Unit machine)
        {
            connection.Open();
            string insertCommand = @"INSERT INTO Machines (idMachine, NameMachine, idState) VALUES (@idMachine, @NameMachine, @idState);";
            SQLiteCommand insert = new SQLiteCommand(insertCommand, connection);
            _ = insert.Parameters.AddWithValue("@idMachine", machine.machineID);
            _ = insert.Parameters.AddWithValue("@NameMachine", machine.GetMachineName());
            _ = insert.Parameters.AddWithValue("@idState", (int)machine.CurrentState);
            _ = insert.ExecuteNonQuery();
            connection.Close();
        }

        public void InsertMachines(int idMachine, string nameMachine, int idState)
        {
            connection.Open();
            string insertCommand = @"INSERT INTO Machines (idMachine, NameMachine, idState) VALUES (@idMachine, @NameMachine, @idState);";
            SQLiteCommand insert = new SQLiteCommand(insertCommand, connection);
            _ = insert.Parameters.AddWithValue("@idMachine", idMachine);
            _ = insert.Parameters.AddWithValue("@NameMachine", nameMachine);
            _ = insert.Parameters.AddWithValue("@idState", (int)idState);
            _ = insert.ExecuteNonQuery();
            connection.Close();
        }

        public void UpdateMachine(Unit machine, int idMachine)
        {
            connection.Open();
            string updateCommand = "UPDATE Machines SET NameMachine=@NameMachine, idState=@idState WHERE idMachine = @idMachine;";
            SQLiteCommand update = new SQLiteCommand(updateCommand, connection);
            _ = update.Parameters.AddWithValue("@NameMachine", machine.GetMachineName());
            _ = update.Parameters.AddWithValue("@idState", (int)machine.CurrentState);
            _ = update.Parameters.AddWithValue("@idMachine", idMachine);
            _ = update.ExecuteNonQuery();
            connection.Close();
        }

        public void InsertAlarms(int idAlarmMachine, string message, int triggered, int idCommand, int idMachine)
        {
            connection.Open();
            string insertCommand = @"INSERT INTO Alarms (idAlarmMachine, MessageAlarm, IsAlarmTriggered, idCommand, idMachine) VALUES (@idAlarmMachine, @MessageAlarm, @IsAlarmTriggered, @idCommand, @idMachine);";
            SQLiteCommand insert = new SQLiteCommand(insertCommand, connection);
            _ = insert.Parameters.AddWithValue("@idAlarmMachine", idAlarmMachine);
            _ = insert.Parameters.AddWithValue("@MessageAlarm", message);
            _ = insert.Parameters.AddWithValue("@IsAlarmTriggered", triggered);
            _ = insert.Parameters.AddWithValue("@idCommand", idCommand);
            _ = insert.Parameters.AddWithValue("@idMachine", idMachine);
            _ = insert.ExecuteNonQuery();
            connection.Close();
        }

        public void InsertAlarms(int idAlarm, int idAlarmMachine, string message, int triggered, int idCommand, int idMachine)
        {
            connection.Open();
            string insertCommand = @"INSERT INTO Alarms (idAlarm, idAlarmMachine, MessageAlarm, IsAlarmTriggered, idCommand, idMachine) VALUES (@idAlarm, @idAlarmMachine, @MessageAlarm, @IsAlarmTriggered, @idCommand, @idMachine);";
            SQLiteCommand insert = new SQLiteCommand(insertCommand, connection);
            _ = insert.Parameters.AddWithValue("@idAlarm", idAlarm);
            _ = insert.Parameters.AddWithValue("@idAlarmMachine", idAlarmMachine);
            _ = insert.Parameters.AddWithValue("@MessageAlarm", message);
            _ = insert.Parameters.AddWithValue("@IsAlarmTriggered", triggered);
            _ = insert.Parameters.AddWithValue("@idCommand", idCommand);
            _ = insert.Parameters.AddWithValue("@idMachine", idMachine);
            _ = insert.ExecuteNonQuery();
            connection.Close();
        }

        public void UpdateAlarms(int idAlarmMachine, string MessageAlarm, int IsAlarmTriggered, int idCommand, int idMachine)
        {
            connection.Open();
            string updateCommand = "UPDATE Alarms SET idAlarmMachine=@idAlarmMachine, MessageAlarm=@MessageAlarm, IsAlarmTriggered=@IsAlarmTriggered, idCommand=@idCommand WHERE idMachine=@idMachine;";
            SQLiteCommand update = new SQLiteCommand(updateCommand, connection);
            _ = update.Parameters.AddWithValue("@idAlarmMachine", idAlarmMachine);
            _ = update.Parameters.AddWithValue("@MessageAlarm", MessageAlarm);
            _ = update.Parameters.AddWithValue("@IsAlarmTriggered", IsAlarmTriggered);
            _ = update.Parameters.AddWithValue("@idCommand", idCommand);
            _ = update.Parameters.AddWithValue("@idMachine", idMachine);
            _ = update.ExecuteNonQuery();
            connection.Close();
        }

        public void InsertTriggers(int idButton, int idCommand)
        {
            connection.Open();
            string insertCommand = @"INSERT INTO Triggers (idButton, idCommand) VALUES (@idButton, @idCommand);";
            SQLiteCommand insert = new SQLiteCommand(insertCommand, connection);
            _ = insert.Parameters.AddWithValue("@idButton", idButton);
            _ = insert.Parameters.AddWithValue("@idCommand", idCommand);
            _ = insert.ExecuteNonQuery();

            connection.Close();
        }

        public void UpdateTriggers(int idButton, int idCommand, int oldIdButton, int oldIdCommand)
        {
            connection.Open();
            string updateCommand = "UPDATE Triggers SET idButton=@idButton, idCommand=@idCommand WHERE idButton=@oldIdButton AND idCommand=@oldIdCommand;";
            SQLiteCommand update = new SQLiteCommand(updateCommand, connection);
            _ = update.Parameters.AddWithValue("@idButton", idButton);
            _ = update.Parameters.AddWithValue("@idCommand", idCommand);
            _ = update.Parameters.AddWithValue("@oldIdButton", oldIdButton);
            _ = update.Parameters.AddWithValue("@oldIdCommand", oldIdCommand);
            _ = update.ExecuteNonQuery();
            connection.Close();
        }

        public void Initialize()
        {
            InsertMachineStates();
            InsertCommands();
        }

        public void AddMachine(Unit machine, List<string> idAlarm, List<string> idButton, List<string> idStackLight)
        {
            InsertMachines(machine);
            int idMachine;
            connection.Open();
            using (SQLiteCommand select = new SQLiteCommand(@"SELECT idMachine FROM Machines ORDER BY idMachine DESC LIMIT 1;", connection))
            {
                object result = select.ExecuteScalar();
                idMachine = Convert.ToInt32(result);
            }
            connection.Close();
            int k = 0;
            foreach (int key in machine.GetAllAlarms().Keys)
            {
                if (machine.IsAlarmOn(machine.GetAlarm(key)))
                {
                    InsertAlarms(int.Parse(idAlarm[k]), key, machine.GetAlarm(key).Message, 1, (int)machine.GetAlarm(key).Command, idMachine);
                }
                else
                {
                    InsertAlarms(int.Parse(idAlarm[k]), key, machine.GetAlarm(key).Message, 0, (int)machine.GetAlarm(key).Command, idMachine);
                }
                k++;
            }

            k = 0;
            foreach (int key in machine.GetAllStackLights().Keys)
            {
                if (machine.IsStackLightOn(machine.GetStackLight(key)))
                {
                    InsertStackLights(int.Parse(idStackLight[k]), key, machine.GetStackLight(key).Description, 1, idMachine);
                }
                else
                {
                    InsertStackLights(int.Parse(idStackLight[k]), key, machine.GetStackLight(key).Description, 0, idMachine);
                }
                k++;
            }

            k = 0;
            foreach (int key in machine.GetAllCommandsMachine().Keys)
            {
                InsertButtons(int.Parse(idButton[k]), key, machine.GetCommandMachine(key).CommandMachineName, idMachine);
                k++;
                foreach (Command command in machine.GetCommandMachine(key).Commands)
                {
                    InsertTriggers(key, (int)command);
                }
            }
        }


        public void AddMachine(Unit machine)
        {
            InsertMachines(machine);
            int idMachine;
            connection.Open();
            using (SQLiteCommand select = new SQLiteCommand(@"SELECT idMachine FROM Machines ORDER BY idMachine DESC LIMIT 1;", connection))
            {
                object result = select.ExecuteScalar();
                idMachine = Convert.ToInt32(result);
            }
            connection.Close();
            foreach (int key in machine.GetAllAlarms().Keys)
            {
                if (machine.IsAlarmOn(machine.GetAlarm(key)))
                {
                    InsertAlarms(key, machine.GetAlarm(key).Message, 1, (int)machine.GetAlarm(key).Command, idMachine);
                }
                else
                {
                    InsertAlarms(key, machine.GetAlarm(key).Message, 0, (int)machine.GetAlarm(key).Command, idMachine);
                }
            }

            foreach (int key in machine.GetAllStackLights().Keys)
            {
                if (machine.IsStackLightOn(machine.GetStackLight(key)))
                {
                    InsertStackLights(key, machine.GetStackLight(key).Description, 1, idMachine);
                }
                else
                {
                    InsertStackLights(key, machine.GetStackLight(key).Description, 0, idMachine);
                }
            }

            List<Command> commandList = new List<Command>();
            int k = 0;
            foreach (int key in machine.GetAllCommandsMachine().Keys)
            {
                commandList.Add(machine.GetCommandMachine(key).Commands[k]);
                InsertButtons(key, machine.GetCommandMachine(key).CommandMachineName, idMachine);
                k++;
            }

            List<string> idButtonInButtons = SelectMultipleCommand("SELECT idButton FROM Buttons", "idButton");

            foreach (string id in idButtonInButtons)
            {
                foreach (Command c in commandList)
                {
                    InsertTriggers(int.Parse(id), (int)c);
                }
            }

        }

        public void DeleteMachine(int machineID)
        {
            ExecuteCommand(@"DELETE FROM Triggers
WHERE idButton IN (
  SELECT Triggers.idButton
  FROM Triggers
  INNER JOIN Buttons ON Triggers.idButton = Buttons.idButton
  WHERE Buttons.idMachine = " + machineID + ");");

            ExecuteCommand("DELETE FROM Alarms WHERE idMachine = " + machineID);
            ExecuteCommand("DELETE FROM Buttons WHERE idMachine = " + machineID);
            ExecuteCommand("DELETE FROM StackLights WHERE idMachine = " + machineID);
            ExecuteCommand("DELETE FROM StackLights WHERE idMachine = " + machineID);
            ExecuteCommand("DELETE FROM Machines WHERE idMachine = " + machineID);
        }

        public int GetIdMachine(Unit machine)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();

            SQLiteCommand command = new SQLiteCommand("SELECT idMachine FROM Machines WHERE NameMachine = @MachineName;", connection);
            _ = command.Parameters.AddWithValue("@MachineName", machine.GetMachineName());

            object result = command.ExecuteScalar();

            connection.Close();
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : -1;
        }

        public int GetIDLastMachine()
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT idMachine FROM Machines ORDER BY idMachine DESC LIMIT 1;", connection);
            connection.Close();
            object result = command.ExecuteScalar();
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : -1;
        }

        public void WriteFromCSVToDB(string csvFileName, char delimiter)
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

            List<string> actionsList = new List<string>();

            List<bool> alarmsState = new List<bool>();
            List<bool> stackLightsState = new List<bool>();

            string machineName = "";
            string stateMachine = "1";
            int idMachine = GetIDLastMachine();

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
                            machineName = values[1];
                            break;
                        case 2:
                            stateMachine = values[1];
                            break;
                        case 4:
                            for (int i = 1; i < values.Length - 1; i++)
                            {
                                alarmsID.Add(int.Parse(values[i]));
                            }
                            break;
                        case 5:
                            for (int i = 1; i < values.Length - 1; i++)
                            {
                                commands.Add((Command)Enum.Parse(typeof(Command), values[i]));
                            }
                            break;
                        case 6:
                            for (int i = 1; i < values.Length - 1; i++)
                            {
                                messages.Add(values[i]);
                            }
                            break;
                        case 7:
                            for (int i = 1; i < values.Length - 1; i++)
                            {
                                alarmsState.Add(bool.Parse(values[i]));
                            }
                            break;
                        case 9:
                            for (int i = 1; i < values.Length - 1; i++)
                            {
                                stacklightID.Add(int.Parse(values[i]));
                            }
                            break;
                        case 10:
                            for (int i = 1; i < values.Length - 1; i++)
                            {
                                description.Add(values[i]);
                            }
                            break;
                        case 11:
                            for (int i = 1; i < values.Length - 1; i++)
                            {
                                stackLightsState.Add(bool.Parse(values[i]));
                            }
                            break;

                        case 13:
                            for (int i = 1; i < values.Length - 1; i++)
                            {

                                if (values[i] != values[i - 1])
                                {
                                    actionsNum = 0;
                                    commandID.Add(int.Parse(values[i]));
                                }
                                else
                                {
                                    actionsNum++;
                                }
                                howManyActionsInThisID[int.Parse(values[i])] = actionsNum + 1;
                            }
                            break;

                        case 14:
                            for (int i = 1; i < values.Length - 1; i++)
                            {
                                if (values[i] != values[i - 1])
                                {
                                    commandName.Add(values[i]);
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

                            for (int i = 1; i < values.Length - 1; i++)
                            {
                                actionsList.Add(values[i]);
                            }
                            break;

                    }
                }
            }

            InsertMachines(GetIDLastMachine() + 1, machineName, (int)Enum.Parse(typeof(State), stateMachine));

            for (int i = 0; i < alarmsID.Count; i++)
            {
                InsertAlarms(alarmsID[i], messages[i], alarmsState[i] ? 1 : 0, (int)commands[i], idMachine);
            }

            for (int i = 0; i < stacklightID.Count; i++)
            {
                InsertStackLights(stacklightID[i], description[i], stackLightsState[i] ? 1 : 0, idMachine);
            }

            for (int i = 0; i < commandID.Count; i++)
            {
                InsertButtons(commandID[i], commandName[i], idMachine);
            }

            for (int i = 0; i < actionsList.Count; i++)
            {
                InsertTriggers(commandID[i], (int)Enum.Parse(typeof(Command), actionsList[i]));
            }

        }

    }
}
