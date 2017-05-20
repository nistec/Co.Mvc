//licHeader
//===============================================================================================================
// System  : Nistec.Cache - Nistec.Cache Class Library
// Author  : Nissim Trujman  (nissim@nistec.net)
// Updated : 01/07/2015
// Note    : Copyright 2007-2015, Nissim Trujman, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a class that is part of cache core.
//
// This code is published under the Microsoft Public License (Ms-PL).  A copy of the license should be
// distributed with the code and can be found at the project website: http://nistec.net/license/nistec.cache-license.txt.  
// This notice, the author's name, and all copyright notices must remain intact in all applications, documentation,
// and source files.
//
//    Date     Who      Comments
// ==============================================================================================================
// 10/01/2006  Nissim   Created the code
//===============================================================================================================
//licHeader|
using Nistec.Channels.RemoteCache;
using Nistec.Serialization;
using Nistec.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Nistec.Channels;

namespace Nistec
{

    class Controller
    {
        [STAThread]
        public static void Window(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            //Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WindowHeight = (int)(Console.LargestWindowHeight * 0.50);
            Console.WindowWidth = (int)(Console.LargestWindowWidth * 0.50);
            Console.Title = "Nistec console";
            Console.WriteLine("Welcome to: Nistec commander...");
            Console.WriteLine("=====================================");
            Controller.Run(args);
            Console.WriteLine("Finished...");
            Console.ReadLine();

        }

