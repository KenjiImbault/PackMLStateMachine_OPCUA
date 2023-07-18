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
** Created: 10.07.2023
**
******************************************************************************/

using PackML_v0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnifiedAutomation.UaBase;
using UnifiedAutomation.UaServer;

namespace FIP.PackMLStateMachine
{
    public partial class PackMLStateModelModel : BaseObjectModel, IPackMLStateModelMethods
    {

        /// <summary>
        /// Get the available commands from the current state of the machine as a list.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="commands">out: </param>
        /// <returns></returns>
        public StatusCode GetAvailableCommands(
            RequestContext context,
            PackMLStateModelModel model,
            out int[] commands
            )
        {
            commands = new int[] {};
            PackMLStateModel packMLStateModel = new PackMLStateModel((State)model.CurrentState);
            foreach(Command c in packMLStateModel.GetAvailableCommands())
            {
                commands = commands.Append((int) c).ToArray();
            }
            return StatusCodes.Good;
        }

        /// <summary>
        /// Returns what would be the next state if we applied the given command. Throws an exception if the command is not applicable.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="command"></param>
        /// <param name="state">out: </param>
        /// <returns></returns>
        public StatusCode GetNext(
            RequestContext context,
            PackMLStateModelModel model,
            int command,
            out int state
            )
        {
            PackMLStateModel packMLStateModel = new PackMLStateModel((State)model.CurrentState);
            state = (int) packMLStateModel.GetNext((Command)command);
            return StatusCodes.Good;
        }

        /// <summary>
        /// Returns true if the command is applicable, return false otherwise.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="command"></param>
        /// <param name="val">out: </param>
        /// <returns></returns>
        public StatusCode IsCommandAvailable(
            RequestContext context,
            PackMLStateModelModel model,
            int command,
            out bool val
            )
        {
            PackMLStateModel packMLStateModel = new PackMLStateModel((State)model.CurrentState);
            val = packMLStateModel.IsCommandAvailable((Command)command);
            return StatusCodes.Good;
        }

        /// <summary>
        /// Applies a command to a State Machine. If a command is not appliable to a State Machine in it's current state, throws an exception.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="command"></param>
        /// <param name="state">out: </param>
        /// <returns></returns>
        public StatusCode MoveNext(
            RequestContext context,
            PackMLStateModelModel model,
            int command,
            out int state
            )
        {
            PackMLStateModel packMLStateModel = new PackMLStateModel((State)model.CurrentState);
            state = (int)packMLStateModel.MoveNext((Command)command);
            model.CurrentState = state;
            return StatusCodes.Good;
        }

        /// <summary>
        /// Changes the state of the state machine to the given state.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public StatusCode SetState(
            RequestContext context,
            PackMLStateModelModel model,
            int state
            )
        {
            model.CurrentState = state;
            return StatusCodes.Good;
        }

    }
}

