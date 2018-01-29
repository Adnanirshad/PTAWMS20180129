using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTAWMS.Helper
{
    public class DMParentModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
    public class DMTMSParentModel
    {
        public List<DMTMSParentList> ChildList { get; set; }
        public int LateIn { get; set; }
        public int LateOut { get; set; }
        public int EarlyIn { get; set; }
        public int EarlyOut { get; set; }
        public int GraphType { get; set; }
    }
    public class DMTMSParentList
    {
        public string Name { get; set; }
        public int CountLI { get; set; }
        public int CountLO { get; set; }
        public int CountEI { get; set; }
        public int CountEO { get; set; }
    }
    public class DMPieChartParentModel
    {
        public List<DMParentModel> ChildList { get; set; }
        public int ID { get; set; }
        public string GraphName { get; set; }
        public string DivDept { get; set; }
        public string Label { get; set; }
        public string SubLabel { get; set; }
        public string HeaderID { get; set; }
        public string HeaderName { get; set; }
        public string HeaderCount { get; set; }
    }
}