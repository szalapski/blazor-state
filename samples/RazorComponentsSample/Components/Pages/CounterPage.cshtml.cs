namespace RazorComponentsSample.Components.Pages
{
  using RazorComponentsSample.Client.Components;
  using RazorComponentsSample.Client.Features.Counter;

  public class CounterPageModel : BaseComponent
  {
    internal void ButtonClick() =>
      Mediator.Send(new IncrementCounterAction { Amount = 5 });
  }
}