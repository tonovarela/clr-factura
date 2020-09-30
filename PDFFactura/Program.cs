using CLRcfdi.clases;
using CLRcfdi.clases.Factura;
using System;

namespace PDFFactura
{
    class Program
    {
        static void Main(string[] args)
        {


            PDF pdf = new PDF();


            string xml = @"C:\Users\tonovarela\Desktop\f.XML";
            FacturaXML facturaXML = new FacturaXML(xml, true);
            CFDI cfdi = facturaXML.ObtenerData();
            pdf.cfdi = cfdi;
            pdf.obtenerRepresentacionImpresa(new CLRcfdi.models.Cliente()
            {
                NoCliente="Varela"
            });
            

        }
    }
}
