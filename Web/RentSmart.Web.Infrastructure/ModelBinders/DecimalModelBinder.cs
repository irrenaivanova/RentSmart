namespace RentSmart.Web.Infrastructure.ModelBinders
{
	using System;
	using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.ModelBinding;

	public class DecimalModelBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext == null)
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}

			var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			if (valueProviderResult != ValueProviderResult.None && !string.IsNullOrWhiteSpace(valueProviderResult.FirstValue))
			{
				string value = valueProviderResult.FirstValue;
				value = value.Replace(',', '.');
				if (decimal.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out decimal result))
				{
					bindingContext.Result = ModelBindingResult.Success(result);
				}
				else
				{
					bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid decimal format.");
				}
			}

			return Task.CompletedTask;
		}
	}
}
