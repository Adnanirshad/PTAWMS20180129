using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HRM_IKAN.Authentication
{
    public class CustomControllerAttributes : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            bool HavePermission = false;
            try
            {
                HttpSessionStateBase session = filterContext.HttpContext.Session;
                filterContext.HttpContext.Session["SelectedMenu"] = controllerName;
                ViewUserEmp loggedUser = session["LoggedUser"] as ViewUserEmp;
                switch (controllerName)
                {
                    case "OTHR":
                        if (loggedUser!= null)
                            HavePermission = true;
                        break;
                    case "OTApproved":
                        if (loggedUser != null)
                            HavePermission = true;
                        break;
                    case "OTApprover":
                        if (loggedUser.UserType == "P" || loggedUser.UserType=="H")
                            HavePermission = true;
                        break;
                    case "OTRecommend":
                        if (loggedUser.UserType == "R")
                            HavePermission = true;
                        break;
                    case "OTSupervisor":
                        if (loggedUser.UserType == "N" || loggedUser.UserType == "E")
                            HavePermission = true;
                        break;


                    case "JobCard":
                        if (loggedUser.MAttJobCard==true)
                            HavePermission = true;
                        break;
                    case "User":
                        if (loggedUser.MUser==true)
                            HavePermission = true;
                        break;
                    case "UserTypes":
                        if (loggedUser.MUser == true)
                            HavePermission = true;
                        break;  
                    case "Category":
                        if (loggedUser.MHRCompHierarchy == true)
                            HavePermission = true;
                        break;
                    case "City":
                        if (loggedUser.MHRCompHierarchy == true)
                            HavePermission = true;
                        break;
                    case "Department":
                        if (loggedUser.MHRCompHierarchy == true)
                            HavePermission = true;
                        break;
                    case "Designation":
                        if (loggedUser.MHRCompHierarchy == true)
                            HavePermission = true;
                        break;
                    case "Division":
                        if (loggedUser.MHRCompHierarchy == true)
                            HavePermission = true;
                        break;
                    case "Employee":
                        if (loggedUser.MHREmployee == true)
                            HavePermission = true;
                        break;
                    case "EmpType":
                        if (loggedUser.MHRCompHierarchy == true)
                            HavePermission = true;
                        break;
                    case "Grade":
                        if (loggedUser.MHRCompHierarchy == true)
                            HavePermission = true;
                        break;
                    case "Group":
                        if (loggedUser.MHRCompHierarchy == true)
                            HavePermission = true;
                        break;
                    case "JobTitle":
                        if (loggedUser.MHRCompHierarchy == true)
                            HavePermission = true;
                        break;
                    case "Location":
                        if (loggedUser.MHRCompHierarchy == true)
                            HavePermission = true;
                        break;
                    case "Region":
                        if (loggedUser.MHRCompHierarchy == true)
                            HavePermission = true;
                        break;
                    case "Section":
                        if (loggedUser.MHRCompHierarchy == true)
                            HavePermission = true;
                        break;
                    case "AttManual":
                        if (loggedUser.MAttEditAttendance == true)
                            HavePermission = true;
                        break;
                    case "DownloadTime":
                        if (loggedUser.MAttDownloadTime == true)
                            HavePermission = true;
                        break;
                    case "OTPolicy":
                        if (loggedUser.MAttOTPolicy == true)
                            HavePermission = true;
                        break;
                    case "PRPeriod":
                        if (loggedUser.MAttOTPolicy == true)
                            HavePermission = true;
                        break;
                    case "OTDiv":
                        if (loggedUser.OTBudget == true)
                            HavePermission = true;
                        break;
                    case "OTCredit":
                        if (loggedUser.OTBudgetCreditDebit == true)
                            HavePermission = true;
                        break;
                    case "OTDebit":
                        if (loggedUser.OTBudgetCreditDebit == true)
                            HavePermission = true;
                        break;
                    case "PendingJobCards":
                        if (loggedUser.MAttJobCard == true)
                            HavePermission = true;
                        break;
                    case "EmployeeJobCard":
                        //if (loggedUser.MAttJobCard == true)
                            HavePermission = true;
                        break;
                    case "ProcessRequest":
                        if (loggedUser.MAttProcess == true)
                            HavePermission = true;
                        break;
                    case "Reader":
                        if (loggedUser.MAttDevice == true)
                            HavePermission = true;
                        break;
                    case "Shift":
                        if (loggedUser.MAttShift == true)
                            HavePermission = true;
                        break;
                    case "Holiday":
                        if (loggedUser.MAttHoliday== true)
                            HavePermission = true;
                        break;
                    case "ScheduleVisitor":
                        if (loggedUser.VisitorEntry == true || loggedUser.VisitorSupervisor==true)
                            HavePermission = true;
                        break;
                    case "Settings":
                        if (loggedUser.UserType == "A" || loggedUser.UserType == "E" || loggedUser.UserType == "H")
                            HavePermission = true;
                        break;
                    case "Profile":
                        if (loggedUser != null)
                            HavePermission = true;
                        break;
                    case "HR":
                        if (loggedUser != null)
                            HavePermission = true;
                        break;
                }
                if (HavePermission == false)
                {
                    //filterContext.Result = new HttpUnauthorizedResult();
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "VDSContainer", area = "" }));
                    filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                }
            }
            catch (Exception ex)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "VDSContainer", area = "" }));
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            }

        }

        //private bool CheckRosterPermision(User _User)
        //{
        //    try
        //    {
        //        if (_User.MRoster == true)
        //            return true;
        //        else
        //            return false;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
    }
}