using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnoGrill.Features.Orders.Repositories;
using TechnoGrill.Features.Orders.Routes;
using TechnoGrill.Features.Products.Routes;
using TechnoGrill.Infrastructure.Data;
using TechnoGrill.Infrastructure.Data.Repositories;
using TechnoGrill.Infrastructure.Web.SchemaFilters;

namespace TechnoGrill;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.FullName!.Replace("+", ".").Replace("TechnoGrill.Features.", string.Empty));
            options.SupportNonNullableReferenceTypes();
            options.SchemaFilter<SwaggerRequiredSchemaFilter>();
        });

        builder.Services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
        builder.Services.Configure<JsonOptions>(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
        builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Services.AddDbContext<AppDbContext>(optionsBuilder => optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
        builder.Services.AddCors(options => options.AddPolicy("Custom", policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

        builder.Services.AddTransient<IOrderRepository, OrderRepository>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("Custom");
        app.UseHttpsRedirection();

        app.MapProductRoutes();
        app.MapOrderRoutes();

        app.Run();
    }
}