using System;

namespace IntegrationTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello world");

            ArdaTestMgr.AllowCreateResultFile = true;

            var test = new Actitivy();

            test.Actitivy_GetAllActivities_Should_ReturnAllValues();
        }
    }
}
