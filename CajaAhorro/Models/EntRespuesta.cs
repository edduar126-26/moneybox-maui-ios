using System;
using System.Collections.Generic;
using System.Data;

namespace Money_Box.Models
{
    public class EntRespuesta
    {
        #region Miembros

        private bool _bEstatus;
        private object _oGenerico;
        private DataSet _oDataSet;
        private DataTable _oDataTable;

        private string _sTipoMensaje;
        private string _sMensaje;
        private MiExcepcion _oExcepcion;
        private string _sHtmlExcepcion;

        private int _nIdNuevoRegistro;
        private int _nFilasObtenidas;
        private int _nFilasAfectadas;
        Dictionary<string, object> _olOutputParameters;

        #endregion

        #region Constructores

        public EntRespuesta()
        {
            _bEstatus = false;
            _sTipoMensaje = "Text";
            _sMensaje = string.Empty;
            _oGenerico = null;
            _oDataSet = null;
            _oDataTable = null;
            _oExcepcion = new MiExcepcion("", "");
            _sHtmlExcepcion = string.Empty;
            _olOutputParameters = new Dictionary<string, object>();
        }

        #endregion

        #region Propiedades

        public bool Estatus
        {
            get { return _bEstatus; }
            set { _bEstatus = value; }
        }
        public bool Estado
        {
            get { return _bEstatus; }
            set { _bEstatus = value; }
        }
       
        public object Datos
        {
            get { return _oGenerico; }
            set { _oGenerico = value; }
        }

        public DataSet oDataSet
        {
            get { return _oDataSet; }
            set { _oDataSet = value; }
        }

        public DataTable oDataTable
        {
            get { return _oDataTable; }
            set { _oDataTable = value; }
        }

        public int IdNuevoRegistro
        {
            get { return _nIdNuevoRegistro; }
            set { _nIdNuevoRegistro = value; }
        }

        public string TipoMensaje
        {
            get { return _sTipoMensaje; }
            set { _sTipoMensaje = value; }
        }

        public string Mensaje
        {
            get { return _sMensaje; }
            set { _sMensaje = value; }
        }

        public Exception Exception
        {
            set
            {
                _oExcepcion = new MiExcepcion(value.Message, value.StackTrace);
                _sHtmlExcepcion = value.Message + "<br/><br/>" + value.StackTrace;
            }
        }

        public MiExcepcion Excepcion
        {
            get { return _oExcepcion; }
            set
            {
                _oExcepcion = value;
                _sHtmlExcepcion = value.Message + "<br/><br/>" + value.StackTrace;
            }
        }

        public string HtmlExcepcion
        {
            get { return _sHtmlExcepcion; }
        }

        public int FilasObtenidas
        {
            get { return _nFilasObtenidas; }
            set { _nFilasObtenidas = value; }
        }

        public int FilasAfectadas
        {
            get { return _nFilasAfectadas; }
            set { _nFilasAfectadas = value; }
        }

        public Dictionary<string, object> ParametrosRegreso
        {
            get { return _olOutputParameters; }
            set { _olOutputParameters = value; }
        }

        public object Resultado {
            get;
            set;
        }

        [Serializable]
        public class MiExcepcion
        {
            public string Message { get; set; }
            public string StackTrace { get; set; }

            public MiExcepcion(string message, string stackTrace)
            {
                Message = message;
                StackTrace = stackTrace;
            }
        }
        #endregion
    }
}
