using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PTAWMS.Helper
{
    public class QueryBuilder
    {
        internal string QueryForRegionInFilters(Models.ViewUserEmp LoggedInUser)
        {
            return "";
        }
        public DataTable GetValuesfromDB(string query)
        {
            DataTable dt = new DataTable();
            string conn = string.Empty;
            using (HRMEntities db = new HRMEntities())
            {
                conn = db.Database.Connection.ConnectionString;
            }
            SqlConnection Conn = new SqlConnection(conn);
            using (SqlCommand cmdd = Conn.CreateCommand())
            using (SqlDataAdapter sda = new SqlDataAdapter(cmdd))
            {
                cmdd.CommandText = query;
                cmdd.CommandType = CommandType.Text;
                Conn.Open();
                sda.Fill(dt);
                Conn.Close();
            }
            return dt;
        }


        internal string QueryForCompanyView(Models.ViewUserEmp LoggedInUser)
        {
            return "";
        }

        internal string MakeCustomizeQueryForEmpView(Models.ViewUserEmp LoggedInUser)
        {
            return "";
        }


        #region -- Reports Filters Data Seggregation according to User Role--

        internal string QueryForReportsCity(ViewUserEmp LoggedInUser)
        {
            string query = " where ";
            HRMEntities db = new HRMEntities();
            List<UserRoleData> uAcc = new List<UserRoleData>();
            uAcc = db.UserRoleDatas.Where(aa => aa.RoleUserID == LoggedInUser.UserID && aa.UserRoleLegend == "L").ToList();
            //List<HR_Region> regions = db.HR_Region.ToList();
            List<HR_City> cities = db.HR_City.ToList();
            List<HR_Location> locs = db.HR_Location.ToList();
            List<string> queryList = new List<string>();
            foreach (var access in uAcc)
            {
                switch (LoggedInUser.UserType)
                {
                    case "A"://ADmin
                        query = "";
                        break;
                    case "R"://REgion
                        List<HR_City> city = new List<HR_City>();
                        // city = db.HR_City.Where(aa => aa.CityID == access.RoleDataValue).ToList();
                        foreach (var c in city)
                        {
                            queryList.Add(" CitID =" + c.CityID);
                        }
                        break;
                    //case "C"://City
                    //    string cityID = cities.Where(aa => aa.CityID == access.RoleDataValue).FirstOrDefault().CityID.ToString();
                    //    queryList.Add(" CitID =" + cityID);
                    //    break;
                    //case "L"://Location
                    //    string cityIDForLoc = locs.Where(aa => aa.LocID == access.RoleDataValue).FirstOrDefault().CityID.ToString();
                    //    queryList.Add(" CitID =" + cityIDForLoc);
                    //    break;
                }
            }
            if (queryList.Count == 1)
            {
                query = query + queryList[0];
            }
            else if (queryList.Count > 1)
            {
                for (int i = 0; i < queryList.Count - 1; i++)
                {
                    query = query + queryList[i] + " or ";
                }
                query = query + queryList[queryList.Count - 1];
            }

            return query;
        }

        internal string QueryForLocReport(ViewUserEmp LoggedInUser)
        {
            string query = "";
            HRMEntities db = new HRMEntities();
            List<UserRoleData> uAcc = new List<UserRoleData>();
            uAcc = db.UserRoleDatas.Where(aa => aa.RoleUserID == LoggedInUser.UserID && aa.UserRoleLegend == "L").ToList();
            List<HR_Location> locss = db.HR_Location.ToList();
            List<string> queryList = new List<string>();
            foreach (var access in uAcc)
            {
                switch (LoggedInUser.UserType)
                {
                    case "A"://ADmin
                        query = "";
                        List<HR_Location> locs = new List<HR_Location>();
                        break;
                    //case "R"://REgion
                    //    List<HR_Location> locs = new List<HR_Location>();
                    //    locs = db.HR_Location.Where(aa => aa.HR_City.CityID == access.RoleDataValue).ToList();
                    //    foreach (var c in locs)
                    //    {
                    //        queryList.Add(" LocID =" + c.LocID);
                    //    }
                    //    break;
                    //case "C"://City
                    //    locs = db.HR_Location.Where(aa => aa.CityID == access.RoleDataValue).ToList();
                    //    foreach (var c in locs)
                    //    {
                    //        queryList.Add(" LocID =" + c.LocID);
                    //    }
                    //    break;
                    case "L"://Location
                        if (locss.Where(aa => aa.LocID == access.RoleDataValue).Count() > 0)
                        {
                            string cityIDForLoc = locss.Where(aa => aa.LocID == access.RoleDataValue).FirstOrDefault().LocID.ToString();
                            queryList.Add(" LocID =" + cityIDForLoc);
                        }
                        break;
                }
            }
            if (queryList.Count == 1)
            {
                query = query + queryList[0];
            }
            else if (queryList.Count > 1)
            {
                for (int i = 0; i < queryList.Count - 1; i++)
                {
                    query = query + queryList[i] + " or ";
                }
                query = query + queryList[queryList.Count - 1];
            }

            return query;
        }

        internal string QueryForDivisionInFilters(ViewUserEmp LoggedInUser)
        {
            string query = " where ";
            HRMEntities db = new HRMEntities();
            List<UserRoleData> uAcc = new List<UserRoleData>();
            uAcc = db.UserRoleDatas.Where(aa => aa.RoleUserID == LoggedInUser.UserID && aa.UserRoleLegend == "D").ToList();
            //List<HR_Division> div = db.HR_Division.ToList();
            List<HR_Department> dept = db.HR_Department.ToList();
            List<HR_Section> sec = db.HR_Section.ToList();
            List<string> queryList = new List<string>();
            foreach (var access in uAcc)
            {
                switch (LoggedInUser.UserType)
                {
                    case "G"://Super ADmin
                        query = "";
                        break;
                    //case "V"://divi
                    //    queryList.Add(" DivID =" + access.RoleDataValue.ToString());
                    //    break;
                    //case "D"://dept
                    //    string regionID = dept.Where(aa => aa.DeptID == access.RoleDataValue).FirstOrDefault().DivsionID.ToString();
                    //    queryList.Add(" DivID =" + regionID);
                    //    break;
                    //case "S"://Section
                    //    string regionIDForLoc = sec.Where(aa => aa.SecID == access.RoleDataValue).FirstOrDefault().HR_Department.DivsionID.ToString();
                    //    queryList.Add(" DivID =" + regionIDForLoc);
                    //    break;
                }
            }
            if (queryList.Count == 1)
            {
                query = query + queryList[0];
            }
            else if (queryList.Count > 1)
            {
                for (int i = 0; i < queryList.Count - 1; i++)
                {
                    query = query + queryList[i] + " or ";
                }
                query = query + queryList[queryList.Count - 1];
            }


            return query;
        }

        internal string QueryForReportsDepartment(ViewUserEmp LoggedInUser)
        {
            string query = "";
            HRMEntities db = new HRMEntities();
            List<UserRoleData> uAcc = new List<UserRoleData>();
            uAcc = db.UserRoleDatas.Where(aa => aa.RoleUserID == LoggedInUser.UserID && aa.UserRoleLegend == "D").ToList();
            //List<HR_Division> regions = db.HR_Division.ToList();
            List<HR_Department> departments = db.HR_Department.ToList();
            List<HR_Section> locs = db.HR_Section.ToList();
            List<string> queryList = new List<string>();
            foreach (var access in uAcc)
            {
                switch (LoggedInUser.UserType)
                {
                    case "G"://ADmin
                        query = "";
                        break;
                    //case "V"://Division
                    //    List<HR_Department> Department = new List<HR_Department>();
                    //    Department = db.HR_Department.Where(aa => aa.DivsionID == access.RoleDataValue).ToList();
                    //    foreach (var c in Department)
                    //    {
                    //        queryList.Add(" DeptID =" + c.DeptID);
                    //    }
                    //    break;
                    case "D"://Depts
                        string DeptID = departments.Where(aa => aa.DeptID == access.RoleDataValue).FirstOrDefault().DeptID.ToString();
                        queryList.Add(" DeptID =" + DeptID);
                        break;
                    case "S"://Sections
                        string deptidForLoc = locs.Where(aa => aa.SecID == access.RoleDataValue).FirstOrDefault().DepartmentID.ToString();
                        queryList.Add(" DeptID =" + deptidForLoc);
                        break;
                }
            }
            if (queryList.Count == 1)
            {
                query = query + queryList[0];
            }
            else if (queryList.Count > 1)
            {
                for (int i = 0; i < queryList.Count - 1; i++)
                {
                    query = query + queryList[i] + " or ";
                }
                query = query + queryList[queryList.Count - 1];
            }

            return query;
        }

        internal string QueryForSectionReport(ViewUserEmp LoggedInUser)
        {
            string query = "";
            HRMEntities db = new HRMEntities();
            List<UserRoleData> uAcc = new List<UserRoleData>();
            uAcc = db.UserRoleDatas.Where(aa => aa.RoleUserID == LoggedInUser.UserID && aa.UserRoleLegend == "D").ToList();
            // List<HR_Division> division = db.HR_Division.ToList();
            List<HR_Department> department = db.HR_Department.ToList();
            List<HR_Section> section = db.HR_Section.ToList();
            List<string> queryList = new List<string>();
            foreach (var access in uAcc)
            {
                switch (LoggedInUser.UserType)
                {
                    case "G"://ADmin
                        query = "";
                        List<HR_Section> locs = new List<HR_Section>();
                        break;
                    //case "V"://Division
                    //    List<HR_Section> locs = new List<HR_Section>();
                    //    locs = db.HR_Section.Where(aa => aa.HR_Department.DivsionID == access.RoleDataValue).ToList();
                    //    foreach (var c in locs)
                    //    {
                    //        queryList.Add(" SecID =" + c.SecID);
                    //    }
                    //    break;
                    case "D"://Depts
                        locs = db.HR_Section.Where(aa => aa.DepartmentID == access.RoleDataValue).ToList();
                        foreach (var c in locs)
                        {
                            queryList.Add(" SecID =" + c.SecID);
                        }
                        break;
                    case "S"://Section
                        string cityIDForLoc = section.Where(aa => aa.SecID == access.RoleDataValue).FirstOrDefault().SecID.ToString();
                        queryList.Add(" SecID =" + cityIDForLoc);
                        break;
                }
            }
            if (queryList.Count == 1)
            {
                query = query + queryList[0];
            }
            else if (queryList.Count > 1)
            {
                for (int i = 0; i < queryList.Count - 1; i++)
                {
                    query = query + queryList[i] + " or ";
                }
                query = query + queryList[queryList.Count - 1];
            }

            return query;
        }
        internal string QueryForEmployeeTypeReport(ViewUserEmp LoggedInUser)
        {
            string query = "";
            HRMEntities db = new HRMEntities();
            List<UserRoleData> uAcc = new List<UserRoleData>();
            uAcc = db.UserRoleDatas.Where(aa => aa.RoleUserID == LoggedInUser.UserID && aa.UserRoleLegend == "T").ToList();
            List<HR_EmpType> emptype = db.HR_EmpType.ToList();
            List<string> queryList = new List<string>();
            foreach (var access in uAcc)
            {
                switch (LoggedInUser.UserType)
                {
                    case "Y"://ADmin
                        query = "";
                        break;
                    case "T"://Type
                        string cityIDForLoc = emptype.Where(aa => aa.TypID == access.RoleDataValue).FirstOrDefault().TypID.ToString();
                        queryList.Add(" TypID =" + cityIDForLoc);
                        break;
                }
            }
            if (queryList.Count == 1)
            {
                query = query + queryList[0];
            }
            else if (queryList.Count > 1)
            {
                for (int i = 0; i < queryList.Count - 1; i++)
                {
                    query = query + queryList[i] + " or ";
                }
                query = query + queryList[queryList.Count - 1];
            }

            return query;
        }
        internal string QueryForEmployeeReports(ViewUserEmp LoggedInUser)
        {
            string query = " where ";
            string queryT = "";
            string queryL = "";
            string queryD = "";
            HRMEntities db = new HRMEntities();
            List<UserRoleData> uAcc = new List<UserRoleData>();
            List<UserRoleData> uAccD = new List<UserRoleData>();
            List<UserRoleData> uAccL = new List<UserRoleData>();
            List<UserRoleData> uAccT = new List<UserRoleData>();
            uAcc = db.UserRoleDatas.Where(aa => aa.RoleUserID == LoggedInUser.UserID).ToList();
            uAccD = uAcc.Where(aa => aa.UserRoleLegend == "D").ToList();
            uAccL = uAcc.Where(aa => aa.UserRoleLegend == "L").ToList();
            uAccT = uAcc.Where(aa => aa.UserRoleLegend == "T").ToList();
            // List<HR_Region> regions = db.HR_Region.ToList();
            List<HR_City> cities = db.HR_City.ToList();
            List<HR_Location> locs = db.HR_Location.ToList();
            List<string> queryListL = new List<string>();
            List<string> queryListD = new List<string>();
            List<string> queryListT = new List<string>();
            foreach (var access in uAccL)
            {
                switch (LoggedInUser.UserType)
                {
                    case "A"://Super ADmin
                        queryL = "";
                        break;
                    //case "R"://REgion
                    //    queryListL.Add(" RegionID =" + access.RoleDataValue.ToString());
                    //    break;
                    case "C"://City

                        queryListL.Add(" CityID =" + access.RoleDataValue.ToString());
                        break;
                    case "L"://Location
                        queryListL.Add(" LocationID =" + access.RoleDataValue.ToString());
                        break;
                }
            }
            foreach (var access in uAccD)
            {
                switch (LoggedInUser.UserType)
                {
                    case "G"://Super ADmin
                        query = "";
                        break;
                    //case "V"://Division
                    //    queryListD.Add(" DivID =" + access.RoleDataValue.ToString());
                    //    break;
                    case "D"://Dept

                        queryListD.Add(" DeptID =" + access.RoleDataValue.ToString());
                        break;
                    case "S"://Sec
                        queryListD.Add(" SectionID =" + access.RoleDataValue.ToString());
                        break;
                }
            }
            foreach (var access in uAccT)
            {
                switch (LoggedInUser.UserType)
                {
                    case "Y"://Super ADmin
                        queryT = "";
                        break;
                    case "T"://type
                        queryListT.Add(" EmpTypeID =" + access.RoleDataValue.ToString());
                        break;
                }
            }
            if (queryListT.Count == 1)
            {
                queryT = queryT + queryListT[0];
            }
            else if (queryListT.Count > 1)
            {
                for (int i = 0; i < queryListT.Count - 1; i++)
                {
                    queryT = queryT + queryListT[i] + " or ";
                }
                queryT = queryT + queryListT[queryListT.Count - 1];
            }

            //Dept
            if (queryListD.Count == 1)
            {
                queryD = queryD + queryListD[0];
            }
            else if (queryListD.Count > 1)
            {
                for (int i = 0; i < queryListD.Count - 1; i++)
                {
                    queryD = queryD + queryListD[i] + " or ";
                }
                queryD = queryD + queryListD[queryListD.Count - 1];
            }
            //Loc
            if (queryListL.Count == 1)
            {
                queryL = queryL + queryListL[0];
            }
            else if (queryListL.Count > 1)
            {
                for (int i = 0; i < queryListL.Count - 1; i++)
                {
                    queryL = queryL + queryListL[i] + " or ";
                }
                queryL = queryL + queryListL[queryListL.Count - 1];
            }
            List<string> list = new List<string>();

            if (queryL != "")
                list.Add(queryL);
            if (queryD != "")
                list.Add(queryD);
            if (queryT != "")
                list.Add(queryT);
            if (list.Count == 0)
                query = "";
            else if (list.Count == 1)
                query = " where " + list[0];
            else if (list.Count == 2)
                query = " where (" + list[0] + ") and (" + list[1] + " )";
            else if (list.Count == 3)
                query = " where (" + list[0] + ") and (" + list[1] + ") and (" + list[2] + " )";
            return query;
        }

        #endregion

        #region Queary for readers
        internal string QueryForReaderInFilters(ViewUserEmp LoggedInUser)
        {
            string query = " where ";
            HRMEntities db = new HRMEntities();
            List<UserRoleData> uAcc = new List<UserRoleData>();
            uAcc = db.UserRoleDatas.Where(aa => aa.RoleUserID == LoggedInUser.UserID && aa.UserRoleLegend == "M").ToList();
            //List<HR_Division> div = db.HR_Division.ToList();
            List<HR_Department> dept = db.HR_Department.ToList();
            List<HR_Section> sec = db.HR_Section.ToList();
            List<string> queryList = new List<string>();
            foreach (var access in uAcc)
            {
                switch (LoggedInUser.UserType)
                {
                    case "G"://Super ADmin
                        query = "";
                        break;
                    case "V"://divi
                        queryList.Add(" DivID =" + access.RoleDataValue.ToString());
                        break;
                    //case "D"://dept
                    //    string regionID = dept.Where(aa => aa.DeptID == access.RoleDataValue).FirstOrDefault().DivsionID.ToString();
                    //    queryList.Add(" DivID =" + regionID);
                    //    break;
                    //case "S"://Section
                    //    string regionIDForLoc = sec.Where(aa => aa.SecID == access.RoleDataValue).FirstOrDefault().HR_Department.DivsionID.ToString();
                    //    queryList.Add(" DivID =" + regionIDForLoc);
                    //    break;
                }
            }
            if (queryList.Count == 1)
            {
                query = query + queryList[0];
            }
            else if (queryList.Count > 1)
            {
                for (int i = 0; i < queryList.Count - 1; i++)
                {
                    query = query + queryList[i] + " or ";
                }
                query = query + queryList[queryList.Count - 1];
            }


            return query;
        }
        #endregion
        //internal string QueryForEmployeeReports(ViewUserEmp LoggedInUser)
        //{
        //    string query = " where ";
        //    string queryT = "";
        //    string queryL = "";
        //    string queryD = "";
        //    HRMEntities db = new HRMEntities();
        //    List<UserRoleData> uAcc = new List<UserRoleData>();
        //    List<UserRoleData> uAccD = new List<UserRoleData>();
        //    List<UserRoleData> uAccL = new List<UserRoleData>();
        //    List<UserRoleData> uAccT = new List<UserRoleData>();
        //    uAcc = db.UserRoleDatas.Where(aa => aa.RoleUserID == LoggedInUser.UserID).ToList();
        //    uAccD = uAcc.Where(aa => aa.UserRoleLegend == "D").ToList();
        //    uAccL = uAcc.Where(aa => aa.UserRoleLegend == "L").ToList();
        //    uAccT = uAcc.Where(aa => aa.UserRoleLegend == "T").ToList();
        //   // List<HR_Region> regions = db.HR_Region.ToList();
        //    List<HR_City> cities = db.HR_City.ToList();
        //    List<HR_Location> locs = db.HR_Location.ToList();
        //    List<string> queryListL = new List<string>();
        //    List<string> queryListD = new List<string>();
        //    List<string> queryListT = new List<string>();
        //    foreach (var access in uAccL)
        //    {
        //        switch (LoggedInUser.UserType)
        //        {
        //            case "A"://Super ADmin
        //                queryL = "";
        //                break;
        //            case "R"://REgion
        //                queryListL.Add(" RegionID =" + access.RoleDataValue.ToString());
        //                break;
        //            case "C"://City

        //                queryListL.Add(" CityID =" + access.RoleDataValue.ToString());
        //                break;
        //            case "L"://Location
        //                queryListL.Add(" LocationID =" + access.RoleDataValue.ToString());
        //                break;
        //        }
        //    }
        //    foreach (var access in uAccD)
        //    {
        //        switch (LoggedInUser.UserType)
        //        {
        //            case "G"://Super ADmin
        //                query = "";
        //                break;
        //            case "V"://Division
        //                queryListD.Add(" DivID =" + access.RoleDataValue.ToString());
        //                break;
        //            case "D"://Dept

        //                queryListD.Add(" DeptID =" + access.RoleDataValue.ToString());
        //                break;
        //            case "S"://Sec
        //                queryListD.Add(" SectionID =" + access.RoleDataValue.ToString());
        //                break;
        //        }
        //    }
        //    foreach (var access in uAccT)
        //    {
        //        switch (LoggedInUser.UserType)
        //        {
        //            case "Y"://Super ADmin
        //                queryT = "";
        //                break;
        //            case "T"://type
        //                queryListT.Add(" EmpTypeID =" + access.RoleDataValue.ToString());
        //                break;
        //        }
        //    }
        //    if (queryListT.Count == 1)
        //    {
        //        queryT = queryT + queryListT[0];
        //    }
        //    else if (queryListT.Count > 1)
        //    {
        //        for (int i = 0; i < queryListT.Count - 1; i++)
        //        {
        //            queryT = queryT + queryListT[i] + " or ";
        //        }
        //        queryT = queryT + queryListT[queryListT.Count - 1];
        //    }

        //    //Dept
        //    if (queryListD.Count == 1)
        //    {
        //        queryD = queryD + queryListD[0];
        //    }
        //    else if (queryListD.Count > 1)
        //    {
        //        for (int i = 0; i < queryListD.Count - 1; i++)
        //        {
        //            queryD = queryD + queryListD[i] + " or ";
        //        }
        //        queryD = queryD + queryListD[queryListD.Count - 1];
        //    }
        //    //Loc
        //    if (queryListL.Count == 1)
        //    {
        //        queryL = queryL + queryListL[0];
        //    }
        //    else if (queryListL.Count > 1)
        //    {
        //        for (int i = 0; i < queryListL.Count - 1; i++)
        //        {
        //            queryL = queryL + queryListL[i] + " or ";
        //        }
        //        queryL = queryL + queryListL[queryListL.Count - 1];
        //    }
        //    List<string> list = new List<string>();

        //    if (queryL != "")
        //        list.Add(queryL);
        //    if (queryD != "")
        //        list.Add(queryD);
        //    if (queryT != "")
        //        list.Add(queryT);
        //    if (list.Count == 0)
        //        query = "";
        //    else if (list.Count == 1)
        //        query = " where " + list[0];
        //    else if (list.Count == 2)
        //        query = " where (" + list[0] + ") and (" + list[1] + " )";
        //    else if (list.Count == 3)
        //        query = " where (" + list[0] + ") and (" + list[1] + ") and (" + list[2] + " )";
        //    return query;
        //}
        internal string QueryForMonthlyEditor(ViewUserEmp LoggedInUser)
        {
            string query = " and (";
            string queryT = "";
            string queryL = "";
            string queryD = "";
            HRMEntities db = new HRMEntities();
            List<UserRoleData> uAcc = new List<UserRoleData>();
            List<UserRoleData> uAccD = new List<UserRoleData>();
            List<UserRoleData> uAccL = new List<UserRoleData>();
            List<UserRoleData> uAccT = new List<UserRoleData>();
            uAcc = db.UserRoleDatas.Where(aa => aa.RoleUserID == LoggedInUser.UserID).ToList();
            uAccD = uAcc.Where(aa => aa.UserRoleLegend == "D").ToList();
            uAccL = uAcc.Where(aa => aa.UserRoleLegend == "L").ToList();
            uAccT = uAcc.Where(aa => aa.UserRoleLegend == "T").ToList();
            // List<HR_Region> regions = db.HR_Region.ToList();
            List<HR_City> cities = db.HR_City.ToList();
            List<HR_Location> locs = db.HR_Location.ToList();
            List<string> queryListL = new List<string>();
            List<string> queryListD = new List<string>();
            List<string> queryListT = new List<string>();
            foreach (var access in uAccL)
            {
                switch (LoggedInUser.UserType)
                {
                    case "A"://Super ADmin
                        queryL = "";
                        break;
                    //case "R"://REgion
                    //    queryListL.Add(" RegionID =" + access.RoleDataValue.ToString());
                    //    break;
                    case "C"://City

                        queryListL.Add(" CityID =" + access.RoleDataValue.ToString());
                        break;
                    case "L"://Location
                        queryListL.Add(" LocIDMonth =" + access.RoleDataValue.ToString());
                        break;
                }
            }
            foreach (var access in uAccD)
            {
                switch (LoggedInUser.UserType)
                {
                    case "G"://Super ADmin
                        query = "";
                        break;
                    //case "V"://Division
                    //    queryListD.Add(" DivID =" + access.RoleDataValue.ToString());
                    //    break;
                    case "D"://Dept

                        queryListD.Add(" DeptID =" + access.RoleDataValue.ToString());
                        break;
                    case "S"://Sec
                        queryListD.Add(" SectionID =" + access.RoleDataValue.ToString());
                        break;
                }
            }
            foreach (var access in uAccT)
            {
                switch (LoggedInUser.UserType)
                {
                    case "Y"://Super ADmin
                        queryT = "";
                        break;
                    case "T"://type
                        queryListT.Add(" EmpTypeID =" + access.RoleDataValue.ToString());
                        break;
                }
            }
            if (queryListT.Count == 1)
            {
                queryT = queryT + queryListT[0];
            }
            else if (queryListT.Count > 1)
            {
                for (int i = 0; i < queryListT.Count - 1; i++)
                {
                    queryT = queryT + queryListT[i] + " or ";
                }
                queryT = queryT + queryListT[queryListT.Count - 1];
            }

            //Dept
            if (queryListD.Count == 1)
            {
                queryD = queryD + queryListD[0];
            }
            else if (queryListD.Count > 1)
            {
                for (int i = 0; i < queryListD.Count - 1; i++)
                {
                    queryD = queryD + queryListD[i] + " or ";
                }
                queryD = queryD + queryListD[queryListD.Count - 1];
            }
            //Loc
            if (queryListL.Count == 1)
            {
                queryL = queryL + queryListL[0];
            }
            else if (queryListL.Count > 1)
            {
                for (int i = 0; i < queryListL.Count - 1; i++)
                {
                    queryL = queryL + queryListL[i] + " or ";
                }
                queryL = queryL + queryListL[queryListL.Count - 1];
            }
            List<string> list = new List<string>();

            if (queryL != "")
                list.Add(queryL);
            if (queryD != "")
                list.Add(queryD);
            if (queryT != "")
                list.Add(queryT);
            if (list.Count == 0)
                query = "";
            else if (list.Count == 1)
                query = " and ( " + list[0] + ")";
            else if (list.Count == 2)
                query = " and (" + list[0] + ") and (" + list[1] + " )";
            else if (list.Count == 3)
                query = " and (" + list[0] + ") and (" + list[1] + ") and (" + list[2] + " )";
            return query;
        }
        internal string QueryForPREditor(ViewUserEmp LoggedInUser)
        {
            string query = " where ";
            string queryT = "";
            string queryL = "";
            string queryD = "";
            HRMEntities db = new HRMEntities();
            List<UserRoleData> uAcc = new List<UserRoleData>();
            List<UserRoleData> uAccD = new List<UserRoleData>();
            List<UserRoleData> uAccL = new List<UserRoleData>();
            List<UserRoleData> uAccT = new List<UserRoleData>();
            uAcc = db.UserRoleDatas.Where(aa => aa.RoleUserID == LoggedInUser.UserID).ToList();
            uAccD = uAcc.Where(aa => aa.UserRoleLegend == "D").ToList();
            uAccL = uAcc.Where(aa => aa.UserRoleLegend == "L").ToList();
            uAccT = uAcc.Where(aa => aa.UserRoleLegend == "T").ToList();
            // List<HR_Region> regions = db.HR_Region.ToList();
            List<HR_City> cities = db.HR_City.ToList();
            List<HR_Location> locs = db.HR_Location.ToList();
            List<string> queryListL = new List<string>();
            List<string> queryListD = new List<string>();
            List<string> queryListT = new List<string>();
            foreach (var access in uAccL)
            {
                switch (LoggedInUser.UserType)
                {
                    case "A"://Super ADmin
                        queryL = "";
                        break;
                    //case "R"://REgion
                    //    queryListL.Add(" RegionID =" + access.RoleDataValue.ToString());
                    //    break;
                    case "C"://City

                        queryListL.Add(" CityID =" + access.RoleDataValue.ToString());
                        break;
                    case "L"://Location
                        queryListL.Add(" PRLocID =" + access.RoleDataValue.ToString());
                        break;
                }
            }
            foreach (var access in uAccD)
            {
                switch (LoggedInUser.UserType)
                {
                    case "G"://Super ADmin
                        query = "";
                        break;
                    //case "V"://Division
                    //    queryListD.Add(" DivID =" + access.RoleDataValue.ToString());
                    //    break;
                    case "D"://Dept

                        queryListD.Add(" DeptID =" + access.RoleDataValue.ToString());
                        break;
                    case "S"://Sec
                        queryListD.Add(" SectionID =" + access.RoleDataValue.ToString());
                        break;
                }
            }
            foreach (var access in uAccT)
            {
                switch (LoggedInUser.UserType)
                {
                    case "Y"://Super ADmin
                        queryT = "";
                        break;
                    case "T"://type
                        queryListT.Add(" EmpTypeID =" + access.RoleDataValue.ToString());
                        break;
                }
            }
            if (queryListT.Count == 1)
            {
                queryT = queryT + queryListT[0];
            }
            else if (queryListT.Count > 1)
            {
                for (int i = 0; i < queryListT.Count - 1; i++)
                {
                    queryT = queryT + queryListT[i] + " or ";
                }
                queryT = queryT + queryListT[queryListT.Count - 1];
            }

            //Dept
            if (queryListD.Count == 1)
            {
                queryD = queryD + queryListD[0];
            }
            else if (queryListD.Count > 1)
            {
                for (int i = 0; i < queryListD.Count - 1; i++)
                {
                    queryD = queryD + queryListD[i] + " or ";
                }
                queryD = queryD + queryListD[queryListD.Count - 1];
            }
            //Loc
            if (queryListL.Count == 1)
            {
                queryL = queryL + queryListL[0];
            }
            else if (queryListL.Count > 1)
            {
                for (int i = 0; i < queryListL.Count - 1; i++)
                {
                    queryL = queryL + queryListL[i] + " or ";
                }
                queryL = queryL + queryListL[queryListL.Count - 1];
            }
            List<string> list = new List<string>();

            if (queryL != "")
                list.Add(queryL);
            if (queryD != "")
                list.Add(queryD);
            if (queryT != "")
                list.Add(queryT);
            if (list.Count == 0)
                query = "";
            else if (list.Count == 1)
                query = " where " + list[0];
            else if (list.Count == 2)
                query = " where (" + list[0] + ") and (" + list[1] + " )";
            else if (list.Count == 3)
                query = " where (" + list[0] + ") and (" + list[1] + ") and (" + list[2] + " )";
            return query;
        }
        public string MakeCustomizeQueryForEmpView(User _user)
        {
            string RoleQuery = "";
            string CatQuery = "";
            HRMEntities db = new HRMEntities();
            List<string> UserRoleString = new List<string>();
            //List<string> CategoryUser = new List<string>();
            //CategoryUser.Add(" where (CatID=1 ");
            //if (_user.ViewContractual == true)
            //{
            //    CategoryUser.Add(" CatID = 4 ");
            //}
            //if (_user.ViewPermanentMgm == true)
            //{
            //    CategoryUser.Add(" CatID = 2  ");
            //}
            //if (_user.ViewPermanentStaff == true)
            //{
            //    CategoryUser.Add(" CatID = 3  ");
            //}
            //userRoleDataD = db.UserRoleDatas.Where(aa => aa.RoleUserID == _user.UserID && aa.UserRoleLegend=="D").ToList();
            //userRoleDataL = db.UserRoleDatas.Where(aa => aa.RoleUserID == _user.UserID && aa.UserRoleLegend == "L").ToList();
            //switch (_user.UserRoleD)
            //{
            //    case "G"://Admin

            //        break;
            //    case "D"://Department
            //        foreach (var urd in userRoleDataD)
            //        {
            //            UserRoleString.Add(" DeptID = " + urd.RoleDataValue + " ");
            //        }
            //        break;
            //    case "S"://Section
            //        foreach (var urd in userRoleDataD)
            //        {
            //            UserRoleString.Add(" SecID = " + urd.RoleDataValue + " ");
            //        }
            //        break;
            //    case "V"://Division
            //        foreach (var urd in userRoleDataD)
            //        {
            //            UserRoleString.Add(" DivID = " + urd.RoleDataValue + " ");
            //        }
            //        break;
            //}
            //switch (_user.UserRoleL)
            //{
            //    case "A"://Admin

            //        break;
            //    case "C"://City
            //        foreach (var urd in userRoleDataL)
            //        {
            //            UserRoleString.Add(" CityID = " + urd.RoleDataValue + " ");
            //        }
            //        break;
            //    case "L"://Location
            //        foreach (var urd in userRoleDataL)
            //        {
            //            UserRoleString.Add(" LocID = " + urd.RoleDataValue + " ");
            //        }
            //        break;
            //    case "R"://Region
            //        foreach (var urd in userRoleDataL)
            //        {
            //            UserRoleString.Add(" RegionID = " + urd.RoleDataValue + " ");
            //        }
            //        break;
            //}
            if (UserRoleString.Count == 1)
            {
                RoleQuery = " and (" + RoleQuery + UserRoleString[0] + " ) ";
            }
            else if (UserRoleString.Count > 1)
            {
                RoleQuery = RoleQuery + " and ( ";
                for (int i = 0; i < UserRoleString.Count - 1; i++)
                {
                    RoleQuery = RoleQuery + UserRoleString[i] + " or ";
                }
                RoleQuery = RoleQuery + UserRoleString[UserRoleString.Count - 1] + " ) ";
            }
            //if (CategoryUser.Count == 1)
            //    CatQuery = CatQuery + CategoryUser[0] + " ) ";
            //else if (CategoryUser.Count > 1)
            //{
            //    for (int i = 0; i < CategoryUser.Count - 1; i++)
            //    {
            //        CatQuery = CatQuery + CategoryUser[i] + " or ";
            //    }
            //    CatQuery = CatQuery + CategoryUser[CategoryUser.Count - 1] + " ) ";
            //}

            return CatQuery + RoleQuery;
        }
        //internal string MakeLocQueryWhereClause(List<WMSLibrary.FiltersAttributes> list)
        //{
        //    if (list.Count > 0)
        //    {
        //        string query = "where (";
        //        List<string> qList = new List<string>();
        //        foreach (var item in list)
        //        {
        //            qList.Add("PRLocID = " + item.ID.ToString());
        //        }
        //        if (qList.Count == 1)
        //        {
        //            query = query + qList[0];
        //        }
        //        else if (qList.Count > 1)
        //        {
        //            for (int i = 0; i < qList.Count - 1; i++)
        //            {
        //                query = query + qList[i] + " or ";
        //            }
        //            query = query + qList[qList.Count - 1];
        //        }
        //        return query + ")";
        //    }
        //    else
        //        return "Where";
        //}
        //internal string MakeLocQueryWhereClause(List<WMSLibrary.FiltersAttributes> list)
        //{
        //    if (list.Count > 0)
        //    {
        //        string query = "where PRLocID in (";
        //        foreach (var item in list)
        //        {
        //            query = query + item.ID.ToString() + ",";
        //        }
        //        query = query.Remove(query.Length - 1);
        //        return query + ")";
        //    }
        //    else
        //        return "Where";
        //}   
    }
}