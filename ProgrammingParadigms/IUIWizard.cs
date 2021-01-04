
namespace ProgrammingParadigms
{
    /// <summary>
    /// An extended interface for Wizard and WizardItem.
    /// </summary>
    public interface IUIWizard : IUI
    {
        bool Checked { get; }

        void GenerateOutputEvent();
    }
}
