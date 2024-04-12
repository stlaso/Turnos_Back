using Data;
using Kiosco.servicios.contrato;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kiosco.servicios.contrato;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kiosco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorariosController : ControllerBase
    {


        private UsuarioContext _context;
        private readonly IHorarios _Ihorarios;


        public HorariosController(UsuarioContext context, IHorarios Ihorarios)
        {
            _context = context;
            _Ihorarios = Ihorarios;

        }


        //Ver los  turnos por persona especifica 
        [HttpGet("Turnos/{id}/")]
        public async Task<IActionResult> HorariosPersona(int id)
        {
            try
            {
                var horarios=await _Ihorarios.BuscarTurnosPersona(id);
                if (horarios == null)
                {
                    return Ok("No se encontraron horarios para el empleado");
                }

                return Ok(horarios);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener los horarios: {ex.Message}");
            }
        }


        // Trae todos los turnos y los pagina
        [Authorize(Policy = "adminpolicy")]
        [HttpGet("TodosLosHorarios")]
        public async Task<IActionResult> TodosLosHorarios([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {

                var (horarios, totalCount) = await _Ihorarios.TodosLosHorarios(page, pageSize);

                if (horarios == null || horarios.Count == 0)
                {
                    return Ok("No se encontraron horarios para el empleado");
                }

                return Ok(new { Horarios = horarios, TotalCount = totalCount });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener los horarios: {ex.Message}");
            }
        }


        [Authorize(Policy = "adminpolicy")]
        //Crear horario
        [HttpPost("CrearHorario")]
        public async Task<IActionResult> CrearHorario(Horario horario)
        {
            try
            {

                var personaExiste=await _Ihorarios.PersonaExiste(horario);

                if (personaExiste == false)
                {
                    return BadRequest("La persona asignada no existe");
                }

                var comparar=await _Ihorarios.CompararHorario(horario);

                if( comparar==false)
                {
                    return BadRequest("El horario de inicio no puede ser menor que el final");
                }

                _context.Horario.Add(horario);
                await _context.SaveChangesAsync();


                return Ok("Horario creado exitosamente."); // Devuelve un mensaje de éxito
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear el horario: {ex.Message}"); // Maneja cualquier error
            }
        }


        [Authorize(Policy = "adminpolicy")]
        //Edita los horarios
        [HttpPut("EditarHorario")]
        public async Task<IActionResult> ModificarHorario( Horario horario)
        {
            try
            {

                var horarioActual = await _Ihorarios.HorarioExiste(horario);
                if (horarioActual == null)
                {
                    return BadRequest("Horario no encontrado."); // Devuelve NotFound si el horario no existe
                }


                var personaExiste = await _Ihorarios.PersonaExiste(horario);

                if (personaExiste == false)
                {
                    return BadRequest("La persona asignada no existe"); //Devuelve si la persona no existe
                }

                // Actualiza las propiedades del horario
                horarioActual.fecha_inicio = horario.fecha_inicio;
                horarioActual.fecha_fin = horario.fecha_fin;
                await _context.SaveChangesAsync();

                return Ok("Horario modificado exitosamente."); 
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al modificar el horario: {ex.Message}"); // Maneja cualquier error
            }
        }

        [Authorize(Policy = "adminpolicy")]
        // DELETE api/Horario/5
        [HttpDelete("Borrar/{id}")]
        public async Task<IActionResult> DeleteHorario(int id)
        {
            try
            {
                var horario = await _context.Horario.FindAsync(id);

                if (horario == null)
                {
                    return NotFound("El horario no existe."); // Devuelve NotFound si el horario no existe
                }

                _context.Horario.Remove(horario);
                await _context.SaveChangesAsync();

                return Ok("Horario eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar el horario: {ex.Message}"); // Maneja cualquier error
            }
        }



    }
    




}



