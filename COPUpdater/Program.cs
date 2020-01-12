using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace COPUpdater
{
    public class Program
    {
        public static void Main(string[] args)
        {


            string connstr = Environment.GetEnvironmentVariable("CONNECTION_STR");
            if (connstr == null || connstr == "")
            {
                System.Console.WriteLine("ERROR:Missing CONNECTION_STR env variable. Process Exit.");
            }
            else settings.ConnectionString = connstr;


            string deviceStartIndex = Environment.GetEnvironmentVariable("deviceStartIndex");
            if (deviceStartIndex == null || deviceStartIndex == "")
            {
              
            }
            else settings.deviceStartIndex = int.Parse(deviceStartIndex);
            string deviceEndIndex = Environment.GetEnvironmentVariable("deviceEndIndex");
            if (deviceEndIndex == null || deviceEndIndex == "")
            {

            }
            else settings.deviceEndIndex = int.Parse(deviceEndIndex);

            string fixedTopic = Environment.GetEnvironmentVariable("fixedTopic");
            if (fixedTopic == null || fixedTopic == "")
            {

            }
            else settings.fixedTopic = fixedTopic;


            string gostServer = Environment.GetEnvironmentVariable("gostServer");
            if (gostServer == null || gostServer == "")
            {

            }
            else settings.gostServer = gostServer;



            string CameraPrefix = Environment.GetEnvironmentVariable("CameraPrefix");

                settings.CameraPrefix = CameraPrefix;
    
            MQTTReciever mqttr = new MQTTReciever();
            string res = "";
            string action = "";
            string message = "";
           
            mqttr.init();

            BuildWebHost(args).Run();
            while (true)
                System.Threading.Thread.Sleep(500);
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
