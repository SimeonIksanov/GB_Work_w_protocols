using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PumpClient.PumpServiceReference;

namespace PumpClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InstanceContext instanceContext = new InstanceContext(new CallbackHandler());
            PumpServiceClient client = new PumpServiceClient(instanceContext);

            client.UpdateAndCompile(@"C:\Users\16696378\Documents\script.txt");
            client.RunScript();

            Console.WriteLine("Please, enter to exit..");
            Console.ReadLine();

            client.Close();
        }
    }
}
