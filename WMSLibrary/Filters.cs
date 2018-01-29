using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace WMSLibrary
{
    public class Filters
    {

        public List<int> SyncDepartmentIDs(GridView gv, List<int> DepartmentIDs)
        {
            for (int i = 0; i < gv.Rows.Count; i++)
            {
                bool isChecked = ((CheckBox)gv.Rows[i].FindControl("CheckOne")).Checked;
                if (isChecked)  // Add to filter list
                {
                    //int DepartmentID = (int)gv.DataKeys[i].Value;
                    int DepartmentID = int.Parse(gv.Rows[i].Cells[0].Text);
                    DepartmentIDs.Add(DepartmentID);
                }
                else    // Remove from filter list
                {
                    //int DepartmentID = (int)gv.DataKeys[i].Value;
                    int DepartmentID = int.Parse(gv.Rows[i].Cells[0].Text);
                    for (int k = 0; k < DepartmentIDs.Count; k++)
                    {
                        if (DepartmentIDs[k] == DepartmentID)
                        {
                            DepartmentIDs.RemoveAt(k);
                        }
                    }
                }
            }
            return DepartmentIDs;
        }
        public List<int> SyncReaderIDs(GridView gv, List<int> ReaderIDs)
        {
            for (int i = 0; i < gv.Rows.Count; i++)
            {
                bool isChecked = ((CheckBox)gv.Rows[i].FindControl("CheckOne")).Checked;
                if (isChecked)  // Add to filter list
                {
                    //int DepartmentID = (int)gv.DataKeys[i].Value;
                    int ReaderID = int.Parse(gv.Rows[i].Cells[0].Text);
                    ReaderIDs.Add(ReaderID);
                }
                else    // Remove from filter list
                {
                    //int DepartmentID = (int)gv.DataKeys[i].Value;
                    int ReaderID = int.Parse(gv.Rows[i].Cells[0].Text);
                    for (int k = 0; k < ReaderIDs.Count; k++)
                    {
                        if (ReaderIDs[k] == ReaderID)
                        {
                            ReaderIDs.RemoveAt(k);
                        }
                    }
                }
            }
            return ReaderIDs;
        }

        public FiltersModel DeleteAllFilters(FiltersModel filtersModel)
        {
            //filtersModel.CityFilter = new List<FiltersAttributes>();
            filtersModel.CompanyFilter = new List<FiltersAttributes>();
            //filtersModel.CrewFilter = new List<FiltersAttributes>();
            filtersModel.DepartmentFilter = new List<FiltersAttributes>();
           // filtersModel.DivisionFilter = new List<FiltersAttributes>();
            filtersModel.EmployeeFilter = new List<FiltersAttributes>();
            filtersModel.LocationFilter = new List<FiltersAttributes>();
           // filtersModel.RegionFilter = new List<FiltersAttributes>();
            filtersModel.SectionFilter = new List<FiltersAttributes>();
            filtersModel.ShiftFilter = new List<FiltersAttributes>();
            filtersModel.TypeFilter = new List<FiltersAttributes>();
            filtersModel.ReaderFilter = new List<FiltersAttributes>();
            //filtersModel.BusinessFilter = new List<FiltersAttributes>();
            //filtersModel.BanksFilter = new List<FiltersAttributes>();
            //filtersModel.PaymentModeFilter = new List<FiltersAttributes>();
            filtersModel.CMDesignationFilter = new List<FiltersAttributes>();
            filtersModel.QualificationFilter = new List<FiltersAttributes>();
            filtersModel.DomicileFilter = new List<FiltersAttributes>();
            filtersModel.InstituteFilter = new List<FiltersAttributes>();
            filtersModel.DegreeFilter = new List<FiltersAttributes>();
            //filtersModel.ProductionOrderFilter = new List<FiltersAttributes>();
            return filtersModel;
        }

        public FiltersModel SyncGridViewIDs(GridView gv, FiltersModel filtersModel, string filterName)
        {

            if (filtersModel == null)
                return new FiltersModel();
            if (filtersModel.ReaderFilter == null)
                return new FiltersModel();
            for (int i = 0; i < gv.Rows.Count; i++)
            {
                bool isChecked = ((CheckBox)gv.Rows[i].FindControl("CheckOne")).Checked;
                if (isChecked)  // Add to filter list
                {
                    string ItemID = gv.Rows[i].Cells[0].Text;
                    //string ItemID = gv.DataKeys[i].Value.ToString();
                    string ItemName = gv.Rows[i].Cells[2].Text;

                    AddObjectToList(filtersModel, filterName, ItemID, ItemName);

                }
                else    // Remove from filter list
                {
                    //string ItemID = gv.DataKeys[i].Value.ToString();
                    string ItemID = gv.Rows[i].Cells[0].Text;
                    RemoveObjectFromList(filtersModel, filterName, ItemID);

                }
            }
            return filtersModel;
        }

        /// <summary>
        /// This function will remove the id of a specific deparment, company... form the session
        /// </summary>
        /// <param name="filtersModel"></param>
        /// <param name="filterName"></param>
        /// <param name="ItemID"></param>
        /// 
        public static FiltersModel DeleteSingleFilter(FiltersModel fml, string id,string filtername)
        {
             
            RemoveObjectFromList(fml, filtername, id);
            return fml;
        }
      
        private static void RemoveObjectFromList(FiltersModel filtersModel, string filterName, string ItemID)
        {
            switch (filterName)
            {
                //case "Company":
                //    for (int k = 0; k < filtersModel.CompanyFilter.Count; k++)
                //    {
                //        if (filtersModel.CompanyFilter[k].ID == ItemID)
                //            filtersModel.CompanyFilter.RemoveAt(k);
                //    }
                //    break;
                //case "Division":
                //    for (int k = 0; k < filtersModel.DivisionFilter.Count; k++)
                //    {
                //        if (filtersModel.DivisionFilter[k].ID == ItemID)
                //            filtersModel.DivisionFilter.RemoveAt(k);
                //    }
                //    break;
                    
                case "Department":
                    for (int k = 0; k < filtersModel.DepartmentFilter.Count; k++)
                    {
                        if (filtersModel.DepartmentFilter[k].ID == ItemID)
                            filtersModel.DepartmentFilter.RemoveAt(k);
                    }
                    break;
                case "Section":
                    for (int k = 0; k < filtersModel.SectionFilter.Count; k++)
                    {
                        if (filtersModel.SectionFilter[k].ID == ItemID)
                            filtersModel.SectionFilter.RemoveAt(k);
                    }
                    break;
                //case "City":
                //    for (int k = 0; k < filtersModel.CityFilter.Count; k++)
                //    {
                //        if (filtersModel.CityFilter[k].ID == ItemID)
                //            filtersModel.CityFilter.RemoveAt(k);
                //    }
                //    break;
                case "Location":
                    for (int k = 0; k < filtersModel.LocationFilter.Count; k++)
                    {
                        if (filtersModel.LocationFilter[k].ID == ItemID)
                            filtersModel.LocationFilter.RemoveAt(k);
                    }
                    break;
                case "Shift":
                    for (int k = 0; k < filtersModel.ShiftFilter.Count; k++)
                    {
                        if (filtersModel.ShiftFilter[k].ID == ItemID)
                            filtersModel.ShiftFilter.RemoveAt(k);
                    }
                    break;
                //case "Crew":
                //    for (int k = 0; k < filtersModel.CrewFilter.Count; k++)
                //    {
                //        if (filtersModel.CrewFilter[k].ID == ItemID)
                //            filtersModel.CrewFilter.RemoveAt(k);
                //    }
                //    break;
                case "Type":
                    for (int k = 0; k < filtersModel.TypeFilter.Count; k++)
                    {
                        if (filtersModel.TypeFilter[k].ID == ItemID)
                            filtersModel.TypeFilter.RemoveAt(k);
                    }
                    break;
                case "Employee":
                    for (int k = 0; k < filtersModel.EmployeeFilter.Count; k++)
                    {
                        if (filtersModel.EmployeeFilter[k].ID == ItemID)
                            filtersModel.EmployeeFilter.RemoveAt(k);
                    }
                    break;
                case "Reader":
                    for (int k = 0; k < filtersModel.ReaderFilter.Count; k++)
                    {
                        if (filtersModel.ReaderFilter[k].ID == ItemID)
                            filtersModel.ReaderFilter.RemoveAt(k);
                    }
                    break;
                //case "Business":
                //    for (int k = 0; k < filtersModel.BusinessFilter.Count; k++)
                //    {
                //        if (filtersModel.BusinessFilter[k].ID == ItemID)
                //            filtersModel.BusinessFilter.RemoveAt(k);
                //    }
                //    break;
                //case "Bank":
                //    for (int k = 0; k < filtersModel.BanksFilter.Count; k++)
                //    {
                //        if (filtersModel.BanksFilter[k].ID == ItemID)
                //            filtersModel.BanksFilter.RemoveAt(k);
                //    }
                //    break;
                //case "PaymentMode":
                //    for (int k = 0; k < filtersModel.PaymentModeFilter.Count; k++)
                //    {
                //        if (filtersModel.PaymentModeFilter[k].ID == ItemID)
                //            filtersModel.PaymentModeFilter.RemoveAt(k);
                //    }
                //    break;
                case "Designation":
                    for (int k = 0; k < filtersModel.CMDesignationFilter.Count; k++)
                    {
                        if (filtersModel.CMDesignationFilter[k].ID == ItemID)
                            filtersModel.CMDesignationFilter.RemoveAt(k);
                    }
                    break;
                case "Qualification":
                    for (int k = 0; k < filtersModel.QualificationFilter.Count; k++)
                    {
                        if (filtersModel.QualificationFilter[k].ID == ItemID)
                            filtersModel.QualificationFilter.RemoveAt(k);
                    }
                    break;
                case "Domicile":
                    for (int k = 0; k < filtersModel.DomicileFilter.Count; k++)
                    {
                        if (filtersModel.DomicileFilter[k].ID == ItemID)
                            filtersModel.DomicileFilter.RemoveAt(k);
                    }
                    break;
                case "Degree":
                    for (int k = 0; k < filtersModel.DegreeFilter.Count; k++)
                    {
                        if (filtersModel.DegreeFilter[k].ID == ItemID)
                            filtersModel.DegreeFilter.RemoveAt(k);
                    }
                    break;
                case "Institute":
                    for (int k = 0; k < filtersModel.InstituteFilter.Count; k++)
                    {
                        if (filtersModel.InstituteFilter[k].ID == ItemID)
                            filtersModel.InstituteFilter.RemoveAt(k);
                    }
                    break;
                    //case "ProductionOrder":
                    //    for (int k = 0; k < filtersModel.ProductionOrderFilter.Count; k++)
                    //    {
                    //        if (filtersModel.ProductionOrderFilter[k].ID == ItemID)
                    //            filtersModel.ProductionOrderFilter.RemoveAt(k);
                    //    }
                    //    break;
            }
        }

        private void AddObjectToList(FiltersModel filtersModel, string filterName, string ItemID, string ItemName)
        {
            switch (filterName)
            {
                case "Company":
                    if (filtersModel.CompanyFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                        filtersModel.CompanyFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                    break;
                //case "Division":
                //    if (filtersModel.DivisionFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                //        filtersModel.DivisionFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                //    break;
                case "Department":
                    if (filtersModel.DepartmentFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                        filtersModel.DepartmentFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                    break;
                //case "City":
                //    if (filtersModel.CityFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                //        filtersModel.CityFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                //    break;
                case "Section":
                    if (filtersModel.SectionFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                        filtersModel.SectionFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                    break;
                case "Location":
                    if (filtersModel.LocationFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                        filtersModel.LocationFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                    break;
                case "Shift":
                    if (filtersModel.ShiftFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                        filtersModel.ShiftFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                    break;
                case "Type":
                    if (filtersModel.TypeFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                        filtersModel.TypeFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                    break;
                case "Employee":
                    if (filtersModel.EmployeeFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                        filtersModel.EmployeeFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                    break;
                //case "Crew":
                //    if (filtersModel.CrewFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                //        filtersModel.CrewFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                //    break;
                case "Reader":
                    if (filtersModel.ReaderFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                        filtersModel.ReaderFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                    break;
                //case "Business":
                //    if (filtersModel.BusinessFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                //        filtersModel.BusinessFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                //    break;
                //case "Bank":
                //    if (filtersModel.BanksFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                //        filtersModel.BanksFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                //    break;
                //case "PaymentMode":
                //    if (filtersModel.PaymentModeFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                //        filtersModel.PaymentModeFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                //    break;
                case "Designation":
                    if (filtersModel.CMDesignationFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                        filtersModel.CMDesignationFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                    break;
                case "Qualification":
                    if (filtersModel.QualificationFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                        filtersModel.QualificationFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                    break;

                case "Domicile":
                    if (filtersModel.DomicileFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                        filtersModel.DomicileFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                    break;
                case "Degree":
                    if (filtersModel.DegreeFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                        filtersModel.DegreeFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                    break;
                case "Institute":
                    if (filtersModel.InstituteFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                        filtersModel.InstituteFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                    break;
                    //case "ProductionOrder":
                    //    if (filtersModel.ProductionOrderFilter.Where(aa => aa.ID == ItemID).Count() == 0)
                    //        filtersModel.ProductionOrderFilter.Add(new FiltersAttributes() { ID = ItemID, FilterName = ItemName });
                    //    break;

            }
        }

        public static void SetGridViewCheckState(GridView gv, FiltersModel filtersModel, string filterName)
        {
            if (filtersModel == null)
                return;
            switch (filterName)
            {
                case "Department":
                    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.DepartmentFilter);
                    break;
                //case "Region":
                //    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.RegionFilter);
                //    break;
                case "Section":
                    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.SectionFilter);
                    break;
                //case "Company":
                //    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.CompanyFilter);
                //    break;
                case "Location":
                    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.LocationFilter);
                    break;
                //case "City":
                //    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.CityFilter);
                //    break;
                case "Shift":
                    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.ShiftFilter);
                    break;
                //case "Division":
                //    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.DivisionFilter);
                //    break;
                case "Employee":
                    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.EmployeeFilter);
                    break;
                //case "Crew":
                //    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.CrewFilter);
                //    break;
                case "Type":
                    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.TypeFilter);
                    break;
                case "Reader":
                    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.ReaderFilter);
                    break;
                //case "Business":
                //    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.BusinessFilter);
                //    break;
                //case "Bank":
                //    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.BanksFilter);
                //    break;
                //case "PaymentMode":
                //    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.PaymentModeFilter);
                //    break;
                case "Designation":
                    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.CMDesignationFilter);
                    break;
                case "Qualification":
                    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.QualificationFilter);
                    break;
                case "Domicile":
                    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.DomicileFilter);
                    break;
                case "Degree":
                    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.DegreeFilter);
                    break;
                case "Institute":
                    SetGridViewCheckStateChild(gv, filtersModel, filtersModel.InstituteFilter);
                    break;
                    //case "ProductionOrder":
                    //   SetGridViewCheckStateChild(gv, filtersModel, filtersModel.ProductionOrderFilter);
                    //   break;
            }

        }

        private static void SetGridViewCheckStateChild(GridView gv, FiltersModel filtersModel, List<FiltersAttributes> list)
        {

            if (list == null)
                return;
            for(int j = 0; j < gv.Rows.Count; j++)
            {
                bool chk = false;
                for (int i = 0; i < list.Count; i++) 
                {
                    if (list[i].ID == gv.Rows[j].Cells[0].Text)
                    {
                        chk = true;
                    }
                }
                ((CheckBox)gv.Rows[j].FindControl("CheckOne")).Checked = chk;
            }
        }


       
    }


    public class FiltersAttributes
    {
        public string FilterName;
        public string ID;
      
    }

    public class FiltersModel
    {
        public List<FiltersAttributes> CompanyFilter = new List<FiltersAttributes>();
        //public List<FiltersAttributes> DivisionFilter = new List<FiltersAttributes>();
        public List<FiltersAttributes> DepartmentFilter = new List<FiltersAttributes>();
        public List<FiltersAttributes> SectionFilter = new List<FiltersAttributes>();
        public List<FiltersAttributes> LocationFilter = new List<FiltersAttributes>();
        public List<FiltersAttributes> ShiftFilter = new List<FiltersAttributes>();
        //public List<FiltersAttributes> RegionFilter = new List<FiltersAttributes>();
       // public List<FiltersAttributes> CityFilter = new List<FiltersAttributes>();
        //public List<FiltersAttributes> CrewFilter = new List<FiltersAttributes>();
        public List<FiltersAttributes> EmployeeFilter = new List<FiltersAttributes>();
        public List<FiltersAttributes> TypeFilter = new List<FiltersAttributes>();
        public List<FiltersAttributes> ReaderFilter = new List<FiltersAttributes>();
       // public List<FiltersAttributes> BusinessFilter = new List<FiltersAttributes>();
       // public List<FiltersAttributes> BanksFilter = new List<FiltersAttributes>();
       // public List<FiltersAttributes> PaymentModeFilter = new List<FiltersAttributes>();
        public List<FiltersAttributes> CMDesignationFilter = new List<FiltersAttributes>();

        public List<FiltersAttributes> QualificationFilter = new List<FiltersAttributes>();
        public List<FiltersAttributes>DomicileFilter = new List<FiltersAttributes>();
        public List<FiltersAttributes> DegreeFilter = new List<FiltersAttributes>();
        public List<FiltersAttributes> InstituteFilter = new List<FiltersAttributes>();
        // public List<FiltersAttributes> ProductionOrderFilter = new List<FiltersAttributes>();
    }
}
