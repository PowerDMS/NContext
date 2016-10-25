namespace NContext.Common.Tests.Specs.AwaitLetAsync
{
    using System;
    using System.Threading.Tasks;

    using FakeItEasy;

    using Machine.Specifications;

    public class with_error : when_using_AwaitLetAsync<int>
    {
        Establish context = () =>
        {
            FutureServiceResponse = Task.Run<IServiceResponse<int>>(() => new ErrorResponse<int>(new Error(400, "code", new []{ "error" })));
            LetAsyncFunc = A.Fake<Func<int, Task>>(source => Task.FromResult<object>(0));
        };

        It should_not_invoke_the_let_function = () => A.CallTo(LetAsyncFunc).MustNotHaveHappened();
    }
}