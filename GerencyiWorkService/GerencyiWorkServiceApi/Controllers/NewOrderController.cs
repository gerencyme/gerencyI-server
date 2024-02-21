using Domain.DomainNewOrderApi.InterfacesNewOrderApi.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Controllers
{
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class NewOrderController : ControllerBase
    {
        private readonly INewOrderServices _newOrderServices;
        public NewOrderController(INewOrderServices newOrderServices)
        {
            _newOrderServices = newOrderServices;
        }

        [Produces("application/json")]
        [HttpPost("/api/GroupAndAnalyzeOrdersByProximity")]
        public async Task<IActionResult> GroupAndAnalyzeOrdersByProximity()
        {
            try
            {
                var returnGetOrdersByIsLiked = await _newOrderServices.GroupAndAnalyzeOrdersByProximity(25000);
                return Ok(returnGetOrdersByIsLiked);
            }
            catch (Exception ex)
            {
                //Logger.LogError($"Exception: {ex.ToString()}");
                throw new ApplicationException("Erro ao processar pedidos. Consulte os logs para mais informações.", ex);
            }
                    
        }
    }
}
