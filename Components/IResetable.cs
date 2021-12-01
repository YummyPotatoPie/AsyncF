namespace Components
{
    /// <summary>
    /// Interface for resetable classes
    /// </summary>
    public interface IResetable
    {
        /// <summary>
        /// Resets object 
        /// </summary>
        /// <param name="stream">Program stream</param>
        public void Reset(string stream);
    }
}
