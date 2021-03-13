namespace BlazorState.Features.Routing
{
  public partial class RouteState
  {
    public record ChangeRouteAction
    (
      string NewRoute
    ) : IAction
    { }
  }
}