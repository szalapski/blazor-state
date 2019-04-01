namespace BlazorHosted_CSharp.Server
{
  using BlazorHosted_CSharp.Client;
  using MediatR;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Http.Endpoints;
  using Microsoft.AspNetCore.Routing;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;
  using Newtonsoft.Json.Serialization;
  using System;
  using System.Reflection;

  public class Startup
  {
    public void Configure(IApplicationBuilder aApplicationBuilder, IWebHostEnvironment aWebHostEnvironment)
    {
      aApplicationBuilder.UseResponseCompression();

      if (aWebHostEnvironment.IsDevelopment())
      {
        aApplicationBuilder.UseDeveloperExceptionPage();
        //aApplicationBuilder.UseBlazorDebugging();
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        aApplicationBuilder.UseHsts();
      }
      //aApplicationBuilder.UseBlazor<Client.Startup>();

      //aApplicationBuilder.UseMvc();
      //aApplicationBuilder.UseBlazor<Client.Startup>();
      //aApplicationBuilder.UseBlazorDualMode<Client.Startup>();
      aApplicationBuilder.UseHttpsRedirection();

      aApplicationBuilder.UseStaticFiles();
      aApplicationBuilder.UseRouting(routes =>
      {
        routes.MapControllers();
        routes.MapRazorPages();
        routes.MapComponentHub<App>("app");
      });
      aApplicationBuilder.UseEndpoint();
      //aApplicationBuilder.UseBlazor<Client.Startup>();

      aApplicationBuilder.Use(next => (context) =>
      {
        Endpoint endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
          Console.WriteLine("********************************");
          Console.WriteLine("Name: " + endpoint.DisplayName);
          Console.WriteLine("Route: " + (endpoint as RouteEndpoint)?.RoutePattern);
          //Console.WriteLine("Metadata: " + string.Join(", ", endpoint.Metadata));
          Console.WriteLine("********************************");
        }
        return next(context);
      });

    }

    public void ConfigureServices(IServiceCollection aServiceCollection)
    {
      aServiceCollection.AddMvc()
        .AddNewtonsoftJson(aOptions =>
           aOptions.SerializerSettings.ContractResolver =
              new DefaultContractResolver());

      aServiceCollection.AddRazorComponents();

      aServiceCollection.AddResponseCompression();
      aServiceCollection.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
      aServiceCollection.Scan(aTypeSourceSelector => aTypeSourceSelector
        .FromAssemblyOf<Startup>()
        .AddClasses()
        .AsSelf()
        .WithScopedLifetime());
      // configure client project stuff for when running serverside
      var x = new Client.Startup();
      x.ConfigureServices(aServiceCollection);
    }

    //private static IServiceCollection AddRazorComponentsCore(
    //    IServiceCollection services,
    //    Type startupType)
    //{
    //  //AddStandardRazorComponentsServices(services);

    //  if (startupType != null)
    //  {
    //    // Call TStartup's ConfigureServices method immediately
    //    var startup = Activator.CreateInstance(startupType);
    //    var wrapper = new ConventionBasedStartup(startup);
    //    wrapper.ConfigureServices(services);

    //    // Configure the circuit factory to call a startup action when each
    //    // incoming connection is established. The startup action is "call
    //    // TStartup's Configure method".
    //    services.Configure<DefaultCircuitFactoryOptions>(circuitFactoryOptions =>
    //    {
    //      var endpoint = ComponentsHub.DefaultPath; // TODO: allow configuring this
    //          if (circuitFactoryOptions.StartupActions.ContainsKey(endpoint))
    //      {
    //        throw new InvalidOperationException(
    //            "Multiple Components app entries are configured to use " +
    //            $"the same endpoint '{endpoint}'.");
    //      }

    //      circuitFactoryOptions.StartupActions.Add(endpoint, builder =>
    //      {
    //        wrapper.Configure(builder, builder.Services);
    //      });
    //    });
    //  }

    //  return services;
    //}
  }
}