namespace NContext.Common
{
    using System;

    public class ErrorResponse<T> : ServiceResponse<T>
    {
        private readonly Error _Error;

        public ErrorResponse(Error error)
        {
            _Error = error;
        }

        public override Boolean IsLeft
        {
            get { return true; }
        }

        public override Error GetLeft()
        {
            return _Error;
        }

        public override T GetRight()
        {
            return default(T);
        }

        public override Error Error
        {
            get { return _Error; }
        }
    }
}