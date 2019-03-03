namespace BlazorHosted_CSharp.Server.Features.Base
{
  using System.Threading.Tasks;
  using BlazorHosted_CSharp.Shared.Features.Base;
  using BlazorHosted_CSharp.Shared.Features.WeatherForecast;
  using MediatR;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.ModelBinding;
  using Microsoft.Extensions.DependencyInjection;

  //[Route(GetWeatherForecastsRequest.Route)]
  public class TheController: Controller
  {
    private IMediator _mediator;

    protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());

    public virtual async Task<IActionResult> Send(IRequest<object> aRequest)
    {
      object response = await Mediator.Send(aRequest);

      return Ok(response);
    }
  }
}