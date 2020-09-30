using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLRcfdi.models
{
    public class Cliente
    {

        public string NoCliente { get; set; }
        public string LugarYFechaExpedicion { get; set; }
        public string Direccion { get; set; }
        public string CP { get; set; }
        public string Colonia { get; set; }
        public string DelegacionEstado { get; set; }
        public string Contacto { get; set; }
        public string Telefono { get; set; }
        public string Vencimiento { get; set; }
        public string DescripcionMetodoPago { get; set; }
        public string DescripcionFormaPago { get; set; }
        public string Referencia { get; set; }
        
    }
}
