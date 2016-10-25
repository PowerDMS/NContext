namespace NContext.Common.Tests.Specs.AwaitLetAsync
{
    using System;
    using System.Threading.Tasks;

    using FakeItEasy;

    using Machine.Specifications;

    public class with_exception : when_using_AwaitLetAsync<int>
    {
        Establish context = () =>
        {
            FutureServiceResponse = Task.Run<IServiceResponse<int>>(() => new DataResponse<int>(5));
            LetAsyncFunc = A.Fake<Func<int, Task>>(o => o.Wrapping(data => Task.Run(() => { throw new Exception("error"); })));
        };

        It should_invoke_the_let_function = () => A.CallTo(LetAsyncFunc).MustHaveHappened();

        It should_return_a_left_response = () => ResultResponse.IsLeft.ShouldBeTrue();
    }
}