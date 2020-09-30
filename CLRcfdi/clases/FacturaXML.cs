using CLRcfdi.clases.Factura;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CLRcfdi.clases
{
    public class FacturaXML
    {
        protected XDocument xDocument;
        public FacturaXML(Stream stream)
        {
            this.xDocument = XDocument.Load(stream);
        }


        public FacturaXML(string xml, bool uri = false)
        {
            if (uri)
            {
                this.xDocument = XDocument.Load(xml);
            }
            else
            {
                this.xDocument = XDocument.Parse(xml);
            }

        }
        private string obtenerUltimoNodo(string elemento, string atributoValor)
        {
            string valor = this.xDocument
                         .Root
                         .Descendants()
                         .Where(r => r.Name.LocalName.ToUpper() == elemento)
                         .Select(c => new
                         {
                             atributos = c.Attributes(),
                             elemento = c
                         })
                        .Where(c => c.atributos.Any(x => x.Name.ToString().ToUpper() == atributoValor))
                        .Select(z => z.elemento.LastAttribute.Value)
                        .FirstOrDefault();

            if (valor == null)
            {
                return "0";
            }
            return valor;
        }
        protected string obtenerInfo(string elemento, string atributoValor, int apariencia = 1, bool esRoot = false)
        {
            XElement xelement = null;
            IEnumerable<XElement> elementos;
            elementos = this.xDocument
                           .Root
                           .Descendants()
                           .Where(r => r
                                       .Name
                                       .LocalName
                                       .ToUpper() == elemento);


            if (esRoot)
            {
                xelement = this.xDocument.Root;
            }
            else
            {

                if (apariencia == 1)
                {
                    xelement = elementos.FirstOrDefault();
                }
                else
                {
                    xelement = elementos.Skip(apariencia - 1).FirstOrDefault();
                }
            }

            XAttribute xatributte = null;
            if (xelement != null && xelement.HasAttributes)
            {
                xatributte = xelement.Attributes().Where(x => x.Name.ToString().ToUpper() == atributoValor).FirstOrDefault();
            }
            return xatributte == null ? "" : xatributte.Value;
        }
        private string obtenerValorAtributo(XElement xelement, string nombre)
        {
            var atributos = xelement.Attributes();
            var valor = atributos.Where(e => e.Name.ToString().ToUpper() == nombre.ToUpper()).FirstOrDefault();
            if (valor == null)
            {
                return "";
            }
            return valor.Value;
        }
        private List<IntegracionComplementoPago> obtenerDoctoRelacionado(XElement padre, string id, string uuid)
        {
            List<IntegracionComplementoPago> integracionComplementoPagosList = new List<IntegracionComplementoPago>();
            List<XElement> dtRelacionado = padre.Descendants()
                                                  .ToList()
                                                  .Where(r => r.Name.LocalName.ToUpper() == "DOCTORELACIONADO")
                                                  .ToList();

            dtRelacionado.ForEach(dtRel =>
            {

                string tipoCambioDR = obtenerValorAtributo(dtRel, "TipoCambioDR");
                string imPagado = obtenerValorAtributo(dtRel, "ImpPagado");
                string impSaldoAnterior = obtenerValorAtributo(dtRel, "ImpSaldoAnt");
                string numParcialidad = obtenerValorAtributo(dtRel, "NumParcialidad");
                string impSaldoInsoluto = obtenerValorAtributo(dtRel, "ImpSaldoInsoluto");

                integracionComplementoPagosList.Add(new IntegracionComplementoPago()
                {
                    folio = obtenerValorAtributo(dtRel, "Folio"),
                    idComplementoPago = id,
                    idDocumento = obtenerValorAtributo(dtRel, "IdDocumento").ToLower(),
                    impPagado = imPagado == "" ? 0 : Decimal.Parse(imPagado),
                    impSalAnt = impSaldoAnterior == "" ? 0 : Decimal.Parse(obtenerValorAtributo(dtRel, "ImpSaldoAnt")),
                    metodoPagoDR = obtenerValorAtributo(dtRel, "MetodoDePagoDR"),
                    monedaRD = obtenerValorAtributo(dtRel, "MonedaDR"),
                    impSaldoInsoluto = impSaldoInsoluto == "" ? 0 : Decimal.Parse(impSaldoInsoluto),
                    numParcialidad = numParcialidad == "" ? 1 : Int32.Parse(numParcialidad),
                    serie = obtenerValorAtributo(dtRel, "Serie"),
                    tipoCambioDR = tipoCambioDR == "" ? 1 : Decimal.Parse(tipoCambioDR),
                    uuid = uuid,
                });
            });

            return integracionComplementoPagosList;
        }
        private List<ComplementoPago> obtenerComplementoPago()
        {
            List<ComplementoPago> resultado = new List<ComplementoPago>();
            List<XElement> xelements = xDocument.Root.Descendants().ToList().Where(r => r.Name.LocalName.ToUpper() == "PAGO").ToList();
            if (xelements.Count == 0)
            {
                return resultado;
            }
            string uuid = this.obtenerInfo("TIMBREFISCALDIGITAL", "UUID");
            int x = 0;
            xelements.ForEach(xel =>
            {
                string t = obtenerValorAtributo(xel, "TipoCambioP");
                x++;
                string idComplementoPago = $"{System.Guid.NewGuid()}";
                List<IntegracionComplementoPago> integracionComplementoPagosList = this.obtenerDoctoRelacionado(xel, idComplementoPago, uuid);

                resultado.Add(new ComplementoPago()
                {
                    fechaPago = this.castToDate(obtenerValorAtributo(xel, "FechaPago")),
                    formadePagoP = obtenerValorAtributo(xel, "FormaDePagoP"),
                    monto = Decimal.Parse(obtenerValorAtributo(xel, "Monto")),
                    monedaP = obtenerValorAtributo(xel, "MonedaP"),
                    numeroOperacion = obtenerValorAtributo(xel, "NumOperacion"),
                    rfcEmisorCtaBen = obtenerValorAtributo(xel, "RfcEmisorCtaBen"),
                    rfcEmisorCtaOrd = obtenerValorAtributo(xel, "RfcEmisorCtaOrd"),
                    tipoCambioP = Decimal.Parse(t == "" ? "1" : t),
                    uuid = uuid,
                    idComplementoPago = idComplementoPago,
                    IntegracionComplementoPago = integracionComplementoPagosList,
                    //id_invoice = this._idInvoice
                });
            }
           );

            return resultado;
        }
        private List<CFDIRelacionado> obtenerCFDIRelacionados()
        {
            List<CFDIRelacionado> resultado = new List<CFDIRelacionado>();
            List<XElement> xelements = xDocument.Root.Descendants().ToList().Where(r => r.Name.LocalName.ToUpper() == "CFDIRELACIONADO").ToList();
            if (xelements.Count == 0)
            {
                return resultado;
            }
            string uuid = this.obtenerInfo("TIMBREFISCALDIGITAL", "UUID");
            string tipoRelacion = this.obtenerInfo("CFDIRELACIONADOS", "TIPORELACION");
            xelements.ForEach(xel =>
            {
                resultado.Add(new CFDIRelacionado()
                {
                    uuidRelacionado = xel.Attributes().Where(x => x.Name.ToString().ToUpper() == "UUID").FirstOrDefault().Value,
                    tipoRelacion = tipoRelacion,
                    uuid = uuid,
                    // id_invoice = this._idInvoice
                });
            }
            );
            return resultado;
        }
        private List<Concepto> obtenerConceptos()
        {

            List<Concepto> resultado = new List<Concepto>();
            List<XElement> xelements = xDocument.Root.Descendants().ToList().Where(r => r.Name.LocalName.ToUpper() == "CONCEPTO").ToList();
            if (xelements.Count == 0)
            {
                return resultado;
            }
            string uuid = this.obtenerInfo("TIMBREFISCALDIGITAL", "UUID");
            xelements.ForEach(x =>
            {

                string c = obtenerValorAtributo(x, "Cantidad");
                string importe = obtenerValorAtributo(x, "Importe");
                string valorUnitario = obtenerValorAtributo(x, "ValorUnitario");
                string descripcion = obtenerValorAtributo(x, "Descripcion");
                string precionUnitario = obtenerValorAtributo(x, "ValorUnitario");
                if (descripcion.Length > 255)
                {
                    descripcion = descripcion.Substring(0, 254);
                }
                resultado.Add(new Concepto()
                {
                    cantidad = Decimal.Parse(c),
                    precioUnitario = Decimal.Parse(precionUnitario),
                    claveProdServ = obtenerValorAtributo(x, "ClaveProdServ"),
                    claveUnidad = obtenerValorAtributo(x, "ClaveUnidad"),
                    uuid = uuid,
                    descripcion = descripcion,
                    noIdentificacion = obtenerValorAtributo(x, "NoIdentificacion"),
                    unidad = obtenerValorAtributo(x, "Unidad"),
                    importe = importe == "" ? 0 : Decimal.Parse(importe),
                    valorUnitario = valorUnitario == "" ? 0 : Decimal.Parse(valorUnitario),
                    //id_invoice = this._idInvoice
                });
            });

            return resultado.Where(x => x.claveProdServ.Length > 0).ToList();
        }
        protected DateTime castToDate(string value)
        {
            return DateTimeOffset.Parse(value).DateTime;
        }
        protected Decimal castToDecimal(string value)
        {
            Decimal result = 0;
            Decimal.TryParse(value, out result);
            return result;
        }

        public CFDI ObtenerData()
        {

            CFDI cfdi = new CFDI();
            cfdi.uuid = this.obtenerInfo("TIMBREFISCALDIGITAL", "UUID");
            //cfdi.id_invoice = this._idInvoice;

            cfdi.folio = this.obtenerInfo("", "FOLIO", 1, true);
            cfdi.fechaEmision = castToDate(this.obtenerInfo("", "FECHA", 1, true));
            cfdi.descuento = 0;
            string descuento = String.Empty;
            descuento = this.obtenerInfo("", "DESCUENTO", 1, true);
            if (!descuento.Equals(""))
            {
                cfdi.descuento = castToDecimal(descuento);
            }
            cfdi.fechaTimbrado = castToDate(this.obtenerInfo("TIMBREFISCALDIGITAL", "FECHATIMBRADO"));
            cfdi.condicionesPago = this.obtenerInfo("", "CONDICIONESDEPAGO", 1, true);
            cfdi.metodoPago = this.obtenerInfo("", "METODOPAGO", 1, true);
            cfdi.moneda = this.obtenerInfo("", "MONEDA", 1, true);
            cfdi.tipoCambio = castToDecimal(obtenerInfo("", "TIPOCAMBIO", 1, true));
            cfdi.lugarExpedicion = this.obtenerInfo("", "LUGAREXPEDICION", 1, true);
            cfdi.regimenFiscalEmisor = this.obtenerInfo("EMISOR", "REGIMENFISCAL");
            cfdi.nombreEmisor = this.obtenerInfo("EMISOR", "NOMBRE");
            cfdi.rfcEmisor = this.obtenerInfo("EMISOR", "RFC");
            cfdi.rfcReceptor = this.obtenerInfo("RECEPTOR", "RFC");
            cfdi.nombreReceptor = this.obtenerInfo("RECEPTOR", "NOMBRE");
            cfdi.tipo = this.obtenerInfo("", "TIPODECOMPROBANTE", 1, true);
            cfdi.totalImpuestosTrasladados = Decimal.Parse(obtenerUltimoNodo("IMPUESTOS", "TOTALIMPUESTOSTRASLADADOS"));
            cfdi.ivaTrasladado = Decimal.Parse(obtenerUltimoNodo("IMPUESTOS", "TOTALIMPUESTOSTRASLADADOS"));
            cfdi.usoCFDI = this.obtenerInfo("RECEPTOR", "USOCFDI");

            cfdi.serie = this.obtenerInfo("", "SERIE", 1, true);
            cfdi.subTotal = castToDecimal(this.obtenerInfo("", "SUBTOTAL", 1, true));
            cfdi.numRegIdTrib = this.obtenerInfo("RECEPTOR", "NUMREGIDTRIB");
            cfdi.residenciaFiscal = this.obtenerInfo("RECEPTOR", "RESIDENCIAFISCAL");
            cfdi.total = castToDecimal(this.obtenerInfo("", "TOTAL", 1, true));
            //cfdi.baseIvaTasaCero = obtenerTasaCero();
            cfdi.ivaTasaCero = cfdi.baseIvaTasaCero > 0 ? true : false;
            cfdi.formaPago = this.obtenerInfo("", "FORMAPAGO", 1, true);
            cfdi.certificadoSerieSAT = this.obtenerInfo("", "", 1, true);
            cfdi.selloDigital = this.obtenerInfo("TIMBREFISCALDIGITAL", "SELLOCFD"); ;
            cfdi.selloSAT = this.obtenerInfo("TIMBREFISCALDIGITAL", "SELLOSAT");
            cfdi.certificadoSerieSAT = this.obtenerInfo("TIMBREFISCALDIGITAL", "NOCERTIFICADOSAT");
            cfdi.certificadoCSD = this.obtenerInfo("", "NOCERTIFICADO", 1, true);
            cfdi.rfcProvCertif = this.obtenerInfo("TIMBREFISCALDIGITAL", "RFCPROVCERTIF");





            #region Propiedades no identificadas hasta ahora en el XML
            //cxc.iepsRetenidoTasa = "0";
            //cxc.iepsTrasladadoTasa = "0";
            //cxc.iepsRetenidoCuota = 0;
            //cxc.iepsTrasladadoCuota = 0;
            //cxc.totalImpuestosRetenidos = 0;
            //cxc.isrRetenido = 0;
            //cxc.isrTraslado = 0;
            //cxc.ivaRetenido = 0;
            //cxc.ivaTrasladado = 0;
            //cxc.baseIvaExento = 0;
            //cxc.ivaExento = false;
            //cxc.totalRetencionesLocales = 0;
            //cxc.totalTrasladosLocales = 0;
            //cxc.impuestoLocalRetenido = 0;
            //cxc.tasaRetencionLocal = "";
            //cxc.importeRetencionLocal = 0;
            //cxc.impuestoLocalTrasladado = 0;
            //cxc.tasaTrasladoLocal = "";
            //cxc.importeTrasladoLocal = 0;
            //cxc.confirmacion=''
            #endregion
            cfdi.CFDIRelacionado = this.obtenerCFDIRelacionados();
            cfdi.ComplementoPago = this.obtenerComplementoPago();
            if (cfdi.CFDIRelacionado.Count > 0)
            {
                cfdi.cfdiRelacionados = $"{cfdi.CFDIRelacionado.First().tipoRelacion}," +
                                        $"{String.Join(",", cfdi.CFDIRelacionado.Select(x => x.uuidRelacionado).ToArray())}";
            }
            cfdi.Concepto = this.obtenerConceptos();



            //Console.WriteLine(cfdi.Concepto.ToList().Sum(x=>x.importe));


            return cfdi;
        }
    }
}
