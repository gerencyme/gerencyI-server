using AutoMapper;
using Domain.Interfaces.IServices;
using GerencylApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GerencylApi.Controllers
{
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


        [Produces("application/json")]
        [HttpPost("/api/Add")]
        public async Task<IActionResult> Add([FromQuery] DemandModel demand)
        {
            await _IDemandServices.AddDemand(demand.DemandId, demand.Observation, demand.DateDemand, demand.ProductId);

            return Ok();

        }

    }
}
