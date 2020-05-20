using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace APEC.DOCS.Helpers.Extensions
{
  public class DateTimeModelBinder : IModelBinder
  {
    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

      DateTime dateTime;

      if (string.IsNullOrEmpty(value.AttemptedValue))
      {
          return null;
      }

      

            var isDate = DateTime.TryParseExact(value.AttemptedValue, "dd/MM/yyyy", Thread.CurrentThread.CurrentUICulture,
                DateTimeStyles.None, out dateTime);

//      if (!isDate)
//      {
//                if (string.IsNullOrEmpty(value.AttemptedValue))
//                {
//                    bindingContext.ModelState.AddModelError(bindingContext.ModelName,
//                        string.Format("1", bindingContext.ModelMetadata.DisplayName));
//                }
//                else
//                {
//                    bindingContext.ModelState.AddModelError(bindingContext.ModelName,
//                        string.Format("1", bindingContext.ModelMetadata.DisplayName));
//                }
//
//        return null;
//      }

      return dateTime;
    }
  }
}