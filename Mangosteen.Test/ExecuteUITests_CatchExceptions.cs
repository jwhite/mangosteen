using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Xunit;

namespace Mangosteen.Test.Images
{
    public class ExecuteUITest_CatchExceptions
    {
        public class ThrowTestException : Exception
        {
            public ThrowTestException(string message) : base(message) { } 
        }

        public void TestMethodReturnsVoidTakesNone()
        {
            Button b = new Button();
            _shouldBeSet = "How now brown cow";

            throw new ThrowTestException("message for the user");
        }

        public void TestMethodReturnsVoidTakesOne(int i)
        {
            Button b = new Button();
            _shouldBeSet = "How now brown cow " + i;

            throw new ThrowTestException("message for the user");
        }

        #pragma warning disable 162  // warning CS0162: Unreachable code detected
        public object TestMethodReturnsObjectTakesNone()
        {
            Button b = new Button();
            _shouldBeSet = "How now brown cow";
            throw new ThrowTestException("message for the user");
            
            return 66;
        }
        #pragma warning restore 162


        #pragma warning disable 162  // warning CS0162: Unreachable code detected
        public object TestMethodReturnsObjectTakesOne(int i)
        {
            Button b = new Button();
            _shouldBeSet = "How now brown cow " + i;
            throw new ThrowTestException("message for the user");

            return 55;
        }
        #pragma warning restore 162

        //
        // Tests start here......
        //

        private string _shouldBeSet;

        [Fact]
        public void TryToCall_ReturnsVoidTakesNone()
        {
            Assert.Throws<ThrowTestException>(() =>
            {
                ExecuteUIThread.ExecuteOnUIThread(() =>
                {
                    TestMethodReturnsVoidTakesNone();
                });
           });
        }

        [Fact]
        public void TryToCall_ReturnsVoidTakesOne()
        {
            Assert.Throws<ThrowTestException>(() =>
            {
                int i = 7;
                ExecuteUIThread.ExecuteOnUIThread(() =>
                {
                    TestMethodReturnsVoidTakesOne(i);
                });
            });
        }

        [Fact]
        #pragma warning disable 162  // warning CS0162: Unreachable code detected
        public void TryToCall_ReturnsObjectTakesNone()
        {
            Assert.Throws<ThrowTestException>(() =>
            {
                object retval = ExecuteUIThread.ExecuteOnUIThread(() =>
                {
                    return TestMethodReturnsObjectTakesNone();
                });
            });
        }
        #pragma warning restore 162

        [Fact]

        public void TryToCall_ReturnsObjectTakesOne()
        {
            Assert.Throws<ThrowTestException>(() =>
            {
                int i = 22;
                object retval = ExecuteUIThread.ExecuteOnUIThread(() =>
                {
                    return TestMethodReturnsObjectTakesOne(i);
                });
            });
        }
        #pragma warning restore 162
    }
}
