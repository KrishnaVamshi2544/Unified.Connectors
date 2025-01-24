namespace ExchangePOC.Helpers
{
    /// <summary>
    ///     Polling Helper Class
    /// </summary>
    public class PollingHelper : IDisposable
    {
        private readonly int _interval;
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();


        public bool continueProcessing;
        private CancellationToken _token;

        public PollingHelper()
        {
            _interval = 1;
        }

        /// <summary>
        ///     Dispose Objects
        /// </summary>
        public void Dispose()
        {
            _tokenSource.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Execute func for every configured interval.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="data"></param>
        public void Execute<T, TResult>(Func<T, TResult> func, T data)
        {
            _token = _tokenSource.Token;
            Task.Delay(TimeSpan.FromMinutes(_interval)).ContinueWith(_ =>
            {
                if (!_token.IsCancellationRequested && continueProcessing)
                    if (func != null)
                    {
                        func(data);
                        Execute(func, data);
                    }
            }, _token);
        }

        /// <summary>
        /// Execute func for every configured interval.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="data"></param>
        public void Execute<T>(Func<T, bool> func, T data)
        {
            _token = _tokenSource.Token;
            Task.Delay(TimeSpan.FromMinutes(_interval)).ContinueWith(_ =>
            {
                if (!_token.IsCancellationRequested)
                    if (func != null)
                    {
                        if (func(data))
                        {
                            Complete();
                            return;
                        }
                        Execute(func, data);
                    }
            }, _token).Wait();
        }

        /// <summary>
        /// Execute func for every configured interval.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <returns></returns>
        public TResult Execute<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 param1, T2 param2)
        {
            _token = _tokenSource.Token;
            var result = default(TResult);
            Task.Delay(TimeSpan.FromMinutes(_interval)).ContinueWith(_ =>
            {
                if (!_token.IsCancellationRequested)
                    if (func != null)
                    {
                        result = func(param1, param2);
                        Execute(func, param1, param2);
                    }
            }, _token);

            return result;
        }

        /// <summary>
        /// Execute func for every configured interval.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="func"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        public void Execute<T1, T2>(Func<T1, T2, bool> func, T1 param1, T2 param2)
        {
            _token = _tokenSource.Token;
            Task.Delay(TimeSpan.FromMinutes(_interval)).ContinueWith(_ =>
            {
                if (!_token.IsCancellationRequested)
                    if (func != null)
                    {
                        if (func(param1, param2))
                        {
                            Complete();
                            return;
                        }
                        Execute(func, param1, param2);
                    }
            }, _token).Wait();
        }

        /// <summary>
        /// Execute func for every configured interval.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="func"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        public void Execute<T1, T2, T3>(Func<T1, T2, T3, bool> func, T1 param1, T2 param2, T3 param3)
        {
            _token = _tokenSource.Token;
            Task.Delay(TimeSpan.FromMinutes(_interval)).ContinueWith(_ =>
            {
                if (!_token.IsCancellationRequested)
                    if (func != null)
                    {
                        if (func(param1, param2, param3))
                        {
                            Complete();
                            return;
                        }
                        Execute(func, param1, param2, param3);
                    }
            }, _token).Wait();
        }

        /// <summary>
        /// Execute func for every configured interval.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="func"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        public void Execute<T1, T2, T3, T4>(Func<T1, T2, T3, T4, bool> func, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            _token = _tokenSource.Token;
            Task.Delay(TimeSpan.FromMinutes(_interval)).ContinueWith(_ =>
            {
                if (!_token.IsCancellationRequested)
                    if (func != null)
                    {
                        if (func(param1, param2, param3, param4))
                        {
                            Complete();
                            return;
                        }
                        Execute(func, param1, param2, param3, param4);
                    }
            }, _token).Wait();
        }

        /// <summary>
        /// Execute func for every configured interval.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="func"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <param name="param5"></param>
        public void Execute<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, bool> func, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
        {
            _token = _tokenSource.Token;
            Task.Delay(TimeSpan.FromMinutes(_interval)).ContinueWith(_ =>
            {
                if (!_token.IsCancellationRequested)
                    if (func != null)
                    {
                        if (func(param1, param2, param3, param4, param5))
                        {
                            Complete();
                            return;
                        }
                        Execute(func, param1, param2, param3, param4, param5);
                    }
            }, _token).Wait();
        }

        /// <summary>
        /// Execute func for every configured interval.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <param name="func"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <param name="param5"></param>
        /// <param name="param6"></param>
        /// <param name="param7"></param>
        public void Execute<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, bool> func, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7)
        {
            _token = _tokenSource.Token;
            Task.Delay(TimeSpan.FromMinutes(_interval)).ContinueWith(_ =>
            {
                if (!_token.IsCancellationRequested)
                    if (func != null)
                    {
                        if (func(param1, param2, param3, param4, param5, param6, param7))
                        {
                            Complete();
                            return;
                        }
                        Execute(func, param1, param2, param3, param4, param5, param6, param7);
                    }
            }, _token).Wait();
        }

        /// <summary>
        /// Executes 'func' for every 'interval'. Quits when 'func' returns a boolean true. 
        /// </summary>
        /// <param name="func">Delegate of type bool</param>
        /// <param name="interval">Polling Interval</param>
        /// <param name="waitToComplete">Polls synchronously or asynchronously</param>
        public void Execute(Func<bool> func, int interval, bool waitToComplete = true)
        {
            _token = _tokenSource.Token;
            Task executeTask = Task.Delay(TimeSpan.FromMinutes(interval), _token).ContinueWith(_ =>
            {
                if (!_token.IsCancellationRequested)
                {
                    if (func != null)
                    {
                        if (func())
                        {
                            Complete();
                            return;
                        }
                        Execute(func, interval, waitToComplete);
                    }
                }
            }, _token);

            if (waitToComplete)
            {
                executeTask.Wait(_token);
            }
        }

        /// <summary>
        /// Executes 'func' for every 'interval'. Quits when 'func' returns a boolean true
        /// </summary>
        /// <param name="func"></param>
        /// <param name="interval"></param>
        public void Execute(Func<bool> func)
        {
            _token = _tokenSource.Token;
            Task.Delay(TimeSpan.FromMinutes(_interval), _token).ContinueWith(_ =>
            {
                if (!_token.IsCancellationRequested)
                {
                    if (func != null)
                    {
                        if (func())
                        {
                            Complete();
                            return;
                        }
                        Execute(func);
                    }
                }
            }, _token).Wait();
        }

        /// <summary>
        ///     Complete Method
        /// </summary>
        public void Complete()
        {
            if (!_tokenSource.Token.IsCancellationRequested)
            {
                _tokenSource.Cancel();
            }
        }
    }
}
