using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Pro.Data;
using Nistec.Data;

namespace Pro.Data.Entities.Props
{
    public class GenderExtension 
    {
        static IList<GenderView> _list;
        public static IEnumerable<GenderView> GenderList()
        {
            if (_list == null)
            {
                _list = new List<GenderView>();
                _list.Add(new GenderView() { PropId="U", PropName="לא ידוע" });
                _list.Add(new GenderView() { PropId = "M", PropName = "זכר" });
                _list.Add(new GenderView() { PropId = "F", PropName = "נקבה" });
            }
            return _list;
        }
    }
    public class GenderView : IEntityItem
    {

        #region properties

        public string PropId { get; set; }
        public string PropName { get; set; }

        #endregion

    }
 
}
