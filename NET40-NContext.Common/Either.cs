namespace NContext.Common
{
    using System;

    public abstract class Either<A, B> : IEither<A, B>
    {
        public abstract Boolean IsLeft { get; }

        public Boolean IsRight { get { return !IsLeft; } }

        public abstract A GetLeft();

        public abstract B GetRight();
    }
}