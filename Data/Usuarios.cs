using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Usuarios
    {
        public int id { get; set; }

        public string nombre { get; set; }

        public string apellido { get; set;}

        public string usuario { get; set; }

        public string contraseña { get; set; }

        public string rol { get; set; }
    }
}
