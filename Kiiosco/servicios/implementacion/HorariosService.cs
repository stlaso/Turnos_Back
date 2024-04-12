using Data;
using Kiosco.servicios.contrato;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kiosco.servicios.implementacion
{
    public class HorariosService : IHorarios
    {
        private UsuarioContext _context;

        public HorariosService(UsuarioContext context, IConfiguration config)
        {
            _context = context;
        }



        public async Task<Boolean> PersonaExiste(Horario horario)
        {
            var persona = await _context.Usuarios.Where(p => p.id == horario.usuario_id).FirstOrDefaultAsync();
            if (persona == null)
            {
                return false;
            }
            return true;
        }


        public async Task<Horario> HorarioExiste(Horario horario)
        {
            var horarioExistente = await _context.Horario.FindAsync(horario.id);


            if (horarioExistente == null)
            {
                return horarioExistente;
            }

            return horarioExistente;
        }

        public async Task<Boolean> CompararHorario(Horario horario)
        {
            if (horario.fecha_inicio < horario.fecha_fin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Horario>> BuscarTurnosPersona(int id)
        {
            // Obtener la fecha actual
            DateTime fechaActual = DateTime.Now.Date; // Esto elimina la parte de la hora, minutos y segundos

            // Filtrar los horarios por el ID del usuario y la fecha actual o futura
            var horarios = await _context.Horario.Where(h => h.usuario_id == id && h.fecha_inicio >= fechaActual).ToListAsync();

            return horarios;
        }


        public async Task<(List<object> Horarios, int TotalCount)> TodosLosHorarios(int page, int pageSize)
        {
            var query = _context.Horario.AsQueryable();

            var horariosQuery = query
                .OrderBy(h => h.fecha_inicio)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var horarios = await horariosQuery.ToListAsync();

            var totalCount = await query.CountAsync();

            var horario = await horariosQuery
                .Join(
                    _context.Usuarios,
                    horario => horario.usuario_id,
                    usuario => usuario.id,
                    (horario, usuario) => new
                    {
                        Id = horario.id,
                        FechaInicio = horario.fecha_inicio,
                        FechaFin = horario.fecha_fin,
                        NombreUsuario = usuario.nombre,
                        ApellidoUsuario = usuario.apellido,
                        id_usuario = usuario.id
                    })
                .ToListAsync();

            return (horario.Cast<object>().ToList(), totalCount);
        }
    }
}
