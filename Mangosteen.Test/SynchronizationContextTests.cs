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
    public class SynchronizationContextTests
    {
        [Fact]
        public void CannotGetSynchronizationContextInTest()
        {
            SynchronizationContext localContext;
            localContext = SynchronizationContext.Current;

            Assert.True(localContext == null);
        }

        [Fact]
        public async Task TryToGrabSynchronizationContextFromUI()
        {
            AsyncHelpers ah = new AsyncHelpers();

            SynchronizationContext uicontext = await ah.GrabUISynchronizationContext();

            Assert.True(uicontext != null);
            Assert.True(uicontext is System.Threading.SynchronizationContext);
        }
    }
}
