using ApiTest.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTest.Application.Inventory.Queries
{
    public record GetMovementsListQuery : IRequest<List<MovementResponseDto>>;
}
