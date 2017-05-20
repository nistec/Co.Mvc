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
using Pro.Lib.Api;

namespace Nistec
{

    class Commands
    {
        public static void SetCommands(Dictionary<string, string> serviceController)
        {
            serviceController.Add("paymentbroker", "/run /auto");
        }


        public static void Do(string cmd, string args)
        {

            switch (cmd.ToLower())
            {
                case "paymentbroker":
                    {
                        Console.WriteLine(cmd + " " + args);
                        if (args == "/run")
                        {
                            PaymentBroker.Run();
                        }
                        else if (args == "/auto")
                        {
                            PaymentBroker.RunAuto();
                        }
                        else
                            Console.WriteLine("Incorrect argument: " + args);

                    }
                    break;
            }
        }
    }
}
