using ClassLibrary1;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // DIパターンで実装されていないクラスインスタンスの作成
            var parent = new Parent();
            var res = parent.GetData();

            // DIパターンで実装されてるクラスインスタンスの作成
            var dataAccess = new DataAccess();
            var parentDi = new ParentDi(dataAccess);
            var res2 = parentDi.GetData(1);
        }
    }

}
