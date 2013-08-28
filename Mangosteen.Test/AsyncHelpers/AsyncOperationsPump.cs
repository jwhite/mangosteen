using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;

namespace Mangosteen.Test
{
    internal class AsyncOperationsPump<T>  where T : AsyncOperation
    {
        private bool _run = true;
        private readonly Queue<T>  _operations = new Queue<T>();
        private readonly AutoResetEvent _operationsAvailable = new AutoResetEvent(false);

        public void ScheduleOperation(T asyncOperation)
        {
            _operations.Enqueue(asyncOperation);
            _operationsAvailable.Set();
        }

        public void SetPumpFinished()
        {
            _run = false;
            _operationsAvailable.Set();
        }

        private void PumpWaiting()
        {
            while ( _operations.Count > 0)
            {
                T operation = _operations.Dequeue();
                operation.ExecuteAction();
            }
        }

        public void ContinuousPump()
        {
            while (_run)
            {
                PumpWaiting();
                _operationsAvailable.WaitOne();
            }

            PumpWaiting();
        }
    }
}
