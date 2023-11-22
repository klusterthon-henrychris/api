using System.Text.Json;

namespace Kluster.Shared.Responses
{
    public class ApiErrorResponse<T>(T errors, string message)
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            WriteIndented = true
        };

        public bool Success { get; set; }
        public string Message { get; set; } = message;
        public T Errors { get; set; } = errors;

        public string ToJsonString()
        {
            return JsonSerializer.Serialize(this, _jsonSerializerOptions);
        }
    }
}
