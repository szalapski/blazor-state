namespace BlazorHostedCSharp.Server.Features.ModelBinding
{
  using System;
  using Microsoft.AspNetCore.Mvc.ModelBinding;
  using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

  public class MediatorModelBinderProvider : IModelBinderProvider
  {
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
      //context.Metadata
      if (context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }


      //var x = new SimpleTypeModelBinder(context.);
      if (!context.Metadata.IsComplexType && context.Metadata.ModelType == typeof(string)) // only encode string types
      {
        //return new MediatorModelBinder(new SimpleTypeModelBinder(context.Metadata.ModelType));
      }

      return null;
    }
  }
}
