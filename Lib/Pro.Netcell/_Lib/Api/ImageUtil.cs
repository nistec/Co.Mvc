using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec;
using Netcell.Lib.Mobile;
using System.IO;
using System.Data;
using Netcell.Data.Client;
using System.Drawing;
using System.Drawing.Drawing2D;
using Netcell.Remoting;

namespace Netcell.Lib
{

    public class ImageUtil
    {

        public static int ImageRatio(string filename, ref int width, ref int height)
        {
            System.Drawing.Image img;
            //System.Drawing.Image thumbnail;

            //Load image from uploaded file stream
            //img = System.Drawing.Image.FromStream(uploadFile.PostedFile.InputStream);

            img = System.Drawing.Image.FromFile(filename);


            //Image variables init
            double img_width = img.Width;
            double img_height = img.Height;
            double img_ratio = img_width / img_height;
            double img_max_width = Convert.ToDouble(width);
            double img_max_height = Convert.ToDouble(height);

            double widthPrc = 0;
            double heightPrc = 0;
            if (img_width > img_height)
            {
                if (img_height > img_max_height)
                {
                    heightPrc = (img_max_height / img_height);
                    img_height = img_max_height;
                    img_width = img_height * img_ratio;
                }
                if (img_width > img_max_width)
                {
                    widthPrc = (img_max_width / img_width);
                    img_width = img_max_width;
                    img_height = img_width / img_ratio;
                }
            }
            else
            {
                if (img_width > img_max_width)
                {
                    widthPrc = (img_max_width / img_width);
                    img_width = img_max_width;
                    img_height = img_width / img_ratio;
                }
                if (img_height > img_max_height)
                {
                    heightPrc = (img_max_height / img_height);
                    img_height = img_max_height;
                    img_width = img_height * img_ratio;
                }
            }

            width = (int)img_width;
            height = (int)img_height;

            if (widthPrc == 0 || heightPrc == 0)
                return (int)(100 * Math.Max(widthPrc, heightPrc));
            else
                return (int)(100 * Math.Min(widthPrc, heightPrc));


        }

        public static ImageItem ImageRatio(int imgWidth, int imgHeight, int maxWidth, int zoom)
        {
            if (maxWidth <= 0)
                maxWidth = WapUtil.DefaultMaxWidth;

            //Image variables init
            double img_width = imgWidth;
            double img_height = imgHeight;
            double img_ratio = img_width / img_height;
            double img_max_width = Convert.ToDouble(maxWidth);
            //double img_max_height = Convert.ToDouble(height);

            if (img_width > img_max_width)
            {
                img_width = img_max_width;
                img_height = (img_width / img_ratio);
            }

            return new ImageItem(null, (int)img_width, (int)img_height, zoom);
        }


        public static string ImgTagFormat(string filename, int maxWidth, int zoom, string upload_path, string vitual_path)
        {

            const string imgTag = "<img src=\"{0}\" alt=\"\" {1} />";
            string ImgTag = "";
            string src = filename;
            string img_path = filename;
            string file_name = System.IO.Path.GetFileName(filename);
            try
            {
                if (filename.ToLower().StartsWith("http://"))
                {
                    //do nothing
                }
                else
                {
                    src = Path.Combine(vitual_path, file_name);
                }
                img_path = Path.Combine(upload_path, file_name);

                System.Drawing.Image img = System.Drawing.Image.FromFile(img_path);

                //Image variables init
                double img_width = img.Width;
                double img_height = img.Height;
                double img_ratio = img_width / img_height;
                double img_max_width = Convert.ToDouble(maxWidth);
                //double img_max_height = Convert.ToDouble(height);
                if (zoom <= 0)
                    zoom = 100;

                float z = (float)zoom / 100;

                if (img_width > img_max_width)
                {
                    img_width = img_max_width * z;
                    img_height = (img_width / img_ratio) * z;
                }
                else
                {
                    img_width = img_width * z;
                    img_height = img_height * z;
                }
                img.Dispose();

                ImgTag = string.Format(imgTag, src, "width=\"" + img_width.ToString() + "px\" height=\"" + img_height + "px\"");

                return ImgTag;
            }
            catch
            {
                return string.Format(imgTag, src, "");
            }
        }

