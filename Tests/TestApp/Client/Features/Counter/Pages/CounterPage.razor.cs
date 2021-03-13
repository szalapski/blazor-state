namespace TestApp.Client.Pages
{
  using BlazorState.Features.Routing;
  using System.Threading.Tasks;
  using TestApp.Client.Features.Base.Components;
  using static TestApp.Client.Features.Counter.CounterState;

  public partial class CounterPage : BaseComponent
  {
    protected Task ChangeRouteToHome() =>
      Mediator.Send(new RouteState.ChangeRouteAction(NewRoute: "/"));

    protected Task SendThrowExceptionAction() =>
      Mediator.Send(new ThrowExceptionAction());

    protected Task SendThrowServerSideExceptionAction() =>
      Mediator.Send(new ThrowServerSideExceptionAction());

  }
}