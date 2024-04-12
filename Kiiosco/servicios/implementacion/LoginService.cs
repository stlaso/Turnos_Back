using Data;
using Kiosco.servicios.contrato;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Kiosco.servicios.implementacion
{
    public class LoginService : Ilogin
    {
        private UsuarioContext _context;
        private readonly IConfiguration _config;

        public LoginService(UsuarioContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        //Loguea un usuario
        public async Task<string> GetUsuario(string correo, string clave)
        {
            var claveEncriptada=EncriptarClave(clave);


            var usuario_encontrado = await _context.Usuarios.Where(u => u.usuario == correo && u.contraseña == claveEncriptada).FirstOrDefaultAsync();

            if(usuario_encontrado == null)
            {
                var usuarioNoExiste="";
                return usuarioNoExiste;
            }
            var usuario = Generate(usuario_encontrado);

            return usuario;
        }


        //Edita un usuario
        public async Task<Usuarios> EditarUsuario(Usuarios usuario)
        {

            var usuarioExistente = await _context.Usuarios.Where(u => u.id == usuario.id).FirstOrDefaultAsync();

            if (usuarioExistente == null)
            {
                return usuarioExistente;
            }


            usuarioExistente.contraseña = EncriptarClave(usuario.contraseña);


            return usuarioExistente;



        }


        //Registra un usuario
        public async Task<Usuarios> RegistrarUsuario(Usuarios usuario)
        {
            usuario.contraseña = EncriptarClave(usuario.contraseña);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }



        //Encripta la clave para guardar o para iniciar
        public static string EncriptarClave(string clave)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(clave));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();


        }


        //Genera un token para autentificacion
        private string Generate(Usuarios login)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("id",login.id.ToString()),
                new Claim("usuario",login.usuario),
                new Claim("nombre",login.nombre),
                new Claim("rol", login.rol),
                new Claim("apellido", login.apellido),
            };
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
