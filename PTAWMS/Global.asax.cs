using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PTAWMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

        }
        protected void Session_Start(object sender, EventArgs e)
        {
            // Initialize Session["FiltersModel"] -- Move to First Page
            Session["FiltersModel"] = new WMSLibrary.FiltersModel();
            Session["LoginCount"] = null;
            Session["PRID"] = null;
            Session["DateStart"] = null;
            Session["DateEnd"] = null;
            LoadSessionValues();
            //LoadSession();
        }
        private void LoadSession()
        {
            using (HRMEntities dc = new HRMEntities())
            {
                var v = dc.Users.Where(aa => aa.UserName == "admin").FirstOrDefault();
                if (v != null)
                {
                        Session["LogedUserID"] = v.UserID.ToString();
                        Session["LogedUserFullname"] = v.UserName;
                        Session["LoggedUser"] = v;
                }
            }
        }

        private void LoadSessionValues()
        {
            Session["ReportSession"] = new List<string>();
            List<string> list = Session["ReportSession"] as List<string>;
            list.Add(DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd"));
            list.Add(DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd"));
            list.Add("00:01");
            list.Add("23:59");
            list.Add("EmpView");
            Session["ReportSession"] = list;
            using (HRMEntities dc = new HRMEntities())
            {
                DateTime dt = DateTime.Today;
                int PRID = 0;
                DateTime stDate = new DateTime(dt.Year, dt.Month, 1);
                ////if (dc.PR_PayrollPeriod.Where(aa => aa.PStartDate == stDate).Count()>0)
                ////{
                //    if (dt.Day > 27)
                //    {
                //        PRID = dc.PR_PayrollPeriod.Where(aa => aa.PStartDate == stDate).First().PID;
                //    }
                //    else
                //    {
                //        DateTime stDatee = new DateTime(dt.Year, dt.Month - 1, 1);
                //        PRID = dc.PR_PayrollPeriod.Where(aa => aa.PStartDate == stDatee).First().PID;
                //    } 
                ////}
                Session["PRID"] = PRID;
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Session["FiltersModel"] = null;
            Session["LogedUserID"] = null;
            Session["LoggedUser"] = null;
            Session["PRID"] = null;
        }
        protected void Session_End()
        {
            Session["FiltersModel"] = null;
            Session["LogedUserID"] = null;
            Session["LoggedUser"] = null;
            Session["PRID"] = null;
        }

        protected void Application_Error()
        {
            //HttpContext httpContext = HttpContext.Current;
            //httpContext.Response.Redirect("~/");
        }
    }
}
