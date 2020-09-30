using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLRcfdi.clases.Factura
{
    public class Concepto
    {
        public int id_concepto { get; set; }

        public decimal precioUnitario { get; set; }


        public string claveUnidad { get; set; }
        public string unidad { get; set; }
        public string noIdentificacion { get; set; }
        public decimal cantidad { get; set; }
        public Nullable<decimal> importe { get; set; }
        public Nullable<decimal> valorUnitario { get; set; }
        public string descripcion { get; set; }
        public string claveProdServ { get; set; }
        public string uuid { get; set; }
        public Nullable<int> id_invoice { get; set; }

        public virtual CFDI CFDI { get; set; }
    }
}
