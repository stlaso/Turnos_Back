using Data;
using Kiosco.servicios.contrato;
using Kiosco.servicios.implementacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kiosco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private UsuarioContext _context;
        private readonly Ilogin _Ilogin;

        public LoginController(UsuarioContext context, Ilogin ilogin)
        {
            _context = context;
            _Ilogin = ilogin;

        }


        [HttpGet("Empleados")]
        public async Task<IActionResult> ObtenerEmpleados()
        {
            try
            {
                var empleados = await _context.Usuarios.ToListAsync();

                return Ok(empleados);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "adminpolicy")]
        [HttpPost ("Registrar")]
        public async Task<ActionResult<Usuarios>> Registrar(Usuarios registro)
        {
            try
            {
                var usuarioRegistrado = await _Ilogin.RegistrarUsuario(registro);
                return Ok("USUARIO CREADO");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost ("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            string usuario_encontrado = await _Ilogin.GetUsuario(login.usuario, login.contraseña);
            if (usuario_encontrado=="")
            {
                return BadRequest("Usuario no encontrado");
            }
            if (usuario_encontrado != null)
            {
                return Ok(usuario_encontrado);
            }

            return NotFound();
        }


        [Authorize(Policy = "adminpolicy")]
        [HttpPut ("Editar")]
        public async Task<ActionResult> Edit(Usuarios usuarioEditado)
        {

         
            if (usuarioEditado == null)
            {
                return BadRequest("Los datos del usuario son nulos."); 
            }


            var usuarioExistente = await _Ilogin.EditarUsuario(usuarioEditado);

            if (usuarioExistente == null)
            {
                return NotFound("El usuario no existe."); 
            }

            usuarioExistente.nombre=usuarioEditado.nombre;
            usuarioExistente.apellido=usuarioEditado.apellido;
            usuarioExistente.usuario= usuarioEditado.usuario;
            usuarioExistente.rol = usuarioEditado.rol;

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Usuario actualizado correctamente"); // Éxito al actualizar
            }
            catch (DbUpdateException)
            {
                return BadRequest("Error al actualizar el usuario"); // Error al actualizar
            }
        }

        [Authorize(Policy = "adminpolicy")]
        [HttpDelete("Borrar/{id}")]
        public async Task<IActionResult> BorrarUsuario(int id)
        {
            
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound("El usuario no existe."); // Devuelve un NotFound si el usuario no existe
            }

            // Eliminar el usuario
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok("Usuario eliminado correctamente"); // Éxito al eliminar
        }




    }
}
