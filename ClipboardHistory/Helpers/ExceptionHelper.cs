using System;

namespace ClipboardHistoryApp.Helpers
{
    public static class ExceptionHelper
    {
        public static bool TrySafe<TException>(Action methodToExecute, Action<TException> methodOnError = null)
            where TException : Exception
        {
            try
            {
                methodToExecute.Invoke();
                return true;
            }
            catch (TException ex)
            {
                if (methodOnError != null)
                {
                    TrySafe<TException>(() => methodOnError.Invoke(ex));
                }
                return false;
            }
        }

        public static bool TrySafe<TException>(Action methodToExecute, Action methodOnError)
            where TException : Exception
        {
            Action<TException> toExecSecond = e => { };
            if (methodOnError != null)
            {
                toExecSecond = e => methodOnError();
            }
            return TrySafe(methodToExecute, toExecSecond);
        }
    }
}
