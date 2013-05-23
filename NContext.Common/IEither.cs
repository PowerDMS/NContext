namespace NContext.Common
{
    using System;

    public interface IEither<TLeft, TRight>
    {
        TLeft Left { get; }

        TRight Right { get; }
    }

    public interface IResponseTransferObject<TLeft, TRight> : IEither<TLeft, TRight>, IDisposable
    {
    }

    public class ServiceResponse<TLeft, TRight> : IResponseTransferObject<TLeft, TRight>
    {
        private readonly TLeft _Left;

        private readonly TRight _Right;

        public ServiceResponse(TRight right)
        {
            _Right = right;
        }

        public ServiceResponse(TLeft left)
        {
            _Left = left;
        }

        public TLeft Left
        {
            get
            {
                return _Left;
            }
        }

        public TRight Right
        {
            get
            {
                return _Right;
            }
        }
        
        #region Implementation of IDisposable

        protected Boolean IsDisposed { get; set; }

        /// <summary>
        /// Finalizes an instance of the <see cref="ServiceResponse{T}" /> class.
        /// </summary>
        ~ServiceResponse()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(Boolean disposeManagedResources)
        {
            if (IsDisposed)
            {
                return;
            }

            if (disposeManagedResources)
            {
            }

            IsDisposed = true;
        }

        #endregion
    }
}