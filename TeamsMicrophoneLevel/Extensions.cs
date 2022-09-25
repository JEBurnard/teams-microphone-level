namespace TeamsMicrophoneLevel
{
    internal static class Extensions
    {
        /// <summary>
        /// Run a task with a timeout.
        /// </summary>
        /// <remarks>
        /// Source: https://stackoverflow.com/a/22078975
        /// </remarks>
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            using var timeoutCancellationTokenSource = new CancellationTokenSource();
            var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
            if (completedTask == task)
            {
                // Task completed ok
                timeoutCancellationTokenSource.Cancel();
                return await task;
            }
            else
            {
                // Timeout expired
                throw new TimeoutException();
            }
        }
    }
}
