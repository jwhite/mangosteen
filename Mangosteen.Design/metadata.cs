//
// Redefine Win8ControlLibrary to be the namespace of our external WinRT dll.
//
extern alias MetroControlLibrary;
using Win8ControlLibrary = MetroControlLibrary::Mangosteen;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.Features;
using System.ComponentModel;
using Microsoft.Windows.Design;


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
                
                //
                // Make this toolbox browsable
                builder.AddCustomAttributes(typeof(Win8ControlLibrary.Panels.WheelPanel), new ToolboxBrowsableAttribute(true));

                //
                // Designer catagories for properties
                builder.AddCustomAttributes(typeof(Win8ControlLibrary.Panels.WheelPanel), "StartAngle", new CategoryAttribute("Shape"));
                builder.AddCustomAttributes(typeof(Win8ControlLibrary.Panels.WheelPanel), "EndAngle", new CategoryAttribute("Shape"));

                builder.AddCustomAttributes(typeof(Win8ControlLibrary.Panels.WheelPanel), "InnerRadius", new CategoryAttribute("Shape"));
                builder.AddCustomAttributes(typeof(Win8ControlLibrary.Panels.WheelPanel), "OuterRadius", new CategoryAttribute("Shape"));

                builder.AddCustomAttributes(typeof(Win8ControlLibrary.Panels.WheelPanel), "Center", CategoryAttribute.Layout);
                       
                //
                // Design time adorner
                builder.AddCustomAttributes(typeof(Win8ControlLibrary.Panels.WheelPanel),
                    new FeatureAttribute(typeof(WheelPanelAdornerProvider)));

                return builder.CreateTable();
            }
        }
    }
}