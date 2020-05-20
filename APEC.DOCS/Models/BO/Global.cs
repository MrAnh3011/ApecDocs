using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace APEC.DOCS.Models.BO
{
  public static class Global
  {
    public static string JsDateFormat { get; private set; }
    public static string DateFormat { get; private set; }
    public static string JsDateTimeFormat { get; private set; }
    public static string DateTimeFormat { get; private set; }
    public static string ThousandSeperator { get; private set; }
    public static string DecimalSeperator { get; private set; }

    static Global()
    {
      var culture = Thread.CurrentThread.CurrentCulture;
      var uiCulture = Thread.CurrentThread.CurrentUICulture;

      DateFormat = culture.DateTimeFormat.ShortDatePattern;
      JsDateFormat = DateFormat.ToLower();
      DateTimeFormat = culture.DateTimeFormat.FullDateTimePattern;
      JsDateTimeFormat = DateTimeFormat.ToLower();
      ThousandSeperator = culture.NumberFormat.NumberGroupSeparator;
      DecimalSeperator = culture.NumberFormat.NumberDecimalSeparator;
    }
  }
}