using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLRcfdi.clases.Factura
{
    public class CFDI
    {
        public CFDI()
        {
            this.CFDIRelacionado = new HashSet<CFDIRelacionado>();
            this.ComplementoPago = new HashSet<ComplementoPago>();
            this.Concepto = new List<Concepto>();
        }

        public string uuid { get; set; }
        public string tipo { get; set; }
        public Nullable<System.DateTime> fechaEmision { get; set; }
        public Nullable<System.DateTime> fechaTimbrado { get; set; }
        public string serie { get; set; }
        public string folio { get; set; }
        public string lugarExpedicion { get; set; }
        public string metodoPago { get; set; }
        public string condicionesPago { get; set; }
        public Nullable<decimal> tipoCambio { get; set; }
        public string moneda { get; set; }
        public Nullable<decimal> subTotal { get; set; }
        public Nullable<decimal> descuento { get; set; }
        public Nullable<decimal> total { get; set; }
        public Nullable<decimal> isrRetenido { get; set; }
        public Nullable<decimal> isrTraslado { get; set; }
        public Nullable<decimal> ivaRetenido { get; set; }
        public Nullable<decimal> ivaTrasladado { get; set; }
        public Nullable<bool> ivaExento { get; set; }
        public Nullable<decimal> baseIvaExento { get; set; }
        public Nullable<bool> ivaTasaCero { get; set; }
        public Nullable<decimal> baseIvaTasaCero { get; set; }
        public Nullable<decimal> totalImpuestosRetenidos { get; set; }
        public Nullable<decimal> totalImpuestosTrasladados { get; set; }
        public string rfcEmisor { get; set; }
        public string nombreEmisor { get; set; }
        public string regimenFiscalEmisor { get; set; }
        public string rfcReceptor { get; set; }
        public string usoCFDI { get; set; }
        public string residenciaFiscal { get; set; }
        public string numRegIdTrib { get; set; }
        public string cfdiRelacionados { get; set; }
        public Nullable<decimal> totalRetencionesLocales { get; set; }
        public Nullable<decimal> totalTrasladosLocales { get; set; }
        public Nullable<decimal> impuestoLocalRetenido { get; set; }
        public string tasaRetencionLocal { get; set; }
        public Nullable<decimal> importeRetencionLocal { get; set; }
        public Nullable<decimal> impuestoLocalTrasladado { get; set; }
        public string tasaTrasladoLocal { get; set; }
        public Nullable<decimal> importeTrasladoLocal { get; set; }
        public string formaPago { get; set; }
        public string nombreReceptor { get; set; }
        public string iepsRetenidoTasa { get; set; }
        public string iepsTrasladadoTasa { get; set; }
        public Nullable<decimal> iepsRetenidoCuota { get; set; }
        public Nullable<decimal> iepsTrasladadoCuota { get; set; }
        public string confirmacion { get; set; }
        public int id_invoice { get; set; }

        public string selloDigital { get; set; }

        public string selloSAT { get; set; }


        public string certificadoCSD { get; set; }

        public string certificadoSerieSAT { get; set; }

        public string rfcProvCertif { get; set; }

        //public virtual Invoice Invoice { get; set; }
        public virtual ICollection<CFDIRelacionado> CFDIRelacionado { get; set; }
        public virtual ICollection<ComplementoPago> ComplementoPago { get; set; }
        public virtual List<Concepto> Concepto { get; set; }
    }
}
