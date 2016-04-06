namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Patching
{
    using System;
    using System.Collections.Generic;

    using AspNet.WebApi.Patching;

    using Machine.Specifications;

    using NContext.Common;

    using Newtonsoft.Json;

    public class when_patching_an_object_with_PatchRequest
    {
        Establish context = () =>
        {
            var patchJson = JsonConvert.SerializeObject(
                new Dictionary<String, Object>
                {
                    {"firstname", "Marc"},
                    {"DisabledOn", new DateTime(2013, 07, 26, 10, 0, 0)},
                    {"isAdmin", true},
                    {"spaceamount", null}
                });

                _PatchRequest = new PatchRequest<DummyDto>(patchJson);

                _DomainTargetDto = new DummyDto
                    {
                        Id = 5,
                        FirstName = "Daniel",
                        CreatedOn = new DateTime(1985, 07, 15, 8, 10, 0, DateTimeKind.Utc),
                        DisabledOn = null,
                        IsAdmin = false,
                        SpaceAmount = 9999,
                        SpaceUsed = 500
                    };
            };

        Because of = () => _PatchResult = _PatchRequest.Patch(_DomainTargetDto);

        It should_patch_all_requested_properties = () =>
            {
                var patchedObject = _PatchResult.Data;
                patchedObject.FirstName.ShouldEqual("Marc");
                patchedObject.DisabledOn.ShouldEqual(new DateTime(2013, 07, 26, 10, 0, 0));
                patchedObject.IsAdmin.ShouldEqual(true);
                patchedObject.SpaceAmount.ShouldEqual(null);
            };

        static PatchRequest<DummyDto> _PatchRequest;

        static DummyDto _DomainTargetDto;

        static IServiceResponse<DummyDto> _PatchResult;
    }
}