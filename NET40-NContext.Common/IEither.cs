namespace NContext.Common
{
    using System;

    public interface IEither<out A, out B>
    {
        Boolean IsLeft { get; }

        Boolean IsRight { get; }

        A GetLeft();

        B GetRight();
    }
}