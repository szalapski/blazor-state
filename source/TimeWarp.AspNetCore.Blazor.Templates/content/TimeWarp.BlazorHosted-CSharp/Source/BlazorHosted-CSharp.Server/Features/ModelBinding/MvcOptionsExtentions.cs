namespace BlazorHostedCSharp.Server.Features.ModelBinding
{
  using System.Linq;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

  public static class MvcOptionsExtensions
  {
    public static void UseMediatorModelBinding(this MvcOptions opts)
    {
      //var binderToFind = opts.ModelBinderProviders.FirstOrDefault(x => x.GetType() == typeof(SimpleTypeModelBinderProvider));

      //if (binderToFind == null)
      //  return;

      //var index = opts.ModelBinderProviders.IndexOf(binderToFind);
      opts.ModelBinderProviders.Insert(0, new MediatorModelBinderProvider());
    }
  }
}
