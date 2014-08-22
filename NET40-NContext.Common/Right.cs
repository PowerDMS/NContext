namespace NContext.Common
{
    using System;

    public class Right<A, B> : Either<A, B>
    {
        private readonly B _B;

        public Right(B b)
        {
            _B = b;
        }

        public override Boolean IsLeft
        {
            get { return true; }
        }

        public override A GetLeft()
        {
            return default(A);
        }

        public override B GetRight()
        {
            return _B;
        }
    }
}