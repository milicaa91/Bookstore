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
    /// <summary>
    /// Controller for managing orders.
    /// </summary>
    [Authorize(Policy = "AdminOperatorUserPolicy")]
    [Route("api/orders")]
    public class OrderManagementController : ControllerBase
    {
        private readonly ISender _sender;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderManagementController"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public OrderManagementController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Adds a new order.
        /// </summary>
        /// <param name="addOrderRequest">The add order request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the add order operation.</returns>
        [HttpPost]
        public async Task<ActionResult> AddOrder([FromBody] AddOrderRequest addOrderRequest, CancellationToken cancellationToken)
        {
            var addOrderCommand = new AddOrderCommand(addOrderRequest.UserId, addOrderRequest.Items);

            var result = await _sender.Send(addOrderCommand, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Gets the order details by ID.
        /// </summary>
        /// <param name="id">The order ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The order details.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetOrderDetailsResponse>> GetOrderById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetOrderDetailsQuery(id);
            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}
