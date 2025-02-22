﻿namespace RentSmart.Web.Infrastructure.ModelBinders
{
    using System;

    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class DecimalModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (context.Metadata.ModelType == typeof(decimal) ||
                context.Metadata.ModelType == typeof(decimal?))
            {
                return new DecimalModelBinder();
            }

            return null!;
        }
    }
}
