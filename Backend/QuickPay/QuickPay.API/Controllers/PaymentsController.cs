using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickPay.Application.DTOs;
using QuickPay.Application.Services;

namespace QuickPay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequestDto request)
        {
            if (request == null) return BadRequest("İstek başarısız!");

            var result = await _paymentService.ProcessPaymentAsync(request);

            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

    }
}
