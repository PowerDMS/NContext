namespace NContext.Extensions.ValueInjecter.Tests.Specs
{
    using System;

    using Omu.ValueInjecter;

    public class ClientToUtcDateTimeConvention : DtoToEntityConvention
    {
        readonly TimeZoneInfo _ClientTimeZoneInfo;

        public ClientToUtcDateTimeConvention(TimeZoneInfo clientTimeZoneInfo)
        {
            _ClientTimeZoneInfo = clientTimeZoneInfo;
        }

        protected override Boolean Match(ConventionInfo c)
        {
            // Allow only datetime and variations
            // Don't allow a nullable datetime with a null value to match a target that is non-nullable

            return 
                
                base.Match(c) 
                
                && 

                ((c.SourceProp.Type == typeof(DateTime) || 
                  c.SourceProp.Type == typeof(DateTimeOffset) ||
                  c.SourceProp.Type == typeof(DateTime?) || 
                  c.SourceProp.Type == typeof(DateTimeOffset?)) 
                  &&
                 (c.TargetProp.Type == typeof(DateTime) || 
                  c.TargetProp.Type == typeof(DateTimeOffset) ||
                  c.TargetProp.Type == typeof(DateTime?) || 
                  c.TargetProp.Type == typeof(DateTimeOffset?)))
                  
                && 

                !((c.SourceProp.Type == typeof(DateTime?) || c.SourceProp.Type == typeof(DateTimeOffset?)) &&
                  (c.TargetProp.Type == typeof(DateTime) || c.TargetProp.Type == typeof(DateTimeOffset)) &&
                  c.SourceProp.Value == null);
        }

        protected override Object SetValue(ConventionInfo c)
        {
            Func<DateTime, DateTime> dateTimeToDateTime = clientDateTime => ConvertDateTimeFromClientToUtc(clientDateTime, _ClientTimeZoneInfo);
            Func<DateTime, DateTimeOffset> dateTimeToDateTimeOffset = clientDateTime => new DateTimeOffset(ConvertDateTimeFromClientToUtc(clientDateTime, _ClientTimeZoneInfo));
            Func<DateTimeOffset, DateTimeOffset> dateTimeOffsetToDateTimeOffset = clientDateTime => new DateTimeOffset(ConvertDateTimeFromClientToUtc(clientDateTime, _ClientTimeZoneInfo));
            Func<DateTimeOffset, DateTime> dateTimeOffsetToDateTime = clientDateTime => ConvertDateTimeFromClientToUtc(clientDateTime, _ClientTimeZoneInfo);
            
            if (c.SourceProp.Value == null)
            {
                return null;
            }

            if (c.SourceProp.Type.Equals(typeof(DateTime)) || c.SourceProp.Type.Equals(typeof(DateTime?)))
            {
                return c.TargetProp.Type.Equals(typeof(DateTime)) || c.TargetProp.Type.Equals(typeof(DateTime?))
                           ? (Object)dateTimeToDateTime((DateTime) c.SourceProp.Value)
                           : (Object)dateTimeToDateTimeOffset((DateTime) c.SourceProp.Value);
            }

            return c.TargetProp.Type.Equals(typeof(DateTimeOffset)) || c.TargetProp.Type.Equals(typeof(DateTimeOffset?))
                       ? (Object)dateTimeOffsetToDateTimeOffset((DateTimeOffset)c.SourceProp.Value)
                       : (Object)dateTimeOffsetToDateTime((DateTimeOffset)c.SourceProp.Value);
        }

        private DateTime ConvertDateTimeFromClientToUtc(DateTime clientDateTime, TimeZoneInfo clientTimeZone)
        {
            return TimeZoneInfo.ConvertTimeToUtc(clientDateTime, clientTimeZone);
        }

        private DateTime ConvertDateTimeFromClientToUtc(DateTimeOffset clientDateTime, TimeZoneInfo clientTimeZone)
        {
            return TimeZoneInfo.ConvertTimeToUtc(clientDateTime.DateTime, clientTimeZone);
        }
    }
}