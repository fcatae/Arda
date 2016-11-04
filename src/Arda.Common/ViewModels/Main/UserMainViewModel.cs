using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.ViewModels.Main
{
    public class UserMainViewModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public string JobTitle { get; set; }

        public string ManagerUniqueName { get; set; }

        public string PhotoBase64 { get; set; }

        public int Status { get; set; }
    }
}
