using System.Collections.Generic;

namespace ClassLibrary1
{
    public interface IDataAccess
    {
        void Delete(int id);
        string Get(int id);
        IEnumerable<string> Gets();
        void Update(string data);
    }
}