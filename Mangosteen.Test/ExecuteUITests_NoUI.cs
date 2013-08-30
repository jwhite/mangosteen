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
        public void TestMethodReturnsVoidTakesNone()
        {
            _shouldBeSet = "How now brown cow";
        }

        public void TestMethodReturnsVoidTakesOne(int i)
        {
            _shouldBeSet = "How now brown cow " + i;
        }

        public object TestMethodReturnsObjectTakesNone()
        {
            _shouldBeSet = "How now brown cow";
            return 66;
        }

        public object TestMethodReturnsObjectTakesOne(int i)
        {
            _shouldBeSet = "How now brown cow " + i;
            return 55;
        }

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
                TestMethodReturnsVoidTakesNone();
            });

            Assert.True(_shouldBeSet == "How now brown cow");
        }

        [Fact]
        public void TryToCall_ReturnsVoidTakesOne()
        {
            int i = 5;

            ExecuteUIThread.ExecuteOnUIThread(() =>
            {
                TestMethodReturnsVoidTakesOne(i);
            });

            Assert.True(_shouldBeSet == "How now brown cow 5");
        }

        [Fact]
        public void TryToCall_ReturnsObjectTakesNone()
        {
            object retval = ExecuteUIThread.ExecuteOnUIThread(() =>
            {
                return TestMethodReturnsObjectTakesNone();
            });

            Assert.True((int)retval == 66);
            Assert.True(_shouldBeSet == "How now brown cow");
        }

        [Fact]
        public void TryToCall_ReturnsObjectTakesOne()
        {
            int i = 22;
            object retval = ExecuteUIThread.ExecuteOnUIThread(() =>
            {
                return TestMethodReturnsObjectTakesOne(i);
            });

            Assert.True((int)retval == 55);
            Assert.True(_shouldBeSet == "How now brown cow 22");
        }


    }
}
