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
using Pro.Lib.Payments;

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


    public class Control
    {
        public static void Runtime()
        {

            List<IMember> list = new List<IMember>();

            IMember member = new Boy(1, "aaa");// Person.Factory("boy", 1, "aaa");
            member.Age = 15;
           ((Boy)member).Title = "not boy";

            if (member.IsValidAge())
                list.Add(member);

            member = Person.Factory("girl", 12, "bbb");
            member.Age = 11;
            if (member.IsValidAge())
                list.Add(member);

            member = Person.Factory("ticher", 13, "ccc");
            member.Age = 17;
            if (member.IsValidAge())
                list.Add(member);
        }
    }

    public interface IMember
    {
        int ID { get; }
        string Name { get;}
        int Age { get; set; }
        bool IsValidAge();
    }

    public class Boy: Person
    {
        string _title= "Boy";
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        public Boy()
        {
           
        }

        public Boy(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        public override bool IsValidAge()
        {
            return Age > 13;
        }

    }

    public class Girl : Person
    {
        public string Title { get { return "Girl"; } }
        public Girl()
        {

        }

        public Girl(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        public override bool IsValidAge()
        {
            return Age > 12;
        }


    }

    public class Ticher : Person
    {
        public string Title { get { return "Ticher"; } }

        public Ticher()
        {

        }

        public Ticher(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        public override bool IsValidAge()
        {
            return Age > 18;
        }

     

    }

    public abstract class Person: IMember
    {
        public int ID { get; protected set; }
        public string Name { get; protected set; }
        public int Age { get; set; }
        public abstract bool IsValidAge();

        //public virtual bool IsValidAge()
        //{
        //    return Age > 10;
        //}

        public static IMember Factory(string title, int id, string name)
        {
            switch(title.ToLower())
            {
                case "boy":
                    return new Boy(id, name);
                case "girl":
                    return new Girl(id, name);
                case "ticher":
                    return new Ticher(id, name);
                default:
                    throw new Exception("Not supported!!");
            }
        }
        

    }


}
