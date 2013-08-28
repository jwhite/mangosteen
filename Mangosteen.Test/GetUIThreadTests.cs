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
    public class GetUIThreadTests
    {
        [Fact]
        public void TryToGrabSynchronizationContextFromUI()
        {
            SynchronizationContext uicontext = GetUIThread.GrabUISynchronizationContext();

            Assert.True(uicontext != null);
            Assert.True(uicontext is System.Threading.SynchronizationContext);
        }

        [Fact]
        public void TryToGrabCoreDispatcherFromUI()
        {
            CoreDispatcher cd = GetUIThread.GrabUIDispatcher();

            Assert.True(cd != null);
            Assert.True(cd is Windows.UI.Core.CoreDispatcher);
        }  
    }
}
