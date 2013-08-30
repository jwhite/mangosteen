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
    public class ExecuteUITests_UIFunctions
    {
        #region Sample async functions to test with ...............................................................

        public void TestMethodReturnsVoidTakesNone()
        {
            Button b = new Button();
            _shouldBeSet = "How now brown cow";
        }

        public void TestMethodReturnsVoidTakesOne(int i)
        {
            Button b = new Button();
            _shouldBeSet = "How now brown cow " + i;
        }

        public object TestMethodReturnsObjectTakesNone()
        {
            Button b = new Button();
            _shouldBeSet = "How now brown cow";
            return 66;
        }

        public object TestMethodReturnsObjectTakesOne(int i)
        {
            Button b = new Button();
            _shouldBeSet = "How now brown cow " + i;
            return 55;
        }


        private string _shouldBeSet;
        //
        // This function has a long delay in it, before setting the variable.
        // This is to insure our test doesn't pass accidentally by satisfying the race condition.
        // 
        // These delays are real, blocking delays that are designed to catch any deadlock situations
        // as well.
        //
        #pragma warning disable 1998
        public async void AsyncVoidUIMethod()
        {
            ExecuteUIThread.RealDelay(1000);

            Button b = new Button();
            _shouldBeSet = "Yep it is set.";
        }

        public async Task AsyncTaskUIMethod()
        {
            ExecuteUIThread.RealDelay(1000);

            Button b = new Button();
            _shouldBeSet = "Yep it is set.";
        }

        public async Task<object> AsyncTaskReturnsObjectUIMethod()
        {
            Button b = new Button();
            _shouldBeSet = "Yep it is set.";

            return 5;
        }

        public async void AsyncVoidUIThrowsException()
        {
            Button b = new Button();
            throw new Exception("We blew up!");
        }

        public async Task AsyncTaskUIThrowsException()
        {
            Button b = new Button();
            throw new Exception("We blew up!");
        }

        #endregion

        public void SetThis()
        {
            Button b = new Button();
            _shouldBeSet = "Yep it is set.";
        }

        [Fact]
        public void TryToCall_ReturnsVoidTakesNone()
        {
            ExecuteUIThread.ExecuteOnUIThread(() =>
            {
                TestMethodReturnsVoidTakesNone();
            });

            Assert.True(_shouldBeSet == "How now brown cow");
        }

        [Fact]
        public void TryToCall_ReturnsVoidTakesOne()
        {
            int i = 7;
            ExecuteUIThread.ExecuteOnUIThread(() =>
            {
                TestMethodReturnsVoidTakesOne(i);
            });

            Assert.True(_shouldBeSet == "How now brown cow 7");
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
