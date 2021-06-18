using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Cinema.Helpers
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            string PropertyName = bindingContext.ModelName;
            ValueProviderResult ValueProvider = bindingContext.ValueProvider.GetValue(PropertyName);

            if (ValueProvider == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            try
            {
                T value = JsonConvert.DeserializeObject<T>(ValueProvider.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(value);
            }
            catch
            {
                bindingContext.ModelState.TryAddModelError(PropertyName, "Invalid value for type: List<int>");
            }

            return Task.CompletedTask;
        }
    }
}
