using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Money_Box.Models;

namespace Money_Box.IService
{
    public interface IPromesasService
    {
        Task<EntRespuesta> GetConceptos(int idConceptoCaja);
        Task<EntRespuesta> GetPromesasXPrestamo(long idMovimiento);
        Task<EntRespuesta> InsertPromesa(int folio, decimal cantidad, int idConcepto, int idSocio, int anio);
    }
}
