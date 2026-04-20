using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.StudentHandlers.DTOs;
using Rafeek.Domain.Entities;
using System.Net.Http.Json;

namespace Rafeek.Application.Handlers.StudentHandlers.Commands.AskAi
{
    public class AskAiCommandHandler : IRequestHandler<AskAiCommand, AiChatResponseDto>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IRafeekDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public AskAiCommandHandler(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IRafeekDbContext dbContext,
            ICurrentUserService currentUserService)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<AiChatResponseDto> Handle(AskAiCommand request, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["AiService:BaseUrl"] ?? "https://rafiq-bot-g003.onrender.com";

            // جلب UserId من التوكن (أو Guid.Empty إذا لم يكن مسجلاً - للاختبار فقط)
            var userId = _currentUserService.UserId;

            // جلب بيانات الطالب مباشرة من DbContext (بدون Mediator لتجنب تعارض DbContext)
            var student = userId != Guid.Empty
                ? await _dbContext.Students
                    .Include(s => s.Department)
                    .Include(s => s.AcademicProfile)
                    .FirstOrDefaultAsync(s => s.UserId == userId || s.Id == userId, cancellationToken)
                : null;

            // إذا لم يُعثر على الطالب (غير مسجل أو لأغراض الاختبار)، استخدم أول طالب
            if (student == null)
            {
                student = await _dbContext.Students
                    .Include(s => s.Department)
                    .Include(s => s.AcademicProfile)
                    .FirstOrDefaultAsync(s => s.IsActive, cancellationToken);
            }

            // جلب المواد الراسبة مباشرة من قاعدة البيانات
            var failedCoursesList = student != null
                ? await _dbContext.Enrollments
                    .Include(e => e.Course)
                    .Where(e => e.StudentId == student.Id && e.Grade == "F")
                    .Select(e => e.Course.Title)
                    .ToListAsync(cancellationToken)
                : new List<string>();

            string failedCoursesText = failedCoursesList.Any()
                ? string.Join("، ", failedCoursesList)
                : "لا يوجد";

            // تحويل المستوى من رقم إلى نص عربي
            string levelText = (student?.Level ?? 1) switch
            {
                1 => "الأولى",
                2 => "الثانية",
                3 => "الثالثة",
                4 => "الرابعة",
                _ => (student?.Level ?? 1).ToString()
            };

            // إنشاء الـ student_profile بالشكل المطلوب من سيرفر AI
            var aiStudentProfile = new
            {
                level = levelText,
                department = student?.Department?.Name ?? "عام",
                status = "منتظم",
                gpa = (student?.AcademicProfile?.CGPA ?? 0f).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture),
                failed_courses = failedCoursesText
            };

            // تحويل History إلى الشكل المطلوب
            var history = (request.History ?? new List<ChatMessageDto>())
                .Select(msg => new Dictionary<string, string>
                {
                    { "role", msg.Role },
                    { "content", msg.Content }
                })
                .ToList();

            var aiRequest = new
            {
                question = request.Question,
                history = history,
                student_profile = aiStudentProfile
            };

            try
            {
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/ask", aiRequest, cancellationToken);
                response.EnsureSuccessStatusCode();

                var aiResponse = await response.Content.ReadFromJsonAsync<AiChatResponseDto>(cancellationToken: cancellationToken);
                var answer = aiResponse?.Answer ?? "حدث خطأ غير متوقع أثناء تحليل الإجابة.";

                // معالجة SessionId: استخدم المرسل أو قم بإنشاء واحد جديد
                Guid sessionId = request.SessionId ?? Guid.NewGuid();

                // حفظ المحادثة في قاعدة البيانات
                if (student != null)
                {
                    if (request.SessionId == null)
                    {
                        var chatSession = new ChatSession
                        {
                            Id = sessionId,
                            UserId = userId != Guid.Empty ? userId : student.UserId, // Use UserId here
                            Title = request.Question.Substring(0, Math.Min(50, request.Question.Length)) + "...",
                            CreatedBy = userId == Guid.Empty ? "anonymous" : userId.ToString()
                        };
                        await _dbContext.ChatSessions.AddAsync(chatSession, cancellationToken);
                    }

                    var chatbotQuery = new ChatbotQuery
                    {
                        StudentId = student.Id,
                        SessionId = sessionId,
                        Query = request.Question,
                        Response = answer,
                        CreatedBy = userId == Guid.Empty ? "anonymous" : userId.ToString()
                    };

                    await _dbContext.ChatbotQueries.AddAsync(chatbotQuery, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }

                return new AiChatResponseDto { Answer = answer, SessionId = sessionId };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AI API ERROR: {ex}");
                return new AiChatResponseDto { Answer = "أعتذر، حدثت مشكلة أثناء الاتصال بالذكاء الاصطناعي. الرجاء المحاولة لاحقاً.", SessionId = Guid.Empty };
            }
        }
    }
}
