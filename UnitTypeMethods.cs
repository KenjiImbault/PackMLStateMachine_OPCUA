/******************************************************************************
**
** <auto-generated>
**     This code was generated by a tool: UaModeler
**     Runtime Version: 1.6.9, using .NET Server 3.3.0 template (version 0)
**
**     This is a template file that was generated for your convenience.
**     This file will not be overwritten when generating code again.
**     ADD YOUR IMPLEMTATION HERE!
** </auto-generated>
**
** Copyright (c) 2006-2023 Unified Automation GmbH All rights reserved.
**
** Software License Agreement ("SLA") Version 2.8
**
** Unless explicitly acquired and licensed from Licensor under another
** license, the contents of this file are subject to the Software License
** Agreement ("SLA") Version 2.8, or subsequent versions
** as allowed by the SLA, and You may not copy or use this file in either
** source code or executable form, except in compliance with the terms and
** conditions of the SLA.
**
** All software distributed under the SLA is provided strictly on an
** "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED,
** AND LICENSOR HEREBY DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT
** LIMITATION, ANY WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
** PURPOSE, QUIET ENJOYMENT, OR NON-INFRINGEMENT. See the SLA for specific
** language governing rights and limitations under the SLA.
**
** Project: .NET OPC UA SDK information model for namespace http://yourorganisation.org/PackMLModel/
**
** Description: OPC Unified Architecture Software Development Kit.
**
** The complete license agreement can be found here:
** http://unifiedautomation.com/License/SLA/2.8/
**
** Created: 12.07.2023
**
******************************************************************************/

using PackML_v0;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnifiedAutomation.UaBase;
using UnifiedAutomation.UaServer;
using static PackML_v0.PackMLStateModel;

