
namespace HealthCloudClient
{
    using System;

    [Flags]
    public enum ActivityIncludes
    {
        None,
        Details = 0b0000_0001,
        MinuteSummaries = 0b0000_0010,
        MapPoints= 0b0000_0100,
    }
}
