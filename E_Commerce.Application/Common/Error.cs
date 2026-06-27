using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common
{
    public sealed record Error(string Code, string Description, ErrorType ErrorType = ErrorType.Failure)
    {
        public static Error Failure(string code = "General.Failure", string description = "General Failure Has Occurred")
            => new(code, description, ErrorType.Failure);
        public static Error Validation(string code = "General.Validation", string description = "General Validation Has Occurred")
            => new(code, description, ErrorType.Validation);
        public static Error NotFound(string code = "General.NotFound", string description = "Resource Not Found")
            => new(code, description, ErrorType.NotFound);
        public static Error Conflict(string code = "General.Conflict", string description = "General Conflict Has Occurred")
            => new(code, description, ErrorType.Conflict);
        public static Error UnAuthorized(string code = "General.UnAuthorized", string description = "Access Denied Due to Bad Authorization")
            => new(code, description, ErrorType.Unauthorized);
        public static Error Forbidden(string code = "General.Forbidden", string description = "this Operation Is Forbidden")
            => new(code, description, ErrorType.Forbidden);
        public static Error InternalServerError(string code = "General.InternalServerError", string description = "Internal Server Error Has Occurred")
            => new(code, description, ErrorType.InternalServerError);
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ErrorType
    {
        Failure = 0,
        Validation = 1,
        NotFound = 2,
        Conflict = 3,
        Unauthorized = 4,
        Forbidden = 5,
        InternalServerError = 6
    }
}
