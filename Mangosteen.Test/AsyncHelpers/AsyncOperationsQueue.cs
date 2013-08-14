using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;

namespace Mangosteen.Test
{
    internal class AsyncOperationsQueue<T>  where T : AsyncOperation
    {
        private bool _run = true;
        private readonly Queue<T>  _operations = new Queue<T>();
        private readonly AutoResetEvent _operationsAvailable = new AutoResetEvent(false);

        public void Enque(T asyncOperation)
        {
            _operations.Enqueue(asyncOperation);
            _operationsAvailable.Set();
        }

        // NOTE : not quite sure what this is doing.
        public void MarkAsComplete()
        {
            _run = false;
            _operationsAvailable.Set();
        }

        private void InvokePendingOperations()
        {
            while ( _operations.Count > 0)
            {
                T operation = _operations.Dequeue();
                operation.Invoke();
            }
        }

        public void InvokeAll()
        {
            while (_run)
            {
                InvokePendingOperations();
                _operationsAvailable.WaitOne();
            }

            InvokePendingOperations();
        }
    }
}
