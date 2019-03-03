namespace BlazorHostedCSharp.Server.Features.ModelBinding
{
  using System;
  using System.Text.Encodings.Web;
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Mvc.ModelBinding;

  public class MediatorModelBinder : IModelBinder
  {
    private readonly IModelBinder _fallbackBinder;

    public MediatorModelBinder(IModelBinder fallbackBinder)
    {
      if (fallbackBinder == null)
        throw new ArgumentNullException(nameof(fallbackBinder));

      _fallbackBinder = fallbackBinder;
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
      if (bindingContext == null)
        throw new ArgumentNullException(nameof(bindingContext));

      var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

      if (valueProviderResult == ValueProviderResult.None)
      {
        return _fallbackBinder.BindModelAsync(bindingContext);
      }

      var valueAsString = valueProviderResult.FirstValue;

      if (string.IsNullOrEmpty(valueAsString))
      {
        return _fallbackBinder.BindModelAsync(bindingContext);
      }

      var result = HtmlEncoder.Default.Encode(valueAsString);
      bindingContext.Result = ModelBindingResult.Success(result);

      return Task.CompletedTask;
    }
  }
}
