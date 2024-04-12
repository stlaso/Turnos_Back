using Data;
using Microsoft.AspNetCore.Mvc;

namespace Kiosco.servicios.contrato
{
    public interface IHorarios
    {
        Task<Boolean> PersonaExiste(Horario horarios);
        Task<Horario> HorarioExiste(Horario horarios);
        Task<Boolean> CompararHorario(Horario horarios);

        Task<List<Horario>> BuscarTurnosPersona(int id);

        Task <(List<object> Horarios, int TotalCount)> TodosLosHorarios(int  page,int  pageSize);
    }
}
