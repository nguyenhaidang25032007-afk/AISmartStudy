using System.Text;
using System.Text.Json;

namespace AISmartStudy.Services
{
    public class GeminiService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public GeminiService(IConfiguration configuration)
        {
            _apiKey = configuration["GeminiApi:ApiKey"];
            _httpClient = new HttpClient();
        }

        public async Task<string> GenerateStudyPlanAsync(string topic)
        {
            // 1. Địa chỉ chuẩn xác của Gemini 1.5 Flash
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";

            // 2. Lời dặn dò AI
            string prompt = $"Hãy đóng vai một chuyên gia giáo dục. Tạo một lộ trình học tập chi tiết, từng bước cho chủ đề: '{topic}'. Trình bày ngắn gọn, dễ hiểu.";

            // 3. Đóng gói dữ liệu
            var requestBody = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };

            string jsonBody = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // 4. Bấm nút gửi đi
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                string errorDetail = await response.Content.ReadAsStringAsync();
                throw new Exception($"Lỗi từ Google: {response.StatusCode} - {errorDetail}");
            }

            // 5. Bóc quà AI trả về
            string responseString = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(responseString);
            var aiText = doc.RootElement
                            .GetProperty("candidates")[0]
                            .GetProperty("content")
                            .GetProperty("parts")[0]
                            .GetProperty("text")
                            .GetString();

            return aiText;
        }
    }
}