namespace NContext.Common
{
    using System;

    public class Left<A, B> : Either<A, B>
    {
        private readonly A _A;

        public Left(A a)
        {
            _A = a;
        }

        public override Boolean IsLeft
        {
            get { return true; }
        }

        public override A GetLeft()
        {
            return _A;
        }

        public override B GetRight()
        {
            return default(B);
        }
    }
}