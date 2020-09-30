using CLRcfdi.clases;
using CLRcfdi.clases.Factura;
using CLRcfdi.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLRcfdi
{
    public class CLRFactura
    {

        public static string createFile(
            string contentXML,
            string NoCliente = "",
            string LugarYFechaExpedicion = "",
            string Direccion = "",
            string CP = "",
            string Colonia = "",
            string DelegacionEstado = "",
            string Contacto = "",
            string Telefono = "",
            string Vencimiento = "",
            string DescripcionMetodoPago="",
            string DescripcionFormaPago="",
            string Referencia=""
            )
        {

            Cliente c = new Cliente();
            c.NoCliente = NoCliente;           
            c.LugarYFechaExpedicion = LugarYFechaExpedicion;
            c.Direccion = Direccion;
            c.CP = CP;
            c.Colonia = Colonia;
            c.DelegacionEstado = DelegacionEstado;
            c.Contacto = Contacto;
            c.Telefono = Telefono;
            c.Vencimiento = Vencimiento;
            c.DescripcionFormaPago = DescripcionFormaPago;
            c.DescripcionMetodoPago = DescripcionMetodoPago;
            c.Referencia = Referencia;


            try
            {
                FacturaXML facturaXML = new FacturaXML(contentXML, false);
                CFDI cfdi = facturaXML.ObtenerData();
                PDF pdf = new PDF();
                pdf.cfdi = cfdi;
                pdf.obtenerRepresentacionImpresa(c);
            }catch(Exception e)
            {
                return $"Error al crear archivo en la factura ";
            }
                       
            

            return "Archivo Creado";
        }
    }
}