        private static string FormatNewFileName(string filename, int width, int height)
        {
            string file_name = System.IO.Path.GetFileNameWithoutExtension(filename);
            string ext = System.IO.Path.GetExtension(filename);
            return file_name + width.ToString() + "_" + height.ToString() + ext;
        }

        public static ImageItem ImageWapFormat(string sourceFilename, string physicalPath, string virtualPath, int maxWidth, int zoom)
        {
            if (maxWidth <= 0)
                maxWidth = WapUtil.DefaultMaxWidth;

            string file_name = System.IO.Path.GetFileName(sourceFilename);
            string file_path = Path.Combine(physicalPath, file_name);
            string virtual_path = Path.Combine(virtualPath, file_name);


            System.Drawing.Image img = System.Drawing.Image.FromFile(sourceFilename);

            //System.Drawing.Image thumbnail;

            //Load image from uploaded file stream
            //img = System.Drawing.Image.FromStream(uploadFile.PostedFile.InputStream);

            //img = System.Drawing.Image.FromFile(sourceFilename);

            //Image variables init
            double img_width = img.Width;
            double img_height = img.Height;
            double img_ratio = img_width / img_height;
            double img_max_width = Convert.ToDouble(maxWidth);
            //double img_max_height = Convert.ToDouble(height);

            bool isResize = false;
            if (img_width > img_max_width)
            {
                img_width = img_max_width;
                img_height = (img_width / img_ratio);
                isResize = true;
            }
            //if (img_height > img_max_height)
            //{
            //    img_height = img_max_height;
            //    img_width = img_height * img_ratio;
            //}

            string newfilename = FormatNewFileName(file_name, (int)img_width, (int)img_height);

            if (isResize)
            {
                return ImageResize(img, newfilename, physicalPath, virtualPath, (int)img_width, (int)img_height, zoom);
            }
            else if (!sourceFilename.Contains(@"\wap\"))
            {
                file_path = Path.Combine(physicalPath, newfilename);
                virtual_path = Path.Combine(virtualPath, newfilename);
                try
                {
                    img.Save(file_path);
                }
                catch (Exception ex)
                {
                    if (img != null)
                        img.Dispose();
                    throw new AppException("אירעה שגיאה בתהליך שמירת התמונה: " + ex.Message);
                }

            }

            if (img != null)
                img.Dispose();

            //-do not delete File.Delete(file_source_path);

            return new ImageItem(virtual_path, (int)img_width, (int)img_height, zoom);
        }

        private static ImageItem ImageResize(System.Drawing.Image img, string newfilename, string physicalPath, string virtualPath, int img_width, int img_height, int zoom)
        {

            // string newfilename = FormatNewFileName(file_name, (int)img_width, (int)img_height);

            try
            {
                System.Drawing.Image.GetThumbnailImageAbort myCallBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
                //thumbnail = img.GetThumbnailImage((int)img_width,(int)img_height,myCallBack,IntPtr.Zero);

                using (System.Drawing.Image thumbnail = new Bitmap((int)img_width, (int)img_height))
                {
                    using (Graphics oGraphic = Graphics.FromImage(thumbnail))
                    {

                        oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        oGraphic.SmoothingMode = SmoothingMode.HighQuality;
                        oGraphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        oGraphic.CompositingQuality = CompositingQuality.HighQuality;

                        oGraphic.DrawImage(img, 0, 0, (int)img_width, (int)img_height);
                    }

                    string file_path = Path.Combine(physicalPath, newfilename);
                    string virtual_path = Path.Combine(virtualPath, newfilename);

                    thumbnail.Save(file_path);

                    /*
                    //TOD:check this;
                    OctreeQuantizer quantizer = new OctreeQuantizer(255, 8);

                    //DeleteCurrentImg(filename);

                    using (Bitmap bmpQuantized = quantizer.Quantize(thumbnail))
                    {
                        bmpQuantized.Save(file_path);//, ImageFormat.Gif);
                    }
                    */

                    //thumbnail.Dispose();


                    return new ImageItem(virtual_path, img_width, img_height, zoom);
                }

            }
            catch (Exception ex)
            {
                throw new AppException("אירעה שגיאה בהמרת התמונה: " + ex.Message);
            }
            finally
            {
                if (img != null)
                    img.Dispose();
            }
        }

        public static ImageItem ImageFormat(string sourceFilename, string newFilename, int maxWidth, int zoom, string upload_path)
        {
            if (maxWidth <= 0)
                maxWidth = WapUtil.DefaultMaxWidth;

            //string file_extention = System.IO.Path.GetExtension(filename.ToLower());
            string file_name = System.IO.Path.GetFileName(newFilename);
            //string file_name = accountId.ToString() + "_" + file_source_name;
            //string file_path = Server.MapPath(ConfigurationSettings.AppSettings["Img_Path_Upload"]) + file_name;
            //string file_source_path = Path.Combine(upload_path, System.IO.Path.GetFileName(sourceFilename));
            string file_path = Path.Combine(upload_path, file_name);


            System.Drawing.Image img;
            //System.Drawing.Image thumbnail;

            //Load image from uploaded file stream
            //img = System.Drawing.Image.FromStream(uploadFile.PostedFile.InputStream);

            img = System.Drawing.Image.FromFile(sourceFilename);

            //Image variables init
            double img_width = img.Width;
            double img_height = img.Height;
            double img_ratio = img_width / img_height;
            double img_max_width = Convert.ToDouble(maxWidth);
            //double img_max_height = Convert.ToDouble(height);
            bool isResize = false;
            if (img_width > img_max_width)
            {
                img_width = img_max_width;
                img_height = (img_width / img_ratio);
                isResize = true;
            }

            if (sourceFilename.Contains(@"\wap\"))
            {
                img.Dispose();
                return new ImageItem(newFilename, (int)img_width, (int)img_height, zoom);
            }

            //if (img_height > img_max_height)
            //{
            //    img_height = img_max_height;
            //    img_width = img_height * img_ratio;
            //}
            if (isResize)
            {
                try
                {
                    System.Drawing.Image.GetThumbnailImageAbort myCallBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
                    //thumbnail = img.GetThumbnailImage((int)img_width,(int)img_height,myCallBack,IntPtr.Zero);

                    using (System.Drawing.Image thumbnail = new Bitmap((int)img_width, (int)img_height))
                    {
                        using (Graphics oGraphic = Graphics.FromImage(thumbnail))
                        {

                            oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            oGraphic.SmoothingMode = SmoothingMode.HighQuality;
                            oGraphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            oGraphic.CompositingQuality = CompositingQuality.HighQuality;

                            oGraphic.DrawImage(img, 0, 0, (int)img_width, (int)img_height);
                        }
                        thumbnail.Save(file_path);

                        /*
                        //TOD:check this;
                        OctreeQuantizer quantizer = new OctreeQuantizer(255, 8);

                        //DeleteCurrentImg(filename);

                        using (Bitmap bmpQuantized = quantizer.Quantize(thumbnail))
                        {
                            bmpQuantized.Save(file_path);//, ImageFormat.Gif);
                        }
                        */
                        //thumbnail.Dispose();
                    }
                }
                catch (Exception ex)
                {

                    if (img != null)
                        img.Dispose();
                    throw new AppException("אירעה שגיאה בהמרת התמונה: " + ex.Message);

                }
            }
            else
            {
                try
                {
                    img.Save(file_path);
                }
                catch (Exception ex)
                {

                    if (img != null)
                        img.Dispose();
                    throw new AppException("אירעה שגיאה בשמירת התמונה: " + ex.Message);

                }
            }
            if (img != null)
                img.Dispose();

            //-do not delete File.Delete(file_source_path);

            return new ImageItem(file_path, (int)img_width, (int)img_height, zoom);
        }

         private static bool ThumbnailCallback()
        {
            return false;
        }

        private static void DeleteCurrentImg(string filename)
        {

            try
            {
                //Deletes From Server
                File.Delete(filename);
            }
            catch
            {
            }

        }

 
#if(RB)

        public static int FormatImportContent(DataTable dt)
        {
            int count = dt.Columns.Count;

            dt.Columns[0].ColumnName = "ContentName";
            if (count > 1)
                dt.Columns[1].ColumnName = "ContentType";
            if (count > 2)
                dt.Columns[2].ColumnName = "FileName";
            if (count > 4)
                dt.Columns[4].ColumnName = "Path";
            if (count > 5)
                dt.Columns[5].ColumnName = "DefaultExtension";
            return count;

        }

        public int ContentAdd(DataTable dtContent, int accountId, string CategoryName)
        {
            DataTable source = DalContent.Instance.ContentWithSchema(accountId);
            DataTable dt = source.Clone();
            DataView dv = new DataView(source, "", "ContentId", DataViewRowState.CurrentRows);
            List<int> contents = new List<int>();
            int count = 0;
            foreach (DataRow dr in dtContent.Rows)
            {
                try
                {
                    object[] record = new object[6];
                    string contentName = Types.NZ(dr["ContentName"], "");
                    string contentType = Types.NZ(dr["ContentType"], "");
                    string fileName = Types.NZ(dr["FileName"], "");
                    string path = Types.NZ(dr["Path"], "");
                    string ext = Types.NZ(dr["DefaultExtension"], "");

                    if (contentName == "" || contentType == "" || fileName == "" || path == "" || ext == "")
                    {
                        continue;
                    }

                    //if (dv.Find(contactId) > -1)
                    //{
                    //    continue;
                    //}
                    record[0] = contentName;
                    record[1] = contentType;
                    record[2] = accountId;
                    record[3] = fileName;
                    record[4] = path;
                    record[5] = ext;

                    dt.Rows.Add(record);
                    //contacts.Add(contactId);
                    count++;
                }
                catch { }
            }
            try
            {
                DalContent.Instance.InsertTable(dt, "Content");
                //if (!string.IsNullOrEmpty(CategoryName))
                //{
                //    ContactGroupAdd(contents, accountId, CategoryName);
                //}
                return count;
            }
            catch (Exception ex)
            {

                throw new UploadException(ex);
            }

        }
#endif

    }
    public struct ImageItem
    {
        public string Src;
        public int Width;
        public int Height;
        public int Zoom;
        public string Alt;

        public string Href;
        public string Title;
        public string Target;


        public ImageItem(string src, int width, int height, int zoom)
        {
            Src = src;
            Width = width;
            Height = height;
            Zoom = (zoom <= 0) ? 100 : zoom;
            Alt = "";
            Href = "";
            Title = "";
            Target = "";

        }
        public ImageItem(string src, string imgSize, int zoom)
        {
            Src = src;
            if (!string.IsNullOrEmpty(imgSize))
            {
                string[] args = imgSize.Split(':');
                Width = (args.Length > 0) ? Types.ToInt(args[0], 0) : 0;
                Height = (args.Length > 1) ? Types.ToInt(args[1], 0) : 0;
            }
            else
            {
                Width = 0;
                Height = 0;
            }
            Zoom = (zoom <= 0) ? 100 : zoom;
            Alt = "";
            Href = "";
            Title = "";
            Target = "";

        }

        public string ToHtml()
        {
            string img = string.Format("<img src=\"{0}\" alt=\"{1}\" {2}{3} />", Src, Alt, Width > 0 ? " width=\"" + Width.ToString() + "px\"" : "", Height > 0 ? " height=\"" + Height.ToString() + "px\"" : "");

            if (!string.IsNullOrEmpty(Href))
            {
                if (string.IsNullOrEmpty(Target))
                    Target = "_blank";
                return string.Format("<a href=\"{0}\" title=\"{1}\" target=\"{2}\">{3}</a>", Href, Title, Target, img);
            }
            return img;
        }
    }


    public struct LinkElement
    {
        public string Href;
        public string Title;
        public string Target;
        public string Value;

        public LinkElement(string href, string value, string target, string title)
        {
            Href = href;
            Title = title;
            Target = target;
            Value = value;
        }

        public string ToHtml()
        {
            if (string.IsNullOrEmpty(Target))
                Target = "_blank";
            return string.Format("<a href=\"{0}\" title=\"{1}\" target=\"{2}\">{3}</a>", Href, Title, Target, Value);
        }
    }
}
