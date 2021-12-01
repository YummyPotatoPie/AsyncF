using System.Collections;

namespace Components
{
    /// <summary>
    /// Class contains table of local identifiers 
    /// </summary>
    public class Environment
    {
        /// <summary>
        /// Table of identifiers 
        /// </summary>
        private readonly Hashtable _table;

        /// <summary>
        /// Previous environment
        /// </summary>
        public Environment Previous { get; }

        /// <summary>
        /// Private constructor to reset table of identifiers
        /// </summary>
        private Environment() => _table = new();

        /// <summary>
        /// Constructor for set previous environment 
        /// </summary>
        /// <param name="previous">Previous environment</param>
        public Environment(Environment previous) : this() => Previous = previous;


        /// <summary>
        /// Puts identifier to table of identifiers
        /// </summary>
        /// <param name="name">Name of identifiers</param>
        /// <param name="value">Value of identifier</param>
        public void PutConstant(string name, long value) => _table.Add(name, value);


        /// <summary>
        /// Gets identifier from table
        /// </summary>
        /// <param name="name">Name of identifier</param>
        /// <returns>Identifier value</returns>
        public long? GetConstant(string name)
        {
            for (Environment env = this; env != null; env = env.Previous) 
                if (env._table[name] != null) return (long?)env._table[name];
            return null;
        }
    }
}
