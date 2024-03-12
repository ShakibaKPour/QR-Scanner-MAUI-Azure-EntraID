using RepRepair.Extensions;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepRepair.Services.DefectListService
{
    public class DefectListFetchService
    {
        private readonly IDatabaseService _databaseService;

        public List<DefectList> CachedList { get; } = new List<DefectList>();

        public DefectListFetchService()
        {
            _databaseService = ServiceHelper.GetService<IDatabaseService>();
        }
        public async Task<List<DefectList>> GetReportTypesAsync()
        {
            if (CachedList != null && CachedList.Count > 0)
            {
                return CachedList.ToList();
            }

            var defectList = await _databaseService.GetDefectListAsync();
            if (defectList != null && defectList.Count > 0)
            {
                CachedList?.Clear();
                foreach (var defect in defectList)
                {
                    CachedList?.Add(defect);
                }
                return CachedList;
            }
            else
            {
                return new List<DefectList>();
            }
        }
    }
}
