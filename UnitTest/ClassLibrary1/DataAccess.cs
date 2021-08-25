using System;
using System.Collections.Generic;

namespace ClassLibrary1
{
    public class DataAccess : IDataAccess
    {
        public IEnumerable<string> Gets()
        {
            var res = new List<string> { "1" };
            return res;
        }

        public string Get(int id)
        {
            return "1";
        }

        public void Update(string data)
        {
        }

        public void Delete(int id)
        {
        }
    }
}
