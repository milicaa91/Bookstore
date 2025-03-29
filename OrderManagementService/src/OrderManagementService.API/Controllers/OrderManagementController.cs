using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderManagementService.Application.Features.Orders.Commands.AddOrder;
using OrderManagementService.Application.Features.Orders.Queries;
using OrderManagementService.Domain.Enums;
using System.Collections.Generic;

namespace OrderManagementService.API.Controllers
{
    [Route("api/orders")]
    public class OrderManagementController : ControllerBase
    {
        private readonly ISender _sender;
        public OrderManagementController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<ActionResult> AddOrder([FromBody] AddOrderRequest addOrderRequest, CancellationToken cancellationToken)
        {
            var addOrderCommand = new AddOrderCommand(addOrderRequest.UserId, addOrderRequest.CreatedAt, addOrderRequest.Total,
                addOrderRequest.Status, addOrderRequest.Items);

            var result = await _sender.Send(addOrderCommand, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetOrderDetailsResponse>> GetOrderById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetOrderDetailsQuery(id);
            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}
