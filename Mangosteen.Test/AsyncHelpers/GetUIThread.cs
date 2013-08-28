using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Mangosteen.Test
{
    public class GetUIThread
    {
        public static SynchronizationContext GrabUISynchronizationContext()
        {
            var helper = new GetUIThread();
            Task<SynchronizationContext> t = helper.GrabUISynchronizationContextAsync();
            t.Wait();
            return t.Result;
        }

        private SynchronizationContext _localcontext;
        private async Task<SynchronizationContext> GrabUISynchronizationContextAsync()
        {
            Task task = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    _localcontext = SynchronizationContext.Current;
                }).AsTask();

            TaskAwaiter awaiter = task.GetAwaiter();

            if (!task.IsCompleted)
            {
                await task;
            }

            return _localcontext;
        }


        public static CoreDispatcher GrabUIDispatcher()
        {
            var helper = new GetUIThread();
            Task<CoreDispatcher> t = helper.GrabUIDispatcherAsync();
            t.Wait();
            return t.Result;
        }

        private CoreDispatcher _localDispatcher;
        private async Task<CoreDispatcher> GrabUIDispatcherAsync()
        {
            // This is how microsoft handles awaiting the task that is passed into runAsync
            // I have deconstructed it a little for the simple case.
            Task task = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    _localDispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
                }).AsTask();

            TaskAwaiter awaiter = task.GetAwaiter();

            if (!task.IsCompleted)
            {
                await task;
            }

            return _localDispatcher;
        }
    }
}
