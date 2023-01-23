using System;
using FinanceManagement.Server.Data;
using FinanceManagement.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EarningController : ControllerBase
    {
        private readonly ILogger<Earning> _logger;
        private readonly IRepository<Earning> _earningRepository;

        public EarningController(ILogger<Earning> logger, IRepository<Earning> repository)
        {
            _logger = logger;
            _earningRepository = repository;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Earning[]>>> GetEarning()
        //{
        //    var earning = _repository.GetAll();
        //    return Ok(earning.AsEnumerable<Earning>());
        //}

        [HttpGet]
        public IEnumerable<Earning> GetEarning()
        {            
            return  _earningRepository.GetAll().ToList();
        }

        [HttpDelete("{id?}")]
        public async Task <IActionResult> Delete(int id)
        {
            var toDeleteItem = _earningRepository.GetAll().FirstOrDefault(e => e.Id == id);

            if (toDeleteItem == null) return NotFound();

            _earningRepository.Remove(toDeleteItem);
            return NoContent();
        }

    }
}
