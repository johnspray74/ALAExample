using System.Windows;

namespace ProgrammingParadigms
{
    /// <summary>
    /// Hierarchical containment structure of the UI
    /// </summary>
    public interface IUI
    {
        UIElement GetWPFElement();
    }

    // The possiblity to add this interface for SystemTrayMenuItem to return as an IElement
    // If unused then deprecate
    public interface IElement<T>
    {
        T GetElement();
    }
}

