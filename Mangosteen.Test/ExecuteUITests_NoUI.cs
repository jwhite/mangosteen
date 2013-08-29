using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Xunit;

namespace Mangosteen.Test
{
    public class ExecuteUITests_NoUI
    {
        // This test demonstrates that we can create different lambdas 
        // and they will match method signatures in the execute utility 
        // class.
        private string _shouldBeSet;

        //
        // Tests start here 
        //

        [Fact]
        public void TryToCall_ReturnsVoidTakesNone()
        {
            ExecuteUIThread.ExecuteOnUIThread(() => {
                ExecuteUIThread.RealDelay(500);
                _shouldBeSet = "How now brown cow"; 
            });

            Assert.True(_shouldBeSet == "How now brown cow");
        }

        [Fact]
        public void TryToCall_ReturnsVoidTakesOne()
        {
            int i = 5;

            ExecuteUIThread.ExecuteOnUIThread(() =>
            {
                ExecuteUIThread.RealDelay(500);
                _shouldBeSet = "How now brown cow " + i;
            });

            Assert.True(_shouldBeSet == "How now brown cow 5");
        }



        [Fact]
        public void TryToCall_ReturnsObjectTakesNone()
        {
            object retval = ExecuteUIThread.ExecuteOnUIThread(() =>
            {
                _shouldBeSet = "How now brown cow";
                return (int)11;
            });

            Assert.True((int)retval == 11);
            Assert.True(_shouldBeSet == "How now brown cow");
        }

        [Fact]
        public void TryToCall_ReturnsObjectTakesOne()
        {
            object retval = ExecuteUIThread.ExecuteOnUIThread(() =>
            {
                _shouldBeSet = "How now brown cow";
                return (int)22;
            });

            Assert.True((int)retval == 22);
            Assert.True(_shouldBeSet == "How now brown cow");
        }


    }
}
