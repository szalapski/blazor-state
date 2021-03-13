namespace TestApp.Client.Features.WeatherForecast
{
  using BlazorState;

  internal partial class WeatherForecastsState
  {
    public record FetchWeatherForecastsAction : IAction { }
  }
}