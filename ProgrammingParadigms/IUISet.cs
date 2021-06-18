
namespace ProgrammingParadigms
{
    /// <summary>
    /// This along with IUI are the consdiered two variants of teh same IUI programming paradigm abstraction
    /// Best to understand this programming paradigm by way of example.
    /// Sometimes a containing UI element has a set child UI elements that need to do something together at a time that only the containing parent UI element knows.
    /// The set of UI elements may be radio buttons, check boxes, or just a set of settings.
    /// The containing parent UI element may have a SAVE button or it may be a Wizard with a NEXT button.
    /// In the case of the SAVE button, all the child elements need to save as a consistent set.
    /// In the case of a Wizard with a set of Radio button, the radio buttons are each set an event when the NEXT button is pressed so that the one that is active can in turn send out an event.
    /// This interface allows the container to tell the set of child elements to do something all at once.
    /// The Event method in this interface should be called by the parent domain abstraction on every child in the set.
    /// </summary>
    public interface IUISet : IUI
    {
        void Event();
    }
}
