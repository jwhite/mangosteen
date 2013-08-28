using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Xunit;



namespace Mangosteen.Test
{
    public class AsyncHelpersTest_GetUIThread
    {
        [Fact]
        public void TryToGrabSynchronizationContextFromUI()
        {
            SynchronizationContext uicontext = AsyncHelpers.GrabUISynchronizationContext();

            Assert.True(uicontext != null);
            Assert.True(uicontext is System.Threading.SynchronizationContext);
        }

        [Fact]
        public void TryToGrabCoreDispatcherFromUI()
        {
            CoreDispatcher cd = AsyncHelpers.GrabUIDispatcher();

            Assert.True(cd != null);
            Assert.True(cd is Windows.UI.Core.CoreDispatcher);
        }  
    }
}
