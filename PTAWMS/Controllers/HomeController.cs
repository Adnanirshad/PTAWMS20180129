using PTAWMS.Areas.Attendance.Controllers;
using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PTAWMS.Areas.Attendance.BusinessLogic;
using PTAWMS.Helper;
using System.Text;
using PTAWMS.App_Start;

namespace PTAWMS.Controllers
{
    public class HomeController : Controller
    {
        #region -- Login & Logout--
        public ActionResult Index()
        {
            try
            {
                if (Session["LoggedUserID"] == null || Session["LoggedUserID"] == "")
                {
                    ViewBag.Message = "";
                    return View();
                }
                else
                {

                    return RedirectToAction("VDSContainer");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        HRMEntities db = new HRMEntities();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User u)
        {
            try
            {
                    if (ModelState.IsValid) // this is check validity
                    {
                        using (HRMEntities dc = new HRMEntities())
                        {
                            var v = dc.ViewUserEmps.Where(a => a.UserName.ToUpper() == u.UserName.ToUpper() && a.Password.ToUpper() == u.Password.ToUpper() && a.Status == true).FirstOrDefault();
                            if (v != null)
                            {
                                DateTime CDate = DateTime.Today;
                                DateTime DDate = DateTime.Today.AddDays(-7);
                                v = SetFocalUserRights(v);
                                Session["LoggedUserID"] = v.UserID.ToString();
                                Session["LoggedUser"] = v;
                                Session["SelDate"] = CDate.ToString("yyyy-MM-dd");
                                Session["DateStart"] = DDate.ToString("yyyy-MM-dd");
                                Session["DateEnd"] = CDate.ToString("yyyy-MM-dd");

                                int PRID = 0;
                                PRID = db.PR_PayrollPeriod.OrderByDescending(aa => aa.PStartDate).First().PID;
                                Session["PRID"] = PRID;
                                if (db.HR_SMTP.Count() > 0)
                                {
                                    EmailManager.SMTPServer = db.HR_SMTP.First().SMTPServer;
                                    EmailManager.SMTPServerPort = Convert.ToInt32(db.HR_SMTP.First().SMTPPort);
                                    EmailManager.SMTPUserName = db.HR_SMTP.First().UserName;
                                    EmailManager.SMTPUserPassword = db.HR_SMTP.First().UserPassword;
                                }
                                if (db.HR_Employee.Where(aa => aa.ReportingToID == v.EmpID).Count() == 0 && v.UserType!="A"&& v.UserType!="H" && v.UserType!="E")
                                {
                                    Session["IsSupervisor"] = "0";
                                    Session["UserRole"] = "Single";
                                    return RedirectToAction("VDSContainer");
                                }
                                else
                                {
                                    if (v.UserType == "A" || v.UserType == "A" || v.UserType == "E")
                                        Session["UserRole"] = "Admin";
                                    else
                                    {
                                        List<EmpView> emps = new List<EmpView>();
                                        emps = OTHelperManager.GetEmployees(db.EmpViews.Where(aa => aa.Status == "Active").ToList(), v);
                                        if (emps.Select(aa => aa.DepartmentID).Distinct().Count() > 1)
                                        {
                                            Session["UserRole"] = "MultiDept";
                                        }
                                        else if (emps.Select(aa => aa.SectionID).Distinct().Count() > 1)
                                        {
                                            Session["UserRole"] = "MultiSec";
                                        }
                                        else
                                        {
                                            Session["UserRole"] = "SingleSec";
                                        }
                                    }
                                    Session["IsSupervisor"] = "1";
                                    if (v.UserType == "N")
                                    {
                                        int PayrollPeriodID = OTHelperManager.GetPayrollPeriodID(db.PR_PayrollPeriod.ToList(), DateTime.Today);
                                        PR_PayrollPeriod prp = OTHelperManager.GetPayrollPeriod(db.PR_PayrollPeriod.ToList(), (int)PayrollPeriodID);
                                        ViewBag.NoOfRequest = db.ViewDailyOTEntries.Where(aa => aa.OTDate >= prp.PStartDate && aa.OTDate <= prp.PEndDate && aa.StatusID == "P" && aa.SupervisorID == v.EmployeeID).Count();    
                                    }
                                    return RedirectToAction("VDSContainer");
                                }
                                //return RedirectToAction("AfterLogin");
                            }
                            else
                            {
                                ModelState.AddModelError("UserName", "User name or Passward is incorrect");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("UserName", "There is an error in connection, Please contact with the administrator");
                }
                return View("Index",u);
        }

        private ViewUserEmp SetFocalUserRights(ViewUserEmp v)
        {
            // Check 
            List<FocalUser> focalUsers = db.FocalUsers.Where(aa => aa.FocalUID == v.UserID && (aa.EndDate == null || aa.EndDate >= DateTime.Today)).ToList();
            if (focalUsers.Count() > 0)
            {
                int userId = (int)focalUsers.First().UserID;
                ViewUserEmp user = new ViewUserEmp();
                user = db.ViewUserEmps.First(aa => aa.UserID == userId);
                v.UserType = user.UserType;
                v.CanEdit = user.CanEdit;
                v.CanDelete = user.CanDelete;
                v.CanView = user.CanView;
                v.CanAdd = user.CanAdd;
                v.MAttProcess = user.MAttProcess;
                v.MOption = user.MOption;
                v.MAttDevice = user.MAttDevice;
                v.MAttDeviceUtility = user.MAttDeviceUtility;
                v.MAttEditAttendance = user.MAttEditAttendance;
                v.MAttJobCard = user.MAttJobCard;
                v.MAttShift = user.MAttShift;
                v.MAttPolicy = user.MAttPolicy;
                v.MAttDownloadTime = user.MAttDownloadTime;
                v.MAttHoliday = user.MAttHoliday;
                v.MAttOTPolicy = user.MAttOTPolicy;
                v.MAttOTCreate = user.MAttOTCreate;
                v.MAttOTEdit = user.MAttOTEdit;
                v.MAttLeaves = user.MAttLeaves;
                v.MAttRoster = user.MAttRoster;
                v.MHRCompHierarchy = user.MHRCompHierarchy;
                v.MUser = user.MUser;
                v.MGrade = user.MGrade;
                v.MHREmpA = user.MHREmpA;
                v.MHREmpE = user.MHREmpE;
                v.MHREmpV = user.MHREmpV;
                v.MHREmpD = user.MHREmpD;
                v.MHREmployee = user.MHREmployee;
                v.MHREmpPersonal = user.MHREmpPersonal;
                v.MHREmpJob = user.MHREmpJob;
                v.MHREmpAtt = user.MHREmpAtt;
                v.HRModule = user.HRModule;
                v.HREmpType = user.HREmpType;
                v.HRLocation = user.HRLocation;
                v.HRDeptartment = user.HRDeptartment;
                v.HRDesignation = user.HRDesignation;
                v.HRSection = user.HRSection;
                v.AttendanceModule = user.AttendanceModule;
                v.VisitorApplication = user.VisitorApplication;
                v.VisitorEntry = user.VisitorEntry;
                v.VisitorSupervisor = user.VisitorSupervisor;
                v.OTBudget = user.OTBudget;
                v.OTBudgetCreditDebit = user.OTBudgetCreditDebit;
                v.UI_Theme = user.UI_Theme;
                v.HRReport = user.HRReport;
                v.HRRequest = user.HRRequest;
            }
            return v;
        }


        public ActionResult AfterLogin()
        {
            try
            {
                if (Session["LoggedUserID"] != null)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View("Index");
            }
        }

        public ActionResult LogOut()
        {
            //Session["LoggedUserID"] = null;
            //Session["Date"] = null;
            //Session["IsSupervisor"] = null;
            //Session["LoggedUser"] = null;
            //Session["PRID"] = null;
            Session.Clear();
            Session.Abandon();
            

            return RedirectToAction("Index");
        }
        #endregion

        #region -- Dashboard Container --
        public ActionResult VDSContainer()
        {
            ViewUserEmp LoggedInUser =  Session["LoggedUser"] as ViewUserEmp;
            List<PR_PayrollPeriod> periods = db.PR_PayrollPeriod.ToList();
            DashboardValue ds = new DashboardValue();
            List<string> messages = new List<string>();
            List<string> links = new List<string>();
            Session["SelectedMenu"] = "Home";
            if (LoggedInUser.UserType == "E")
                ds.VisitorPending = db.VMS_SVisitor.Where(aa => aa.Status == "Pending").Count();
            if (LoggedInUser.UserType == "A" || LoggedInUser.UserType == "H")
            {
                ds.JobCardPending = db.Att_JobCardApp.Where(aa => aa.StatusID == "Pending").Count();
                ds.OvertimePending = db.Att_OTDailyEntry.Where(aa => (aa.StatusID == "P" || aa.StatusID == "F")).Select(aa => aa.EmpID).Distinct().Count();            
            }
            else
            {
                foreach (var item in db.Att_OTDailyEntry.Where(aa => (aa.StatusID == "P" && aa.SupervisorID == LoggedInUser.UserID) || (aa.StatusID == "F" && aa.ForwardToID == LoggedInUser.UserID)).Select(aa => aa.PeriodID).Distinct().ToList())
                {
                    PR_PayrollPeriod prp = periods.First(aa => aa.PID == item);
                    messages.Add("You have Pending Overtime Requests for Month of " + prp.PName);
                    links.Add("<a href='~/'></a>");
                }
                ds.JobCardPending = db.Att_JobCardApp.Where(aa => aa.SupervisorID==LoggedInUser.UserID && aa.StatusID == "Pending").Count();
                ds.OvertimePending = db.Att_OTDailyEntry.Where(aa => (aa.StatusID == "P" && aa.SupervisorID == LoggedInUser.UserID) || (aa.StatusID == "F" && aa.ForwardToID == LoggedInUser.UserID)).Count();
            }
            ds.DateStart = Session["DateStart"].ToString();
            ds.DateEnd= Session["DateEnd"].ToString();
            ds.Messages = messages;
            ds.Links = links;
            return View(ds);
        }


        #endregion
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult SaveDateInSession(string dateS, string dateE)
        {
            Session["DateStart"] = dateS;
            Session["DateEnd"] = dateE;
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        // Get User Image
        public ActionResult RetrieveUserImage(int id)
        {
            try
            {
                byte[] cover = GetImageFromDataBase(id);
                if (cover != null && cover.Count() > 0)
                {
                    return File(cover, "image/jpg");
                }
                else
                {
                    return File("~/images/defaultimage.png", "image/png");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public byte[] GetImageFromDataBase(int Id)
        {
            try
            {
                try
                {
                    if (db.HR_EmpImage.Where(aa => aa.EmpID == Id).Count() > 0)
                    {
                        var q = from temp in db.HR_EmpImage where temp.EmpID == Id select temp.EmpPic;
                        byte[] cover = q.First();
                        return cover;
                    }
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult RegisterUser()
        {
            ViewBag.Employees = new SelectList(db.EmpViews, "EmployeeID", "FullName");
            RegisterNewUser rg = new RegisterNewUser();
            return View(rg);
        }
        [HttpGet]
        public ActionResult ForGotPassword( string email)
        {
            string msg = "";
            if (email != null)
            {
                msg = "Your UserName and Password is sent to you via Email";
            }
            return Json(msg, JsonRequestBehavior.AllowGet); 
        }    
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult LoadEmployeeProfile(int? id)
        {

            return RedirectToAction("EmpProfileIndex","Employee", new {area="HumanResource", id = id });
        }

        private List<EmpView> GetEmployees(List<EmpView> emps, ViewUserEmp LoggedInUser)
        {
            List<EmpView> nEmps = emps.Where(aa => aa.ReportingToID == LoggedInUser.EmpID).ToList();
            // Add LoggedInUser Employee object in list
            if (nEmps.Where(aa => aa.EmployeeID == LoggedInUser.EmpID).Count() == 0)
                nEmps.AddRange(emps.Where(aa => aa.EmployeeID == LoggedInUser.EmpID).ToList());
            List<EmpView> rEmps = GetReportingToEmps(emps, nEmps);
            if (rEmps.Count > 0)
            {
                while (true)
                {
                    rEmps = GetReportingToEmps(emps, rEmps).ToList();
                    nEmps.AddRange(rEmps);
                    if (rEmps.Count == 0)
                        return nEmps;
                    if (nEmps.Count() >= emps.Count())
                        return nEmps;
                }
            }
            else
                return nEmps;
        }
        private List<EmpView> GetReportingToEmps(List<EmpView> emps, List<EmpView> checkemps)
        {
            List<EmpView> rEmps = new List<EmpView>();
            foreach (var emp in checkemps)
            {
                rEmps.AddRange(emps.Where(aa => aa.ReportingToID == emp.EmployeeID).ToList());

            }
            return rEmps;

        }


        public ActionResult GetQualificationNotification(int ID)
        {

            try
            {
                List<HR_Emp_Notification> notifications = new List<HR_Emp_Notification>();
                int typeID = Convert.ToInt16(Utilities.NotificationType.Qualification);
                User model = db.Users.FirstOrDefault(x => x.EmpID == ID);
                StringBuilder list = new StringBuilder();
                int total = notifications.Count;
                list.Append("<ul class='dropdown-menu dropdown-navbar navbar-pink'>");
                notifications = new List<HR_Emp_Notification>();
                typeID = Convert.ToInt16(Utilities.NotificationType.Pre_Job_History);
                if (model.EmpID > 0)
                {
                    if (model.HRRequest == true)
                        notifications = db.HR_Emp_Notification.Where(x => x.RoleType == "HRModule" && x.Deleted != true && x.UserSpecific == false && x.NotificationTypeID == typeID).ToList();
                    else
                        notifications = db.HR_Emp_Notification.Where(x => x.EmployeeID == ID && x.Deleted != true && x.UserSpecific == true && x.NotificationTypeID == typeID).ToList();
                }
                total = notifications.Count;

                if (total > 0)
                {
                    list.Append("<li><a href='" + notifications[total - 1].NotificationURL +
                        "'><div class='clearfix'><span class='pull-left'><i class='btn btn-xs no-hover btn-pink fa fa-book'></i> Pre-Job History</span><span class='pull-right badge badge-info'>");
                    list.Append(total.ToString());
                    list.Append("</span></span></div></a></li>");
                }

                notifications = new List<HR_Emp_Notification>();
                typeID = Convert.ToInt16(Utilities.NotificationType.Appreciation);
                if (model.EmpID > 0)
                {
                    if (model.HRRequest == true)
                        notifications = db.HR_Emp_Notification.Where(x => x.RoleType == "HRModule" && x.Deleted != true && x.UserSpecific == false && x.NotificationTypeID == typeID).ToList();
                    else
                        notifications = db.HR_Emp_Notification.Where(x => x.EmployeeID == ID && x.Deleted != true && x.UserSpecific == true && x.NotificationTypeID == typeID).ToList();
                }
                total = notifications.Count;

                if (total > 0)
                {
                    list.Append("<li><a href='" + notifications[total - 1].NotificationURL +
                        "'><div class='clearfix'><span class='pull-left'><i class='btn btn-xs btn-primary fa fa-user'></i> " + notifications[total - 1].Notification + "</span><span class='pull-right badge badge-info'>");
                    list.Append(total.ToString());
                    list.Append("</span></span></div></a></li>");
                }

                notifications = new List<HR_Emp_Notification>();
                typeID = Convert.ToInt16(Utilities.NotificationType.Warning);
                if (model.EmpID > 0)
                {
                    if (model.HRRequest == true)
                        notifications = db.HR_Emp_Notification.Where(x => x.RoleType == "HRModule" && x.Deleted != true && x.UserSpecific == false && x.NotificationTypeID == typeID).ToList();
                    else
                        notifications = db.HR_Emp_Notification.Where(x => x.EmployeeID == ID && x.Deleted != true && x.UserSpecific == true && x.NotificationTypeID == typeID).ToList();
                }
                total = notifications.Count;

                if (total > 0)
                {
                    list.Append("<li><a href='" + notifications[total - 1].NotificationURL +
                        "'><div class='clearfix'><span class='pull-left'><i class='btn btn-xs no-hover btn-pink fa fa-bell'></i> " + notifications[total - 1].Notification + "</span><span class='pull-right badge badge-info'>");
                    list.Append(total.ToString());
                    list.Append("</span></span></div></a></li>");
                }
                notifications = new List<HR_Emp_Notification>();
                typeID = Convert.ToInt16(Utilities.NotificationType.Training);
                if (model.EmpID > 0)
                {
                    if (model.HRRequest == true)
                        notifications = db.HR_Emp_Notification.Where(x => x.RoleType == "HRModule" && x.Deleted != true && x.UserSpecific == false && x.NotificationTypeID == typeID).ToList();
                    else
                        notifications = db.HR_Emp_Notification.Where(x => x.EmployeeID == ID && x.Deleted != true && x.UserSpecific == true && x.NotificationTypeID == typeID).ToList();
                }
                total = notifications.Count;

                if (total > 0)
                {
                    list.Append("<li><a href='" + notifications[total - 1].NotificationURL +
                        "'><div class='clearfix'><span class='pull-left'><i class='btn btn-xs no-hover btn-success fa fa-certificate'></i> " + notifications[total - 1].Notification + "</span><span class='pull-right badge badge-info'>");
                    list.Append(total.ToString());
                    list.Append("</span></span></div></a></li>");
                }

                notifications = new List<HR_Emp_Notification>();
                typeID = Convert.ToInt16(Utilities.NotificationType.Job_Card);
                notifications = db.HR_Emp_Notification.Where(x => x.EmployeeID == ID && x.Deleted != true && x.UserSpecific == true && x.NotificationTypeID == typeID).ToList();
                total = notifications.Count;

                if (total > 0)
                {
                    list.Append("<li><a href='" + notifications[total - 1].NotificationURL +
                         "'><div class='clearfix'><span class='pull-left'><i class='btn btn-xs no-hover btn-blue fa fa-bell'></i> " + notifications[total - 1].Notification + "</span><span class='pull-right badge badge-info'>");
                    list.Append(total.ToString());
                    list.Append("</span></span></div></a></li>");
                }
                notifications = new List<HR_Emp_Notification>();
                typeID = Convert.ToInt16(Utilities.NotificationType.Pending_Job_Card);
                notifications = db.HR_Emp_Notification.Where(x => x.EmployeeID == ID && x.Deleted != true && x.UserSpecific == true && x.NotificationTypeID == typeID).ToList();
                total = notifications.Count;

                if (total > 0)
                {
                    list.Append("<li><a href='" + notifications[total - 1].NotificationURL +
                         "'><div class='clearfix'><span class='pull-left'><i class='btn btn-xs no-hover btn-blue fa fa-bell'></i> " + notifications[total - 1].Notification + "</span><span class='pull-right badge badge-info'>");
                    list.Append(total.ToString());
                    list.Append("</span></span></div></a></li>");
                }
                notifications = new List<HR_Emp_Notification>();
                typeID = Convert.ToInt16(Utilities.NotificationType.PendingVisitorEntry);
                notifications = db.HR_Emp_Notification.Where(x => x.EmployeeID == ID && x.Deleted != true && x.UserSpecific == true && x.NotificationTypeID == typeID).ToList();
                total = notifications.Count;

                if (total > 0)
                {
                    list.Append("<li><a href='" + notifications[total - 1].NotificationURL +
                         "'><div class='clearfix'><span class='pull-left'><i class='btn btn-xs no-hover btn-blue fa fa-bell'></i> " + notifications[total - 1].Notification + "</span><span class='pull-right badge badge-info'>");
                    list.Append(total.ToString());
                    list.Append("</span></span></div></a></li>");
                }


                list.Append("</ul>");
                //list.Append(@"<ul class='dropdown-menu dropdown-navbar navbar-pink'>
                //                            <li>
                //                                <a href = '#'>
                //                                    <div class='clearfix'>
                //                                        <span class='pull-left'>
                //                                            <i class='btn btn-xs no-hover btn-pink fa fa-comment'></i>
                //                                            New Comments
                //                                        </span>
                //                                        <span class='pull-right badge badge-info'>+12</span>
                //                                    </div>
                //                                </a>
                //                            </li>

                //                            <li>
                //                                <a href = '#' >
                //                                    < i class='btn btn-xs btn-primary fa fa-user'></i>
                //                                    Bob just signed up as an editor...
                //                                </a>
                //                            </li>

                //                            <li>
                //                                <a href = '#' >
                //                                    < div class='clearfix'>
                //                                        <span class='pull-left'>
                //                                            <i class='btn btn-xs no-hover btn-success fa fa-shopping-cart'></i>
                //                                            New Orders
                //                                        </span>
                //                                        <span class='pull-right badge badge-success'>+8</span>
                //                                    </div>
                //                                </a>
                //                            </li>

                //                            <li>
                //                                <a href = '#' >
                //                                    < div class='clearfix'>
                //                                        <span class='pull-left'>
                //                                            <i class='btn btn-xs no-hover btn-info fa fa-twitter'></i>
                //                                            Followers
                //                                        </span>
                //                                        <span class='pull-right badge badge-info'>+11</span>
                //                                    </div>
                //                                </a>
                //                            </li>
                //                        </ul>");
                return Content(list.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetDependentNotification(int ID)
        {

            try
            {
                List<HR_Emp_Notification> notifications = new List<HR_Emp_Notification>();
                int typeID = Convert.ToInt16(Utilities.NotificationType.Dependent);
                ViewUserEmp model = db.ViewUserEmps.FirstOrDefault(x => x.EmpID == ID);
                if (model.EmpID > 0)
                {
                    if (model.HRModule == true)
                    {
                        notifications = db.HR_Emp_Notification.Where(x => x.RoleType == "HRModule" && x.Deleted != true && x.UserSpecific == false && x.NotificationTypeID == typeID).ToList();
                    }
                    else
                    {
                        notifications = db.HR_Emp_Notification.Where(x => x.EmployeeID == ID && x.Deleted != true && x.UserSpecific == true && x.NotificationTypeID == typeID).ToList();
                    }
                }
                int total = notifications.Count;
                StringBuilder list = new StringBuilder();
                if (total > 0)
                {
                    list.Append("<a href='" + notifications[0].NotificationURL +
                        "'><div class='clearfix'><span class='pull-left'><i class='btn btn-xs btn-primary fa fa-user'></i>Dependent</span><span class='pull-right'>");
                    list.Append(total.ToString());
                    list.Append("</span></span></div></a>");
                }
                else
                {
                    list.Append("<a href='#'><div class='clearfix'><span class='pull-left'><i class='btn btn-xs btn-primary fa fa-user'></i>Dependent</span><span class='pull-right'>0</span></span></div></a>");

                }
                return Content(list.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetTotalNotification(int ID)
        {

            try
            {
                List<HR_Emp_Notification> notifications = new List<HR_Emp_Notification>();
                User model = db.Users.FirstOrDefault(x => x.EmpID == ID);
                int TotalCount = 0;
                if (model.EmpID > 0)
                {
                    if (model.HRRequest == true)
                    {
                        TotalCount = db.HR_Emp_Notification.Count(x => x.RoleType == "HRModule" && x.Deleted == false);

                    }
                    else
                    {
                        TotalCount = db.HR_Emp_Notification.Count(x => x.EmployeeID == model.EmpID && x.UserSpecific == true && x.Deleted == false);
                    }
                }

                return Content(TotalCount.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetTotalHRNotification(int ID)
        {

            try
            {
                List<HR_Emp_Notification> notifications = new List<HR_Emp_Notification>();
                User model = db.Users.FirstOrDefault(x => x.EmpID == ID);
                int TotalCount = 0;
                if (model.EmpID > 0)
                {
                    if (model.HRRequest == true)
                    {
                        TotalCount = db.HR_Emp_Notification.Count(x => x.RoleType == "HRModule" && x.Deleted == false && x.NotificationTypeID < 237);

                    }
                    else
                    {
                        TotalCount = db.HR_Emp_Notification.Count(x => x.EmployeeID == model.EmpID && x.UserSpecific == true && x.Deleted == false && x.NotificationTypeID < 237);
                    }
                }

                return Content(TotalCount.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Department Wise Visitors Count
        public ActionResult PVSDeptSummary()
        {
            dateS = Convert.ToDateTime(Session["DateStart"].ToString());
            dateE = Convert.ToDateTime(Session["DateEnd"].ToString());
            List<DMParentModel> dmList = new List<DMParentModel>();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            if (LoggedInUser.UserType == "A" || LoggedInUser.UserType == "E" || LoggedInUser.UserType == "H")
            {
                List<ViewVisitEmp> visitList = db.ViewVisitEmps.Where(aa => aa.VisitDate >= dateS && aa.VisitDate <= dateE).ToList();

                dmList = DashboardManager.GetDataForDeptVMSummary(visitList, visitList.Select(aa=>aa.SecID).Distinct().ToList());
            }
            else
            {
                List<EmpView> emps = new List<EmpView>();
                emps = OTHelperManager.GetEmployees(db.EmpViews.Where(aa => aa.Status == "Active").ToList(), LoggedInUser);
                List<ViewVisitEmp> visitList = db.ViewVisitEmps.Where(aa => aa.VisitDate >= dateS && aa.VisitDate <= dateE).ToList();
                visitList = DashboardManager.GetVisitDataForSpecificEmp(visitList, emps);
                dmList = DashboardManager.GetDataForDeptVMSummary(visitList, emps.Select(aa => aa.SecID).Distinct().ToList());
            }
            return PartialView(dmList);
        }
        #endregion

        #region Employee Wise Visitors Count
        public ActionResult PVSEmpSummary(int? deptid)
        {
            dateS = Convert.ToDateTime(Session["DateStart"].ToString());
            dateE = Convert.ToDateTime(Session["DateEnd"].ToString());
            List<DMParentModel> dmList = new List<DMParentModel>();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            if (LoggedInUser.UserType == "A" || LoggedInUser.UserType == "E" || LoggedInUser.UserType == "H")
            {
                List<ViewVisitEmp> visitList = db.ViewVisitEmps.Where(aa => aa.VisitDate >= dateS && aa.VisitDate <= dateE && aa.SecID == deptid).ToList();
                dmList = DashboardManager.GetDataForEmpVMSummary(visitList, visitList.Select(aa => aa.EmployeeID).Distinct().ToList());
                
            }
            else
            {
                List<EmpView> emps = new List<EmpView>();
                emps = OTHelperManager.GetEmployees(db.EmpViews.Where(aa => aa.Status == "Active" && aa.SecID==deptid).ToList(), LoggedInUser);
                List<ViewVisitEmp> visitList = db.ViewVisitEmps.Where(aa => aa.VisitDate >= dateS && aa.VisitDate <= dateE && aa.SecID == deptid).ToList();
                dmList = DashboardManager.GetDataForEmpVMSummary(visitList, emps.Select(aa => aa.EmployeeID).Distinct().ToList());
                
            }
            return PartialView(dmList);
        }
        public ActionResult PVSEmpSummaryDetails(int? id)
        {
            dateS = Convert.ToDateTime(Session["DateStart"].ToString());
            dateE = Convert.ToDateTime(Session["DateEnd"].ToString());
            List<ViewVisitEmp> list = db.ViewVisitEmps.Where(aa => aa.VisitDate >= dateS && aa.VisitDate <= dateE && aa.EmployeeID == id).ToList();
            List<ModelVMSDetail> tempList = new List<ModelVMSDetail>();
            foreach (var item in list.Select(aa => aa.VisitorID).Distinct().ToList())
            {
                ModelVMSDetail obj = new ModelVMSDetail();
                obj.VisitorName = list.Where(aa => aa.VisitorID == item).First().VName;
                obj.VisitorID = list.Where(aa => aa.VisitorID == item).First().VisitorID;
                obj.VisitorCompany = list.Where(aa => aa.VisitorID == item).First().VComapny;
                obj.CNIC = list.Where(aa => aa.VisitorID == item).First().VisitorCNIC;
                obj.EmployeeID = (int)id;
                obj.NoOfDays = list.Where(aa => aa.VisitorID == item).Count();
                tempList.Add(obj);
            }
            ViewBag.Name = db.EmpViews.FirstOrDefault(aa => aa.EmployeeID == id).FullName + " >> " + dateS.ToString("dd-MMM-yyyy") + " to " + dateE.ToString("dd-MMM-yyyy");
            return View(tempList.OrderBy(aa=>aa.VisitorName));
        }
        public ActionResult PVSVisitorDetail(int? visitorId, string EmpNo)
        {
            dateS = Convert.ToDateTime(Session["DateStart"].ToString());
            dateE = Convert.ToDateTime(Session["DateEnd"].ToString());
            List<ViewVisitEmp> list = db.ViewVisitEmps.Where(aa => aa.VisitorID == visitorId && aa.VisitDate >= dateS && aa.VisitDate <= dateE && aa.EmpNo== EmpNo).ToList();
            ViewBag.Name = db.EmpViews.FirstOrDefault(aa => aa.EmpNo == EmpNo).FullName + " >> " + dateS.ToString("dd-MMM-yyyy") + " to " + dateE.ToString("dd-MMM-yyyy");
            return View(list.OrderByDescending(aa=>aa.VisitDate));
        }
        #endregion

        #region TMS Summary

        public ActionResult PTMSSummary()
        {
            dateS = Convert.ToDateTime(Session["DateStart"].ToString());
            dateE = Convert.ToDateTime(Session["DateEnd"].ToString());
            DMTMSParentModel dmModel = new DMTMSParentModel();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            if (LoggedInUser.UserType == "A" || LoggedInUser.UserType == "E" || LoggedInUser.UserType == "H")
            {
                List<ViewAttData> AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE &&aa.DutyCode=="D").ToList();
                dmModel = DashboardManager.GetDataForTMSummary(AttList, dmModel);

            }
            else
            {
                List<EmpView> emps = new List<EmpView>();
                emps = OTHelperManager.GetEmployees(db.EmpViews.Where(aa => aa.Status == "Active").ToList(), LoggedInUser);
                List<ViewAttData> AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.DutyCode == "D").ToList();
                dmModel = DashboardManager.GetDataForTMSummary(DashboardManager.GetAttDataForSpecificEmp(AttList, emps),dmModel);

            }
            if (dmModel.ChildList.Count() > 0)
            {
                return PartialView(dmModel);
            }
            else
                return Json("OK", JsonRequestBehavior.AllowGet);
        }

        #region TMS Summary Detail

        public ActionResult TMSDetailLateIn()
        {
            ViewBag.BreadCrumb = "Late In Detail";
            ViewBag.Header = "Late In Detail";
            return View("TMSDetail", GetLateInData());
        }
        public ActionResult TMSDetailLateOut()
        {
            ViewBag.BreadCrumb = "Late Out Detail";
            ViewBag.Header = "Late Out Detail";
            return View("TMSDetail", GetLateOutData());
        }
        public ActionResult TMSDetailEarlyIn()
        {
            ViewBag.BreadCrumb = "Early In Detail";
            ViewBag.Header = "Early In Detail";
            return View("TMSDetail", GetEarlyInData());
        }
        public ActionResult TMSDetailEarlyOut()
        {
            ViewBag.BreadCrumb = "Early Out Detail";
            ViewBag.Header = "Early Out Detail";
            return View("TMSDetail", GetEarlyOutData());
        }
        private List<ModelTMSDetail> GetLateInData()
        {
            dateS = Convert.ToDateTime(Session["DateStart"].ToString());
            dateE = Convert.ToDateTime(Session["DateEnd"].ToString());
            ViewBag.SubHeader = dateS.ToString("dd-MMM-yyyy") + " to " + dateS.ToString("dd-MMM-yyyy");
            List<Att_DailyAttendance> tAttData = new List<Att_DailyAttendance>();
            List<Att_DailyAttendance> attData = new List<Att_DailyAttendance>();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            if (dateS == dateE)
                tAttData = db.Att_DailyAttendance.Where(aa => aa.AttDate == dateS && aa.LateIn > 0).ToList();
            else
                tAttData = db.Att_DailyAttendance.Where(aa => aa.LateIn > 0 && aa.AttDate >= dateS && aa.AttDate <= dateE).ToList();
            List<EmpView> emps = db.EmpViews.Where(aa => aa.Status == "Active").ToList();
            if (LoggedInUser.UserType != "A")
                emps = GetEmployees(emps, LoggedInUser);
            else
                emps = db.EmpViews.Where(aa => aa.Status == "Active").ToList();
            foreach (var item in emps)
            {
                attData.AddRange(tAttData.Where(aa => aa.EmpID == item.EmployeeID).ToList());
            }
            List<ModelTMSDetail> list = new List<ModelTMSDetail>();
            if (dateS != null && dateE != null)
            {
                foreach (var item in attData.Select(aa => aa.EmpID).Distinct().ToList())
                {
                    ModelTMSDetail mae = new ModelTMSDetail();
                    mae.EmpNo = attData.Where(aa => aa.EmpID == item).First().EmpNo;
                    mae.EmpName = attData.Where(aa => aa.EmpID == item).First().HR_Employee.FullName;
                    mae.Designation = attData.Where(aa => aa.EmpID == item).First().HR_Employee.HR_Designation.DesignationName;
                    mae.NoOfDays = attData.Where(aa => aa.EmpID == item && aa.LateIn > 0).Count();
                    mae.TotalTime = (int)(attData.Where(aa => aa.EmpID == item && aa.LateIn > 0).Sum(aa => aa.LateIn));
                    list.Add(mae);
                }

            }
            return list.OrderBy(aa=>aa.EmpName).ToList();
        }
        private List<ModelTMSDetail> GetLateOutData()
        {
            dateS = Convert.ToDateTime(Session["DateStart"].ToString());
            dateE = Convert.ToDateTime(Session["DateEnd"].ToString());
            ViewBag.SubHeader = dateS.ToString("dd-MMM-yyyy") + " to " + dateS.ToString("dd-MMM-yyyy");
            List<Att_DailyAttendance> tAttData = new List<Att_DailyAttendance>();
            List<Att_DailyAttendance> attData = new List<Att_DailyAttendance>();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            if (dateS == dateE)
                tAttData = db.Att_DailyAttendance.Where(aa => aa.AttDate == dateS && aa.LateOut > 0).ToList();
            else
                tAttData = db.Att_DailyAttendance.Where(aa => aa.LateOut > 0 && aa.AttDate >= dateS && aa.AttDate <= dateE).ToList();
            List<EmpView> emps = db.EmpViews.Where(aa => aa.Status == "Active").ToList();
            if (LoggedInUser.UserType != "A")
                emps = GetEmployees(emps, LoggedInUser);
            else
                emps = db.EmpViews.Where(aa => aa.Status == "Active").ToList();
            foreach (var item in emps)
            {
                attData.AddRange(tAttData.Where(aa => aa.EmpID == item.EmployeeID).ToList());
            }
            List<ModelTMSDetail> list = new List<ModelTMSDetail>();
            if (dateS != null && dateE != null)
            {
                foreach (var item in attData.Select(aa => aa.EmpID).Distinct().ToList())
                {
                    ModelTMSDetail mae = new ModelTMSDetail();
                    mae.EmpNo = attData.Where(aa => aa.EmpID == item).First().EmpNo;
                    mae.EmpName = attData.Where(aa => aa.EmpID == item).First().HR_Employee.FullName;
                    mae.Designation = attData.Where(aa => aa.EmpID == item).First().HR_Employee.HR_Designation.DesignationName;
                    mae.NoOfDays = attData.Where(aa => aa.EmpID == item && aa.LateOut > 0).Count();
                    mae.TotalTime = (int)(attData.Where(aa => aa.EmpID == item && aa.LateOut > 0).Sum(aa => aa.LateOut));
                    list.Add(mae);
                }

            }
            return list.OrderBy(aa => aa.EmpName).ToList();
        }
        private List<ModelTMSDetail> GetEarlyInData()
        {
            dateS = Convert.ToDateTime(Session["DateStart"].ToString());
            dateE = Convert.ToDateTime(Session["DateEnd"].ToString());
            ViewBag.SubHeader = dateS.ToString("dd-MMM-yyyy") + " to " + dateS.ToString("dd-MMM-yyyy");
            List<Att_DailyAttendance> tAttData = new List<Att_DailyAttendance>();
            List<Att_DailyAttendance> attData = new List<Att_DailyAttendance>();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            if (dateS == dateE)
                tAttData = db.Att_DailyAttendance.Where(aa => aa.AttDate == dateS && aa.EarlyIn > 0).ToList();
            else
                tAttData = db.Att_DailyAttendance.Where(aa => aa.EarlyIn > 0 && aa.AttDate >= dateS && aa.AttDate <= dateE).ToList();
            List<EmpView> emps = db.EmpViews.Where(aa => aa.Status == "Active").ToList();
            if (LoggedInUser.UserType != "A")
                emps = GetEmployees(emps, LoggedInUser);
            else
                emps = db.EmpViews.Where(aa => aa.Status == "Active").ToList();
            foreach (var item in emps)
            {
                attData.AddRange(tAttData.Where(aa => aa.EmpID == item.EmployeeID).ToList());
            }
            List<ModelTMSDetail> list = new List<ModelTMSDetail>();
            if (dateS != null && dateE != null)
            {
                foreach (var item in attData.Select(aa => aa.EmpID).Distinct().ToList())
                {
                    ModelTMSDetail mae = new ModelTMSDetail();
                    mae.EmpNo = attData.Where(aa => aa.EmpID == item).First().EmpNo;
                    mae.EmpName = attData.Where(aa => aa.EmpID == item).First().HR_Employee.FullName;
                    mae.Designation = attData.Where(aa => aa.EmpID == item).First().HR_Employee.HR_Designation.DesignationName;
                    mae.NoOfDays = attData.Where(aa => aa.EmpID == item && aa.EarlyIn > 0).Count();
                    mae.TotalTime = (int)(attData.Where(aa => aa.EmpID == item && aa.EarlyIn > 0).Sum(aa => aa.EarlyIn));
                    list.Add(mae);
                }

            }
            return list.OrderBy(aa => aa.EmpName).ToList();
        }
        private List<ModelTMSDetail> GetEarlyOutData()
        {
            dateS = Convert.ToDateTime(Session["DateStart"].ToString());
            dateE = Convert.ToDateTime(Session["DateEnd"].ToString());
            List<Att_DailyAttendance> tAttData = new List<Att_DailyAttendance>();
            List<Att_DailyAttendance> attData = new List<Att_DailyAttendance>();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            ViewBag.SubHeader = dateS.ToString("dd-MMM-yyyy") + " to " + dateS.ToString("dd-MMM-yyyy");
            if (dateS == dateE)
                tAttData = db.Att_DailyAttendance.Where(aa => aa.AttDate == dateS && aa.EarlyOut > 0).ToList();
            else
                tAttData = db.Att_DailyAttendance.Where(aa => aa.EarlyOut > 0 && aa.AttDate >= dateS && aa.AttDate <= dateE).ToList();
            List<EmpView> emps = db.EmpViews.Where(aa => aa.Status == "Active").ToList();
            if (LoggedInUser.UserType != "A")
                emps = GetEmployees(emps, LoggedInUser);
            else
                emps = db.EmpViews.Where(aa => aa.Status == "Active").ToList();
            foreach (var item in emps)
            {
                attData.AddRange(tAttData.Where(aa => aa.EmpID == item.EmployeeID).ToList());
            }
            List<ModelTMSDetail> list = new List<ModelTMSDetail>();
            if (dateS != null && dateE != null)
            {
                foreach (var item in attData.Select(aa => aa.EmpID).Distinct().ToList())
                {
                    ModelTMSDetail mae = new ModelTMSDetail();
                    mae.EmpNo = attData.Where(aa => aa.EmpID == item).First().EmpNo;
                    mae.EmpName = attData.Where(aa => aa.EmpID == item).First().HR_Employee.FullName;
                    mae.Designation = attData.Where(aa => aa.EmpID == item).First().HR_Employee.HR_Designation.DesignationName;
                    mae.NoOfDays = attData.Where(aa => aa.EmpID == item && aa.EarlyOut > 0).Count();
                    mae.TotalTime = (int)(attData.Where(aa => aa.EmpID == item && aa.EarlyOut > 0).Sum(aa => aa.EarlyOut));
                    list.Add(mae);
                }

            }
            return list.OrderBy(aa => aa.EmpName).ToList();
        }

        public ActionResult TMSAttDetail(string id, string type, string FullName)
        {
            dateS = Convert.ToDateTime(Session["DateStart"].ToString());
            dateE = Convert.ToDateTime(Session["DateEnd"].ToString());
            ViewBag.SubHeader = " for " + FullName;
            List<ViewAttData> tAttData = new List<ViewAttData>();
            List<ViewAttData> attData = new List<ViewAttData>();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            if (dateS == dateE)
            {
                switch (type)
                {
                    case "LateIn":
                        ViewBag.BreadCrumb = "Late In Detail";
                        ViewBag.Header = "Late In Detail";
                        tAttData = db.ViewAttDatas.Where(aa => aa.AttDate == dateS && aa.LateIn > 0 && aa.EmpNo == id).ToList();
                        break;
                    case "LateOut":
                        ViewBag.BreadCrumb = "Late Out Detail";
                        ViewBag.Header = "Late Out Detail";
                        tAttData = db.ViewAttDatas.Where(aa => aa.AttDate == dateS && aa.LateOut > 0 && aa.EmpNo == id).ToList();
                        break;
                    case "EarlyIn":
                        ViewBag.BreadCrumb = "Early In Detail";
                        ViewBag.Header = "Early In Detail";
                        tAttData = db.ViewAttDatas.Where(aa => aa.AttDate == dateS && aa.EarlyIn > 0 && aa.EmpNo == id).ToList();
                        break;
                    case "EarlyOut":
                        ViewBag.BreadCrumb = "Early Out In Detail";
                        ViewBag.Header = "Early Out Detail";
                        tAttData = db.ViewAttDatas.Where(aa => aa.AttDate == dateS && aa.EarlyOut > 0 && aa.EmpNo == id).ToList();
                        break;
                    case "Absent":
                        ViewBag.BreadCrumb = "Absent In Detail";
                        ViewBag.Header = "Absent Detail";
                        tAttData = db.ViewAttDatas.Where(aa => aa.AttDate == dateS && aa.StatusAB==true && aa.EmpNo == id).ToList();
                        break;
                    case "Leave":
                        ViewBag.BreadCrumb = "Leaves In Detail";
                        ViewBag.Header = "Leaves Detail";
                        tAttData = db.ViewAttDatas.Where(aa => aa.AttDate == dateS && aa.StatusLeave == true && aa.EmpNo == id).ToList();
                        break;
                }

            }
            else
            {
                switch (type)
                {
                    case "LateIn":
                        ViewBag.BreadCrumb = "Late In Detail";
                        ViewBag.Header = "Late In Detail";
                        tAttData = db.ViewAttDatas.Where(aa => aa.LateIn > 0 && aa.AttDate >= dateS && aa.AttDate <= dateE && aa.EmpNo == id).ToList();
                        break;
                    case "LateOut":
                        ViewBag.BreadCrumb = "Late Out Detail";
                        ViewBag.Header = "Late Out Detail";
                        tAttData = db.ViewAttDatas.Where(aa => aa.LateOut > 0 && aa.AttDate >= dateS && aa.AttDate <= dateE && aa.EmpNo == id).ToList();
                        break;
                    case "EarlyIn":
                        ViewBag.BreadCrumb = "Early In Detail";
                        ViewBag.Header = "Early In Detail";
                        tAttData = db.ViewAttDatas.Where(aa => aa.EarlyIn > 0 && aa.AttDate >= dateS && aa.AttDate <= dateE && aa.EmpNo == id).ToList();
                        break;
                    case "EarlyOut":
                        ViewBag.BreadCrumb = "Early Out In Detail";
                        ViewBag.Header = "Early Out Detail";
                        tAttData = db.ViewAttDatas.Where(aa => aa.EarlyOut > 0 && aa.AttDate >= dateS && aa.AttDate <= dateE && aa.EmpNo == id).ToList();
                        break;
                    case "Absent":
                        ViewBag.BreadCrumb = "Absent Detail";
                        ViewBag.Header = "Absent Detail";
                        tAttData = db.ViewAttDatas.Where(aa => aa.StatusAB==true && aa.AttDate >= dateS && aa.AttDate <= dateE && aa.EmpNo == id).ToList();
                        break;
                    case "Leave":
                        ViewBag.BreadCrumb = "Leaves Detail";
                        ViewBag.Header = "Leaves Detail";
                        tAttData = db.ViewAttDatas.Where(aa => aa.StatusLeave == true && aa.AttDate >= dateS && aa.AttDate <= dateE && aa.EmpNo == id).ToList();
                        break;
                }
            }
            List<EmpView> emps = db.EmpViews.Where(aa => aa.Status == "Active").ToList();
            if (LoggedInUser.UserType != "A")
                emps = GetEmployees(emps, LoggedInUser);
            else
                emps = db.EmpViews.Where(aa => aa.Status == "Active").ToList();
            foreach (var item in emps)
            {
                attData.AddRange(tAttData.Where(aa => aa.EmpID == item.EmployeeID).ToList());
            }
            return View(attData.OrderByDescending(aa => aa.AttDate).ToList());
        }
        #endregion
        #endregion

        #region -- TMS Pie Chart --
        public ActionResult RenderPCForDivs(string GraphType)
        {
            dateS = Convert.ToDateTime(Session["DateStart"].ToString());
            dateE = Convert.ToDateTime(Session["DateEnd"].ToString());
            DMPieChartParentModel vm = new DMPieChartParentModel();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            List<ViewAttData> AttList = new List<ViewAttData>();
            vm.DivDept = "Div";
            vm.GraphName = GraphType;
            vm.HeaderID = "DeptID";
            vm.HeaderName = "Division";
            vm.HeaderCount = "Employees";
            vm.SubLabel = "Click on below Division to View Details";
            switch (GraphType)
            {
                case "LateIn":
                    vm.Label = "Division Late In Detail";
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.LateIn > 0).ToList();
                    break;
                case "LateOut":
                    vm.Label = "Division Late Out Detail";
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.LateOut > 0).ToList();
                    break;
                case "EarlyIn":
                    vm.Label = "Division Early In Detail";
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.EarlyIn > 0).ToList();
                    break;
                case "EarlyOut":
                    vm.Label = "Division Early Out Detail";
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.EarlyOut > 0).ToList();
                    break;
                case "Absent":
                    vm.Label = "Division Absent Detail";
                    vm.HeaderCount = "Total Days";
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.AbDays >0).ToList();
                    break;
                case "Leave":
                    vm.Label = "Division Leaves Detail";
                    vm.HeaderCount = "Total Days";
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.LeaveDays>0).ToList();
                    break;
            }
            if (LoggedInUser.UserType != "A" && LoggedInUser.UserType != "E" && LoggedInUser.UserType != "H")
            {
                List<EmpView> emps = new List<EmpView>();
                emps = OTHelperManager.GetEmployees(db.EmpViews.Where(aa => aa.Status == "Active").ToList(), LoggedInUser);
                AttList = DashboardManager.GetAttDataForSpecificEmp(AttList, emps);
            }
            vm = DashboardManager.GetDataForPieChartDivision(AttList, vm, GraphType);
            if (vm.ChildList.Count() > 0)
            {
                return PartialView("PVDivPieChart",vm);
            }
            else
                return Json("OK", JsonRequestBehavior.AllowGet);
        }
        public ActionResult RenderPCForSpecificDivs(string GraphType, int? DeptID)
        {
            dateS = Convert.ToDateTime(Session["DateStart"].ToString());
            dateE = Convert.ToDateTime(Session["DateEnd"].ToString());
            DMPieChartParentModel vm = new DMPieChartParentModel();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            List<ViewAttData> AttList = new List<ViewAttData>();
            vm.DivDept = "Dept";
            vm.GraphName = GraphType;
            vm.HeaderID = "DeptID";
            vm.HeaderName = "Department";
            vm.HeaderCount = "Employees";
            switch (GraphType)
            {
                case "LateIn":
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.LateIn > 0 && aa.DeptID == DeptID).ToList();
                    break;
                case "LateOut":
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.LateOut > 0 && aa.DeptID == DeptID).ToList();
                    break;
                case "EarlyIn":
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.EarlyIn > 0 && aa.DeptID == DeptID).ToList();
                    break;
                case "EarlyOut":
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.EarlyOut > 0 && aa.DeptID == DeptID).ToList();
                    break;
                case "Absent":
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.StatusAB == true && aa.DeptID == DeptID).ToList();
                    break;
                case "Leave":
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.StatusLeave == true && aa.DeptID == DeptID).ToList();
                    break;
            }
            if (LoggedInUser.UserType != "A" && LoggedInUser.UserType != "E" && LoggedInUser.UserType != "H")
            {
                List<EmpView> emps = new List<EmpView>();
                emps = OTHelperManager.GetEmployees(db.EmpViews.Where(aa => aa.Status == "Active" && aa.DeptID == DeptID).ToList(), LoggedInUser);
                AttList = DashboardManager.GetAttDataForSpecificEmp(AttList, emps);
            }
            vm = DashboardManager.GetDataForPieChartDept(AttList, vm, GraphType);
            if (vm.ChildList.Count() > 0)
            {
                return PartialView("PVDeptPieChart", vm);
            }
            else
                return Json("OK", JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadDeptTMSEmployeeList(int? secid, string GraphType)
        {
            dateS = Convert.ToDateTime(Session["DateStart"].ToString());
            dateE = Convert.ToDateTime(Session["DateEnd"].ToString());
            ViewBag.SubHeader = dateS.ToString("dd-MMM-yyyy") + " to " + dateS.ToString("dd-MMM-yyyy");
            List<ViewAttData> AttList = new List<ViewAttData>();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            switch (GraphType)
            {
                case "LateIn":
                    ViewBag.BreadCrumb = "Late In Detail";
                    ViewBag.Header = "Late In Detail";
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.LateIn > 0 && aa.SecID == secid).ToList();
                    break;
                case "LateOut":
                    ViewBag.BreadCrumb = "Late Out Detail";
                    ViewBag.Header = "Late Out Detail";
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.LateOut > 0 && aa.SecID == secid).ToList();
                    break;
                case "EarlyIn":
                    ViewBag.BreadCrumb = "Early In Detail";
                    ViewBag.Header = "Early In Detail";
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.EarlyIn > 0 && aa.SecID == secid).ToList();
                    break;
                case "EarlyOut":
                    ViewBag.BreadCrumb = "Early Out Detail";
                    ViewBag.Header = "Early Out Detail";
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.EarlyOut > 0 && aa.SecID == secid).ToList();
                    break;
                case "Absent":
                    ViewBag.BreadCrumb = "Absent Detail";
                    ViewBag.Header = "Absent Detail";
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.StatusAB == true && aa.SecID == secid).ToList();
                    break;
                case "Leave":
                    ViewBag.BreadCrumb = "Leaves Detail";
                    ViewBag.Header = "Leaves Detail";
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.StatusLeave == true && aa.SecID == secid).ToList();
                    break;
            }
            if (LoggedInUser.UserType != "A" && LoggedInUser.UserType != "E" && LoggedInUser.UserType != "H")
            {
                List<EmpView> emps = new List<EmpView>();
                emps = OTHelperManager.GetEmployees(db.EmpViews.Where(aa => aa.Status == "Active" && aa.SecID == secid).ToList(), LoggedInUser);
                AttList = DashboardManager.GetAttDataForSpecificEmp(AttList, emps);
            }
            List<ModelTMSDetail> list = new List<ModelTMSDetail>();
            if (dateS != null && dateE != null)
            {
                foreach (var item in AttList.Select(aa => aa.EmpID).Distinct().ToList())
                {
                    ModelTMSDetail mae = new ModelTMSDetail();
                    mae.EmpNo = AttList.Where(aa => aa.EmpID == item).First().EmpNo;
                    mae.EmpName = AttList.Where(aa => aa.EmpID == item).First().FullName;
                    mae.Designation = AttList.Where(aa => aa.EmpID == item).First().DesignationName;
                    switch (GraphType)
                    {
                        case "LateIn":
                            mae.NoOfDays = AttList.Where(aa => aa.EmpID == item && aa.LateIn > 0).Count();
                            mae.TotalTime = (int)(AttList.Where(aa => aa.EmpID == item && aa.LateIn > 0).Sum(aa => aa.LateIn));
                            break;
                        case "LateOut":
                            mae.NoOfDays = AttList.Where(aa => aa.EmpID == item && aa.LateOut > 0).Count();
                            mae.TotalTime = (int)(AttList.Where(aa => aa.EmpID == item && aa.LateOut > 0).Sum(aa => aa.LateOut));
                            break;
                        case "EarlyIn":
                            mae.NoOfDays = AttList.Where(aa => aa.EmpID == item && aa.EarlyIn > 0).Count();
                            mae.TotalTime = (int)(AttList.Where(aa => aa.EmpID == item && aa.EarlyIn > 0).Sum(aa => aa.EarlyIn));
                            break;
                        case "EarlyOut":
                            mae.NoOfDays = AttList.Where(aa => aa.EmpID == item && aa.EarlyOut > 0).Count();
                            mae.TotalTime = (int)(AttList.Where(aa => aa.EmpID == item && aa.EarlyOut > 0).Sum(aa => aa.EarlyOut));
                            break;
                        case "Absent":
                            mae.NoOfDays = AttList.Where(aa => aa.EmpID == item && aa.StatusAB==true).Count();
                            mae.TotalTime = (int)(AttList.Where(aa => aa.EmpID == item && aa.StatusAB == true).Sum(aa =>Convert.ToDecimal( aa.StatusAB)));
                            break;
                        case "Leave":
                            mae.NoOfDays = AttList.Where(aa => aa.EmpID == item && aa.StatusLeave == true).Count();
                            mae.TotalTime = (int)(AttList.Where(aa => aa.EmpID == item && aa.StatusLeave == true).Sum(aa => Convert.ToDecimal(aa.StatusLeave)));
                            break;
                    }
                    list.Add(mae);
                }

            }
            return PartialView("TMSDetail",list.OrderBy(aa => aa.EmpName).ToList());
        }
        public ActionResult RenderPCForDepts(string GraphType)
        {
            dateS = Convert.ToDateTime(Session["DateStart"].ToString());
            dateE = Convert.ToDateTime(Session["DateEnd"].ToString());
            DMPieChartParentModel vm = new DMPieChartParentModel();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            List<ViewAttData> AttList = new List<ViewAttData>();
            vm.DivDept = "Div";
            vm.GraphName = GraphType;
            vm.HeaderID = "DeptID";
            vm.HeaderName = "Division";
            vm.HeaderCount = "Employees";
            vm.SubLabel = "Click on below Division to View Details";
            switch (GraphType)
            {
                case "LateIn":
                    vm.Label = "Division Late In Detail";
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.LateIn > 0).ToList();
                    break;
                case "LateOut":
                    vm.Label = "Division Late Out Detail";
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.LateOut > 0).ToList();
                    break;
                case "EarlyIn":
                    vm.Label = "Division Early In Detail";
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.EarlyIn > 0).ToList();
                    break;
                case "EarlyOut":
                    vm.Label = "Division Early Out Detail";
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.EarlyOut > 0).ToList();
                    break;
                case "Absent":
                    vm.Label = "Division Absent Detail";
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.StatusAB==true).ToList();
                    break;
                case "Leave":
                    vm.Label = "Division Leaves Detail";
                    AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.StatusLeave==true).ToList();
                    break;
            }
            if (LoggedInUser.UserType != "A" && LoggedInUser.UserType != "E" && LoggedInUser.UserType != "H")
            {
                List<EmpView> emps = new List<EmpView>();
                emps = OTHelperManager.GetEmployees(db.EmpViews.Where(aa => aa.Status == "Active").ToList(), LoggedInUser);
                AttList = DashboardManager.GetAttDataForSpecificEmp(AttList, emps);
                vm = DashboardManager.GetDataForPieChartDivision(AttList, vm, GraphType);
            }
            vm = DashboardManager.GetDataForPieChartDept(AttList, vm, GraphType);
            if (vm.ChildList.Count() > 0)
            {
                return PartialView("PVDeptPieChart",vm);
            }
            else
                return Json("OK", JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult PVEmployeeVisitor()
        {
            dateS = Convert.ToDateTime(Session["DateStart"].ToString());
            dateE = Convert.ToDateTime(Session["DateEnd"].ToString());
            List<DMParentModel> dmList = new List<DMParentModel>();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            List<ViewVisitEmp> visitList = db.ViewVisitEmps.Where(aa => aa.VisitDate >= dateS && aa.VisitDate <= dateE && aa.EmployeeID == LoggedInUser.EmployeeID).ToList();
            dmList = DashboardManager.GetDataForEmpVMSummary(visitList);
            ViewBag.EmpNo = LoggedInUser.EmpNo;
            if (dmList.Count > 0)
                return PartialView(dmList);
            else
                return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult PVEmployeeAttendance()
        {
            dateS = Convert.ToDateTime(Session["DateStart"].ToString());
            dateE = Convert.ToDateTime(Session["DateEnd"].ToString());
            DMTMSParentModel dmModel = new DMTMSParentModel();
            ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
            List<ViewAttData> AttList = db.ViewAttDatas.Where(aa => aa.AttDate >= dateS && aa.AttDate <= dateE && aa.DutyCode == "D" && aa.EmpID==LoggedInUser.EmployeeID).ToList();
            dmModel = DashboardManager.GetDataAttendanceEmployee(AttList, dmModel);
            ViewBag.EmpID = LoggedInUser.EmployeeID;
            ViewBag.Name = LoggedInUser.FullName;
            if (dmModel.ChildList.Count() > 0)
            {
                return PartialView(dmModel);
            }
            else
                return Json("OK", JsonRequestBehavior.AllowGet);
        }


        public DateTime dateS { get; set; }
        public DateTime dateE { get; set; }
    }
    public class DashboardValue
    {
        public int VisitorPending { get; set; }
        public int JobCardPending { get; set; }
        public int OvertimePending { get; set; }
        public List<string> Messages { get; set; }
        public List<string> Links { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
    }
    public class RegisterNewUser
    {
        public int id { get; set; }
        public string Employees { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    //Absent 
    public class ModelTMSDetail
    {
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string Designation { get; set; }
        public int NoOfDays { get; set; }
        public int TotalTime { get; set; }
    }
    public class ModelVMSDetail
    {
        public int VisitorID { get; set; }
        public int EmployeeID { get; set; }
        public string VisitorName { get; set; }
        public string VisitorCompany { get; set; }
        public string CNIC { get; set; }
        public int NoOfDays { get; set; }
    }
    
}