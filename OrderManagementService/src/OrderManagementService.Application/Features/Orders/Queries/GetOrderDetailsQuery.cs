using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementService.Application.Features.Orders.Queries
{
    public sealed record GetOrderDetailsQuery(Guid Id) : IRequest<GetOrderDetailsResponse>;
}
