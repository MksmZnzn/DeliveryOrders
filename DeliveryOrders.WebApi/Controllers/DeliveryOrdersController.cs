using DeliveryOrders.Application.CQRS.Queries;
using DeliveryOrders.Application.CQRS.Validators;
using DeliveryOrders.Domain;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DeliveryOrders.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveryOrdersController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly ILogger<DeliveryOrdersController> _logger;
        public DeliveryOrdersController(IMediator mediator, ILogger<DeliveryOrdersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpGet(Name = "GetAllOrders")]
        [ProducesResponseType(201, Type = typeof(Order))]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Выполняется запрос для получения всех заказов.");
            
            var result = await _mediator.Send(new GetAllOrdersQuery());
            
            _logger.LogInformation("Запрос завершен успешно. Получено {Count} заказов.", result.Count);

            return Ok(result);
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetFilteredOrders([FromQuery] GetFilteredOrdersQuery query)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Ошибка валидации запроса: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Получение отфильтрованных заказов для района: {District}, время: {FromTime}", query.District, query.Time);
                var orders = await _mediator.Send(query);
                _logger.LogInformation("Запрос завершен успешно. Найдено {Count} заказов.", orders.Count);
                return Ok(orders);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Ошибка валидации: {Message}", ex.Message);
                return BadRequest(ex.Message);  // Возвращаем BadRequestObjectResult для ValidationException
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogWarning("Ошибка фильтрации данных: {Message}", ex.Message);
                return BadRequest("Ошибка фильтрации данных: " + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Внутренняя ошибка сервера при фильтрации заказов.");
                return StatusCode(500, "Внутренняя ошибка сервера: " + ex.Message);
            }
        }
    }
}