        public static void Run(string[] args)
        {

            ServiceManager manager = new ServiceManager();

            NetProtocol cmdProtocol = NetProtocol.Tcp;
            string protocol = "tcp";
            
            string cmd = "";
            string operationType = "command";
            string cmdName = "";
            string cmdArg1 = "";

            SetCommands();

            DisplayOperationType(operationType);

            DisplayMenu("menu", "", "");

            while (cmd != "quit")
            {
                Console.WriteLine("Enter command :");

                cmd = Console.ReadLine();

                try
                {

                    string[] cmdargs = SplitCmd(cmd);
                    cmdName = GetCommandType(cmdargs[0], cmdName);
                    cmdArg1 = GetCommandType(cmdargs[1], cmdArg1);

                    switch (cmdName.ToLower())
                    {
                        case "menu":
                            DisplayMenu("menu", "", "");
                            break;
                        case "items":
                            DisplayMenu("items", operationType, "");
                            break;
                        case "operation":
                            DisplayOperationMessage();
                            operationType = GetOperationType(Console.ReadLine().ToLower(), operationType);
                            Console.WriteLine("Current operation type : {0}.", operationType);
                            break;
                        case "protocol":
                            Console.WriteLine("Choose protocol : tcp , pipe, http");
                            protocol = EnsureProtocol(Console.ReadLine().ToLower(), protocol);
                            cmdProtocol = GetProtocol(protocol, cmdProtocol);
                            Console.WriteLine("Current protocol : {0}.", protocol);
                            break;
                        case "args":
                            DisplayMenu("args", operationType, cmdArg1);
                            break;
                        case "quit":

                            break;
                        default:
                            switch (operationType)
                            {
                                case "service":

                                    switch (cmdName.ToLower())
                                    {
                                        case "status":
                                            manager.DispalyServiceStatus();
                                            break;
                                        case "details":
                                            manager.ShowServiceDetails();
                                            break;
                                        case "install":
                                            manager.DoServiceCommand(ServiceCmd.Install);
                                            break;
                                        case "uninstall":
                                            manager.DoServiceCommand(ServiceCmd.Uninstall);
                                            break;
                                        case "start":
                                            manager.DoServiceCommand(ServiceCmd.Start);
                                            break;
                                        case "stop":
                                            manager.DoServiceCommand(ServiceCmd.Stop);
                                            break;
                                        case "restart":
                                            manager.DoServiceCommand(ServiceCmd.Install);
                                            break;
                                        case "paus":
                                            manager.DoServiceCommand(ServiceCmd.Install);
                                            break;
                                    }
                                    //CmdController.DoCommandCache(cmdProtocol,cmdName, cmdArg1, cmdargs[2]);
                                    break;
                                case "command":
                                     Commands.Do(cmdName.ToLower(), cmdArg1);
                                    break;
                                default:
                                    break;
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                }
                Console.WriteLine();
            }
        }

        static Dictionary<string, string> serviceController = new Dictionary<string, string>();
        static Dictionary<string, string> commandController = new Dictionary<string, string>();
        static void SetCommands()
        {
            Commands.SetCommands(commandController);

            serviceController.Add("status", "no args");
            serviceController.Add("details", "no args");
            serviceController.Add("install", "no args");
            serviceController.Add("uninstall", "no args");
            serviceController.Add("start", "no args");
            serviceController.Add("stop", "no args");
            serviceController.Add("restart", "no args");
            serviceController.Add("paus", "no args");
        }

        static string EnsureArg(string arg)
        {
            if (arg == null)
                return "";
            return arg.Replace("/", "").ToLower();
        }
        static void DisplayOperationMessage()
        {
            Console.WriteLine("Choose operation : command, service, or quit");
        }
        static void DisplayOperationType(string operationType)
        {
            Console.WriteLine("Current operation type : {0}.", operationType);
        }
        
        static void DisplayArgs(string cmdType, string arg)
        {
            string a = EnsureArg(arg);
            KeyValuePair<string, string> kv = new KeyValuePair<string, string>();
            switch (cmdType)
            {
                case "command":
                    kv = commandController.Where(p => p.Key.ToLower() == a).FirstOrDefault();
                    break;
                case "service":
                    kv = serviceController.Where(p => p.Key.ToLower() == a).FirstOrDefault();
                    break;
            }

            if (kv.Key != null)
                Console.WriteLine("commands: {0} Arguments: {1}.", kv.Key, kv.Value);
            else
                Console.WriteLine("Bad commands: {0} Arguments: {1}.", cmdType, arg);
        }

        static void DisplayCommands(string cmdType, string prefix)
        {
            string cmd = "";

            switch (cmdType)
            {
                case "command":
                    foreach (var entry in commandController)
                    {
                        cmd += entry.Key + " " + entry.Value;
                    }
                    Console.WriteLine("{0}{1}.", prefix, cmd);
                    break;
                case "service":
                    foreach (string s in serviceController.Keys)
                    {
                        cmd += s + " ";
                    }
                    Console.WriteLine("{0}{1}.", prefix, cmd);
                    break;
            }

        }

        static void DisplayMenu(string cmdType, string operationType, string arg)
        {
            //string menu = "cache-type: remote-cache, remote-sync, remote-session";
            //Console.WriteLine(menu);

            switch (cmdType)
            {
                case "menu":
                    Console.WriteLine("Enter: operation, To change operation type");
                    Console.WriteLine("Enter: protocol, To change protocol (tcp, pipe, http)");
                    Console.WriteLine("Enter: menu, To display menu");
                    Console.WriteLine("Enter: items, To display menu items for current operation");
                    Console.WriteLine("Enter: args, and /command to display command argument");
                    break;
                case "items":
                    switch (operationType)
                    {
                        case "service":
                            DisplayCommands(operationType, "service commands: ");
                            break;
                        case "command":
                            DisplayCommands(operationType, "application commands: ");
                            break;
                        default:
                            Console.Write("Bad commands: Invalid operation");
                            break;
                    }
                    break;
                case "args":
                    if (arg != null && arg.StartsWith("/"))
                    {
                        DisplayArgs(operationType, arg);
                    }
                    break;
            }
            Console.WriteLine("");

        }
        static string[] SplitCmd(string cmd)
        {
            string[] args = new string[4] { "", "", "", "" };

            string[] cmdargs = cmd.SplitTrim(' ');
            if (cmdargs.Length > 0)
                args[0] = cmdargs[0];
            if (cmdargs.Length > 1)
                args[1] = cmdargs[1];
            if (cmdargs.Length > 2)
                args[2] = cmdargs[2];
            if (cmdargs.Length > 3)
                args[3] = cmdargs[3];
            return args;
        }

        static string GetOperationType(string cmd, string curItem)
        {
            switch (cmd.ToLower())
            {
                case "command":
                case "service":
                    return cmd.ToLower();
                default:
                    Console.WriteLine("Invalid operation {0}", cmd);
                    return curItem;
            }
        }
        static string GetCommandType(string cmd, string curItem)
        {
            if (cmd == "..")
                return curItem;
            return cmd;
        }

        static string EnsureProtocol(string protocol, string curProtocol)
        {
            switch (protocol.ToLower())
            {
                case "tcp":
                case "pipe":
                case "http":
                    return protocol.ToLower();
                default:
                    return curProtocol;
            }
        }

        static NetProtocol GetProtocol(string protocol, NetProtocol curProtocol)
        {
            switch (protocol.ToLower())
            {
                case "tcp":
                    return NetProtocol.Tcp;
                case "pipe":
                    return NetProtocol.Pipe;
                case "http":
                    return NetProtocol.Http;
                default:
                    return curProtocol;
            }
        }

        public static int GetUsage(string procName)
        {

            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName(procName);
            int usage = 0;
            if (process == null)
                return 0;
            for (int i = 0; i < process.Length; i++)
            {
                usage += (int)((int)process[i].WorkingSet64) / 1024;
            }

            return usage;
        }

    }

}
