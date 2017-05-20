using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleTest
{
    public class UploadTest
    {

        public static void Run()
        {
            string path = @"D:\Dev\Logs";
            string content = "טעינת המנויים בתהליך סנכרון ותסתיים בעוד מספר דקות";
            int maxitems = 10;
            for (int i = 0; i < maxitems; i++)
            {
                string filename = path + "\\item_" + i.ToString() + ".mcq";
                File.AppendAllText(filename, content, Encoding.UTF8);
                //File.SetAttributes(filename, FileAttributes.Normal);
            }

            DirectoryInfo di = new DirectoryInfo(path);

            for (int i = 0; i < maxitems; i++)
            {
                IEnumerable<FileInfo> files = di.GetFiles("*.mcq").Where(f => (f.Attributes & FileAttributes.Archive) == FileAttributes.Archive);
                if (files.Count() == 0)
                    break;

                var file = files.OrderBy(f => f.LastWriteTime).FirstOrDefault();
                File.SetLastWriteTime(file.FullName, DateTime.Now);
                File.SetAttributes(file.FullName, FileAttributes.Normal);
            }
            for (int i = 0; i < maxitems; i++)
            {
                IEnumerable<FileInfo> files = di.GetFiles("*.mcq").Where(f => (f.Attributes & FileAttributes.Normal) == FileAttributes.Normal);
                var file = files.FirstOrDefault();
                File.SetLastWriteTime(file.FullName, DateTime.Now);
                File.SetAttributes(file.FullName, FileAttributes.Archive);
            }

            for (int i = 0; i < maxitems; i++)
            {
                IEnumerable<FileInfo> files = di.GetFiles("*.mcq").Where(f => (f.Attributes & FileAttributes.Archive) == FileAttributes.Archive);
                if (files.Count() == 0)
                    break;

                var file = files.OrderBy(f => f.LastWriteTime).FirstOrDefault();
                File.SetLastWriteTime(file.FullName, DateTime.Now);
                File.SetAttributes(file.FullName, FileAttributes.Normal);
            }



            //string json = "{\"AproxUnits\":2,\"BatchId\":12345,\"Count\":0,\"Reason\":\"Exception:Invalid IP address for Account: 0000\"}";

            //ApiResult model = JsonSerializer.Deserialize<ApiResult>(json);

            //dynamic res= Nistec.Generic.JsonConverter.DeserializeDynamic(json);
            //ApiResult api = res;

            //ApiResult api2 = new ApiResult()
            //{
            //    AproxUnits = res.AproxUnits,
            //    BatchId = res.BatchId,
            //    Count = res.Count,
            //    Reason = res.Reason
            //};




            //Nistec.Generic.GenericJson conv = new Nistec.Generic.GenericJson(json);
            //var res=conv.Dynamic();
        }

    }
}
