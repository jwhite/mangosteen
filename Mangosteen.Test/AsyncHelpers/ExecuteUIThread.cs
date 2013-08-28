using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Xunit;


namespace Mangosteen.Test
{
    public class ExecuteUIThread
    {
        //
        // Good delay for testing debugging of async calls because it will *block* this thread.
        //
        public static void RealDelay(int miliseconds)
        {
            Task.Delay(miliseconds).Wait();
        }

        #pragma warning disable 1998  // Intentioanlly don't have await
        public static async Task SuperSimpleAsyncFunction()
        {
            RealDelay(2000);
            return;
        }

        //
        // Simplest async task I could come up with to test my tests...
        //
        #pragma warning disable 1998    // Intentioanlly don't have await
        public static async Task<int> SlowCalculate(int number1, int number2)
        {
            RealDelay(2000);
            return number1 + number2;
        }

        // This function accepts a lambda that has no parameters and returns void
        public static void ExecuteOnUIThread(Action expression)
        {
          
        }

        // This function accepts a lambda that takes one parameter and returns void
        public static void ExecuteOnUIThread(Action<object> expression)
        {

        }

        // This function accepts a lambda that takes one parameter and returns void
        public static void ExecuteOnUIThread(Action<object,object> expression)
        {

        }

        // This function accepts a lambda that has one of more parameters
        public static object ExecuteOnUIThread<T>(Func<T,object> expression)
        {
            return null;
        }



        /// <summary>
        /// Executes a method on the UI thread using Invoke and waits for its completeion 
        /// before returning.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="instance"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object ExecuteOnUIThread(MethodInfo method, object instance, object [] args)
        {
            Exception exceptionoccured = null;
            object returnobject = null;

            // See if we can find the mainwindow
            if (CoreApplication.MainView == null || 
                CoreApplication.MainView.CoreWindow == null || 
                CoreApplication.MainView.CoreWindow.Dispatcher == null)
            {
                throw new Exception("We don't have a valid UI dispatcher!");
            }

            CoreDispatcher dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

            dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    returnobject = method.Invoke(instance, args);
                }
                catch (Exception e)
                {
                    
                    exceptionoccured = e;
                }
            }).AsTask().Wait();

            if (exceptionoccured != null)
            {
                // Rethrow the exception
                // TODO : Will this overwrite the stack trace?  Or do I need to pack more?
                throw new Exception("UI thread exception", exceptionoccured);
            }

            return returnobject;
        }
    }
}
