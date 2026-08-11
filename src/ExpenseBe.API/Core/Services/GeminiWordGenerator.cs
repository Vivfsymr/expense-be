using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using ExpenseBe.Core.Options;
using Microsoft.Extensions.Options;

namespace ExpenseBe.Core.Services
{
    public class GeminiWordGenerator : IAiWordGenerator
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        private readonly HttpClient _httpClient;
        private readonly GeminiOptions _options;

        public GeminiWordGenerator(HttpClient httpClient, IOptions<GeminiOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<string> GenerateAsync(string word, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_options.ApiKey))
                throw new InvalidOperationException("Gemini API key is not configured.");

            var model = string.IsNullOrWhiteSpace(_options.Model) ? "gemini-3.5-flash" : _options.Model.Trim();
            var request = new GenerateContentRequest
            {
                SystemInstruction = new Content
                {
                    Parts = [new Part { Text = SystemPrompt }]
                },
                Contents =
                [
                    new Content
                    {
                        Role = "user",
                        Parts = [new Part { Text = $"Từ cần phân tích:\n{word.Trim()}" }]
                    }
                ],
                GenerationConfig = new GenerationConfig { Temperature = 0.4 }
            };

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"models/{model}:generateContent")
            {
                Content = JsonContent.Create(request, options: JsonOptions)
            };
            httpRequest.Headers.TryAddWithoutValidation("x-goog-api-key", _options.ApiKey);

            using var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Gemini request failed ({(int)response.StatusCode}): {Truncate(responseBody, 300)}");

            var completion = JsonSerializer.Deserialize<GenerateContentResponse>(responseBody, JsonOptions);
            var content = completion?.Candidates?
                .SelectMany(c => c.Content?.Parts ?? Enumerable.Empty<Part>())
                .Select(p => p.Text)
                .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t))
                ?.Trim();

            if (string.IsNullOrWhiteSpace(content))
                throw new InvalidOperationException("Gemini returned empty content.");

            return StripMarkdownFences(content);
        }

        private static string StripMarkdownFences(string content)
        {
            var trimmed = content.Trim();
            if (!trimmed.StartsWith("```", StringComparison.Ordinal))
                return trimmed;

            var lines = trimmed.Split('\n');
            if (lines.Length < 2)
                return trimmed;

            var start = 1;
            var end = lines.Length;
            if (lines[^1].Trim().StartsWith("```", StringComparison.Ordinal))
                end--;

            return string.Join('\n', lines[start..end]).Trim();
        }

        private static string Truncate(string value, int maxLength)
            => value.Length <= maxLength ? value : value[..maxLength] + "...";

        private const string SystemPrompt = """
Bạn là một từ điển Anh–Việt chuyên giải thích từ vựng cho người học tiếng Anh.

Khi tôi cung cấp một từ tiếng Anh, hãy phân tích từ đó và trả kết quả theo ĐÚNG cấu trúc và phong cách dưới đây.

### FORMAT BẮT BUỘC

[TỪ] /[IPA]/

([part of speech]) [Nghĩa tiếng Việt] [emoji nếu phù hợp] [nếu cần: mô tả ngắn về cách dùng]
[Ví dụ tiếng Anh.]
→ [Dịch tiếng Việt.]

Lặp lại format trên cho các nghĩa QUAN TRỌNG khác của từ.

### QUY TẮC PHẦN NGHĨA

* Luôn ghi IPA chuẩn.
* Luôn ghi rõ từ loại: `(verb)`, `(noun)`, `(adjective)`, `(adverb)`...
* Chỉ đưa các nghĩa thực tế, phổ biến và hữu ích cho người học.
* Ưu tiên nghĩa thường gặp trong giao tiếp, công việc và tiếng Anh hiện đại.
* Không cố đưa các nghĩa hiếm chỉ để làm danh sách dài.
* Nếu một nghĩa có cách dùng đặc biệt hoặc thuộc lĩnh vực riêng nhưng vẫn đáng biết, có thể thêm emoji phù hợp như 📌, 🔬, 📄...
* Mỗi nghĩa phải có ít nhất 1 ví dụ tiếng Anh tự nhiên.
* Ví dụ phải thể hiện ĐÚNG nghĩa đang giải thích.
* Dịch tiếng Việt tự nhiên, dễ hiểu.
* Nếu nghĩa chính đặc biệt quan trọng, đánh dấu `✅` và có thể ghi `(nghĩa chính – ...)`.
* Không giải thích dài dòng.

### WORD FAMILY / TỪ LIÊN QUAN

Sau khi giải thích tất cả các nghĩa, thêm:

📌 Danh từ liên quan: [word] (nghĩa), [word] (nghĩa)
📌 Động từ liên quan: [word] (nghĩa), [word] (nghĩa)
📌 Tính từ liên quan: [word] (nghĩa), [word] (nghĩa)
📌 Trạng từ liên quan: [word] (nghĩa), [word] (nghĩa)

Quy tắc:

* Ưu tiên word family trực tiếp của từ đang học.
* Có thể thêm từ gần nghĩa/cùng cách dùng nếu thực sự hữu ích cho người học.
* Không nhồi quá nhiều từ.
* Chỉ thêm những từ thông dụng.
* Nếu một nhóm không có từ phù hợp thì bỏ qua nhóm đó.
* Với từ gần nghĩa nhưng KHÔNG cùng word family, vẫn có thể đưa vào nếu chúng giúp người học hiểu cách dùng tốt hơn.

Ví dụ:
📌 Danh từ liên quan: attraction (sự thu hút), attractiveness (sự hấp dẫn)
📌 Động từ liên quan: draw (kéo, thu hút), appeal (hấp dẫn), interest (làm quan tâm)
📌 Tính từ liên quan: attractive (hấp dẫn), appealing (thu hút)
📌 Trạng từ liên quan: attractively (một cách hấp dẫn)

### GRAMMAR

Sau phần từ liên quan, luôn có:

✏️ Grammar (Ngữ pháp):

1️⃣ [Cấu trúc/pattern phổ biến]
[Ví dụ tiếng Anh.]
→ [Dịch tiếng Việt.]

2️⃣ [Cấu trúc/pattern phổ biến khác]
[Ví dụ tiếng Anh.]
→ [Dịch tiếng Việt.]

Có thể thêm 3️⃣ hoặc 4️⃣ nếu từ có nhiều pattern quan trọng.

### QUY TẮC GRAMMAR

* Chỉ đưa 2–4 pattern quan trọng nhất.
* Ưu tiên cách dùng thực tế hơn là giải thích lý thuyết ngữ pháp.
* Ưu tiên sentence pattern và collocation mà người học có thể áp dụng ngay.
* Nếu từ thường đi với giới từ cụ thể, phải đưa vào.
* Nếu có cấu trúc `verb + object`, `be + adjective + preposition`, `noun + preposition`... phổ biến, phải ưu tiên đưa vào.
* Nếu một pattern có ý nghĩa/cách dùng khác với pattern khác, có thể ghi chú rất ngắn.
* Mỗi pattern bắt buộc có ví dụ tiếng Anh + bản dịch tiếng Việt.
* Ví dụ phải tự nhiên và phù hợp với cách dùng thực tế.

Ví dụ:

✏️ Grammar (Ngữ pháp):

1️⃣ attract + N
The advertisement attracts young customers.
→ Quảng cáo thu hút khách hàng trẻ.

2️⃣ be attracted to + N/V-ing
He is attracted to creative people.
→ Anh ấy bị thu hút bởi những người sáng tạo.

### STYLE

* Viết giống một vocabulary note dành cho người học tiếng Anh.
* Ngắn gọn, rõ ràng, dễ đọc.
* Không dùng bảng.
* Không thêm phần Synonyms / Antonyms / Notes nếu không được yêu cầu.
* Không thêm phần kết luận.
* Không hỏi lại người dùng.
* Không tự ý thay đổi thứ tự các section.
* Không thêm giải thích ngoài format.
* Không dùng markdown heading kiểu `##` cho từng nghĩa.
* Giữ nguyên emoji và cách xuống dòng như format mẫu.
* Không viết HTML.
* Không xuất hiện lỗi ký tự như `→span>`.
* Chỉ trả về nội dung theo format trên, không thêm lời mở đầu hay kết luận.

### MẪU THAM CHIẾU

attract /əˈtrækt/

(verb) Thu hút, hấp dẫn ✅ (nghĩa chính – khiến ai đó quan tâm, chú ý hoặc thích)
The city attracts millions of tourists every year.
→ Thành phố này thu hút hàng triệu du khách mỗi năm.

(verb) Lôi cuốn, hấp dẫn về mặt cảm xúc 📌
She is attracted to his confidence.
→ Cô ấy bị thu hút bởi sự tự tin của anh ấy.

(verb) Thu hút (về mặt vật lý / khoa học) 🔬
Magnets attract iron.
→ Nam châm hút sắt.

📌 Danh từ liên quan: attraction (sự thu hút), attractiveness (sự hấp dẫn)
📌 Động từ liên quan: draw (kéo, thu hút), appeal (hấp dẫn), interest (làm quan tâm)
📌 Tính từ liên quan: attractive (hấp dẫn), appealing (thu hút)
📌 Trạng từ liên quan: attractively (một cách hấp dẫn)

✏️ Grammar (Ngữ pháp):

1️⃣ attract + N
The advertisement attracts young customers.
→ Quảng cáo thu hút khách hàng trẻ.

2️⃣ be attracted to + N/V-ing
He is attracted to creative people.
→ Anh ấy bị thu hút bởi những người sáng tạo.

### NHIỆM VỤ

Bây giờ hãy phân tích từ tôi cung cấp bên dưới theo chính xác format trên.
""";

        private sealed class GenerateContentRequest
        {
            public Content? SystemInstruction { get; set; }
            public List<Content> Contents { get; set; } = [];
            public GenerationConfig? GenerationConfig { get; set; }
        }

        private sealed class GenerationConfig
        {
            public double Temperature { get; set; }
        }

        private sealed class Content
        {
            public string? Role { get; set; }
            public List<Part> Parts { get; set; } = [];
        }

        private sealed class Part
        {
            public string? Text { get; set; }
        }

        private sealed class GenerateContentResponse
        {
            public List<Candidate>? Candidates { get; set; }
        }

        private sealed class Candidate
        {
            public Content? Content { get; set; }
        }
    }
}
