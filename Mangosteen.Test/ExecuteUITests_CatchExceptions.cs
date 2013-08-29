using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Xunit;

namespace Mangosteen.Test.Images
{
    class ExecuteUITest_CatchExceptions
    {
        private string _shouldBeSet;

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
