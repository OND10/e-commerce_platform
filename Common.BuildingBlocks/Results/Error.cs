using System.Collections.Generic;
using System.Linq;

namespace Common.BuildingBlocks.Results
{
    public sealed record Error(string Code, string Description)
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

        public static implicit operator Error(string message) => new("Error.General", message);
    }
}