using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Netcell.Remoting
{

 
    public class MailUtil
    {
        /// <summary>
        /// ValidateMailSender
        /// </summary>
        /// <param name="Sender"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string ValidateMailSender(string Sender)
        {
            string maSender = Sender;
            if (!Nistec.Regx.IsEmail(maSender))
            {
                maSender = string.Format(ViewConfig.MailSender, Sender);
                if (!Nistec.Regx.IsEmail(maSender))
                {
                     throw new ArgumentException("Mail sender is in correct");
                }
            }

            return maSender;
        }

       

        //public static string RemoveShowLinks(string html)
        //{
        //    html = html.Replace(MailView.cnRemoveText, "");
        //    html = html.Replace(MailView.cnShowText, "");
        //    return html;
        //}

        #region SmtpStatus

        public static string GetSmtpStatus(int status)
        {
            switch (status)
            {
                case 211: return "A system status message. ";
                case 214: return "A help message for a human reader follows. ";
                case 220: return "Service ready. ";
                case 221: return "Service closing. ";
                case 250: return "Requested action taken and completed. The best message of them all. ";
                case 251: return "The recipient is not local to the server, but it will accept and forward the message. ";
                case 252: return "The recipient cannot be VRFYed, but the server accepts the message and attempts delivery. ";
                case 354: return "Start message input and end with <CRLF>.<CRLF>. This indicates that the server is ready to accept the message itself. ";
                case 421: return "The service is not available and the connection will be closed.";
                case 422: return "The recipient has exceeded their mailbox limit.  It could also be that the delivery directory on the Virtual server has exceeded its limit. ";
                case 431: return "Not enough disk space on the delivery server.  Microsoft say this NDR maybe reported as out-of-memory error. ";
                case 432: return "Classic temporary problem, the Administrator has frozen the queue. ";
                case 441: return "Intermittent network connection.  The server has not yet responded.  Classic temporary problem.  If it persists, you will also a 5.4.x status code error. ";
                case 442: return "The server started to deliver the message but then the connection was broken. ";
                case 446: return "Too many hops.  Most likely, the message is looping. ";
                case 447: return "Problem with a timeout.  Check receiving server connectors. ";
                case 449: return "A DNS problem.  Check your smart host setting on the SMTP connector.  For example, check correct SMTP format. Also, use square brackets in the IP address [197.89.1.4]  You can get this same NDR error if you have been deleting routing groups. ";
                case 450: return "The requested command failed because the user's mailbox was unavailable. ";
                case 451: return "The command has been aborted due to a server error. Not your fault. ";
                case 452: return "The command has been aborted because the server has insufficient system storage. ";
                case 465: return "Multi-language situation.  Your server does not have the correct language code page installed.";
                case 500: return "The server could not recognize the command due to a syntax error. ";
                case 501: return "A syntax error was encountered in command arguments. ";
                case 502: return "This command is not implemented. ";
                case 503: return "The server has encountered a bad sequence of commands. ";
                case 504: return "A command parameter is not implemented. ";
                //case 51x: return" Problem with email address. ";
                case 510: return "Often seen with contacts. Check the recipient address. ";
                case 511: return "Another problem with the recipient address.  Maybe an Outlook client replied to a message while offline. ";
                case 512: return "SMTP; 550 Host unknown.  An error is triggered when the host name can’t be found.  For example, when trying to send an email to bob@ nonexistantdomain.com. ";
                case 513: return "Another problem with contacts.  Address field maybe empty.  Check the address information. ";
                case 514: return "Two objects have the same address, which confuses the categorizer. ";
                case 515: return "Destination mailbox address invalid. ";
                case 516: return "Mailbox may have moved. ";
                case 517: return "Problem with senders mail attribute, check properties sheet in ADUC. ";
                //case 52x: return" NDR caused by a problem with the large size of the email. ";
                case 521: return "The message is too large.  Else it could be a permissions problem.  Check the recipient's mailbox. ";
                case 522: return "The recipient has exceeded their mailbox limit. ";
                case 523: return "Recipient cannot receive messages this big.  Server or connector limit exceeded. ";
                case 524: return "Most likely, a distribution list or group is trying to send an email.  Check where the expansion server is situated. ";
                case 530: return "Problem with MTA, maybe someone has been editing the registry to disable the MTA / Store driver. ";
                case 531: return "Mail system full. ";
                case 532: return "System not accepting network messages. ";
                case 533: return "Remote server has insufficient disk space to hold email. ";
                case 534: return "Message too big. ";
                case 535: return "Multiple Virtual Servers are using the same IP address and port. Email probably looping. ";
                case 540: return "DNS Problem. There is no DNS server that can resolve this email address. ";
                case 541: return "No answer from host. ";
                case 542: return "Bad connection. ";
                case 543: return "Routing server failure.  No available route. ";
                case 544: return "Cannot find the next hop. ";
                case 546: return "Tricky looping problem, a contact has the same email address as an Active Directory user.  One user is probably using an Alternate Recipient with the same email address as a contact. ";
                case 547: return "Delivery time-out.  Message is taking too long to be delivered. ";
                case 548: return "Bad recipient policy. ";
                case 550: return "The requested command failed because the user's mailbox was unavailable (for example because it was not found, or because the command was rejected for policy reasons). Underlying SMTP 500 error.  Our server tried ehlo, the recipient's server did not understand and returned a 550 or 500 error. ";
                case 551: return "The recipient is not local to the server. The server then gives a forward address to try. ";
                case 552: return "The action was aborted due to exceeded storage allocation.  Possibly the disk holding the operating system is full.  Or could be a syntax error if you are executing SMTP from a telnet shell. ";
                case 553: return "The command was aborted because the mailbox name is invalid.  Could also be More than 5,000 recipients specified. ";
                case 554: return "The transaction failed. ";
                case 555: return "Wrong protocol version. ";
                case 563: return "More than 250 attachments. ";
                case 571: return "Permissions problem.  For some reason the sender is not allowed to email this account.  Perhaps an anonymous user is trying to send mail to a distribution list. ";
                case 572: return "Distribution list cannot expand and so is unable to deliver its messages. ";
                case 573: return "Internal server error, IP address related. ";
                case 574: return "Extra security features not supported. ";
                case 575: return "Cryptographic failure.  Try a plain message with encryption. ";
                case 576: return "Certificate problem, encryption level maybe to high. ";
                case 577: return "Message integrity problem. ";
                default:
                    if (status >= 510 && status < 520)//51x
                        return " Problem with email address. ";
                    if (status >= 520 && status < 530)//52x
                        return " NDR caused by a problem with the large size of the email. ";

                    return "unexpcted error";

            }


        }
        #endregion
    }
}