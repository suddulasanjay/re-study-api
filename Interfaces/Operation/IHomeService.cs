using ReStudyAPI.Models.Operation;

namespace ReStudyAPI.Interfaces.Operation
{
    public interface IHomeService
    {
        Task<List<AgendaDto>> GetAgendaAsync();
    }
}
