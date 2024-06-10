namespace ChatNet.Utility
{
    public static class ConfigurationHelper
    {
        public static string GetConnectionString(string name)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            return configuration.GetConnectionString(name);
        }
    }
}
