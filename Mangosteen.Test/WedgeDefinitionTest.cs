﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;
using Mangosteen.Panels.Wedge;
using Windows.Foundation;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Mangosteen.Test
{
#if false

    public class WedgeDefinitionTest
    {
        public IAsyncAction ExecuteOnUIThread<TException>(DispatchedHandler action)
        {
            return CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
        }

        [Fact]
        public async Task WedgeDefinition_Can_Be_Constructed()
        {
            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                WedgeDefinition wd = new WedgeDefinition();

                Assert.True(wd != null);
            });
        }
    }
#endif
}
