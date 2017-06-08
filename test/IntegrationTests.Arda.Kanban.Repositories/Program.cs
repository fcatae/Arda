using System;

namespace IntegrationTests
{
    public class Program
    {
        public static void TestMain(string[] args)
        {
            Console.WriteLine("Hello world");

            ArdaTestMgr.AllowCreateResultFile = true;

            var test = new Appointment();
            test.Appointment_GetAllAppointments();
        }
    }
}
