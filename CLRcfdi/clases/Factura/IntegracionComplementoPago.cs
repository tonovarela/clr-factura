using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLRcfdi.clases.Factura
{
    public class IntegracionComplementoPago
    {
        public string serie { get; set; }
        public string idDocumento { get; set; }
        public string folio { get; set; }
        public string monedaRD { get; set; }
        public Nullable<decimal> impSalAnt { get; set; }
        public Nullable<decimal> impPagado { get; set; }
        public Nullable<decimal> impSaldoInsoluto { get; set; }
        public Nullable<int> numParcialidad { get; set; }
        public string metodoPagoDR { get; set; }
        public string idComplementoPago { get; set; }
        public int id_integracionComplementoPago { get; set; }
        public string uuid { get; set; }
        public Nullable<decimal> tipoCambioDR { get; set; }

        public virtual ComplementoPago ComplementoPago { get; set; }
    }
}
