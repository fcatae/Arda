using System;

namespace IntegrationTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello world");

            ArdaTestMgr.AllowCreateResultFile = true;

            //var test = new Appointment();
            //test.ppointment_GetAllAppointments_Should_ReturnAllValues();
        }
    }
}
