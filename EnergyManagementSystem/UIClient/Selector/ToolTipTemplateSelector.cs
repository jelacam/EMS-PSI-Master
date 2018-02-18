using EMS.Services.NetworkModelService.DataModel.Wires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UIClient.Selector
{
    public class ToolTipTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SyncMachineTemplate { get; set; }
        public DataTemplate EnConsumerTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item,
                   DependencyObject container)
        {
            if (item is SynchronousMachine)
            {
                return SyncMachineTemplate;
            }
            else if (item is EnergyConsumer)
            {
                return EnConsumerTemplate;
            }

            return null;
        }
    }
}
