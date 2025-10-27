using System;
using System.Collections.Generic;
using System.Text;

namespace Money_Box.Models
{
    public class ResponseError
    {
        #region MyRegion

        private string sMessage;
        private string sMessageDetail;

        #endregion

        #region Constructores

        public ResponseError()
        {
            sMessage = string.Empty;
            sMessageDetail = string.Empty;
        }

        #endregion

        #region Propiedades

        public string Message
        {
            get { return sMessage; }
            set { sMessage = value; }
        }

        public string MessageDetail
        {
            get { return sMessageDetail; }
            set { sMessageDetail = value; }
        }
        #endregion
    }
}
