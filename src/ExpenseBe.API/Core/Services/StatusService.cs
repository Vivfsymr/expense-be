using ExpenseBe.Core.Interfaces;
using ExpenseBe.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseBe.Core.Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;

        public StatusService(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public async Task<IEnumerable<Status>> GetAllStatusesAsync()
        {
            return await _statusRepository.GetAllAsync();
        }
    }
}