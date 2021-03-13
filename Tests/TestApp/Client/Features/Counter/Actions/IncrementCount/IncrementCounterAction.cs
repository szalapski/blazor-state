namespace TestApp.Client.Features.Counter
{
  using BlazorState;

  public partial class CounterState
  {
    public record IncrementCounterAction
    (
      int Amount
    ) : IAction
    { }
  }
}