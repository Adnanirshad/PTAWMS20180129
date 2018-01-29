using PTAWMS.App_Start;
using PTAWMS.Areas.HumanResource.Models;
using PTAWMS.DAL;
using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Text;
using System.Data.Entity;
using System.Configuration;
using HRM_IKAN.Authentication;
using PTA.Authentication;

namespace PTAWMS.Areas.HumanResource.Controllers
{
    [CustomControllerAttributes]
    public class HRController : Controller
    {
        private HRMEntities db = new HRMEntities();
        private static string BaseURL
        {
            get { return ConfigurationManager.AppSettings["BaseURL"].ToString(); }
        }
        // GET: HumanResource/HR
        public ActionResult Index(string sortOrder, string SearchString, string currentFilter, int? page)
        {
            try
            {
                List<object> modals = new List<object>();
                List<ViewEmpQualification> model;
                int pendingValue = Convert.ToInt16(Utilities.ProfileStatus.Pending);
                if (page == null)
                    page = 1;

                if (!string.IsNullOrEmpty(SearchString) && SearchString.Length > 0)
                {
                    SearchString = SearchString.Trim();
                    int result;
                    if (int.TryParse(SearchString, out result))
                    {
                        model = db.ViewEmpQualifications.OrderByDescending(x => x.QualificationID).
                        Where(x => x.StatusID == pendingValue && x.Deleted == false && x.EmployeeID == result).ToList();
                    }
                    else
                        model = db.ViewEmpQualifications.OrderByDescending(x => x.QualificationID).
                            Where(x => x.StatusID == pendingValue && x.Deleted == false && (x.FullName.ToUpper().Contains(SearchString.ToUpper())
                            || x.FullName.ToUpper().Contains(SearchString.ToUpper()) || x.Degree.ToUpper().Contains(SearchString.ToUpper())
                            || x.Institute.ToUpper().Contains(SearchString.ToUpper()) || x.Specialization.ToUpper().Contains(SearchString.ToUpper())
                            || x.Grade.ToUpper().Contains(SearchString.ToUpper()) || x.Status.ToUpper().Contains(SearchString.ToUpper())
                            || x.EmployeeID.ToString().Contains(SearchString)
                            )).ToList();

                }
                else
                    model = db.ViewEmpQualifications.OrderByDescending(x => x.QualificationID).Where(x => x.StatusID == pendingValue && x.Deleted == false).ToList();
                modals.Add(model);
                ViewBag.NotificationType = Convert.ToInt16(Utilities.NotificationType.Qualification);
                //return View("PendingQualification", modals);
                int pageSize = 12;
                int pageNumber = (page ?? 1);
                return View("PendingQualification", model.OrderBy(aa => aa.Status).ThenBy(aa => aa.FullName).ToPagedList(pageNumber, pageSize));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetPendingPreJobHistory(string sortOrder, string SearchString, string currentFilter, int? page)
        {
            try
            {
                Session["SelectedMenu"] = "HRPreJobHistory";
                List<object> modals = new List<object>();
                List<ViewEmpPreJobHistory> model;
                int pendingValue = Convert.ToInt16(Utilities.ProfileStatus.Pending);
                if (page == null)
                    page = 1;

                if (!string.IsNullOrEmpty(SearchString) && SearchString.Length > 0)
                {
                    SearchString = SearchString.Trim();
                    int result;
                    if (int.TryParse(SearchString, out result))
                    {
                        model = db.ViewEmpPreJobHistories.OrderByDescending(x => x.ExperienceID).
                        Where(x => x.StatusID == pendingValue && x.Deleted == false && x.EmployeeID == result).ToList();
                    }
                    else
                        model = db.ViewEmpPreJobHistories.OrderByDescending(x => x.ExperienceID).
                            Where(x => x.StatusID == pendingValue && x.Deleted == false && (x.FullName.ToUpper().Contains(SearchString.ToUpper())
                            || x.FullName.ToUpper().Contains(SearchString.ToUpper()) || x.OrganisationName.ToUpper().Contains(SearchString.ToUpper())
                            || x.Designation.ToUpper().Contains(SearchString.ToUpper()) || x.Department.ToUpper().Contains(SearchString.ToUpper())
                            || x.JobDescription.ToUpper().Contains(SearchString.ToUpper()) || x.Status.ToUpper().Contains(SearchString)
                            || x.EmployeeID.ToString().Contains(SearchString.ToUpper())
                            )).ToList();

                }
                else
                    model = db.ViewEmpPreJobHistories.OrderByDescending(x => x.ExperienceID).Where(x => x.StatusID == pendingValue && x.Deleted == false).ToList();
                modals.Add(model);
                ViewBag.NotificationType = Convert.ToInt16(Utilities.NotificationType.Pre_Job_History);
                int pageSize = 12;
                int pageNumber = (page ?? 1);
                return View("PendingPreJobHistory", model.ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetPendingAppreciation(string sortOrder, string SearchString, string currentFilter, int? page)
        {
            try
            {
                Session["SelectedMenu"] = "HRAppreciation";
                List<object> modals = new List<object>();
                List<ViewEmpAppreciation> model;
                int pendingValue = Convert.ToInt16(Utilities.ProfileStatus.Pending);
                if (page == null)
                    page = 1;

                if (!string.IsNullOrEmpty(SearchString) && SearchString.Length > 0)
                {
                    SearchString = SearchString.Trim();
                    int result;
                    if (int.TryParse(SearchString, out result))
                    {
                        model = db.ViewEmpAppreciations.OrderByDescending(x => x.AppreciationID).
                        Where(x => x.StatusID == pendingValue && x.Deleted == false && x.EmployeeID == result).ToList();
                    }
                    else
                        model = db.ViewEmpAppreciations.OrderByDescending(x => x.AppreciationID).
                            Where(x => x.StatusID == pendingValue && x.Deleted == false && (x.FullName.ToUpper().Contains(SearchString.ToUpper())
                            || x.FullName.ToUpper().Contains(SearchString.ToUpper()) || x.Appreciations_Reason.ToUpper().Contains(SearchString.ToUpper())
                            )).ToList();

                }
                else
                    model = db.ViewEmpAppreciations.OrderByDescending(x => x.AppreciationID).Where(x => x.StatusID == pendingValue && x.Deleted == false).ToList();
                modals.Add(model);
                ViewBag.NotificationType = Convert.ToInt16(Utilities.NotificationType.Appreciation);

                return View("PendingHRAppreciation", model.OrderByDescending(aa => aa.AppreciationID).ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetPendingWarning(string sortOrder, string SearchString, string currentFilter, int? page)
        {
            try
            {
                Session["SelectedMenu"] = "HRWarning";
                List<object> modals = new List<object>();
                List<ViewEmpWarning> model;
                int pendingValue = Convert.ToInt16(Utilities.ProfileStatus.Pending);
                if (page == null)
                    page = 1;

                if (!string.IsNullOrEmpty(SearchString) && SearchString.Length > 0)
                {
                    SearchString = SearchString.Trim();
                    int result;
                    if (int.TryParse(SearchString, out result))
                    {
                        model = db.ViewEmpWarnings.OrderByDescending(x => x.WarningID).
                        Where(x => x.StatusID == pendingValue && x.Deleted == false && x.EmployeeID == result).ToList();
                    }
                    else
                        model = db.ViewEmpWarnings.OrderByDescending(x => x.WarningID).
                            Where(x => x.StatusID == pendingValue && x.Deleted == false && (x.FullName.ToUpper().Contains(SearchString.ToUpper())
                            || x.FullName.ToUpper().Contains(SearchString.ToUpper()) || x.Warnings___Explations_Reason.ToUpper().Contains(SearchString.ToUpper())
                            || x.Warnings___Explations_From.ToUpper().Contains(SearchString.ToUpper())
                            )).ToList();

                }
                else
                    model = db.ViewEmpWarnings.OrderByDescending(x => x.WarningID).Where(x => x.StatusID == pendingValue && x.Deleted == false).ToList();
                modals.Add(model);
                ViewBag.NotificationType = Convert.ToInt16(Utilities.NotificationType.Warning);
                return View("PendingHRWarning", model.OrderByDescending(aa => aa.WarningID).ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetPendingTraining(string sortOrder, string SearchString, string currentFilter, int? page)
        {
            try
            {
                Session["SelectedMenu"] = "HRTraining";
                List<object> modals = new List<object>();
                List<ViewEmpTraining> model;
                int pendingValue = Convert.ToInt16(Utilities.ProfileStatus.Pending);
                if (page == null)
                    page = 1;

                if (!string.IsNullOrEmpty(SearchString) && SearchString.Length > 0)
                {
                    SearchString = SearchString.Trim();
                    int result;
                    if (int.TryParse(SearchString, out result))
                    {
                        model = db.ViewEmpTrainings.OrderByDescending(x => x.TrainingID).
                        Where(x => x.StatusID == pendingValue && x.Deleted == false && x.EmployeeID == result).ToList();
                    }
                    else
                        model = db.ViewEmpTrainings.OrderByDescending(x => x.TrainingID).
                            Where(x => x.StatusID == pendingValue && x.Deleted == false && (x.FullName.ToUpper().Contains(SearchString.ToUpper())
                            || x.FullName.ToUpper().Contains(SearchString.ToUpper()) || x.Training_Course.ToUpper().Contains(SearchString.ToUpper())
                            || x.Training_School_Institution.ToUpper().Contains(SearchString.ToUpper())
                            )).ToList();

                }
                else
                    model = db.ViewEmpTrainings.OrderByDescending(x => x.TrainingID).Where(x => x.StatusID == pendingValue && x.Deleted == false).ToList();
                modals.Add(model);
                ViewBag.NotificationType = Convert.ToInt16(Utilities.NotificationType.Training);

                return View("PendingHRTraining", model.OrderByDescending(aa => aa.TrainingID).ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetPendingDependent(string sortOrder, string SearchString, string currentFilter, int? page)
        {
            try
            {
                List<object> modals = new List<object>();
                //int pendingValue = Convert.ToInt16(Utilities.ProfileStatus.Pending);
                //List<ViewEmpDependent> model = db.ViewEmpDependents.OrderByDescending(x => x.DependentID).Where(x => x.StatusID == pendingValue).ToList();
                //modals.Add(model);
                //ViewBag.NotificationType = Convert.ToInt16(Utilities.NotificationType.Dependent);
                //return View("PendingDependents", modals);

                List<ViewEmpDependent> model;
                int pendingValue = Convert.ToInt16(Utilities.ProfileStatus.Pending);
                if (page == null)
                    page = 1;

                if (!string.IsNullOrEmpty(SearchString) && SearchString.Length > 0)
                {
                    SearchString = SearchString.Trim();
                    int result;
                    if (int.TryParse(SearchString, out result))
                    {
                        model = db.ViewEmpDependents.OrderByDescending(x => x.DependentID).
                        Where(x => x.StatusID == pendingValue && x.Deleted == false && x.EmployeeID == result).ToList();
                    }
                    else
                        model = db.ViewEmpDependents.OrderByDescending(x => x.DependentID).
                            Where(x => x.StatusID == pendingValue && x.Deleted == false && (x.FullName.ToUpper().Contains(SearchString.ToUpper())
                            || x.Name.ToUpper().Contains(SearchString.ToUpper()) || x.Status.ToUpper().Contains(SearchString.ToUpper())
                            || x.Relationship.ToUpper().Contains(SearchString.ToUpper()) || x.EmployeeID.ToString().Contains(SearchString)
                            )).ToList();

                }
                else
                    model = db.ViewEmpDependents.OrderByDescending(x => x.DependentID).Where(x => x.StatusID == pendingValue && x.Deleted == false).ToList();
                modals.Add(model);
                ViewBag.NotificationType = Convert.ToInt16(Utilities.NotificationType.Dependent);
                int pageSize = 12;
                int pageNumber = (page ?? 1);
                return View("PendingDependents", model.OrderBy(aa => aa.Status).ThenBy(aa => aa.FullName).ToPagedList(pageNumber, pageSize));
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public ActionResult GetQualifications(string sortOrder, string SearchString, string currentFilter, int? page)
        {
            try
            {
                Session["SelectedMenu"] = "HRQualifications";
                List<object> modals = new List<object>();
                List<ViewEmpQualification> model = new List<ViewEmpQualification>();
                int pendingValue = Convert.ToInt16(Utilities.ProfileStatus.Pending);
                if (page == null)
                    page = 1;

                if (!string.IsNullOrEmpty(SearchString) && SearchString.Length > 0)
                {
                    SearchString = SearchString.Trim();
                    //int result;
                    //if (int.TryParse(SearchString, out result))
                    //{
                    //    model = db.ViewEmpQualifications.OrderByDescending(x => x.QualificationID).
                    //    Where(x => x.Deleted == false && x.EmployeeID == result).ToList();
                    //}
                    //else
                    //model = db.ViewEmpQualifications.OrderByDescending(x => x.QualificationID).
                    //    Where(x => x.Deleted == false && (x.FullName.ToUpper().Contains(SearchString.ToUpper())
                    //    || x.FullName.ToUpper().Contains(SearchString.ToUpper()) || x.Degree.ToUpper().Contains(SearchString.ToUpper())
                    //    || x.Institute.ToUpper().Contains(SearchString.ToUpper()) || x.Specialization.ToUpper().Contains(SearchString.ToUpper())
                    //    || x.Grade.ToUpper().Contains(SearchString.ToUpper()) || x.Status.ToUpper().Contains(SearchString.ToUpper())
                    //    || x.EmployeeID.ToString().Contains(SearchString)
                    //    )).ToList();
                    
                        model = db.ViewEmpQualifications.OrderByDescending(x => x.QualificationID).
                            Where(x => x.QualificationID > 0 && (x.Degree.ToUpper().Equals(SearchString.ToUpper())
                            )).ToList();
                    
                    ViewBag.SearchData =  SearchString;
                    
                }
                else
                    model = db.ViewEmpQualifications.OrderBy(x => x.EmployeeID).Where(x => x.EmployeeID > 0).ToList();
                modals.Add(model);
                ViewBag.NotificationType = Convert.ToInt16(Utilities.NotificationType.Qualification);

                return View("Qualifications", model.OrderBy(aa => aa.Degree).ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }

        

        //public ActionResult LoadData()
        //{
        //    try
        //    {
        //        //var data = db.HR_Emp_Pending_Qualification.OrderBy(x => x.PQualificationID).ToList();
        //        return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public ActionResult SetApprovedStatus(int ID, int Type, string Message, int EmpID)
        {
            try
            {
                int status = Convert.ToInt32(PTAWMS.App_Start.Utilities.ProfileStatus.Approved);
                SubmitComments(ID, Type, EmpID, Message, status);
                Utilities.SetStatus(ID, Type, status);
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                SubmitAuditLog(ID, Type, Convert.ToInt16(AuditManager.AuditOperation.Approved), LoggedInUser.UserID);
                return RedirectToAction("Index", "HR");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult SetRejectedStatus(int ID, int Type, string Message, int EmpID)
        {
            try
            {
                int status = Convert.ToInt32(PTAWMS.App_Start.Utilities.ProfileStatus.Rejected);
                SubmitComments(ID, Type, EmpID, Message, status);
                Utilities.SetStatus(ID, Type, status);
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                SubmitAuditLog(ID, Type, Convert.ToInt16(AuditManager.AuditOperation.Reject), LoggedInUser.UserID);
                return RedirectToAction("Index", "HR");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SetQualificationStatus(int ID, int StatusID, string typeName, int type)
        {
            try
            {
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                HR_Emp_Qualification oldModel = new HR_Emp_Qualification();
                HR_Emp_Qualification pModel = db.HR_Emp_Qualification.FirstOrDefault(x => x.QualificationID == ID);

                db.Entry(pModel).State = System.Data.Entity.EntityState.Modified;
                pModel.StatusID = StatusID;
                pModel.DateModified = DateTime.Now;
                db.SaveChanges();

                Utilities.DeleteNotification(ID, Convert.ToInt16(Utilities.NotificationType.Qualification));
                Utilities.InsertEMPNotification(type, typeName, LoggedInUser.UserID, ID, pModel.EmployeeID, BaseURL+"HumanResource/Profile/EmpDetails/" + pModel.EmployeeID + "?class=qualification");

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SetDependentStatus(int ID, int StatusID, string typeName, int type)
        {
            try
            {
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                HR_Emp_Dependents pModel = db.HR_Emp_Dependents.FirstOrDefault(x => x.DependentID == ID);

                db.Entry(pModel).State = System.Data.Entity.EntityState.Modified;
                pModel.StatusID = StatusID;
                pModel.DateModified = DateTime.Now;
                db.SaveChanges();

                Utilities.DeleteNotification(ID, Convert.ToInt16(Utilities.NotificationType.Dependent));
                Utilities.InsertEMPNotification(type, typeName, LoggedInUser.UserID, ID, pModel.EmployeeID ?? 0, BaseURL+"HumanResource/Profile/EmpDetails/" + pModel.EmployeeID + "?class=dependents");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult HREmpAction(int ID, int TypeID, int EmpID, int StatusID)
        {
            try
            {
                List<ViewEmpCommunication> model = db.ViewEmpCommunications.OrderBy(x => x.CommunicationID).Where(x => x.TypeID == TypeID && x.RecordID == ID).ToList();
                ViewBag.TypeID = TypeID;
                ViewBag.ID = ID;
                ViewBag.EmpID = EmpID;
                ViewBag.StatusID = StatusID;
                ViewBag.PersonName = db.HR_Employee.FirstOrDefault(x => x.EmployeeID == EmpID).FullName;
                ViewBag.Area = Enum.GetName(typeof(Utilities.NotificationType), TypeID).Replace("_", " ");

                ViewBag.Values = GetValues(ID, TypeID);
                return View("HREmpCommunication", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private StringBuilder GetValues(int ID, int TypeID)
        {
            try
            {
                StringBuilder values = new StringBuilder();
                if (TypeID == Convert.ToUInt16(Utilities.NotificationType.Qualification))
                {
                    ViewEmpQualification entity = db.ViewEmpQualifications.FirstOrDefault(x => x.QualificationID == ID);
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>Employee Name</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.FullName + "</span></div>");
                    values.Append("<div class='profile-info-name'>Degree</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.Degree + "</span></div></div>");
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>Institute</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.Institute + "</span></div>");
                    values.Append("<div class='profile-info-name'>Specialization</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.Specialization + "</span></div></div>");
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>From</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.StartSession + "</span></div>");
                    values.Append("<div class='profile-info-name'>To</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.EndSession + "</span></div></div>");
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>Grade</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.Grade + "</span></div>");
                    values.Append("<div class='profile-info-name'>Attachment</div>");
                    ViewBag.filePath = entity.DocumentPath;
                }
                else if (TypeID == Convert.ToUInt16(Utilities.NotificationType.Dependent))
                {
                    ViewEmpDependent entity = db.ViewEmpDependents.FirstOrDefault(x => x.DependentID == ID);
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>Employee Name</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.FullName + "</span></div>");
                    values.Append("<div class='profile-info-name'>Relative Name</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.Name + "</span></div></div>");
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>Relationship</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.Relationship + "</span></div>");
                    values.Append("<div class='profile-info-name'>Medical Facility</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + (entity.MedicalFacilityAllowed ? "Yes" : "No") + "</span></div></div>");
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>Provident Fund</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + (entity.ProvidentFundAllowed ? "Yes" : "No") + "</span></div>");
                    values.Append("<div class='profile-info-name'>Benevolent Fund </div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + (entity.BenevolentFundAllowed ? "Yes" : "No") + "</span></div></div>");
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>Graduity</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + (entity.Graduity ? "Yes" : "No") + "</span></div>");
                    values.Append("<div class='profile-info-name'>Death Compensation</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + (entity.DeathCompensation ? "Yes" : "No") + "</span></div></div>");
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>CPF</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + (entity.CTF ? "Yes" : "No") + "</span></div>");

                    values.Append("<div class='profile-info-name'>Attachment</div>");
                    ViewBag.filePath = entity.DocumentPath;
                }
                else if (TypeID == Convert.ToUInt16(Utilities.NotificationType.Pre_Job_History))
                {
                    ViewEmpPreJobHistory entity = db.ViewEmpPreJobHistories.FirstOrDefault(x => x.ExperienceID == ID);
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>Employee Name</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.FullName + "</span></div>");
                    values.Append("<div class='profile-info-name'>Organization Name</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.OrganisationName + "</span></div></div>");
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>Address</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.OrgAddress + "</span></div>");
                    values.Append("<div class='profile-info-name'>Contact No.</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.OrgContactNumber + "</span></div></div>");
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>Department</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.Department + "</span></div>");
                    values.Append("<div class='profile-info-name'>Designation</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.Designation + "</span></div></div>");
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>From</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.FromDate.Value.ToShortDateString() + "</span></div>");
                    values.Append("<div class='profile-info-name'>To</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.ToDate.Value.ToShortDateString() + "</span></div></div>");
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>Description</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.JobDescription + "</span></div>");

                    values.Append("<div class='profile-info-name'>Attachment</div>");
                    ViewBag.filePath = entity.ExperienceLetterPath;
                }
                else if (TypeID == Convert.ToUInt16(Utilities.NotificationType.Appreciation))
                {
                    ViewEmpAppreciation entity = db.ViewEmpAppreciations.FirstOrDefault(x => x.AppreciationID == ID);
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>Employee Name</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.FullName + "</span></div>");
                    values.Append("<div class='profile-info-name'>Date</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.Appreciations_Date.Value.ToShortDateString() + "</span></div></div>");
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>Reason</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.Appreciations_Reason + "</span></div>");
                    values.Append("<div class='profile-info-name'>From</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.Appreciations_From + "</span></div></div>");

                    values.Append("<div class='profile-info-name'>Attachment</div>");
                    ViewBag.filePath = entity.DocumentPath;
                }
                else if (TypeID == Convert.ToUInt16(Utilities.NotificationType.Warning))
                {
                    ViewEmpWarning entity = db.ViewEmpWarnings.FirstOrDefault(x => x.WarningID == ID);
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>Employee Name</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.FullName + "</span></div>");
                    values.Append("<div class='profile-info-name'>Date</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.Warnings___Explations_Date.Value.ToShortDateString() + "</span></div></div>");
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>Detail</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.Warnings___Explations_Reason + "</span></div>");
                    values.Append("<div class='profile-info-name'>From</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.Warnings___Explations_From + "</span></div></div>");
                    values.Append("<div class='profile-info-name'>Attachment</div>");
                    ViewBag.filePath = entity.DocumentPath;
                }
                else if (TypeID == Convert.ToUInt16(Utilities.NotificationType.Training))
                {
                    ViewEmpTraining entity = db.ViewEmpTrainings.FirstOrDefault(x => x.TrainingID == ID);
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>Employee Name</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.FullName + "</span></div>");
                    values.Append("<div class='profile-info-name'>Year</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.Year_ + "</span></div></div>");
                    values.Append("<div class='profile-info-row'><div class='profile-info-name'>Course</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.Training_Course + "</span></div>");
                    values.Append("<div class='profile-info-name'>Institute</div>");
                    values.Append("<div class='profile-info-value'><span class='editable' id='username'>" + entity.Training_School_Institution + "</span></div></div>");

                    values.Append("<div class='profile-info-name'>Attachment</div>");
                    ViewBag.filePath = entity.DocumentPath;
                }

                return values;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool SubmitComments(int ID, int TypeID, int EmpID, string Comments, int StatusID)
        {
            try
            {
                using (HRMEntities db = new HRMEntities())
                {
                    HR_Emp_Communication entity = new HR_Emp_Communication();
                    ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                    Session["EMP_ID"] = EmpID;
                    try
                    {
                        entity.EmployeeID = EmpID;
                        entity.TypeID = TypeID;
                        entity.Comment = Comments;
                        entity.StatusID = StatusID;
                        entity.RecordID = ID;
                        entity.ActionBy = LoggedInUser.EmpID ?? 0;
                        entity.Active = true;
                        entity.DateCreated = DateTime.Now;
                        db.HR_Emp_Communication.Add(entity);
                        Utilities.SetStatus(ID, TypeID, Convert.ToUInt16(Utilities.ProfileStatus.Query_by_HR));
                        db.SaveChanges();
                        string TypeName = Enum.GetName(typeof(Utilities.NotificationType), TypeID);
                        Utilities.DeleteNotification(ID, TypeID);
                        if (EmpID != LoggedInUser.EmpID)
                        {
                            TypeName = TypeName.Replace("_", "");
                            Utilities.InsertEMPNotification(TypeID, TypeName, LoggedInUser.UserID, ID, EmpID, BaseURL+"HumanResource/Profile/EmpDetails/" + EmpID + "?" + TypeName);
                        }
                        else
                            Utilities.InsertHRNotification("HRModule", TypeID, TypeName, LoggedInUser.UserID, ID, BaseURL+"HumanResource/HR");
                        SubmitAuditLog(ID, TypeID, Convert.ToInt16(AuditManager.AuditOperation.Comment), LoggedInUser.UserID);

                    }
                    catch (Exception)
                    {

                        db.Dispose();
                    }
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool SubmitAuditLog(int ID, int TypeID, short OperationID, int UserID)
        {
            try
            {

                if (TypeID == Convert.ToInt16(Utilities.NotificationType.Pre_Job_History))
                {
                    AuditManager.SaveAuditLog(UserID, Convert.ToInt16(AuditManager.AuditForm.Pre_Job_History), OperationID, DateTime.Now, ID);
                }
                else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Appreciation))
                {
                    AuditManager.SaveAuditLog(UserID, Convert.ToInt16(AuditManager.AuditForm.Pre_Job_History), OperationID, DateTime.Now, ID);
                }
                else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Warning))
                {
                    AuditManager.SaveAuditLog(UserID, Convert.ToInt16(AuditManager.AuditForm.Pre_Job_History), OperationID, DateTime.Now, ID);
                }
                else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Training))
                {
                    AuditManager.SaveAuditLog(UserID, Convert.ToInt16(AuditManager.AuditForm.Pre_Job_History), OperationID, DateTime.Now, ID);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult EmpInstitutes()
        {
            try
            {
                Session["SelectedMenu"] = "HRInstitute";
                var empInstitute = from emp in db.ViewEmpQualifications.OrderBy(x => x.Institute).Where(x => x.Active == true && x.Deleted == false)
                                   group emp by emp.Degree into empg
                                   select new InstituteList
                                   {
                                       InstituteName = empg.Key,
                                       NoOfInst = empg.Count(x => x.Institute != null)
                                   };

                List<InstituteList> list = empInstitute.ToList<InstituteList>();
                return View("EmpInstitutes", list);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult EmpDegree()
        {
            try
            {
                Session["SelectedMenu"] = "HRDegree";
                var empInstitute = from emp in db.ViewEmpQualifications.OrderBy(x => x.Institute).Where(x => x.EmployeeID > 0)
                                   group emp by emp.Degree into empg
                                   select new ValueList
                                   {
                                       Name = empg.Key,
                                       Total = empg.Count(x => x.Degree != null)
                                   };

                List<ValueList> list = empInstitute.ToList<ValueList>();
                return View("EmpDegree", list);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}