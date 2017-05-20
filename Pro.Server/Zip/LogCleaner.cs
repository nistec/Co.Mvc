using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using System.IO;
using System.Configuration;

namespace Pro.Server
{
    public class LogCleaner
    {

        public int DoClean()
        {
            Console.WriteLine("start cleaner...");
            int count = 0;
            try
            {
                string DirectoryToClean = ConfigurationManager.AppSettings["clean_directories"];
                int OldThenDays = 1;
                int.TryParse(ConfigurationManager.AppSettings["clean_OldThenDays"], out OldThenDays);
                if (string.IsNullOrEmpty(DirectoryToClean))
                {
                    Console.WriteLine("Invalid direcories");
                    return 0;
                }
                string[] direcories = DirectoryToClean.Split(',', ';');

                foreach (String dir in direcories)
                {
                    if (string.IsNullOrEmpty(dir))
                        continue;

                    Console.WriteLine("clean directory:" + dir);

                    // note: this does not recurse directories! 
                    //String[] filenames = System.IO.Directory.GetFiles(dir, "*.log");

                    string[] filenames = GetFiles(new string[] { dir }, new string[] { "*.log", "*.rar", "*.zip" });
                    
                    foreach (String filename in filenames)
                    {
                        FileInfo info = new FileInfo(filename);
                        if (info.LastWriteTime < DateTime.Now.AddDays(OldThenDays * -1))
                        {
                            InvokeCleanerAsync(info);
                            count++;
                        }
                    }

                }
            }
            catch (System.Exception ex1)
            {
                System.Console.Error.WriteLine("exception: " + ex1);
            }
            Console.WriteLine("cleaner finshed");
            //Console.ReadKey();
            return count;
        }


        delegate void CleanerAsyncCallBack(FileInfo info);

        void InvokeCleanerAsync(FileInfo info)
        {

            CleanerAsyncCallBack d = new CleanerAsyncCallBack(InvokeDelegate);

            IAsyncResult ar = d.BeginInvoke(info, null, null);

            d.EndInvoke(ar);

        }

        void InvokeDelegate(FileInfo info)
        {

            string filename = info.FullName;
            info.Delete();
            Console.WriteLine("Deleted {0}...", filename);

        }

        /// <summary>
        /// Returns the names of files in a specified directories that match the specified patterns using LINQ
        /// </summary>
        /// <param name="srcDirs">The directories to seach</param>
        /// <param name="searchPatterns">the list of search patterns</param>
        /// <param name="searchOption"></param>
        /// <returns>The list of files that match the specified pattern</returns>
        public static string[] GetFiles(string[] srcDirs,
             string[] searchPatterns,
             SearchOption searchOption = SearchOption.AllDirectories)
        {
            var r = from dir in srcDirs
                    from searchPattern in searchPatterns
                    from f in Directory.GetFiles(dir, searchPattern, searchOption)
                    select f;

            return r.ToArray();
        }

    }
}
