using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;

namespace Netcell.Lib
{
    public delegate void UploadFilesComletedEventHandler(UploadFilesComletedEventArgs e);

    public class UploadFilesComletedEventArgs : EventArgs
    {
        UploadItem[] items;
        //string key;

        public UploadFilesComletedEventArgs(UploadItem[] items)
        {
            this.items = items;
        }

        public UploadItem[] UploadItems
        {
            get { return items; }
        }
        public int Count
        {
            get
            {
                if (items == null)
                    return 0;
                return items.Length;
            }
        }
    }


    public delegate void UploadImageComletedEventHandler(UploadImageComletedEventArgs e);

    public class UploadImageComletedEventArgs : EventArgs
    {
        DataTable items;
        
        public UploadImageComletedEventArgs(DataTable items)
        {
            this.items = items;
        }

        public DataTable UploadItems
        {
            get { return items; }
        }
        public int Count
        {
            get
            {
                if (items == null)
                    return 0;
                return items.Rows.Count;
            }
        }
    }


    public delegate void UploadBarcodeComletedEventHandler(UploadBarcodeComletedEventArgs e);

    public class UploadBarcodeComletedEventArgs : EventArgs
    {
        Barcode item;

        public UploadBarcodeComletedEventArgs(Barcode item)
        {
            this.item = item;
        }

        public Barcode Barcode
        {
            get { return item; }
        }

    }


}
