namespace ProgrammingParadigms
{
    /// <summary>
    /// Fired when the state of persistance changes.
    /// </summary>
    /// <param name="selected">The state of persistance.</param>
    public delegate void PersistableDelegate(bool selected);

    /// <summary>
    /// Defines an item that requires persistance. Usually implemented by UI elements
    /// such as radio buttons and check boxes.
    /// </summary>
    public interface IPersistable
    {
        /// <summary>
        /// The identifier of the selectable element.
        /// </summary>
        object Key { get; }

        /// <summary>
        /// Sets the selected state of the element.
        /// </summary>
        /// <param name="selected">Whether the element is selected.</param>
        void SetState(bool selected);

        /// <summary>
        /// Emitted when the selected state of the element changes.
        /// </summary>
        event PersistableDelegate Selected;
    }
}
