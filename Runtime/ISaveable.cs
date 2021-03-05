namespace Varollo.SavingSystem
{
    public interface ISaveable
    {
        /// <summary>
        /// Use to return a serializable struct with the data that is going to be saved.
        /// </summary>
        object CaptureState();
        /// <summary>
        /// Use to restore the saved data from a serializable struct.
        /// </summary>
        /// <param name="state"></param>
        void RestoreState(object state);
        /// <summary>
        /// This method is called when no state was retrieved
        /// </summary>
        void OnNullState();
    }
}