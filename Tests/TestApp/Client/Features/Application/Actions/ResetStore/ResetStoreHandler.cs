namespace TestApp.Client.Features.Application
{
  using BlazorState;
  using MediatR;
  using System.Threading;
  using System.Threading.Tasks;
  using static BlazorState.Features.Routing.RouteState;
  using static TestApp.Client.Features.Application.ApplicationState;

  internal class ResetStoreHandler : IRequestHandler<ResetStoreAction>
  {
    private readonly ISender Sender;
    private readonly IStore Store;
    public ResetStoreHandler(IStore aStore, ISender aSender)
    {
      Sender = aSender;
      Store = aStore;
    }


    public Task<Unit> Handle(ResetStoreAction aResetStoreAction, CancellationToken aCancellationToken)
    {
      Store.Reset();
      return Sender.Send(new ChangeRouteAction(NewRoute: "/"), aCancellationToken);
    }
  }
}