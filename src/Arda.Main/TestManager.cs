using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ArdaSDK.Kanban;
using ArdaSDK.Kanban.Models;

namespace Arda.Main
{
    public class TestManager
    {
        static KanbanClient _client;
        static string _user;

        public static void TestStatic()
        {
            Console.WriteLine("Test");

            _client = Arda.Common.Utils.Util.KanbanClient;

            _user = "fcatae@microsoft.com";

            var test = new TestManager();            
        }

        public void TestMain()
        {
        }

    }
}
