using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using System.IO;
using System.Configuration;

namespace Pro.Server
{
    public class Zipper
    {

        public int DoZip()
        {
            Console.WriteLine("start zipper...");
            int count = 0;
            try
            {
                string DirectoryToZip = ConfigurationManager.AppSettings["zip_directories"];
                int OldThenDays = 1;
                int.TryParse(ConfigurationManager.AppSettings["zip_OldThenDays"], out OldThenDays);
                if (string.IsNullOrEmpty(DirectoryToZip))
                {
                    Console.WriteLine("Invalid direcories");
                    return 0;
                }
                string[] direcories = DirectoryToZip.Split(',', ';');

                foreach (String dir in direcories)
                {
                    if (string.IsNullOrEmpty(dir))
                        continue;

                    Console.WriteLine("zip directory:" + dir);

                    // note: this does not recurse directories! 
                    String[] filenames = System.IO.Directory.GetFiles(dir, "*.log", SearchOption.AllDirectories);

                    foreach (String filename in filenames)
                    {
                        FileInfo info = new FileInfo(filename);
                        if (info.LastWriteTime < DateTime.Now.AddDays(OldThenDays * -1))
                        {
                            InvokeZipAsync(info);
                            count++;
                        }
                    }

                }
            }
            catch (System.Exception ex1)
            {
                System.Console.Error.WriteLine("exception: " + ex1);
            }
            Console.WriteLine("zipper finshed");
            //Console.ReadKey();
            return count;
        }


        delegate void ZipAsyncCallBack(FileInfo info);

        void InvokeZipAsync(FileInfo info)
        {

            ZipAsyncCallBack d = new ZipAsyncCallBack(InvokeDelegate);

            IAsyncResult ar = d.BeginInvoke(info, null, null);

            d.EndInvoke(ar);

        }

        void InvokeDelegate(FileInfo info)
        {

            string filename = info.FullName;
            Console.WriteLine("Adding {0}...", filename);
            using (ZipFile zip = new ZipFile())
            {
                ZipEntry e = zip.AddFile(filename, "");
                e.Comment = "Added by Mc's CreateZip utility.";
                e.FileName = info.Name;
                zip.Save(Path.Combine(info.DirectoryName, info.Name + ".zip"));
            }
            info.Delete();
            Console.WriteLine("Completed {0}...", filename);

        }
    }
}
