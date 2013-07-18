namespace NContext.Extensions.ValueInjecter.Tests.Specs
{
    using System;

    using Omu.ValueInjecter;

    public abstract class DtoToEntityConvention : ConventionInjection
    {
        protected override Boolean Match(ConventionInfo c)
        {
            return c.Source.Type.Name.EndsWith("Dto", StringComparison.OrdinalIgnoreCase) &&
                   !c.Target.Type.Name.EndsWith("Dto", StringComparison.OrdinalIgnoreCase);
        }
    }
}