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
        public void TryToCall_TakesNoneReturnsVoid()
        {
            ExecuteUIThread.ExecuteOnUIThread(() =>
            {
                Button b = new Button();
                _shouldBeSet = "How now brown cow";
            });

            Assert.True(_shouldBeSet == "How now brown cow");
        }

        [Fact]
        public void TryToCall_TakesOneReturnsVoid()
        {
            int i = 7;
            ExecuteUIThread.ExecuteOnUIThread(() =>
            {
                Button b = new Button();
                _shouldBeSet = "How now brown cow " + i;
            });

            Assert.True(_shouldBeSet == "How now brown cow 7");
        }

        [Fact]
        public void TryToCall_TakesNoneReturnsObject()
        {
            object retval = ExecuteUIThread.ExecuteOnUIThread(() =>
            {
                Button b = new Button();
                _shouldBeSet = "How now brown cow";
                return (int)11;
            });

            Assert.True((int)retval == 11);
            Assert.True(_shouldBeSet == "How now brown cow");
        }

        [Fact]
        public void TryToCall_TakesOneReturnsObject()
        {
            int i = 22;
            object retval = ExecuteUIThread.ExecuteOnUIThread(() =>
            {
                Button b = new Button();
                _shouldBeSet = "How now brown cow " + i;
                return (int)22;
            });

            Assert.True((int)retval == 22);
            Assert.True(_shouldBeSet == "How now brown cow 22");
        }
    }
}
