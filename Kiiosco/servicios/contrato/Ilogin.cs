using Data;

namespace Kiosco.servicios.contrato
{
    public interface Ilogin
    {
        Task<string> GetUsuario(string correo, string clave);
        Task<Usuarios> RegistrarUsuario(Usuarios usuario);

        Task<Usuarios> EditarUsuario(Usuarios usuario);
    }
}
