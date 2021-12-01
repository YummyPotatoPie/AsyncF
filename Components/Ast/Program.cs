using System.Collections;

namespace Components.Ast
{
    public class Program
    {
        private readonly Hashtable _functionsTable;

        public Program() => _functionsTable = new();

        public Hashtable GetAllFunctions() => _functionsTable;

        public void AddFunction(Function function)
        {
            _functionsTable.Add(function.Name.Name, function);
        }

        public Function GetFunction(string functionName) => (Function)_functionsTable[functionName];

        public void AddFunctionsTable(Hashtable functionsTable)
        {
            foreach (Function function in functionsTable.Values) AddFunction(function);
        }
    }
}
