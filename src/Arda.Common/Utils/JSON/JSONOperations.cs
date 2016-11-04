using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Arda.Common.JSON
{
    public class JSONOperations
    {
        // Get a string formated with JSON based in any structured object.
        public static string GetJsonResult(object objectToBeConverted)
        {
            return JsonConvert.SerializeObject(objectToBeConverted).ToString();
        }
    }

    // Generate object compatible with datatable
    public class SourceDataTablesFormat
    {
        public IList<IList<string>> aaData = new List<IList<string>>();
    }
}
