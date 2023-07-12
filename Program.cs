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

using FIP.PackMLStateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnifiedAutomation.UaBase;
using UnifiedAutomation.UaServer;

namespace FIP.PackMLModel
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // There is no license file configured in UaModeler.
                // After you have added a license file to the project you can add the license with the following
                // line of code.
                // ApplicationLicenseManager.AddProcessLicenses(System.Reflection.Assembly.GetExecutingAssembly(), "License.lic");

                MessageContext context = new MessageContext();
                context.Factory.AddEncodeableType(typeof(FIP.PackMLStateMachine.AlarmType));
                context.Factory.AddEncodeableType(typeof(FIP.PackMLStateMachine.ButtonType));
                context.Factory.AddEncodeableType(typeof(FIP.PackMLStateMachine.StackLightType));
                context.Factory.AddEncodeableType(typeof(FIP.PackMLStateMachine.ProcessType));

                // Start the server.
                ServerManager server = new TestServerManager();
                ApplicationInstance.Default.AutoCreateCertificate = true;
                ApplicationInstance.Default.Start(server, null, server);
                Console.WriteLine("Endpoint URL: opc.tcp://localhost:48030");
                // Block until the server exits.
                Console.WriteLine("Press <enter> to exit the program.");
                Console.ReadLine();
                // Stop the server.
                server.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
                Console.WriteLine("Press <enter> to exit the program.");
                Console.ReadLine();
            }
        }

    }
}

