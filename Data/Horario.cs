using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Horario
    {
        public int id { get; set; }
        public int usuario_id {  get; set; }

        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_fin { get; set; }

    }
}
