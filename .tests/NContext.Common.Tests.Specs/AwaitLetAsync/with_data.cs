namespace NContext.Common.Tests.Specs.AwaitLetAsync
{
    using System;
    using System.Threading.Tasks;

    using FakeItEasy;

    using Machine.Specifications;

    public class with_data : when_using_AwaitLetAsync<int>
    {
        Establish context = () =>
        {
            FutureServiceResponse = Task.Run<IServiceResponse<int>>(() => new DataResponse<int>(5));
            LetAsyncFunc = A.Fake<Func<int, Task>>(source => Task.FromResult<object>(0));
        };

        It should_invoke_the_let_function = () => A.CallTo(LetAsyncFunc).MustHaveHappened();
    }
}