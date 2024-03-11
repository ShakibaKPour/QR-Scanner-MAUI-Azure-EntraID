using RepRepair.Extensions;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.DB;

namespace RepRepair.Services.ReportTypesService
{
    public class ReportServiceType
    {
        private readonly IDatabaseService _databaseService;

        public List<ReportType> CachedReportTypes { get; } = new List<ReportType>();

        public ReportServiceType()
        {
                _databaseService = ServiceHelper.GetService<IDatabaseService>();
        }
        public async Task<List<ReportType>> GetReportTypesAsync()
        {
            if( CachedReportTypes != null && CachedReportTypes.Count > 0)
            {
                return CachedReportTypes;
            }

            var reportTypes = await _databaseService.GetReportTypesAsync();
            if (reportTypes != null && reportTypes.Count > 0)
            {
                CachedReportTypes?.Clear();
                foreach (var reportType in reportTypes)
                {
                    CachedReportTypes?.Add(reportType);
                }
                return CachedReportTypes;
            }
            return new List<ReportType>();
        }

    }
}
