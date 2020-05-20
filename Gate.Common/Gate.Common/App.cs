using System.Configuration;

namespace Common
{
    public static class App
    {
        static App()
        {
            ConnectionString = ConfigurationManager.AppSettings["OTConnection"];
        }

        public static string ConnectionString;
    }
}
