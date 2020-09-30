using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLRcfdi.clases.Factura
{
    public  class ComplementoPago
    {
        public ComplementoPago()
        {
            this.IntegracionComplementoPago = new HashSet<IntegracionComplementoPago>();
        }

        public Nullable<System.DateTime> fechaPago { get; set; }
        public string formadePagoP { get; set; }
        public string monedaP { get; set; }
        public Nullable<decimal> tipoCambioP { get; set; }
        public Nullable<decimal> monto { get; set; }
        public string numeroOperacion { get; set; }
        public string rfcEmisorCtaBen { get; set; }
        public string rfcEmisorCtaOrd { get; set; }
        public string uuid { get; set; }
        public string idComplementoPago { get; set; }
        public Nullable<int> id_invoice { get; set; }

        public virtual ICollection<IntegracionComplementoPago> IntegracionComplementoPago { get; set; }
        public virtual CFDI CFDI { get; set; }
    }
}
