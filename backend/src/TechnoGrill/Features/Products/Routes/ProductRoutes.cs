using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechnoGrill.Features.Products.Commands;
using TechnoGrill.Features.Products.Queries;

namespace TechnoGrill.Features.Products.Routes;

public static class ProductRoutes
{
    public static void MapProductRoutes(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/product");

        group.MapGet("", async ([FromServices] IMediator mediator, [AsParameters] GetProducts.Query query) =>
            {
                var result = await mediator.Send(query);
                return Results.Ok(result);
            })
            .Produces<List<GetProducts.Dto>>()
            .WithOpenApi();

        group.MapPost("", async ([FromServices] IMediator mediator, [FromBody] AddProduct.Command command) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            })
            .Produces((int)HttpStatusCode.NoContent)
            .WithOpenApi();

        group.MapPut("", async ([FromServices] IMediator mediator, [FromBody] ChangeProduct.Command command) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            })
            .Produces((int)HttpStatusCode.NoContent)
            .WithOpenApi();

        group.MapDelete("", async ([FromServices] IMediator mediator, [FromBody] DeleteProduct.Command command) =>
            {
                await mediator.Send(command);
                return Results.NoContent();
            })
            .Produces((int)HttpStatusCode.NoContent)
            .WithOpenApi();
    }
}