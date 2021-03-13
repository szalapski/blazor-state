namespace TestApp.Client.Features.Counter
{
  using BlazorState;

  public class WrongNesting
  {
    public record ImproperNestedAction : IAction
    { }
  }
}