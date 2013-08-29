using System;
using System.Collections.Generic;
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

        private string _shouldBeSet;

        [Fact]
        public void TryToCall_TakesNoneReturnsVoid()
        {
            Assert.Throws<ThrowTestException>(() =>
            {
                ExecuteUIThread.ExecuteOnUIThread(() =>
                {
                    Button b = new Button();
                    _shouldBeSet = "How now brown cow";

                    throw new ThrowTestException("message for the user");
                });
           });
        }

        [Fact]
        public void TryToCall_TakesOneReturnsVoid()
        {
            Assert.Throws<ThrowTestException>(() =>
            {
                int i = 7;
                ExecuteUIThread.ExecuteOnUIThread(() =>
                {
                    Button b = new Button();
                    _shouldBeSet = "How now brown cow " + i;

                    throw new ThrowTestException("message for the user");
                });
            });
        }

        [Fact]
        public void TryToCall_TakesNoneReturnsObject()
        {
            Assert.Throws<ThrowTestException>(() =>
            {
                object retval = ExecuteUIThread.ExecuteOnUIThread(() =>
                {
                    Button b = new Button();
                    _shouldBeSet = "How now brown cow";
                    return (int)11;

                    throw new ThrowTestException("message for the user");
                });
            });
        }

        [Fact]
        public void TryToCall_TakesOneReturnsObject()
        {
            Assert.Throws<ThrowTestException>(() =>
            {
                int i = 22;
                object retval = ExecuteUIThread.ExecuteOnUIThread(() =>
                {
                    Button b = new Button();
                    _shouldBeSet = "How now brown cow " + i;
                    return (int)22;

                    throw new ThrowTestException("message for the user");
                });
            });
        }
    }
}
