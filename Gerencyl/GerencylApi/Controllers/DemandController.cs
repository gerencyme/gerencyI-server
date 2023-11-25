using AutoMapper;
using Domain.Interfaces.IServices;
using GerencylApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GerencylApi.Controllers
{
    
    [ApiController]
    public class DemandController : ControllerBase
    {
        private readonly IMapper _IMapper;
        private readonly IDemandServices _IDemandServices;
        //private readonly IServiceCollection _serviceCollection;

        public DemandController(IMapper IMapper, IDemandServices IDemandServices)
        {
            _IDemandServices = IDemandServices;
            _IMapper = IMapper;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/AddDemand")]
        public async Task<IActionResult> Add([FromBody] DemandModel demand)
        {
            await _IDemandServices.AddDemand(demand.DemandId, demand.Observation, demand.DateDemand);

            return Ok();

        }

    }
}
