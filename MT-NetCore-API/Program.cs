using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MT_NetCore_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.UseKestrel(options =>
                //{
                //    options.ListenLocalhost(5001);
                //})
                .UseStartup<Startup>();
                //.UseUrls("http://127.0.0.1:5001");
    }
}
