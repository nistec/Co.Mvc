using System;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Data.Factory;
using Netcell.Data;
using Netcell.Data.DbServices.Entities;

namespace Netcell.Remoting
{
    

    public class MailHosts : Dictionary<string, Mail_Host>, IDisposable
    {

        public static MailHosts GetChannels()
        {
            MailHosts channels = new MailHosts();
            channels.LoadChannels();//Cache.Values.ToArray());
            return channels;
        }

        public void LoadChannels()
        {
            int server = ViewConfig.Server;
            var list = Mail_Host_Context.GetListItems();// (server);

            this.Clear();
            foreach (Mail_Host host in list)
            {
                this.Add(host.HostId, host);
            }


            //DataTable dt = DalServices.Instance.Mail_Hosts(server);
            //LoadChannels(dt);
        }

        //public void LoadChannels(DataTable dt)
        //{
        //    this.Clear();
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        MailHost channel = new MailHost(dr);
        //        this.Add(channel.HostId, channel);
        //    }
        //}

        public void LoadChannels(Mail_Host[] channels)
        {
            this.Clear();
            foreach (Mail_Host channel in channels)
            {
                this.Add(channel.HostId, channel);
            }
        }

       
        public MailHosts()//string hostId)
        {
            try
            {
                int server = ViewConfig.Server;

                var list = Mail_Host_Context.GetListItems();// (server);

                 foreach (Mail_Host host in list)
                {
                    this.Add(host.HostId, host);
                }

                //DataTable dt = DalServices.Instance.Mail_Hosts(server);
                //if (dt == null || dt.Rows.Count==0)
                //{
                //    MsgException.Trace(AckStatus.MailServerError, "Invalid mail hosts in server:" + server.ToString());
                //}

                //foreach (DataRow dr in dt.Rows)
                //{
                //    MailHost host=new MailHost(dr);
                //    this.Add(host.HostId,host);
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(" Could not load MailHosts " + ex.Message);
            }
        }

        public void Dispose()
        {
            //string [] array=new  string[this.Count];

            //this.Keys.CopyTo(array,0);

            //foreach (string s in array)
            //{
            //    IMailHost cnn =(IMailHost) this[s];
            //    if (cnn != null)
            //    {
            //        cnn.Dispose();
            //    }
            //}

            
            this.Clear();
        }

    }


}
