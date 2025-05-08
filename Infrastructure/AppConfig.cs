namespace ManyRoomStudio.Infrastructure
{
    public class AppConfig
    {
        public static string Get(string key)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json").Build();
            return config[key];

            //if (string.IsNullOrEmpty(key))
            //    return "";

            //if (key == "Email:FromAddress")
            //{
            //    return "noreply@orbitsoft.co.uk";
            //}
            //else if (key == "Email:Password")
            //{
            //    return "Y4JhqQoKsWMcM9nZrFz8nL2ky";
            //}
            //else if (key == "Email:Smtp")
            //{
            //    return "smtp.office365.com";
            //}
            //else if (key == "Email:Port")
            //{
            //    return "587";
            //}
            //else if (key == "LoginUrl:loginurl")
            //{
            //    return "https://tutor.orbitsoft.co.uk/";

            //}
            //else
            //{

            //    //Live
            //    return "Data Source=mssqluk20.prosql.net;Initial Catalog=orbittutor05032025;User ID=orbittutor05032025user;Password=bucympolxert9nvjzfq;Trust Server Certificate=True;";

            //}
        }
    }
}
