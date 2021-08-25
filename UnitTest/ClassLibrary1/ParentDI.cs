using System;
using System.Collections.Generic;

namespace ClassLibrary1
{
    public class ParentDi
    {
        private readonly IDataAccess _dataAccess;
        public ParentDi(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public string GetData(int id)
        {
            return _dataAccess.Get(id);
        }
    }
}