namespace FIP.PackMLStateMachine
{
    public partial class UnitModel : PackMLStateModelModel, IUnitMethods
    {
        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="alarm"></param>
        /// <returns></returns>
        public StatusCode AddAlarm(
            RequestContext context,
            UnitModel model,
            AlarmType alarm
            )
        {
            if (model.Dictionnaries.Alarms == null)
            {
                model.Dictionnaries.Alarms = new AlarmType[] { };
            }

            bool idExists = false;
            List<AlarmType> alarmsToList = new List<AlarmType>(model.Dictionnaries.Alarms.ToList());

            int key = 0;
            foreach(AlarmType alarmT in alarmsToList)
            {
                if(alarmT.Id == alarm.Id)
                {
                    idExists = true;
                    break;
                }
                key++;
            }

            if(idExists)
            {
                model.Dictionnaries.Alarms[key] = alarm;
                return StatusCodes.GoodEntryReplaced;
            }
            else
            {
                alarmsToList.Add(alarm);
                model.Dictionnaries.Alarms = alarmsToList.ToArray();
            }

            return StatusCodes.GoodEntryInserted;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="fileName"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public StatusCode AddAlarmsFromCSV(
            RequestContext context,
            UnitModel model,
            string fileName,
            string delimiter
            )
        {
            string path = @"Data/" + fileName;
            List<int> alarmsID = new List<int>();
            List<int> commands = new List<int>();
            List<string> messages = new List<string>();
            int lineNumber = 1;
            using (StreamReader reader = new StreamReader(path))
            {

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line != null ? line.Split(char.Parse(delimiter)) : Array.Empty<string>();
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
                AlarmType alarm = new AlarmType();
                alarm.Id = alarmsID[i];
                alarm.AlarmTransition = commands[i];
                alarm.AlarmMessage = messages[i];
                AddAlarm(context, model, alarm);
            }

            return StatusCodes.Good;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        public StatusCode AddButtons(
            RequestContext context,
            UnitModel model,
            ButtonType button
            )
        {
            if (model.Dictionnaries.Buttons == null)
            {
                model.Dictionnaries.Buttons = new ButtonType[] { };
            }

            bool idExists = false;
            List<ButtonType> buttonsToList = new List<ButtonType>(model.Dictionnaries.Buttons.ToList());

            int key = 0;
            foreach (ButtonType buttonT in buttonsToList)
            {
                if (buttonT.Id == button.Id)
                {
                    idExists = true;
                    break;
                }
                key++;
            }

            if (idExists)
            {
                model.Dictionnaries.Buttons[key] = button;
                return StatusCodes.GoodEntryReplaced;
            }
            else
            {
                buttonsToList.Add(button);
                model.Dictionnaries.Buttons = buttonsToList.ToArray();
            }

            return StatusCodes.GoodEntryInserted;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="fileName"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public StatusCode AddButtonsFromCSV(
            RequestContext context,
            UnitModel model,
            string fileName,
            string delimiter
            )
        {
            string path = @"Data/" + fileName;
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
                    string[] value = currentLine.Split(char.Parse(delimiter));
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

            List<int[]> actionsArray = new List<int[]>();
            List<int> commandsArrayList = new List<int>();

            foreach (List<Command> commands in actions)
            {
                foreach (int command in commands)
                {
                    commandsArrayList.Add(command);
                }
                actionsArray.Add(commandsArrayList.ToArray());
                commandsArrayList = new List<int>();
            }



            for (int i = commandID.Count - 1; i >= 0; i--)
            {
                ButtonType button = new ButtonType();
                button.Id = commandID[i];
                button.ButtonName = commandName[i];
                button.Commands = actionsArray[i];
                AddButtons(context, model, button);
            }

            return StatusCodes.Good;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="stacklight"></param>
        /// <returns></returns>
        public StatusCode AddStackLight(
            RequestContext context,
            UnitModel model,
            StackLightType stacklight
            )
        {
            if (model.Dictionnaries.StackLights == null)
            {
                model.Dictionnaries.StackLights = new StackLightType[] { };
            }

            bool idExists = false;
            List<StackLightType> stackLightsToList = new List<StackLightType>(model.Dictionnaries.StackLights.ToList());

            int key = 0;
            foreach (StackLightType alarmT in stackLightsToList)
            {
                if (alarmT.Id == stacklight.Id)
                {
                    idExists = true;
                    break;
                }
                key++;
            }

            if (idExists)
            {
                model.Dictionnaries.StackLights[key] = stacklight;
                return StatusCodes.GoodEntryReplaced;
            }
            else
            {
                stackLightsToList.Add(stacklight);
                model.Dictionnaries.StackLights = stackLightsToList.ToArray();
            }

            return StatusCodes.GoodEntryInserted;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="fileName"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public StatusCode AddStackLightsFromCSV(
            RequestContext context,
            UnitModel model,
            string fileName,
            string delimiter
            )
        {
            string path = @"Data/" + fileName;
            List<int> stacklightID = new List<int>();
            List<string> description = new List<string>();
            int lineNumber = 1;
            using (StreamReader reader = new StreamReader(path))
            {

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line != null ? line.Split(char.Parse(delimiter)) : Array.Empty<string>();
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
                StackLightType stackLightType = new StackLightType();
                stackLightType.Id = stacklightID[i];
                stackLightType.Description = description[i];
                AddStackLight(context, model, stackLightType);
            }

            return StatusCodes.Good;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        public StatusCode ExecuteProcess(
            RequestContext context,
            UnitModel model,
            ProcessType process
            )
        {
            List<int> ints = new List<int>(process.Commands.ToList());

            Unit unit = new Unit((State)model.CurrentState);

            int i = 0;
            while (i < process.Commands.Count)
            {
                if (unit.IsCommandAvailable(Command.StateCompleted))
                {
                    Task.Delay((int)process.SCTime);
                    unit.SafeMoveNext(Command.StateCompleted);
                    model.CurrentState = (int)unit.CurrentState;
                }
                else
                {
                    Task.Delay((int)process.CommandTime);
                    unit.SafeMoveNext((Command)process.Commands[i]);
                    model.CurrentState = (int)unit.CurrentState;
                    i++;
                }
                if (i == process.Commands.Count)
                {
                    Task.Delay((int)process.SCTime);
                    unit.SafeMoveNext(Command.StateCompleted);
                    model.CurrentState = (int)unit.CurrentState;

                }
            }
            return StatusCodes.Good;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="csvFileName"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public StatusCode ReadDataFromCSV(
            RequestContext context,
            UnitModel model,
            string csvFileName,
            string delimiter
            )
        {
            return StatusCodes.BadNotImplemented;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="DBFileName"></param>
        /// <returns></returns>
        public StatusCode ReadDataFromDB(
            RequestContext context,
            UnitModel model,
            string DBFileName
            )
        {
            return StatusCodes.BadNotImplemented;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="alarmID"></param>
        /// <returns></returns>
        public StatusCode RemoveAlarm(
            RequestContext context,
            UnitModel model,
            int alarmID
            )
        {
            if (alarmID < 0)
            {
                return StatusCodes.BadIndexRangeNoData;
            }
            if (model.Dictionnaries.Alarms == null)
            {
                return StatusCodes.BadIndexRangeNoData;
            }

            int i = 0;

            List<AlarmType> alarmTypes = model.Dictionnaries.Alarms.ToList();

            foreach(AlarmType alarm in alarmTypes)
            {
                if(alarm.Id == alarmID)
                {
                    alarmTypes.Remove(alarm);
                    model.Dictionnaries.Alarms = alarmTypes.ToArray();
                    return StatusCodes.Good;
                }
                i++;
            }
            return StatusCodes.BadIndexRangeNoData;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="buttonID"></param>
        /// <returns></returns>
        public StatusCode RemoveButton(
            RequestContext context,
            UnitModel model,
            int buttonID
            )
        {
            if (buttonID < 0)
            {
                return StatusCodes.BadIndexRangeNoData;
            }
            if (model.Dictionnaries.Buttons == null)
            {
                return StatusCodes.BadIndexRangeNoData;
            }

            int i = 0;

            List<ButtonType> buttonTypes = model.Dictionnaries.Buttons.ToList();

            foreach (ButtonType button in buttonTypes)
            {
                if (button.Id == buttonID)
                {
                    buttonTypes.Remove(button);
                    model.Dictionnaries.Buttons = buttonTypes.ToArray();
                    return StatusCodes.Good;
                }
                i++;
            }
            return StatusCodes.BadIndexRangeNoData;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="stacklightID"></param>
        /// <returns></returns>
        public StatusCode RemoveStackLight(
            RequestContext context,
            UnitModel model,
            int stacklightID
            )
        {
            if (stacklightID < 0)
            {
                return StatusCodes.BadIndexRangeNoData;
            }
            if (model.Dictionnaries.StackLights == null)
            {
                return StatusCodes.BadIndexRangeNoData;
            }

            int i = 0;

            List<StackLightType> stacklightTypes = model.Dictionnaries.StackLights.ToList();

            foreach (StackLightType stacklight in stacklightTypes)
            {
                if (stacklight.Id == stacklightID)
                {
                    stacklightTypes.Remove(stacklight);
                    model.Dictionnaries.StackLights = stacklightTypes.ToArray();
                    return StatusCodes.Good;
                }
                i++;
            }
            return StatusCodes.BadIndexRangeNoData;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public StatusCode SafeMoveNext(
            RequestContext context,
            UnitModel model,
            int command
            )
        {
            StateTransition transition = new StateTransition((State)model.CurrentState, (Command)command);
            Unit temp = new Unit((State)model.CurrentState);
            if (temp.GetTransitions().TryGetValue(transition, out State nextState))
            {
                model.CurrentState = (int)temp.MoveNext((Command)command);
                return StatusCodes.Good;
            }
            return StatusCodes.BadInvalidState;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="csvFileName"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public StatusCode SaveDataToCSV(
            RequestContext context,
            UnitModel model,
            string csvFileName,
            string delimiter
            )
        {
            return StatusCodes.BadNotImplemented;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="DBFileName"></param>
        /// <returns></returns>
        public StatusCode SaveDataToDB(
            RequestContext context,
            UnitModel model,
            string DBFileName
            )
        {
            return StatusCodes.BadNotImplemented;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public StatusCode SetMachineName(
            RequestContext context,
            UnitModel model,
            string name
            )
        {
            model.MachineName = name;
            return StatusCodes.Good;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="alarmID"></param>
        /// <returns></returns>
        public StatusCode TriggerAlarm(
            RequestContext context,
            UnitModel model,
            int alarmID
            )
        {
            if (alarmID < 0)
            {
                return StatusCodes.BadIndexRangeNoData;
            }
            if (model.Dictionnaries.Alarms == null)
            {
                return StatusCodes.BadIndexRangeNoData;
            }
            if(alarmID>model.Dictionnaries.Alarms.Count())
            {
                return StatusCodes.BadIndexRangeInvalid;
            }

            int i = 0;
            bool changed = false;
            List<AlarmType> alarmTypes = new List<AlarmType>();

            foreach (AlarmType alarm in model.Dictionnaries.Alarms)
            {
                if (alarm.Id == alarmID)
                {
                    AlarmType newAlarm = new AlarmType();
                    newAlarm.Id = alarm.Id;
                    newAlarm.AlarmMessage = alarm.AlarmMessage;
                    newAlarm.AlarmTransition = alarm.AlarmTransition;
                    newAlarm.On = true;
                    alarmTypes.Add( newAlarm );
                    changed = true;
                }
                else
                {
                    alarmTypes.Add(alarm);
                }
                i++;
            }
            if(changed==false)
            {
                return StatusCodes.BadIndexRangeNoData;
            }

            model.Dictionnaries.Alarms = alarmTypes.ToArray();
            return StatusCodes.Good;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="buttonID"></param>
        /// <returns></returns>
        public StatusCode TriggerButton(
            RequestContext context,
            UnitModel model,
            int buttonID
            )
        {
            return StatusCodes.BadNotImplemented;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="stacklightID"></param>
        /// <returns></returns>
        public StatusCode TriggerStackLight(
            RequestContext context,
            UnitModel model,
            int stacklightID
            )
        {
            if (stacklightID < 0)
            {
                return StatusCodes.BadIndexRangeNoData;
            }
            if (model.Dictionnaries.StackLights == null)
            {
                return StatusCodes.BadIndexRangeNoData;
            }
            if (stacklightID > model.Dictionnaries.StackLights.Count())
            {
                return StatusCodes.BadIndexRangeInvalid;
            }

            int i = 0;
            bool changed = false;
            List<StackLightType> stacklightTypes = new List<StackLightType>();

            foreach (StackLightType stacklight in model.Dictionnaries.StackLights)
            {
                if (stacklight.Id == stacklightID)
                {
                    StackLightType newStackLight = new StackLightType();
                    newStackLight.Id = stacklight.Id;
                    newStackLight.Description = stacklight.Description;
                    newStackLight.On = true;
                    stacklightTypes.Add(newStackLight);
                    changed = true;
                }
                else
                {
                    stacklightTypes.Add(stacklight);
                }
                i++;
            }
            if (changed == false)
            {
                return StatusCodes.BadIndexRangeNoData;
            }

            model.Dictionnaries.StackLights = stacklightTypes.ToArray();
            return StatusCodes.Good;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="alarmID"></param>
        /// <returns></returns>
        public StatusCode UntriggerAlarm(
            RequestContext context,
            UnitModel model,
            int alarmID
            )
        {
            if (alarmID < 0)
            {
                return StatusCodes.BadIndexRangeNoData;
            }
            if (model.Dictionnaries.Alarms == null)
            {
                return StatusCodes.BadIndexRangeNoData;
            }
            if (alarmID > model.Dictionnaries.Alarms.Count())
            {
                return StatusCodes.BadIndexRangeInvalid;
            }

            int i = 0;
            bool changed = false;
            List<AlarmType> alarmTypes = new List<AlarmType>();

            foreach (AlarmType alarm in model.Dictionnaries.Alarms)
            {
                if (alarm.Id == alarmID)
                {
                    AlarmType newAlarm = new AlarmType();
                    newAlarm.Id = alarm.Id;
                    newAlarm.AlarmMessage = alarm.AlarmMessage;
                    newAlarm.AlarmTransition = alarm.AlarmTransition;
                    newAlarm.On = false;
                    alarmTypes.Add(newAlarm);
                    changed = true;
                }
                else
                {
                    alarmTypes.Add(alarm);
                }
                i++;
            }
            if (changed == false)
            {
                return StatusCodes.BadIndexRangeNoData;
            }

            model.Dictionnaries.Alarms = alarmTypes.ToArray();
            return StatusCodes.Good;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="stacklightID"></param>
        /// <returns></returns>
        public StatusCode UntriggerStackLight(
            RequestContext context,
            UnitModel model,
            int stacklightID
            )
        {
            if (stacklightID < 0)
            {
                return StatusCodes.BadIndexRangeNoData;
            }
            if (model.Dictionnaries.StackLights == null)
            {
                return StatusCodes.BadIndexRangeNoData;
            }
            if (stacklightID > model.Dictionnaries.StackLights.Count())
            {
                return StatusCodes.BadIndexRangeInvalid;
            }

            int i = 0;
            bool changed = false;
            List<StackLightType> stacklightTypes = new List<StackLightType>();

            foreach (StackLightType stacklight in model.Dictionnaries.StackLights)
            {
                if (stacklight.Id == stacklightID)
                {
                    StackLightType newStackLight = new StackLightType();
                    newStackLight.Id = stacklight.Id;
                    newStackLight.Description = stacklight.Description;
                    newStackLight.On = false;
                    stacklightTypes.Add(newStackLight);
                    changed = true;
                }
                else
                {
                    stacklightTypes.Add(stacklight);
                }
                i++;
            }
            if (changed == false)
            {
                return StatusCodes.BadIndexRangeNoData;
            }

            model.Dictionnaries.StackLights = stacklightTypes.ToArray();
            return StatusCodes.Good;
        }

    }
}
