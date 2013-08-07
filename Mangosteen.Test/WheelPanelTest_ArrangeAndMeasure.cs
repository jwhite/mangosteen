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
using WinRTXamlToolkit.AwaitableUI;
using Xunit;
using Xunit.Extensions;

namespace Mangosteen.Test
{
    public class WheelPanelTest_ArrangeAndMeasure
    {
        public IAsyncAction ExecuteOnUIThread<TException>(DispatchedHandler action)
        {
            return CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
        }

        private static async Task AwaitableUpdate(FrameworkElement element)
        {
            element.UpdateLayout();

            await EventAsync.FromEvent<object>(
                eh => element.LayoutUpdated += eh,
                eh => element.LayoutUpdated -= eh
                );
        }

        private static WheelPanel CreateAndHostPanel_180Degrees_ChildButtons()
        {
            Window nonVisibleMainWindow = Windows.UI.Xaml.Window.Current;

            Grid _grid;
            WheelPanel p;

            _grid = new Grid { Width = 800, Height = 800 };
            nonVisibleMainWindow.Content = _grid;
            nonVisibleMainWindow.Activate();

            p = new WheelPanel();
            p.StartAngle = 0;
            p.EndAngle = 180;
            
            _grid.Children.Add(p);

            Button[] buttons = new Button[4];
            for (int i = 0; i < 4; i++)
            {
                Button b = new Button();
                b.Name = String.Format("Button {0}", i);
                p.Children.Add(b);
            }

            return p;
        }

        private static WheelPanel CreateAndHostPanel()
        {
            Window nonVisibleMainWindow = Windows.UI.Xaml.Window.Current;

            Grid _grid;
            WheelPanel p;

            _grid = new Grid { Width = 800, Height = 800 };
            nonVisibleMainWindow.Content = _grid;
            nonVisibleMainWindow.Activate();

            p = new WheelPanel();

            _grid.Children.Add(p);

            return p;
        }

        [Theory]
        [InlineData(50)]
        public async Task Test_4_Even_Star_Sized(double size)
        {
            await ExecuteOnUIThread<ArgumentException>(async () =>
            {
                var panel = CreateAndHostPanel_180Degrees_ChildButtons();
                                
                await AwaitableUpdate(panel);

                Assert.True(false);

               // CoreApplication.MainView.CoreWindow.Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessAllIfPresent);
                
                 
            });
        }
    }
}
