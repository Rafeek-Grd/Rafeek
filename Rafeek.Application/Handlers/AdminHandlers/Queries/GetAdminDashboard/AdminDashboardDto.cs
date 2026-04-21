namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetAdminDashboard
{
    public class AdminDashboardDto
    {
        public AcademicLevelTrendDto AcademicLevelTrend { get; set; } = new();
        public BatchDistributionDto BatchDistribution { get; set; } = new();
        public AcademicStatusAnalysisDto AcademicStatusAnalysis { get; set; } = new();
        public AcademicObstaclesDto AcademicObstacles { get; set; } = new();
    }

    public class AcademicLevelTrendDto
    {
        /// <summary>نسبة التغيير مقارنة بالفصل السابق (مثال: +10)</summary>
        public float ChangePercentage { get; set; }

        /// <summary>نقاط بيانات الرسم البياني الخطي مرتبة زمنياً</summary>
        public List<GpaTrendPointDto> DataPoints { get; set; } = new();
    }

    public class GpaTrendPointDto
    {
        /// <summary>اسم الشهر أو الفصل (مثال: سبتمبر)</summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>متوسط المعدل لهذه النقطة الزمنية</summary>
        public float AverageGpa { get; set; }
    }

    public class BatchDistributionDto
    {
        public BatchSliceDto FirstYear { get; set; } = new();
        public BatchSliceDto SecondYear { get; set; } = new();
        public BatchSliceDto AdvancedYears { get; set; } = new();
    }

    public class BatchSliceDto
    {
        public int Count { get; set; }
        public float Percentage { get; set; }
    }

    public class AcademicStatusAnalysisDto
    {
        public StatusSliceDto Stable { get; set; } = new();
        public StatusSliceDto Warning { get; set; } = new();
        public StatusSliceDto Monitored { get; set; } = new();
    }

    public class StatusSliceDto
    {
        public int Count { get; set; }
        public float Percentage { get; set; }
    }

    public class AcademicObstaclesDto
    {
        /// <summary>قيود التسجيل</summary>
        public int RegistrationHolds { get; set; }

        /// <summary>إنذار أكاديمي</summary>
        public int AcademicProbation { get; set; }

        /// <summary>متطلبات ناقصة</summary>
        public int MissingRequirements { get; set; }
    }
}
