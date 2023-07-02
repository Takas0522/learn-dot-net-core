using System;
using System.Collections.Generic;

namespace ClassLibrary1
{
    public class Parent
    {
        public string GetData()
        {
            var da = new DataAccess();
            return da.Get(1);
        }
    }
}
