using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Linq;

namespace Netcell.Lib
{
    /// <summary>
    /// Summary description for WapCss
    /// </summary>
    public class ApiCss
    {
        public string color;
        public string bgColor;
        public string borderColor;
        public string align;
        public string textAlign;
        public string fontSize;
        public string fontWeight;
        public string fontStyle;
        public string fontFamily;
        public string direction="";

        public System.Drawing.Color ForeColor;
        public System.Drawing.Color BackColor;
        //public System.Web.UI.MobileControls.Alignment Alignment;
        //public System.Web.UI.MobileControls.FontSize FontSize;
        //public System.Web.UI.MobileControls.BooleanOption Bold;
        //public System.Web.UI.MobileControls.BooleanOption Italic;

        public bool IsDefined(string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        public static string[] Fonts = new string[]{
            "Arial","Tahoma","Verdana","helvetica","sans-serif","Times New Roman"
        };
        public static string[] FontsSize = new string[]{
            "10px","12px","16px"
        };
        public static string[] FontsSizeValues = new string[]{
           "Small", "10px","Normal","12px","Large","16px"
        };

        public static string[] FontsSizePx = new string[]{
           "10", "10px","12","12px","14","14px","16","16px","18","18px","20","20px","24","24px","28","28px","32","32px","36","36px","40","40px","48","48px"
        };

        public string GetValidFont()
        {
            if (Fonts.Contains(fontFamily))
                return fontFamily;
            return Fonts[0];
        }
        public string GetValidFontSize()
        {
            if (Fonts.Contains(fontSize))
                return fontSize;
            return Fonts[1];
        }

        public bool IsEmpty
        {
            get { return isEmpty; }
        }
        private bool isEmpty;

        public ApiCss(string backColor, string forecolor)
        {
            bgColor = backColor;
            color = forecolor;
        }
        public ApiCss(string style)
        {

            isEmpty = true;
            try
            {
                if (string.IsNullOrEmpty(style))
                    return;
                string[] el = style.Split('=');
                string els = el.Length > 1 ? el[1] : el[0];
                string[] args = els.Split(';');

                foreach (string s in args)
                {
                    string[] cs = s.Split(':');
                    switch (cs[0].Trim().ToLower())
                    {
                        case "background-color":
                            bgColor = cs[1];
                            BackColor = Nistec.Drawing.ColorUtils.StringToColor(cs[1]);
                            isEmpty = false;
                            break;
                        case "color":
                            color = cs[1];
                            ForeColor = Nistec.Drawing.ColorUtils.StringToColor(cs[1]);
                            isEmpty = false;
                            break;
                        case "border-color":
                            borderColor = cs[1];
                            isEmpty = false;
                            break;
                        case "align":
                            align = cs[1];
                            //Alignment = ParseAlignment(cs[1]);
                            isEmpty = false;
                            break;
                        case "text-align":
                            textAlign = cs[1];
                            //Alignment = ParseAlignment(cs[1]);
                            isEmpty = false;
                            break;
                        case "font-size":
                            fontSize = cs[1];
                            //if (cs[1].ToLower().Contains("large"))
                            //    FontSize = System.Web.UI.MobileControls.FontSize.Large;
                            //else if (cs[1].ToLower().Contains("medium"))
                            //    FontSize = System.Web.UI.MobileControls.FontSize.Normal;
                            //else if (cs[1].ToLower().Contains("small"))
                            //    FontSize = System.Web.UI.MobileControls.FontSize.Small;
                            //else
                            //    FontSize = System.Web.UI.MobileControls.FontSize.NotSet;
                            isEmpty = false;
                            break;
                        case "font-weight":
                            fontWeight = cs[1];
                            //if (cs[1].ToLower().Contains("bold"))
                            //{
                            //    Bold = System.Web.UI.MobileControls.BooleanOption.True; break;
                            //}
                            isEmpty = false;
                            break;
                        case "font-style":
                            fontStyle = cs[1];
                            //if (cs[1].ToLower().Contains("italic"))
                            //{
                            //    Italic = System.Web.UI.MobileControls.BooleanOption.True; break;
                            //}
                            isEmpty = false;
                            break;
                        case "font-family":
                            fontFamily = cs[1];
                            isEmpty = false;
                            break;
                        case "direction":
                            direction = cs[1];
                            isEmpty = false;
                            break;
                    }

                }
            }
            catch { }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(color))
                sb.AppendFormat("color:{0};", color);
            if (!string.IsNullOrEmpty(bgColor))
                sb.AppendFormat("background-color:{0};", bgColor);
            if (!string.IsNullOrEmpty(borderColor))
                sb.AppendFormat("border:solid 1px {0};", borderColor);
            if (!string.IsNullOrEmpty(align))
                sb.AppendFormat("align:{0};", align);
            if (!string.IsNullOrEmpty(textAlign))
                sb.AppendFormat("text-align:{0};", textAlign);
            if (!string.IsNullOrEmpty(fontSize))
                sb.AppendFormat("font-size:{0};", fontSize);
            if (!string.IsNullOrEmpty(fontWeight))
                sb.AppendFormat("font-weight:{0};", fontWeight);
            if (!string.IsNullOrEmpty(fontStyle))
                sb.AppendFormat("font-style:{0};", fontStyle);
            if (!string.IsNullOrEmpty(fontStyle))
                sb.AppendFormat("font-family:{0};", fontFamily);
            return sb.ToString();
        }

        //public static System.Web.UI.MobileControls.Alignment ParseAlignment(string align)
        //{
        //    switch (align.ToLower())
        //    {
        //        case "left":
        //            return System.Web.UI.MobileControls.Alignment.Left;
        //        case "center":
        //            return System.Web.UI.MobileControls.Alignment.Center;
        //        case "right":
        //            return System.Web.UI.MobileControls.Alignment.Right;
        //        default:
        //            return System.Web.UI.MobileControls.Alignment.NotSet;
        //    }
        //}

    }
}