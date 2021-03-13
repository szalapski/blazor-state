namespace TestApp.Client.Features.EventStream
{
  using BlazorState;

  internal partial class EventStreamState
  {
    public record AddEventAction
    (
      string Message
    ) : IAction
    { };
  }
}