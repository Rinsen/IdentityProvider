using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rinsen.IdentityProviderWeb
{
    public static class ExtensionMethods
    {
        public static void AddModelErrorAndClearModelValue<TModel>(this ModelStateDictionary dictionary, Expression<Func<TModel, object>> expression, string errorMessage)
        {
            var key = ExpressionHelper.GetExpressionText(expression);
            ClearModelValue(dictionary, key);
            dictionary.AddModelError(key, errorMessage);
        }

        public static void ClearModelValue<TModel>(this ModelStateDictionary dictionary, Expression<Func<TModel, object>> expression)
        {
            var key = ExpressionHelper.GetExpressionText(expression);
            ClearModelValue(dictionary, key);
        }

        private static void ClearModelValue(ModelStateDictionary dictionary, string key)
        {
            ModelStateEntry modelState;
            if (!dictionary.TryGetValue(key, out modelState))
                return;

            var type = modelState.RawValue.GetType();

            var rawValue = type == typeof(string) ? string.Empty : Activator.CreateInstance(type);

            // TODO Is this close to correct?

            modelState.RawValue = rawValue;
            modelState.ValidationState = ModelValidationState.Valid;
            modelState.AttemptedValue = string.Empty;
            modelState.Errors.Clear();
        }
    }
}
