using System.Text.Json;

namespace Kluster.Shared.Responses
{
    public class ApiResponse<T>(T? data, string message, bool success)
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            WriteIndented = true
        };

        public bool Success { get; set; } = success;
        public string Message { get; set; } = message;
        public string Note { get; set; } = "N/A";
        public T? Data { get; set; } = data;

        public string ToJsonString()
        {

            return JsonSerializer.Serialize(this, _jsonSerializerOptions);
        }
    }
}
