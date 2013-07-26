// --------------------------------------------------------------------------------------------------------------------
// <copyright file="when_patching_an_object_with_PatchRequest.cs" company="Waking Venture, Inc.">
//   Copyright (c) 2013 Waking Venture, Inc.
// 
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//   documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//   the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//   and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//   of the Software.
// 
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//   TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//   CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//   DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Extensions.AspNetWebApi.Tests.Specs.Patching
{
    using System;

    using Machine.Specifications;

    using NContext.Common;
    using NContext.Extensions.AspNetWebApi.Patching;

    public class when_patching_an_object_with_PatchRequest
    {
        Establish context = () =>
            {
                _PatchRequest = new PatchRequest<DummyDto>
                    {
                        { "firstname", "Marc" },
                        { "DisabledOn", new DateTime(2013, 07, 26, 10, 0, 0) },
                        { "isAdmin", true },
                        { "spaceamount", null }
                    };

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
                var patchedObject = _PatchResult.Data.PatchedObject;
                patchedObject.FirstName.ShouldEqual("Marc");
                patchedObject.DisabledOn.ShouldEqual(new DateTime(2013, 07, 26, 10, 0, 0));
                patchedObject.IsAdmin.ShouldEqual(true);
                patchedObject.SpaceAmount.ShouldEqual(null);
            };

        static PatchRequest<DummyDto> _PatchRequest;

        static DummyDto _DomainTargetDto;

        static IResponseTransferObject<PatchResult<DummyDto>> _PatchResult;
    }

    internal class DummyDto
    {
        public Int32 Id { get; set; }

        [Writable]
        public String FirstName { get; set; }

        public DateTime CreatedOn { get; set; }

        [Writable]
        public DateTime? DisabledOn { get; set; }

        [Writable]
        public Boolean IsAdmin { get; set; }

        [Writable]
        public Int32? SpaceAmount { get; set; }

        public Int32 SpaceUsed { get; set; }
    }
}