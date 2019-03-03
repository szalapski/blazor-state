namespace BlazorHostedCSharp.Server.Features.ModelBinding
{
  using System;
  using System.Reflection;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.ModelBinding;
  using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
  using Microsoft.Extensions.Options;

  public class MediatorModelMetadataProvider : DefaultModelMetadataProvider
  {
    public MediatorModelMetadataProvider(ICompositeMetadataDetailsProvider detailsProvider) : base(detailsProvider) { }

    public MediatorModelMetadataProvider(ICompositeMetadataDetailsProvider detailsProvider, IOptions<MvcOptions> optionsAccessor) : base(detailsProvider, optionsAccessor) { }

    public override ModelMetadata GetMetadataForParameter(ParameterInfo parameter)
    {
      return base.GetMetadataForParameter(parameter);
    }
    public override ModelMetadata GetMetadataForParameter(ParameterInfo parameter, Type modelType)
    {
      return base.GetMetadataForParameter(parameter, modelType);
    }

    public override ModelMetadata GetMetadataForType(Type modelType)
    {
      return base.GetMetadataForType(modelType);
    }
  }
}
