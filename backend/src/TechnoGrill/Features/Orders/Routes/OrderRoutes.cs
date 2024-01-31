using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechnoGrill.Features.Orders.Commands;
using TechnoGrill.Features.Orders.Queries;

namespace TechnoGrill.Features.Orders.Routes;

public static class OrderRoutes
{
    public static void MapOrderRoutes(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/order");

        group.MapGet("", async ([FromServices] IMediator mediator, [AsParameters] GetOrders.Query query) =>
            {
                var result = await mediator.Send(query);
                return Results.Ok(result);
            })
            .Produces<List<GetOrders.Dto>>()
            .WithOpenApi();

        group.MapGet("{id:guid}", async ([FromServices] IMediator mediator, [AsParameters] GetOrder.Query query) =>
            {
                var result = await mediator.Send(query);
                return Results.Ok(result);
            })
            .Produces<GetOrder.Dto?>()
            .WithOpenApi();

        group.MapPost("", async ([FromServices] IMediator mediator, [FromBody] AddOrder.Command command) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            })
            .Produces((int)HttpStatusCode.NoContent)
            .WithOpenApi();

        group.MapDelete("", async ([FromServices] IMediator mediator, [FromBody] DeleteOrder.Command command) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            })
            .Produces((int)HttpStatusCode.NoContent)
            .WithOpenApi();

        group.MapPost("add-item", async ([FromServices] IMediator mediator, [FromBody] AddOrderItem.Command command) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            })
            .Produces((int)HttpStatusCode.NoContent)
            .WithOpenApi();

        group.MapPost("delete-item", async ([FromServices] IMediator mediator, [FromBody] DeleteOrderItem.Command command) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            })
            .Produces((int)HttpStatusCode.NoContent)
            .WithOpenApi();

        group.MapPost("change-item-amount", async ([FromServices] IMediator mediator, [FromBody] ChangeOrderItemAmount.Command command) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            })
            .Produces((int)HttpStatusCode.NoContent)
            .WithOpenApi();

        group.MapPost("approve", async ([FromServices] IMediator mediator, [FromBody] ApproveOrder.Command command) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            })
            .Produces((int)HttpStatusCode.NoContent)
            .WithOpenApi();

        group.MapPost("decline", async ([FromServices] IMediator mediator, [FromBody] DeclineOrder.Command command) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            })
            .Produces((int)HttpStatusCode.NoContent)
            .WithOpenApi();
    }
}