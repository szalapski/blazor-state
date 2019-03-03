namespace BlazorHosted_CSharp.Server
{
  using System.Reflection;
  using BlazorHosted_CSharp.Shared.Features.WeatherForecast;
  using BlazorHostedCSharp.Server.Features.ModelBinding;
  using MediatR;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.AspNetCore.Mvc.ModelBinding;
  using Microsoft.Extensions.DependencyInjection;
  using Newtonsoft.Json.Serialization;

  public class Startup
  {
    public void Configure(IApplicationBuilder aApplicationBuilder, IHostingEnvironment aHostingEnvironment)
    {
      aApplicationBuilder.UseResponseCompression();

      if (aHostingEnvironment.IsDevelopment())
      {
        aApplicationBuilder.UseDeveloperExceptionPage();
      }

      aApplicationBuilder.UseMvc(
        routes =>
        {
          routes.MapRoute(
            name: "MediatorRoute",
            template: GetWeatherForecastsRequest.Route,
            defaults: new { controller = "The", action = "Send" },
            constraints: null,
            dataTokens: new { requestType = typeof(GetWeatherForecastsRequest) }
            );
        });
      aApplicationBuilder.UseBlazorDualMode<Client.Startup>();
      aApplicationBuilder.UseBlazorDebugging();

    }

    public void ConfigureServices(IServiceCollection aServiceCollection)
    {
      aServiceCollection.AddSingleton<IModelMetadataProvider, MediatorModelMetadataProvider>();
      aServiceCollection
        .AddMvc(options => { options.UseMediatorModelBinding(); })
        .AddNewtonsoftJson(aOptions =>
           aOptions.SerializerSettings.ContractResolver =
              new DefaultContractResolver());

      aServiceCollection.AddRazorComponents<Client.Startup>();

      aServiceCollection.AddResponseCompression();
      aServiceCollection.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
      aServiceCollection.Scan(aTypeSourceSelector => aTypeSourceSelector
        .FromAssemblyOf<Startup>()
        .AddClasses()
        .AsSelf()
        .WithScopedLifetime());
    }
  }
}