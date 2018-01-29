using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PTAWMS.Models;
using PTAWMS.Helper;
using HRM_IKAN.Authentication;
using PTAWMS.App_Start;


namespace PTAWMS.Areas.HumanResource.Controllers
{
    public class AuditLogController : Controller
    {
    
        private HRMEntities db = new HRMEntities();

        // GET: /HumanResource/Auditlog
        public ActionResult Index(int? UserID, DateTime? DateFrom, DateTime? DateTo)
        {
            try
            {
                List<ViewAuditLog> auditLogList = new List<ViewAuditLog>();
                List<VMUser> vmUsers = new List<VMUser>();
                foreach(var item in db.ViewUserEmps.ToList())
                {
                    VMUser vm = new VMUser();
                    vm.UserID = item.UserID;
                    vm.UserName = item.FullName + " (" + item.DesignationName + ")";
                    vmUsers.Add(vm);
                }
                if (UserID ==null)
                {
                    UserID = 0;
                    DateFrom = DateTime.Today.AddDays(-30);
                    DateTo = DateTime.Today;
                }
                else
                {
                    DateTo = DateTo + new TimeSpan(23,59,59);
                }
                {
                    VMUser vm = new VMUser();
                    vm.UserID = 0;
                    vm.UserName = " All Employees ";
                    vmUsers.Insert(0,vm);
                }
                if(UserID==0)
                {
                    auditLogList = db.ViewAuditLogs.Where(aa => aa.AuditDateTime >= DateFrom && aa.AuditDateTime <= DateTo).OrderByDescending(aa => aa.AuditDateTime).ToList();

                }
                else
                {
                    auditLogList = db.ViewAuditLogs.Where(aa => aa.AuditUserID == UserID && aa.AuditDateTime >= DateFrom && aa.AuditDateTime <= DateTo).OrderByDescending(aa => aa.AuditDateTime).ToList();
                }
                ViewBag.DateFrom = DateFrom.Value.ToString("yyyy-MM-dd");
                ViewBag.DateTo = DateTo.Value.ToString("yyyy-MM-dd");
                ViewBag.UserID = new SelectList(vmUsers.OrderBy(aa=>aa.UserName).ToList(), "UserID", "UserName",UserID);
                return View(auditLogList.ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
    public class VMUser
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}