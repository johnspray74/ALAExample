using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>Contains a WPF DatePicker that outputs the selected date as a DateTime object.</para>
    /// <para>Ports:</para>
    /// <para>1. IUI wpfElement: returns the contained DatePicker.</para>
    /// <para>2. IDataFlow&lt;DateTime&gt; selectedDate: outputs the selected date as a DateTime object.</para>
    /// </summary>
    public class DatePicker : IUI
    {
        // properties ---------------------------------------------------------------
        public string InstanceName = "Default";

        // private fields
        private System.Windows.Controls.DatePicker datePicker = new System.Windows.Controls.DatePicker();


        // Outputs
        private IDataFlow<DateTime> selectedDate;

        /// <summary>
        /// <para>Contains a WPF DatePicker that outputs the selected date as a DateTime object.</para>
        /// </summary>
        public DatePicker()
        {
            datePicker.SelectedDateChanged += DatePicker_SelectedDatesChanged;
        }

        private void DatePicker_SelectedDatesChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var date = datePicker.SelectedDate.Value;
            if (selectedDate != null) selectedDate.Data = date;
        }

        // IUI implementation ----------------------------------------------------------------------------------------
        System.Windows.UIElement IUI.GetWPFElement()
        {
            return datePicker;
        }
    }
}
