using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace APEC.DOCS.Helpers.Extensions
{
  public class NullableDateTimeModelBinder : IModelBinder
  {
    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

      if (string.IsNullOrWhiteSpace(value.AttemptedValue))
      {
        return null;
      }

      DateTime dateTime;

      var isDate = DateTime.TryParseExact(value.AttemptedValue,"dd/MM/yyyy", Thread.CurrentThread.CurrentUICulture, DateTimeStyles.None, out dateTime);

      if (!isDate)
      {
        bindingContext.ModelState.AddModelError(bindingContext.ModelName, string.Format("1", bindingContext.ModelName));
        return DateTime.UtcNow;
      }

      return dateTime;
    }
  }
}