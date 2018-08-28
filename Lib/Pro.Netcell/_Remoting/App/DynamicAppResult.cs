using System;
using System.Text;


namespace Netcell.Remoting
{

    #region interface
    /// <summary>
    /// Summary description for IAppResult.
    /// </summary>
    public interface IDynamicAppResult
    {
        /// <summary>
        /// Result
        /// </summary>
        string Result { get; }
        /// <summary>
        /// Status 0=success -1=failed 1=success without sender
        /// </summary>
        int Status { get;}
        /// <summary>
        /// Status 0=success -1=failed 1=success without sender
        /// </summary>
        int ResultId { get;}
        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        string ToString();
        /// <summary>
        /// Serialize
        /// </summary>
        /// <returns></returns>
        string Serialize();

    }
    #endregion

    #region enum

    public enum DynamicAppResultState
    {
        /// <summary>
        /// Time out
        /// </summary>
        Timeout = -2,
        /// <summary>
        /// Error
        /// </summary>
        Error=-1,
        /// <summary>
        /// Success
        /// </summary>
        Ok=0,
        /// <summary>
        /// Success withot sender
        /// </summary>
        Register=1
    }

    #endregion

    /// <summary>
	/// Summary description for AppResult.
	/// </summary>
    [Serializable]
    public class DynamicAppResult : IDynamicAppResult
	{
		#region ctor
		private DynamicAppResult(){}

        public DynamicAppResult(string result, int resultId)
        {
            _Result = result;
            _ResultId = resultId;
            _Status = (int)DynamicAppResultState.Ok;
        }

        public DynamicAppResult(int status, string result, int resultId)
		{
            _Status = status;
            _Result = result;
            _ResultId = resultId;
		}

		#endregion

		#region members

		private int _Status;
        private string _Result = null;
        private int _ResultId;

		#endregion

		#region property

        public string Result 
        {
            get { return _Result; }
            set { _Result = value; }
        }
 		public int Status 
		{
			get{return _Status;}
			set{_Status=value;}
		}
       
        public int ResultId
        {
            get { return _ResultId; }
            set { _ResultId = value; }
        }
		#endregion


        public string Serialize()
        {
            //return Nistec.Runtime.Serialization.SerializeToBase64(this);
            return Nistec.Xml.XSerializer.Serialize(this);//, this.GetType());
        }

        public static DynamicAppResult Desrialize(string xml)
        {
            //return (AppResult)Nistec.Runtime.Serialization.DeserializeFromBase64(base64);
            object obj = Nistec.Xml.XSerializer.Deserialize(xml, typeof(DynamicAppResult));
            return (DynamicAppResult)obj;

        }

        public override string ToString()
        {
            //result format= Result:{key=value;key=value}
            return string.Format("Status:{0},ResultId:{2},Result:{3}", Status, ResultId, Result);
        }

	}

}
