
extern alias ControlLibrary;
using Win8ControlLibrary = Mangosteen;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.Features;
using System.ComponentModel;

[assembly: ProvideMetadata(typeof(Mangosteen.Design.Metadata))]
namespace Mangosteen.Design
{
    public class Metadata : IProvideAttributeTable
    {
        public AttributeTable AttributeTable
        {
            get
            {
                AttributeTableBuilder builder = new AttributeTableBuilder();

                // 
                // Custom attributes should be added to the attribute table here.
                //
                //builder.AddCustomAttributes( typeof(Win8ControlLibrary.RedButton), "CustomCount", new CategoryAttribute("CustomCategory"));
                //builder.AddCustomAttributes( typeof(Win8ControlLibrary.RedButton), "CustomColor", new CategoryAttribute("CustomCategory"));
                //builder.AddCustomAttributes( typeof(Win8ControlLibrary.RedButton), "CustomBrush", new CategoryAttribute("CustomCategory"));
              
                return builder.CreateTable();
            }
        }
    }
}