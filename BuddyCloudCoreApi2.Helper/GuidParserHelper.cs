using System;

namespace BuddyCloudCoreApi2.Helper
{
    public class GuidParserHelper
    {
        public static Guid StringToGuidParser(string input)
        {
            return Guid.Parse(input);
        }
    }
}