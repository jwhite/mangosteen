using Mangosteen.Panels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Xunit;
using Xunit.Extensions;

namespace Mangosteen.Test
{
    public class WheelPanelTest_UI
    {
        public IAsyncAction ExecuteOnUIThread<TException>(DispatchedHandler action)
        {
            return CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
        }


        //var test = XamlReader.Load("<Viewbox xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" Width=\"50\" Height=\"50\" Fill=\"Orange\" />");
        
        [Fact]
        public async Task WheelPanel_Can_Be_Hosted__attempt1()
        {
            // Need to create this outside of the UI thread scope or it will 
            // disappear for next async call
            Viewbox viewbox = null;

            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                viewbox = new Viewbox(); 

                WheelPanel panel = new WheelPanel();
                viewbox.Child = (UIElement)panel;
                viewbox.UpdateLayout();
            }); 
            
            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                CoreApplication.MainView.CoreWindow.Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessAllIfPresent);
            });
             
            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                Assert.True(viewbox.Child.GetType() == typeof(WheelPanel));
            });
        }

        [Fact]
        public async Task WheelPanel_Can_Be_Hosted__attempt2()
        {
            // Need to create this outside of the UI thread scope or it will 
            // disappear for next async call
            Grid grid = null;

            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                grid = new Grid { Width= 300, Height=300}; 

                WheelPanel panel = new WheelPanel();

                grid.Children.Add(panel);

                grid.Measure(new Size(grid.Width, grid.Height));
                grid.Arrange(new Rect(new Point(0,0),new Size(grid.Width, grid.Height)));
                grid.UpdateLayout();
               
            }); 
            
            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                CoreApplication.MainView.CoreWindow.Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessAllIfPresent);
            });
             
            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                Assert.True(grid.Children[0].GetType() == typeof(WheelPanel));
            });
        }

        [Fact]
        public async Task WheelPanel_Can_Be_Hosted__attempt3()
        {
            // Need to create this outside of the UI thread scope or it will 
            // disappear for next async call
            Grid grid = null;

            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                Window nonVisibleMainWindow = Windows.UI.Xaml.Window.Current;
                grid = new Grid { Width = 300, Height = 300 };
                nonVisibleMainWindow.Content = grid;

                WheelPanel panel = new WheelPanel();

                grid.Children.Add(panel);

                grid.UpdateLayout();
            });

            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                Assert.True(grid.Children[0].GetType() == typeof(WheelPanel));
            });
        }
    }
}
