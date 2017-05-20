using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Pro.Mvc.Controllers
{
    /*
    public sealed class DownloadResult : FileResult
    {
        public static string GetContentType(string fileExt)
        {
            if (fileExt == null)
                return "";

            switch (fileExt.ToLower())
            {
                case "jpg":
                case "jpeg":
                    return "image/jpeg";
                case "png":
                    return "image/png";
                case "gif":
                    return "image/gif";
                case "tif":
                case "tiff":
                    return "image/tiff";
                case "mpeg":
                    return "video/mpeg";
                case "mp4":
                    return "video/mp4";
                case "pdf":
                    return "application/pdf";
                case "doc":
                case "docx":
                    return "application/msword";
                case "xls":
                case "xlsx":
                    return "application/vnd.ms-excel";
                case "csv":
                    return "text/csv";
                case "txt":
                    return "text/plain";
                case "json":
                    return "application/json";
                case "xml":
                    return "application/xml";
                case "htm":
                case "html":
                    return "text/html";
                case "css":
                    return "text/css";
                case "js":
                    return "application/javascript";
                case "zip":
                    return "application/zip";
            }
            return "";
        }

        public static DownloadResult DownloadFiles(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            string contentType = GetContentType(ext);

            return new FilePathResult(fileName, contentType);
        }

        //    public ActionResult GetPdf(string filename)
        //{
        //    return File(filename, "application/pdf", Server.UrlEncode(filename));
        //}

        public FilePathResult DownloadFiles(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            string contentType = GetContentType(ext);

            return new FilePathResult(fileName, contentType);
        }
    }
    */
    //return new CsvActionResult(dataTableObject) { FileDownloadName = "ExportedFileName.csv" };

    public class DownLoadFileInfo
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }  

    public class DownloadFiles
    {
        public List<DownLoadFileInfo> GetFiles()
        {
            List<DownLoadFileInfo> lstFiles = new List<DownLoadFileInfo>();
            DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/uploads"));

            int i = 0;
            foreach (var item in dirInfo.GetFiles())
            {
                lstFiles.Add(new DownLoadFileInfo()
                {

                    FileId = i + 1,
                    FileName = item.Name,
                    FilePath = dirInfo.FullName + @"\" + item.Name
                });
                i = i + 1;
            }
            return lstFiles;
        }
    }  

    public sealed class CsvActionResult : FileResult
    {

        public static CsvActionResult ExportToCsv(DataTable dt,string name)
        {
            string filename=string.Format("{0}_{1}.csv",name, DateTime.Now.ToString("yyyyMMdd"));
            return new CsvActionResult(dt) { FileDownloadName = filename };
        }

        private readonly DataTable _dataTable;

        public CsvActionResult(DataTable dataTable)
            : base("text/csv")
        {
            _dataTable = dataTable;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            var outputStream = response.OutputStream;
            using (var memoryStream = new MemoryStream())
            {
                WriteDataTable(memoryStream);
                outputStream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
        }

        private void WriteDataTable(Stream stream)
        {
            var streamWriter = new StreamWriter(stream, Encoding.Default);

            WriteHeaderLine(streamWriter);
            streamWriter.WriteLine();
            WriteDataLines(streamWriter);

            streamWriter.Flush();
        }

        private void WriteHeaderLine(StreamWriter streamWriter)
        {
            foreach (DataColumn dataColumn in _dataTable.Columns)
            {
                WriteValue(streamWriter, dataColumn.ColumnName);
            }
        }

        private void WriteDataLines(StreamWriter streamWriter)
        {
            foreach (DataRow dataRow in _dataTable.Rows)
            {
                foreach (DataColumn dataColumn in _dataTable.Columns)
                {
                    WriteValue(streamWriter, dataRow[dataColumn.ColumnName].ToString());
                }
                streamWriter.WriteLine();
            }
        }


        private static void WriteValue(StreamWriter writer, String value)
        {
            writer.Write("\"");
            writer.Write(value.Replace("\"", "\"\""));
            writer.Write("\",");
        }
    }
}