﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalogService.Application.Features.Books.Commands.DeleteBook
{
    public record DeleteBookCommand(Guid Id) : IRequest<string>;
}
