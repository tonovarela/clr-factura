using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLRcfdi.clases.Factura
{
    public  class CFDIRelacionado
    {
        public int id_tipoRelacionado { get; set; }
        public string tipoRelacion { get; set; }
        public string uuid { get; set; }
        public string uuidRelacionado { get; set; }
        public Nullable<int> id_invoice { get; set; }

        public virtual CFDI CFDI { get; set; }
    }
}
