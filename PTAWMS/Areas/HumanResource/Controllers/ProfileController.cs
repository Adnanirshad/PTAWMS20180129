using HRM_IKAN.Authentication;
using PTAWMS.App_Start;
using PTAWMS.Areas.HumanResource.BusinessLogic;
using PTAWMS.Areas.HumanResource.Models;
using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace PTAWMS.Areas.HumanResource.Controllers
{
    [CustomControllerAttributes]
    public class ProfileController : Controller
    {
        private HRMEntities db = new HRMEntities();
        private short auditOperation = Convert.ToInt16(AuditManager.AuditOperation.Add);
        private short auditForm;
        private static string BaseURL
        {
            get { return ConfigurationManager.AppSettings["BaseURL"].ToString(); }
        }

        // GET: HumanResource/Profile
        public ActionResult Index(int id)
        {
            ModelEmpProfileIndex memp = new ModelEmpProfileIndex();
            HR_Employee emp = db.HR_Employee.First(aa => aa.EmployeeID == id);
            memp.EmpID = emp.EmployeeID;
            memp.FullName = emp.FullName;
            memp.EmpNo = emp.EmpNo;
            return View(memp);
        }

        [HttpGet]
        public ActionResult EmpDetails(int id)
        {
            try
            {
                ModelEmpProfileIndex memp = new ModelEmpProfileIndex();
                HR_Employee emp = db.HR_Employee.First(aa => aa.EmployeeID == id);
                memp.EmpID = emp.EmployeeID;
                memp.FullName = emp.FullName;
                memp.EmpNo = emp.EmpNo;
                Session["EMP_ID"] = memp.EmpID;
                return View(memp);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public ActionResult EmpPersonalDetails(int ID)
        {
            try
            {
                HR_Employee hr_employee = db.HR_Employee.First(aa => aa.EmployeeID == ID);
                ViewBag.BloodGroup = new SelectList(db.HR_EmpBloodGroup.OrderBy(aa => aa.BloodGroupName).ToList(), "BloodGroupName", "BloodGroupName", hr_employee.BloodGroup);
                ViewBag.Gender = new SelectList(db.HR_EmpGender.OrderBy(aa => aa.GenderName).ToList(), "GenderName", "GenderName", hr_employee.Gender);
                //ViewBag.Nationality = new SelectList(db.HR_EmpNationality.OrderBy(aa => aa.NationalityName).ToList(), "NationalityName", "NationalityName", hr_employee.Nationality);
                //ViewBag.Religion = new SelectList(db.HR_EmpReligion.OrderBy(aa => aa.ReligionName).ToList(), "ReligionName", "ReligionName", hr_employee.Religion);
                ViewBag.MaritalStatus = new SelectList(db.HR_MartialStatus.OrderBy(aa => aa.MartialStatusName).ToList(), "MartialStatusName", "MartialStatusName", hr_employee.MaritalStatus);

                ModelEmpPersonal meJD = new ModelEmpPersonal();
                ViewBag.Nationality = new SelectList("-- Select --", "NationalityName", "NationalityName", meJD.Nationality);
                meJD.EmpID = hr_employee.EmployeeID;
                meJD.FirstName = hr_employee.FirstName;
                meJD.MiddleName = hr_employee.MiddleName;
                meJD.LastName = hr_employee.LastName;
                meJD.FullName = hr_employee.FullName;
                //meJD.OldIKNo = hr_employee.OldIKNo;
                meJD.FathersName = hr_employee.FathersName;
                // meJD.MothersName = hr_employee.MothersName;
                meJD.CNICNo = hr_employee.CNICNo;
                meJD.MaritalStatus = hr_employee.MaritalStatus;
                meJD.PresentAddress = hr_employee.Address;
                meJD.PermanentAddress = hr_employee.Address2;
                meJD.BloodGroup = hr_employee.BloodGroup;
                meJD.ReportTo = db.HR_Employee.FirstOrDefault(x => x.EmployeeID == hr_employee.ReportingToID).FullName;
                meJD.Category = db.HR_EmpType.FirstOrDefault(x => x.TypID == hr_employee.EmpTypeID).TypeName;
                meJD.Designation = db.HR_Designation.FirstOrDefault(x => x.DesgID == hr_employee.DesignationID).DesignationName;
                meJD.Grade = hr_employee.GrdName + " " + hr_employee.ScaleName;
                meJD.EmergencyContactNo = hr_employee.EmergencyContactNo;
                meJD.HomeLandLineNo = hr_employee.LandLine;
                meJD.OfficeLandLine = hr_employee.OfficialContactNo;
                meJD.MobileNo = hr_employee.MobileNo;
                meJD.Gender = hr_employee.Gender;
                meJD.OfficeCardNo = hr_employee.CardNo;
                meJD.Section = db.HR_Section.FirstOrDefault(x => x.SecID == hr_employee.SectionID).SectionName;
                meJD.OverTimePolicy = hr_employee.OTPolicyID != null ? db.Att_OTPolicy.FirstOrDefault(x => x.OTPolicyID == hr_employee.OTPolicyID).OTPolicyName : "";
                HR_Employee hr_ReportTo = db.HR_Employee.FirstOrDefault(x => x.EmployeeID == hr_employee.ReportingToID);
                meJD.ReportTo = hr_ReportTo.FullName + " (" + hr_ReportTo.Status + ")";
                meJD.RoomNo = hr_employee.RoomNo;
                meJD.ExtNo = hr_employee.ExtensionNo;
                meJD.DomicileProvince = hr_employee.DomicileProvince;
                meJD.DomicileCity = hr_employee.DomicileCity;
                meJD.Location = hr_employee.HR_Location.LocationName;

                if (hr_employee.LeavingDate != null && hr_employee.LeavingDate > DateTime.MinValue)
                    meJD.RetirementDate = hr_employee.LeavingDate.Value;
                if (hr_employee.DOB != null)
                    meJD.DOB = hr_employee.DOB.Value;
                if (hr_employee.DOJ != null)
                {

                    meJD.JoiningDate = hr_employee.DOJ.Value;
                }
                if (hr_employee.StationJoinDate != null)
                {
                    meJD.AppointmentDate = hr_employee.StationJoinDate.Value;
                }
                //if (hr_employee.CNICExpireDate != null)
                //    meJD.CNICExpireDate = hr_employee.CNICExpireDate.Value;
                //meJD.PECNo = hr_employee.PECNo;
                meJD.Email = hr_employee.EmailID;
                int status = Convert.ToInt16(Utilities.ProfileStatus.Approved);
                ViewBag.TotalQualification = db.HR_Emp_Qualification.Count(x => x.EmployeeID == hr_employee.EmployeeID && x.StatusID == status && x.Deleted == false);
                ViewBag.TotalDependent = db.HR_Emp_Dependents.Count(x => x.EmployeeID == hr_employee.EmployeeID && x.StatusID == status && x.Deleted == false);
                return View("EmpPersonalDetail", meJD);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult EMPContactInfo(int ID)
        {

            try
            {
                HR_Employee model = db.HR_Employee.FirstOrDefault(x => x.EmployeeID == ID);
                return PartialView("AddEmpInfo", model);


            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddContactInfo(HR_Employee model)
        {

            try
            {
                HR_Employee entity = db.HR_Employee.FirstOrDefault(x => x.EmployeeID == model.EmployeeID);
                if (ModelState.IsValid)
                {
                    entity.OfficialContactNo = model.OfficialContactNo;
                    entity.MobileNo = model.MobileNo;
                    entity.EmailID = model.EmailID;
                    entity.RoomNo = model.RoomNo;
                    entity.ExtensionNo = model.ExtensionNo;
                    db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("EmpDetails", new { ID = model.EmployeeID });

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult EMPQualification(int ID)
        {

            try
            {

                int pendingStatus = Convert.ToInt16(Utilities.ProfileStatus.Query_by_HR);
                int approvedStatus = Convert.ToInt16(Utilities.ProfileStatus.Approved);
                List<object> models = new List<object>();
                //List<ViewEmpQualification> q_model = db.ViewEmpQualifications.OrderByDescending(x => x.QualificationID).Where(x => x.EmployeeID == ID).ToList();
                List<ViewEmpQualification> q_model = db.ViewEmpQualifications.OrderByDescending(x => x.QualificationID).Where(x => x.EmployeeID == ID).ToList();
                models.Add(q_model);
                //List<ViewEmpQualification> p_model = db.ViewEmpQualifications.OrderByDescending(x => x.QualificationID).Where(x => x.EmployeeID == ID && x.Deleted == false && x.StatusID != approvedStatus).ToList();
                //models.Add(p_model);

                ViewBag.TypeID = Convert.ToInt16(Utilities.NotificationType.Qualification);
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;

                if (ID == LoggedInUser.EmployeeID)
                    Utilities.DeleteNotificationByType(ID, Convert.ToInt16(Utilities.NotificationType.Qualification));

                return PartialView("EMPQualification", models);


            }
            catch (Exception)
            {

               throw;
            }
        }

        [HttpGet]
        public ActionResult ModifyQualification(int ID)
        {

            try
            {

                ModelEmpQualification qualification = new ModelEmpQualification();
                var degree = Convert.ToInt32(PTAWMS.App_Start.Utilities.HR_ListType.DegreeName);
                qualification.DegreesList = new SelectList(db.HR_ListEntry.Where(x => x.intListTypeId == degree), "intListEntryId", "vchListEntryName");
                var institute = Convert.ToInt32(PTAWMS.App_Start.Utilities.HR_ListType.InstituteName);
                qualification.InstituteList = new SelectList(db.HR_ListEntry.Where(x => x.intListTypeId == institute), "intListEntryId", "vchListEntryName");


                if (ID != null)
                {
                    HR_Emp_Qualification qualif = db.HR_Emp_Qualification.FirstOrDefault(x => x.QualificationID == ID);
                    if (qualif != null)
                    {
                        qualification.ID = qualif.QualificationID;
                        qualification.Specialization = qualif.Specialization;
                        qualification.DegreeName = qualif.Degree;
                        qualification.Institute = qualif.Institute;
                        qualification.Grade = qualif.Grade;
                        qualification.StartDate = !String.IsNullOrEmpty(qualif.StartSession) ? Convert.ToDateTime("1/" + qualif.StartSession) : DateTime.MinValue;
                        qualification.EndDate = Convert.ToDateTime("1/" + qualif.EndSession);
                        qualification.StartDate.ToString("dd/MM/yy");
                        qualification.EndDate.ToString("dd/MM/yy");
                        //if (!qualification.InstituteList.Items.Equals(qualif.Institute.ToString()))
                        //    qualification.InstituteList = new SelectListItem(qualif.Institute.ToString());


                    }
                }


                return PartialView("CreateQualification", qualification);


            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult NewQualification()
        {

            try
            {

                ModelEmpQualification qualification = new ModelEmpQualification();
                var degree = Convert.ToInt32(PTAWMS.App_Start.Utilities.HR_ListType.DegreeName);
                qualification.DegreesList = new SelectList(db.HR_ListEntry.Where(x => x.intListTypeId == degree), "intListEntryId", "vchListEntryName");
                var institute = Convert.ToInt32(PTAWMS.App_Start.Utilities.HR_ListType.InstituteName);
                qualification.InstituteList = new SelectList(db.HR_ListEntry.Where(x => x.intListTypeId == institute), "intListEntryId", "vchListEntryName");
                //qualification.StartDate = DateTime.Now.Date;
                //qualification.EndDate = DateTime.Now.Date;


                return PartialView("CreateQualification", qualification);


            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQualification(ModelEmpQualification model)
        {

            try
            {
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                Session["EMP_ID"] = LoggedInUser.EmpID;
                
                HR_Emp_Qualification q_model = db.HR_Emp_Qualification.FirstOrDefault(x => x.QualificationID == model.ID) ?? new HR_Emp_Qualification();
                if (ModelState.IsValid)
                {
                    int empID = Session["EMP_ID"] != null ? Convert.ToInt32(Session["EMP_ID"]) : 0;
                    string path = string.Empty;
                    foreach (string file in Request.Files)
                    {
                        HttpPostedFileBase hpf = Request.Files[file];
                        if (Request.Files[file].ContentLength > 0)
                        {
                            path = SaveDocumentFile(hpf, empID);
                            path = path.Replace(@"\", @"/");
                        }
                    }
                    if (path != string.Empty)
                        q_model.DocumentPath = path;

                    q_model.Grade = model.Grade;
                    q_model.Specialization = model.Specialization;
                    q_model.StartSession = model.StartDate.ToString("MM/yyyy");
                    q_model.EndSession = model.EndDate.ToString("MM/yyyy");
                    q_model.DateCreated = DateTime.Now;
                    q_model.EmployeeID = empID;
                    q_model.DocumentPath = path;
                    if (LoggedInUser.HRRequest == true && q_model.QualificationID == 0)
                        q_model.StatusID = Convert.ToInt32(Utilities.ProfileStatus.Approved);
                    else
                        q_model.StatusID = Convert.ToInt32(Utilities.ProfileStatus.Pending);
                    q_model.Active = true;
                    if (q_model.QualificationID > 0)
                        db.Entry(q_model).State = System.Data.Entity.EntityState.Modified;
                    else
                        db.HR_Emp_Qualification.Add(q_model);

                    db.SaveChanges();
                    int id = q_model.QualificationID;
                    int TypeID = Convert.ToInt16(Utilities.NotificationType.Qualification);
                    string TypeName = Utilities.NotificationType.Qualification.ToString();
                    if (LoggedInUser.HRRequest == true && q_model.QualificationID > 0)
                        Utilities.InsertEMPNotification(TypeID, TypeName, LoggedInUser.UserID, id, empID, BaseURL+"HumanResource/Profile/EmpDetails/" + empID + "?" + TypeName);
                    else if (LoggedInUser.HRRequest != true)
                        Utilities.InsertHRNotification("HRModule", TypeID, TypeName, empID, id, BaseURL+"HumanResource/HR/Index");

                    

                }

                return RedirectToAction("EmpDetails", new { ID = q_model.EmployeeID, @class = "qualification" });

            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<ModelEmpQualification> GetEmpQualification(int ID)
        {
            List<ModelEmpQualification> lstQulif = new List<ModelEmpQualification>();
            List<HR_Emp_Qualification> q_model = new List<HR_Emp_Qualification>();
            var result = db.HR_Emp_Qualification.Where(x => x.EmployeeID == ID);

            foreach (var qualif in result)
            {
                if (qualif != null)
                {
                    ModelEmpQualification qualification = new ModelEmpQualification();
                    qualification.DegreeID = 0;
                    qualification.DegreeName = db.HR_ListEntry.FirstOrDefault(x => x.intListEntryId == 0).vchEntryDescription;
                    qualification.InstituteID = 0;
                    qualification.Institute = db.HR_ListEntry.FirstOrDefault(x => x.intListEntryId == 0).vchEntryDescription;
                    qualification.Specialization = qualif.Specialization;
                    qualification.SessionStart = qualif.StartSession;
                    qualification.SessionEnd = qualif.EndSession;
                    qualification.Grade = qualif.Grade;
                    qualification.ID = qualif.QualificationID;
                    lstQulif.Add(qualification);
                }
            }
            return lstQulif;
        }

        [HttpGet]
        public ActionResult EMPDependents(int? ID)
        {

            try
            {
                int pendingStatus = Convert.ToInt16(Utilities.ProfileStatus.Pending);
                int approvedStatus = Convert.ToInt16(Utilities.ProfileStatus.Approved);
                List<object> models = new List<object>(); ;
                List<ViewEmpDependent> aModelList = db.ViewEmpDependents.OrderByDescending(x => x.DependentID).Where(x => x.EmployeeID == ID).ToList();
                models.Add(aModelList);
                //List<ViewEmpDependent> pModelList = db.ViewEmpDependents.OrderByDescending(x => x.DependentID).Where(x => x.EmployeeID == ID && x.StatusID != approvedStatus && x.EmployeeID == ID).ToList();
                //models.Add(pModelList);
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                ViewBag.TypeID = Convert.ToInt16(Utilities.NotificationType.Dependent);
                int empID = Session["EMP_ID"] != null ? Convert.ToInt32(Session["EMP_ID"]) : 0;
                if (empID == LoggedInUser.EmployeeID)
                    Utilities.DeleteNotificationByType(empID, Convert.ToInt16(Utilities.NotificationType.Dependent));
                return PartialView("EmpDependents", models);


            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult ModifyDependent(int? ID)
        {

            try
            {
                int approvedStatus = Convert.ToInt16(Utilities.ProfileStatus.Approved);
                int relationship = Convert.ToInt16(Utilities.HR_ListType.Relationship);
                ModelEmpDependents model = new ModelEmpDependents();

                var institute = Convert.ToInt32(PTAWMS.App_Start.Utilities.HR_ListType.InstituteName);
                model.RelationshipList = new SelectList(db.HR_ListEntry.OrderBy(x => x.vchListEntryName).Where(x => x.intListTypeId == relationship), "intListEntryId", "vchListEntryName");

                HR_Emp_Dependents entity = db.HR_Emp_Dependents.FirstOrDefault(x => x.DependentID == ID);
                if (entity != null && entity.DependentID > 0)
                {
                    model.ID = entity.DependentID;
                    model.Name = entity.Name;
                    //model.RelationshipID = entity.RelationshipID ?? 0;
                    model.ProvidentFund = entity.ProvidentFundAllowed;
                    model.NominationsBenevolentFund = entity.BenevolentFundAllowed;
                    model.Graduity = entity.Graduity;
                    model.CPF = entity.CTF;
                    model.DeathCompensation = entity.DeathCompensation;
                    model.MedicalFacilityAllowed = entity.MedicalFacilityAllowed;


                }



                ViewBag.Allowctx = true;
                ViewBag.AllowBenevolentFund = true;
                ViewBag.AllowProvidentFund = true;
                ViewBag.AllowGraduity = true;
                ViewBag.AllowDeathCompensation = true;
                int emp_id = Convert.ToInt32(Session["EMP_ID"]);
                var result = db.HR_Emp_Dependents.Where(x => x.EmployeeID == emp_id &&
                (x.CTF == true || x.DeathCompensation == true || x.BenevolentFundAllowed == true
                || x.ProvidentFundAllowed == true || x.Graduity == true));

                if (result.Any(x => x.Graduity == true && x.DependentID != ID))
                    ViewBag.AllowGraduity = false;
                if (result.Any(x => x.BenevolentFundAllowed == true && x.DependentID != ID))
                    ViewBag.AllowBenevolentFund = false;
                if (result.Any(x => x.ProvidentFundAllowed == true && x.DependentID != ID))
                    ViewBag.AllowProvidentFund = false;
                if (result.Any(x => x.CTF == true && x.DependentID != ID))
                    ViewBag.Allowctx = false;
                if (result.Any(x => x.DeathCompensation == true && x.DependentID != ID))
                    ViewBag.AllowDeathCompensation = false;

                return PartialView("AddDependents", model);
            }

            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult AddDependent()
        {

            try
            {
                ModelEmpDependents model = new ModelEmpDependents();
                ViewBag.Allowctx = true;
                ViewBag.AllowBenevolentFund = true;
                ViewBag.AllowProvidentFund = true;
                ViewBag.AllowGraduity = true;
                ViewBag.AllowDeathCompensation = true;
                int relationship = Convert.ToInt16(Utilities.HR_ListType.Relationship);
                model.RelationshipList = new SelectList(db.HR_ListEntry.OrderBy(x => x.vchListEntryName).Where(x => x.intListTypeId == relationship), "intListEntryId", "vchListEntryName");


                int emp_id = Convert.ToInt32(Session["EMP_ID"]);
                var result = db.HR_Emp_Dependents.Where(x => x.EmployeeID == emp_id &&
                (x.CTF == true || x.DeathCompensation == true || x.BenevolentFundAllowed == true
                || x.ProvidentFundAllowed == true || x.Graduity == true));

                if (result.Any(x => x.Graduity == true))
                    ViewBag.AllowGraduity = false;
                if (result.Any(x => x.BenevolentFundAllowed == true))
                    ViewBag.AllowBenevolentFund = false;
                if (result.Any(x => x.ProvidentFundAllowed == true))
                    ViewBag.AllowProvidentFund = false;
                if (result.Any(x => x.CTF == true))
                    ViewBag.Allowctx = false;
                if (result.Any(x => x.DeathCompensation == true))
                    ViewBag.AllowDeathCompensation = false;


                return PartialView("AddDependents", model);
            }

            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddExpDependent(ModelEmpDependents model)
        {

            try
            {
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                Session["EMP_ID"] = LoggedInUser.EmpID;
                int empID = Session["EMP_ID"] != null ? Convert.ToInt32(Session["EMP_ID"]) : 0;
                string path = string.Empty;
                HR_Emp_Dependents entity = db.HR_Emp_Dependents.FirstOrDefault(x => x.DependentID == model.ID) ?? new HR_Emp_Dependents();
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpf = Request.Files[file];
                    if (Request.Files[file].ContentLength > 0)
                    {
                        path = SaveDocumentFile(hpf, empID);
                        path = path.Replace(@"\", @"/");
                    }
                }
                if (path != string.Empty)
                    entity.DocumentPath = path;
                entity.DependentID = model.ID;
                entity.Name = model.Name;
                //entity.RelationshipID = model.RelationshipID;
                entity.MedicalFacilityAllowed = model.MedicalFacilityAllowed;
                entity.BenevolentFundAllowed = model.NominationsBenevolentFund;
                entity.CTF = model.CPF;
                entity.DeathCompensation = model.DeathCompensation;
                entity.ProvidentFundAllowed = model.ProvidentFund;
                entity.DateCreated = DateTime.Now;
                entity.EmployeeID = empID;
                entity.Active = true;
                entity.StatusID = Convert.ToInt32(Utilities.ProfileStatus.Pending);
                if (LoggedInUser.HRRequest == true && entity.DependentID == 0)
                    entity.StatusID = Convert.ToInt32(Utilities.ProfileStatus.Approved);
                else
                    entity.StatusID = Convert.ToInt32(Utilities.ProfileStatus.Pending);

                if (entity.DependentID > 0)
                {
                    db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                }
                else
                    db.HR_Emp_Dependents.Add(entity);

                db.SaveChanges();
                int dept = entity.DependentID;
                int TypeID = Convert.ToInt16(Utilities.NotificationType.Dependent);
                string TypeName = Utilities.NotificationType.Dependent.ToString();
                if (LoggedInUser.HRRequest == true && entity.DependentID > 0)
                    Utilities.InsertEMPNotification(TypeID, TypeName, LoggedInUser.UserID, dept, empID, BaseURL+"HumanResource/Profile/EmpDetails/" + empID + "?" + TypeName);
                else if (LoggedInUser.HRRequest != true)
                    Utilities.InsertHRNotification("HRModule", Convert.ToInt16(Utilities.NotificationType.Dependent), Utilities.NotificationType.Dependent.ToString(), empID, entity.DependentID, BaseURL+"HumanResource/HR/GetPendingDependent");
                
                return RedirectToAction("EmpDetails", new { ID = empID, @class = "dependents" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult EMPExperienceHistory(int ID)
        {

            try
            {
                int pendingStatus = Convert.ToInt16(Utilities.ProfileStatus.Query_by_HR);
                int approvedStatus = Convert.ToInt16(Utilities.ProfileStatus.Approved);
                List<object> models = new List<object>();
                List<ViewEmpPreJobHistory> model = db.ViewEmpPreJobHistories.OrderByDescending(x => x.ExperienceID).Where(x => x.EmployeeID == ID && x.Deleted == false && x.StatusID == approvedStatus).ToList();
                models.Add(model);
                List<ViewEmpPreJobHistory> p_model = db.ViewEmpPreJobHistories.OrderByDescending(x => x.ExperienceID).Where(x => x.EmployeeID == ID && x.Deleted == false && x.StatusID != approvedStatus).ToList();
                models.Add(p_model);
                Session["EMP_ID"] = ID.ToString();


                ViewBag.TypeID = Convert.ToInt16(Utilities.NotificationType.Pre_Job_History);
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                int empID = Session["EMP_ID"] != null ? Convert.ToInt32(Session["EMP_ID"]) : 0;
                if (empID == LoggedInUser.EmployeeID)
                    Utilities.DeleteNotificationByType(empID, Convert.ToInt16(Utilities.NotificationType.Pre_Job_History));
                return PartialView("ExperienceHistory", models);


            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult ModifyExperience(int ID)
        {

            try
            {
                ModelExperienceHistory model = new ModelExperienceHistory();
                List<HR_Emp_Experience> entity = new List<HR_Emp_Experience>();
                Session["EMP_ID"] = ID.ToString();
                if (ID != null)
                {
                    int TypeID = Convert.ToInt16(Utilities.NotificationType.Pre_Job_History);
                    ViewBag.TypeID = TypeID.ToString();
                    entity.Add(db.HR_Emp_Experience.FirstOrDefault(x => x.ExperienceID == ID));

                    foreach (var ent in entity)
                    {
                        if (ent != null)
                        {
                            model.Organisation = ent.OrganisationName;
                            model.OrgAddress = ent.OrgAddress;
                            model.Designation = ent.Designation;
                            model.JobDescription = ent.JobDescription;
                            model.FromDate = ent.FromDate.Value.ToString("dd/MM/yyyy");
                            model.ToDate = ent.ToDate.Value.ToString("dd/MM/yyyy");
                            model.ID = ent.ExperienceID;
                            model.ContactNumber = ent.OrgContactNumber;
                            model.Department = ent.Department;
                            model.ExperiencePath = ent.ExperienceLetterPath;
                            model.ID = ent.ExperienceID;
                            ViewEmpCommunication comm = db.ViewEmpCommunications.OrderByDescending(x => x.CommunicationID).Where(x => x.TypeID == TypeID && x.RecordID == model.ID).FirstOrDefault();
                            if (comm != null)
                            {
                                if (comm.Comment.Length > 120)
                                    ViewBag.Comm = comm.Comment.Substring(1, 120) + " ... (Continue)";
                                else
                                    ViewBag.Comm = comm.Comment;
                            }
                        }
                    }



                }
                return PartialView("AddExperience", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult AddExperience()
        {

            try
            {


                ModelExperienceHistory model = new ModelExperienceHistory();
                List<HR_Emp_Experience> entity = new List<HR_Emp_Experience>();
                model.ToDate = DateTime.Now.ToString("dd/MM/yyyy");
                model.FromDate = DateTime.Now.ToString("dd/MM/yyyy");
                Session["EMP_ID"] = Session["EMP_ID"];
                return PartialView("AddExperience", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddExpHistory(ModelExperienceHistory model)
        {

            try
            {
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                auditOperation = Convert.ToInt16(AuditManager.AuditOperation.Add);
                auditForm = Convert.ToInt16(AuditManager.AuditForm.Pre_Job_History);
                HR_Emp_Experience entity = db.HR_Emp_Experience.FirstOrDefault(x => x.ExperienceID == model.ID) ?? new HR_Emp_Experience();
                int TypeID = Convert.ToInt16(Utilities.NotificationType.Pre_Job_History);
                if (ModelState.IsValid)
                {
                    int empID = Session["EMP_ID"] != null ? Convert.ToInt32(Session["EMP_ID"]) : 0;
                    string path = string.Empty;
                    foreach (string file in Request.Files)
                    {
                        HttpPostedFileBase hpf = Request.Files[file];
                        if (Request.Files[file].ContentLength > 0)
                        {
                            path = SaveDocumentFile(hpf, empID);
                            path = path.Replace(@"\", @"/");
                        }
                    }
                    if (path != string.Empty)
                        entity.ExperienceLetterPath = path;
                    entity.OrganisationName = model.Organisation;
                    entity.OrgAddress = model.OrgAddress;
                    entity.Department = model.Department;
                    entity.Designation = model.Designation;
                    entity.EmployeeID = empID;
                    //Utilities.WriteToLogFile(model.FromDate + " " + model.ToDate);
                    entity.FromDate = DateTime.ParseExact(model.FromDate, "dd/MM/yyyy", null);  //Convert.ToDateTime(model.FromDate);
                    entity.ToDate = DateTime.ParseExact(model.ToDate, "dd/MM/yyyy", null);  //Convert.ToDateTime(model.ToDate);
                    entity.JobDescription = model.JobDescription;
                    entity.OrgContactNumber = model.ContactNumber;
                    entity.DateCreated = DateTime.Now;
                    entity.Active = true;
                    if (LoggedInUser.HRRequest == true)
                    {
                        entity.StatusID = Convert.ToInt32(Utilities.ProfileStatus.Approved);
                    }
                    else
                    {
                        entity.StatusID = Convert.ToInt32(Utilities.ProfileStatus.Pending);
                    }
                    if (entity.ExperienceID > 0)
                    {
                        string comm = Request["txtComment"].Trim();
                        if (comm.Length > 0)
                            SubmitComments(entity.ExperienceID, TypeID, entity.EmployeeID, comm, entity.StatusID);
                        db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                        auditOperation = Convert.ToInt16(AuditManager.AuditOperation.Edit);
                    }
                    else
                    {
                        db.HR_Emp_Experience.Add(entity);
                    }

                    db.SaveChanges();
                    int id = entity.ExperienceID;
                    string TypeName = Utilities.NotificationType.Pre_Job_History.ToString();
                    if (LoggedInUser.HRRequest == true && entity.ExperienceID > 0)
                        Utilities.InsertEMPNotification(TypeID, TypeName, LoggedInUser.UserID, id, empID, BaseURL+"HumanResource/Profile/EmpDetails/" + empID + "?prejobhistory");
                    else if (LoggedInUser.HRRequest != true)
                        Utilities.InsertHRNotification("HRModule", TypeID, TypeName, empID, id, BaseURL+"HumanResource/HR/GetPendingPreJobHistory");
                    AuditManager.SaveAuditLog(LoggedInUser.UserID, auditForm, auditOperation, DateTime.Now, id);

                }

                return RedirectToAction("EmpDetails", new { ID = entity.EmployeeID, @class = "prejobhistory" });

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult EMPPostingTransfer(int ID)
        {

            try
            {
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                int pendingStatus = Convert.ToInt16(Utilities.ProfileStatus.Query_by_HR);
                int approvedStatus = Convert.ToInt16(Utilities.ProfileStatus.Approved);
                List<ViewEMPTransfer> modelList = new List<ViewEMPTransfer>();
                List<object> models = new List<object>();
                var model = db.ViewEMPTransfers.OrderByDescending(x => x.PHEmpID).Where(x => x.EmpID == ID).Select(x => new { x.OLocID, x.ODeptID, x.OSectionID }).Distinct().ToList();
                foreach (var Item in model)
                {
                    ViewEMPTransfer trans = db.ViewEMPTransfers.FirstOrDefault(x => x.EmpID == ID && x.OLocID == Item.OLocID && x.ODeptID == Item.ODeptID && x.OSectionID == Item.OSectionID);
                    modelList.Add(trans);
                }

                //List<HR_EmpTranfers> model = db.HR_EmployeeHistory.OrderByDescending(x => x.PHEmpID).Where(x => x.EmpID == ID && x.).ToList();
                models.Add(modelList);
                //List<HR_EmpTranfers> p_model = db.ViewEmpPreJobHistories.OrderByDescending(x => x.ExperienceID).Where(x => x.EmployeeID == ID && x.Deleted == false && x.StatusID != approvedStatus).ToList();
                //models.Add(p_model);
                ViewBag.TypeID = Convert.ToInt16(Utilities.NotificationType.Posting_Transfer);

                if (ID == LoggedInUser.EmployeeID)
                    Utilities.DeleteNotificationByType(ID, Convert.ToInt16(Utilities.NotificationType.Posting_Transfer));
                return PartialView("EmpTransferHistory", models);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult ModifyPosting(int ID)
        {

            try
            {
                HR_EmployeeHistory model = new HR_EmployeeHistory();
                List<HR_EmployeeHistory> entity = new List<HR_EmployeeHistory>();
                if (ID != null)
                {
                    int TypeID = Convert.ToInt16(Utilities.NotificationType.Posting_Transfer);
                    ViewBag.TypeID = TypeID.ToString();
                    entity.Add(db.HR_EmployeeHistory.FirstOrDefault(x => x.PHEmpID == ID));

                    foreach (var ent in entity)
                    {
                        if (ent != null)
                        {
                            model.OLocID = ent.OLocID;
                            //model.LocationName = ent.LocationName;
                            model.ODeptID = ent.ODeptID;
                            //model.DepartmentName = ent.DepartmentName;
                            model.OSectionID = ent.OSectionID;
                            model.OSecID = ent.OSecID;
                            //model.SectionName = ent.SectionName;
                            model.OGrd = ent.OGrd;
                            model.OGradeID = ent.OGradeID;
                            model.OScale = ent.OScale;
                            model.Remarks = ent.Remarks;
                            model.PHEmpID = ent.PHEmpID;
                            model.OStationJoinDate = ent.OStationJoinDate;
                            //model.ExperiencePath = ent.ExperienceLetterPath;
                            //model.ID = ent.ExperienceID;


                            ViewBag.Location = new SelectList(db.HR_Location.OrderBy(x => x.LocationName).Where(x => x.LocID > 0).ToList(), "LocID", "LocationName");
                            //ViewBag.OLocID = new SelectList(db.ViewEMPTransfers.Where(aa => aa.OLocID == model.OLocID).OrderBy(aa => aa.FullName).ToList(), "OLocID", "LocationName");
                            ViewBag.Section = new SelectList(db.HR_Section.OrderBy(x => x.SectionName).Where(x => x.SecID > 0).ToList(), "SecID", "SectionName");

                        }
                    }



                }
                return PartialView("AddTransferPosting", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult ModifyPostingTransfer([Bind(Include = "[PHEmpID],[EmpID],[ODesignationID],[OLocID],[OSectionID],[OSecID],[ODeptID],[OEmpTypeID],[OGradeID],[OGrd],[OReportingTo],[OScale],[OStationJoinDate],[OCurrentDate],[ORetirementDate],[ODateOfCommision],[OGovtSrvsDate],[OOrgJoinDate],[OTerminationDate],[Remarks],[FullName],[DesignationName],[LocationName],[SectionName],[DepartmentName],[TypeName]")] ViewEMPTransfer model)
        public ActionResult ModifyPostingTransfer(HR_EmployeeHistory model)
        {

            try
            {
                HR_EmployeeHistory entity = db.HR_EmployeeHistory.FirstOrDefault(x => x.PHEmpID == model.PHEmpID);
                if (entity != null)
                {

                    entity.OSectionID = model.OSectionID;
                    entity.OLocID = model.OLocID;
                    entity.Remarks = model.Remarks;
                    db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("EmpDetails", new { ID = entity.EmpID, @class = "transfer" });

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult EMPPromotion(int ID)
        {

            try
            {
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                int pendingStatus = Convert.ToInt16(Utilities.ProfileStatus.Query_by_HR);
                int approvedStatus = Convert.ToInt16(Utilities.ProfileStatus.Approved);

                List<object> models = new List<object>();

                List<HR_EmpPromotions> model = db.HR_EmpPromotions.OrderByDescending(x => x.PromotionID).Where(x => x.EmployeeID == ID && x.Deleted == false).ToList();
                models.Add(model);
                //List<HR_EmpTranfers> p_model = db.ViewEmpPreJobHistories.OrderByDescending(x => x.ExperienceID).Where(x => x.EmployeeID == ID && x.Deleted == false && x.StatusID != approvedStatus).ToList();
                //models.Add(p_model);
                ViewBag.TypeID = Convert.ToInt16(Utilities.NotificationType.Promotion);

                if (ID == LoggedInUser.EmployeeID)
                    Utilities.DeleteNotificationByType(ID, Convert.ToInt16(Utilities.NotificationType.Promotion));
                return PartialView("EmpPromotions", models);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult EMPAppreciation(int ID)
        {

            try
            {
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                Session["EMP_ID"] = ID.ToString();
                int pendingStatus = Convert.ToInt16(Utilities.ProfileStatus.Query_by_HR);
                int approvedStatus = Convert.ToInt16(Utilities.ProfileStatus.Approved);
                List<object> models = new List<object>();
                List<ViewEmpAppreciation> model = db.ViewEmpAppreciations.OrderByDescending(x => x.AppreciationID).Where(x => x.EmployeeID == ID && x.Deleted == false && x.StatusID == approvedStatus).ToList();
                models.Add(model);
                List<ViewEmpAppreciation> p_model = db.ViewEmpAppreciations.OrderByDescending(x => x.AppreciationID).Where(x => x.EmployeeID == ID && x.Deleted == false && x.StatusID != approvedStatus).ToList();
                models.Add(p_model);
                ViewBag.TypeID = Convert.ToInt16(Utilities.NotificationType.Appreciation);
                if (ID == LoggedInUser.EmployeeID)
                    Utilities.DeleteNotificationByType(ID, Convert.ToInt16(Utilities.NotificationType.Appreciation));

                ViewBag.TypeID = Convert.ToInt16(Utilities.NotificationType.Appreciation);
                if (ID == LoggedInUser.EmployeeID)
                    Utilities.DeleteNotificationByType(ID, Convert.ToInt16(Utilities.NotificationType.Appreciation));
                return PartialView("EmpAppreciations", models);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult ModifyAppreciation(int ID)
        {

            try
            {
                HR_EmpAppreciations model = db.HR_EmpAppreciations.FirstOrDefault(x => x.AppreciationID == ID);
                ViewBag.Designation = model.Appreciations_From.Contains("Chairman") ? "Chairman" : "Member (Compliance & Enforcement)";
                Session["EMP_ID"] = ID.ToString();
                string value = "CH";
                int designationID = db.HR_Designation.FirstOrDefault(x => x.ODesigID == value).DesgID;
                ViewBag.CH = db.HR_Employee.FirstOrDefault(x => x.DesignationID == designationID && x.Status == "Active").FullName;
                value = "MCE";
                designationID = db.HR_Designation.FirstOrDefault(x => x.ODesigID == value).DesgID;
                ViewBag.MCE = db.HR_Employee.FirstOrDefault(x => x.DesignationID == designationID && x.Status == "Active").FullName;
                value = "MF";
                designationID = db.HR_Designation.FirstOrDefault(x => x.ODesigID == value).DesgID;
                ViewBag.MF = db.HR_Employee.FirstOrDefault(x => x.DesignationID == designationID && x.Status == "Active").FullName;
                return PartialView("AddAppreciation", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult AddAppreciation()
        {

            try
            {
                HR_EmpAppreciations model = new HR_EmpAppreciations();
                Session["EMP_ID"] = Session["EMP_ID"];
                string value = "CH";
                int designationID = db.HR_Designation.FirstOrDefault(x => x.ODesigID == value).DesgID;
                ViewBag.CH = db.HR_Employee.FirstOrDefault(x => x.DesignationID == designationID && x.Status == "Active")?.FullName;
                value = "MCE";
                designationID = db.HR_Designation.FirstOrDefault(x => x.ODesigID == value).DesgID;
                ViewBag.MCE = db.HR_Employee.FirstOrDefault(x => x.DesignationID == designationID && x.Status == "Active")?.FullName;
                value = "MF";
                designationID = db.HR_Designation.FirstOrDefault(x => x.ODesigID == value).DesgID;
                ViewBag.MF = db.HR_Employee.FirstOrDefault(x => x.DesignationID == designationID && x.Status == "Active")?.FullName;
                return PartialView("AddAppreciation", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAppreciation(HR_EmpAppreciations model)
        {

            try
            {
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                auditOperation = Convert.ToInt16(AuditManager.AuditOperation.Add);
                auditForm = Convert.ToInt16(AuditManager.AuditForm.Appreciation);
                int TypeID = Convert.ToInt16(Utilities.NotificationType.Appreciation);
                //HR_EmpAppreciations entity = db.HR_EmpAppreciations.FirstOrDefault(x => x.AppreciationID == model.AppreciationID) ?? new HR_EmpAppreciations();
                Utilities.WriteToLogFile("Place 1 " + model.Appreciations_Date.ToString());
                model.Appreciations_Date = DateTime.ParseExact(model.Appreciations_Date.ToString(), "dd/MM/yyyy", null);
                if (ModelState.IsValid)
                {
                    Utilities.WriteToLogFile("Place 2 ");
                    int empID = Session["EMP_ID"] != null ? Convert.ToInt32(Session["EMP_ID"]) : 0;
                    string path = string.Empty;
                    foreach (string file in Request.Files)
                    {
                        HttpPostedFileBase hpf = Request.Files[file];
                        if (Request.Files[file].ContentLength > 0)
                        {
                            path = SaveDocumentFile(hpf, empID);
                            path = path.Replace(@"\", @"/");
                        }
                    }
                    if (path != string.Empty)
                        model.DocumentPath = path;

                    model.EmployeeID = empID;
                    model.Active = true;
                    model.Deleted = false;

                    if (LoggedInUser.HRRequest == true)
                    {
                        model.StatusID = Convert.ToInt32(Utilities.ProfileStatus.Approved);
                    }
                    else
                    {
                        model.StatusID = Convert.ToInt32(Utilities.ProfileStatus.Pending);
                        
                    }
                    if (model.AppreciationID > 0)
                    {
                        string comm = Request["txtComment"].Trim();
                        if (comm.Length > 0)
                            SubmitComments(model.AppreciationID, TypeID, model.EmployeeID ?? 0, comm, model.StatusID ?? 0);
                        model.DateModified = DateTime.Now;
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        auditOperation = Convert.ToInt16(AuditManager.AuditOperation.Edit);
                    }
                    else
                    {
                        model.DateCreated = DateTime.Now;
                        db.HR_EmpAppreciations.Add(model);
                    }
                    Utilities.WriteToLogFile("Appreciation Date: " + model.Appreciations_Date);
                    
                    db.SaveChanges();
                    int id = model.AppreciationID;

                    string TypeName = Utilities.NotificationType.Appreciation.ToString();
                    if (LoggedInUser.HRRequest == true)
                        Utilities.InsertEMPNotification(TypeID, TypeName, LoggedInUser.UserID, id, empID, BaseURL+"HumanResource/Profile/EmpDetails/" + empID + "?appreciation");
                    else if (LoggedInUser.HRRequest != true)
                        Utilities.InsertHRNotification("HRModule", TypeID, TypeName, empID, id, BaseURL+"HumanResource/HR/GetPendingAppreciation");
                    AuditManager.SaveAuditLog(LoggedInUser.UserID, auditForm, auditOperation, DateTime.Now, id);
                }

                return RedirectToAction("EmpDetails", new { ID = model.EmployeeID??0, @class = "appreciation" });

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult EMPWarning(int ID)
        {

            try
            {
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                int pendingStatus = Convert.ToInt16(Utilities.ProfileStatus.Query_by_HR);
                int approvedStatus = Convert.ToInt16(Utilities.ProfileStatus.Approved);
                Session["EMP_ID"] = ID.ToString();

                List<object> models = new List<object>();

                List<ViewEmpWarning> model = db.ViewEmpWarnings.OrderByDescending(x => x.WarningID).Where(x => x.EmployeeID == ID && x.StatusID == approvedStatus && x.Deleted == false).ToList();
                models.Add(model);
                List<ViewEmpWarning> p_model = db.ViewEmpWarnings.OrderByDescending(x => x.WarningID).Where(x => x.EmployeeID == ID && x.Deleted == false && x.StatusID != approvedStatus).ToList();
                models.Add(p_model);
                ViewBag.TypeID = Convert.ToInt16(Utilities.NotificationType.Warning);

                if (ID == LoggedInUser.EmployeeID)
                    Utilities.DeleteNotificationByType(ID, Convert.ToInt16(Utilities.NotificationType.Warning));
                return PartialView("EmpWarnings", models);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult ModifyWarning(int ID)
        {

            try
            {
                HR_EmpWarnings model = db.HR_EmpWarnings.FirstOrDefault(x => x.WarningID == ID);
                ViewBag.Designation = model.Warnings___Explations_From.Contains("Chairman") ? "Chairman" : "Member (Compliance & Enforcement)";
                Session["EMP_ID"] = ID.ToString();
                string value = "CH";
                int designationID = db.HR_Designation.FirstOrDefault(x => x.ODesigID == value).DesgID;
                ViewBag.CH = db.HR_Employee.FirstOrDefault(x => x.DesignationID == designationID && x.Status == "Active")?.FullName;
                value = "MCE";
                designationID = db.HR_Designation.FirstOrDefault(x => x.ODesigID == value).DesgID;
                ViewBag.MCE = db.HR_Employee.FirstOrDefault(x => x.DesignationID == designationID && x.Status == "Active")?.FullName;
                value = "MF";
                designationID = db.HR_Designation.FirstOrDefault(x => x.ODesigID == value).DesgID;
                ViewBag.MF = db.HR_Employee.FirstOrDefault(x => x.DesignationID == designationID && x.Status == "Active")?.FullName;
                return PartialView("AddWarning", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult AddWarning()
        {

            try
            {
                HR_EmpWarnings model = new HR_EmpWarnings();
                Session["EMP_ID"] = Session["EMP_ID"];
                string value = "CH";
                int designationID = db.HR_Designation.FirstOrDefault(x => x.ODesigID == value).DesgID;
                ViewBag.CH = db.HR_Employee.FirstOrDefault(x => x.DesignationID == designationID && x.Status == "Active")?.FullName;
                value = "MCE";
                designationID = db.HR_Designation.FirstOrDefault(x => x.ODesigID == value).DesgID;
                ViewBag.MCE = db.HR_Employee.FirstOrDefault(x => x.DesignationID == designationID && x.Status == "Active")?.FullName;
                value = "MF";
                designationID = db.HR_Designation.FirstOrDefault(x => x.ODesigID == value).DesgID;
                ViewBag.MF = db.HR_Employee.FirstOrDefault(x => x.DesignationID == designationID && x.Status == "Active")?.FullName;
                return PartialView("AddWarning", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddWarning(HR_EmpWarnings model)
        {

            try
            {
                Utilities.WriteToLogFile("Start ... ");
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                auditOperation = Convert.ToInt16(AuditManager.AuditOperation.Add);
                auditForm = Convert.ToInt16(AuditManager.AuditForm.Warning);
                int TypeID = Convert.ToInt16(Utilities.NotificationType.Warning);
                if (ModelState.IsValid)
                {
                    int empID = Session["EMP_ID"] != null ? Convert.ToInt32(Session["EMP_ID"]) : 0;
                    string path = string.Empty;
                    foreach (string file in Request.Files)
                    {
                        HttpPostedFileBase hpf = Request.Files[file];
                        if (Request.Files[file].ContentLength > 0)
                        {
                            path = SaveDocumentFile(hpf, empID);
                            path = path.Replace(@"\", @"/");
                        }
                    }
                    if (path != string.Empty)
                        model.DocumentPath = path;
                    model.EmployeeID = empID;
                    model.Active = true;
                    model.Deleted = false;
                    if (LoggedInUser.HRRequest == true)
                    {
                        model.StatusID = Convert.ToInt32(Utilities.ProfileStatus.Approved);
                    }
                    else
                    {
                        model.StatusID = Convert.ToInt32(Utilities.ProfileStatus.Pending);
                        
                    }
                    if (model.WarningID > 0)
                    {
                        string comm = Request["txtComment"].Trim();
                        if (comm.Length > 0)
                            SubmitComments(model.WarningID, TypeID, model.EmployeeID ?? 0, comm, model.StatusID ?? 0);
                        model.DateModified = DateTime.Now;
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        auditOperation = Convert.ToInt16(AuditManager.AuditOperation.Edit);
                    }
                    else
                    {
                        model.DateCreated = DateTime.Now;
                        db.HR_EmpWarnings.Add(model);
                    }
                    Utilities.WriteToLogFile("Warning Date: " + model.Warnings___Explations_Date);
                    model.Warnings___Explations_Date = DateTime.ParseExact(model.Warnings___Explations_Date.ToString(), "dd/MM/yyyy", null);
                    db.SaveChanges();
                    int id = model.WarningID;

                    string TypeName = Utilities.NotificationType.Warning.ToString();
                    if (LoggedInUser.HRRequest == true)
                        Utilities.InsertEMPNotification(TypeID, TypeName, LoggedInUser.UserID, id, empID, BaseURL+"HumanResource/Profile/EmpDetails/" + empID + "?warning");
                    else if (LoggedInUser.HRRequest != true)
                        Utilities.InsertHRNotification("HRModule", TypeID, TypeName, empID, id, BaseURL+"HumanResource/HR/GetPendingWarning");
                    AuditManager.SaveAuditLog(LoggedInUser.UserID, auditForm, auditOperation, DateTime.Now, id);
                }

                return RedirectToAction("EmpDetails", new { ID = model.EmployeeID??0, @class = "warning" });

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult EMPTraining(int ID)
        {

            try
            {
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                int pendingStatus = Convert.ToInt16(Utilities.ProfileStatus.Query_by_HR);
                int approvedStatus = Convert.ToInt16(Utilities.ProfileStatus.Approved);
                Session["EMP_ID"] = ID.ToString();
                List<object> models = new List<object>();

                List<ViewEmpTraining> model = db.ViewEmpTrainings.OrderByDescending(x => x.TrainingID).Where(x => x.EmployeeID == ID && x.StatusID == approvedStatus && x.Deleted == false).ToList();
                models.Add(model);
                List<ViewEmpTraining> p_model = db.ViewEmpTrainings.OrderByDescending(x => x.TrainingID).Where(x => x.EmployeeID == ID && x.Deleted == false && x.StatusID != approvedStatus).ToList();
                models.Add(p_model);
                ViewBag.TypeID = Convert.ToInt16(Utilities.NotificationType.Training);

                if (ID == LoggedInUser.EmployeeID)
                    Utilities.DeleteNotificationByType(ID, Convert.ToInt16(Utilities.NotificationType.Training));
                return PartialView("EmpTrainings", models);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult ModifyTraining(int ID)
        {

            try
            {
                HR_EmpTrainings model = db.HR_EmpTrainings.FirstOrDefault(x => x.TrainingID == ID);
                Session["EMP_ID"] = ID.ToString();
                return PartialView("AddTraining", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult AddTraining()
        {

            try
            {
                HR_EmpTrainings model = new HR_EmpTrainings();
                return PartialView("AddTraining", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTraining(HR_EmpTrainings model)
        {

            try
            {
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                auditOperation = Convert.ToInt16(AuditManager.AuditOperation.Add);
                auditForm = Convert.ToInt16(AuditManager.AuditForm.Training);
                int TypeID = Convert.ToInt16(Utilities.NotificationType.Training);
                //HR_EmpAppreciations entity = db.HR_EmpAppreciations.FirstOrDefault(x => x.AppreciationID == model.AppreciationID) ?? new HR_EmpAppreciations();
                //if (ModelState.IsValid)
                //{
                int empID = Session["EMP_ID"] != null ? Convert.ToInt32(Session["EMP_ID"]) : 0;
                string path = string.Empty;
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpf = Request.Files[file];
                    if (Request.Files[file].ContentLength > 0)
                    {
                        path = SaveDocumentFile(hpf, empID);
                        path = path.Replace(@"\", @"/");
                    }
                }
                if (path != string.Empty)
                    model.DocumentPath = path;

                model.EmployeeID = empID;
                model.Active = true;
                model.Deleted = false;
                if (LoggedInUser.HRRequest == true)
                {
                    model.StatusID = Convert.ToInt32(Utilities.ProfileStatus.Approved);
                }
                else
                {
                    model.StatusID = Convert.ToInt32(Utilities.ProfileStatus.Pending);
                }
                if (model.TrainingID > 0)
                {
                    string comm = Request["txtComment"].Trim();
                    if (comm.Length > 0)
                        SubmitComments(model.TrainingID, TypeID, model.EmployeeID ?? 0, comm, model.StatusID ?? 0);
                    model.DateModified = DateTime.Now;
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    auditOperation = Convert.ToInt16(AuditManager.AuditOperation.Edit);
                }
                else
                {
                    model.DateCreated = DateTime.Now;
                    db.HR_EmpTrainings.Add(model);
                }
                db.SaveChanges();
                int id = model.TrainingID;

                string TypeName = Utilities.NotificationType.Training.ToString();
                if (LoggedInUser.HRRequest == true)
                    Utilities.InsertEMPNotification(TypeID, TypeName, LoggedInUser.UserID, id, empID, BaseURL+"HumanResource/Profile/EmpDetails/" + empID + "?training");
                else if (LoggedInUser.HRRequest != true)
                    Utilities.InsertHRNotification("HRModule", TypeID, TypeName, empID, id, BaseURL+"HumanResource/HR/GetPendingTraining");

                AuditManager.SaveAuditLog(LoggedInUser.UserID, auditForm, auditOperation, DateTime.Now, id);
                //}

                return RedirectToAction("EmpDetails", new { ID = model.EmployeeID??0, @class = "training" });

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]  
        public ActionResult EMPLeaves(int ID)
        {

            try
            {
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                int pendingStatus = Convert.ToInt16(Utilities.ProfileStatus.Query_by_HR);
                int approvedStatus = Convert.ToInt16(Utilities.ProfileStatus.Approved);
                Session["EMP_ID"] = ID.ToString();
                List<object> models = new List<object>();
                string status = "Approved";
                string casual = "CASUAL";
                List<HR_LeaveReq> model = db.HR_LeaveReq.OrderByDescending(x => x.LEAVEID).Where(x => x.EMPLOYEEID == ID && x.LEAVETYPE != casual && x.STATUS == status).ToList();
                models.Add(model);
                //List<HR_LeaveReq> p_model = db.HR_LeaveReq.OrderByDescending(x => x.LEAVEID).Where(x => x.EMPLOYEEID == ID && x.LEAVETYPE != casual && x.STATUS != status).ToList();
                //models.Add(p_model);
                ViewBag.TypeID = Convert.ToInt16(Utilities.NotificationType.Leave);

                if (ID == LoggedInUser.EmployeeID)
                    Utilities.DeleteNotificationByType(ID, Convert.ToInt16(Utilities.NotificationType.Leave));
                return PartialView("EmpLeaves", models);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult AddLeave()
        {

            try
            {
                ModelLeaveRequest model = new ModelLeaveRequest();

                ViewBag.Type = new SelectList(db.HR_LeaveType.Where(x => x.HIDE == 0), "DESCR", "DESCR");
                ViewBag.Type = db.HR_LeaveType.Where(x => x.HIDE == 0);
                int empID = Session["EMP_ID"] != null ? Convert.ToInt32(Session["EMP_ID"]) : 0;
                ViewBag.PersonID = empID.ToString();
                DateTime DOJ = db.ViewUserEmps.FirstOrDefault(x => x.EmpID == empID)?.DOJ ?? DateTime.Now;
                short empType = db.ViewUserEmps.FirstOrDefault(x => x.EmpID == empID)?.TypID ?? 0;
                string type = db.HR_EmpType.FirstOrDefault(x => x.TypID == empType)?.OTypeID;
                string gradeName = db.ViewUserEmps.FirstOrDefault(x => x.EmpID == empID)?.GradeName;
                ViewBag.CLeave = LeaveUtil.GetCasualLeaveBalance(empID.ToString(), DOJ.ToString(), type.ToString()).ToString();
                ViewBag.ELeave = LeaveUtil.GetELBalance(empID.ToString(), gradeName, DOJ.ToString(), type.ToString()).ToString();
                ViewBag.PermanentAddress = db.HR_Employee.FirstOrDefault(x => x.EmployeeID == empID).Address;
                ViewBag.TemporaryAddress = db.HR_Employee.FirstOrDefault(x => x.EmployeeID == empID).Address2;
                short deptID = db.ViewUserEmps.FirstOrDefault(x => x.EmpID == empID)?.DepartmentID ?? 0;
                short secID = db.ViewUserEmps.FirstOrDefault(x => x.EmpID == empID)?.SectionID ?? 0;
                int grade = Convert.ToInt32(db.ViewUserEmps.FirstOrDefault(x => x.EmpID == empID)?.GradeName);
                int ReportTo = db.ViewUserEmps.FirstOrDefault(x => x.EmpID == empID)?.ReportingToID ?? 0;
                string status = "Active";
                model.TELNO = db.ViewUserEmps.FirstOrDefault(x => x.EmpID == empID)?.OfficialContactNo;
                ViewBag.Substitute = db.EmpViews.//OrderByDescending(x => int.Parse(x.GradeName)).
                    Where(x => x.EmployeeID != empID && x.DepartmentID == deptID && x.SecID == secID && SqlFunctions.IsNumeric(x.GradeName) > 0);
                model.SUBSTITUTENAMELIST = new SelectList(db.EmpViews.Where(x => x.EmployeeID != empID && x.DepartmentID == deptID && x.SecID == secID && x.Status == status).Select(x => new { EmpNo = x.EmpNo, FullName = x.FullName + " (" + x.DesignationName + ")" }), "EmpNo", "FullName");
                List<EmpView> refList = db.EmpViews.Where(x => x.EmployeeID != empID && SqlFunctions.IsNumeric(x.GradeName) > 0 && x.Status == status
                                                            && x.EmailID != null && x.LeavingDate == null).ToList();
                SelectList ReportToItem = new SelectList(refList.Where(y => int.Parse(y.GradeName) > grade && y.EmployeeID == ReportTo).Select(x => new { EmpNo = x.EmpNo, FullName = x.FullName + " (" + x.DesignationName + ")" }), "EmpNo", "FullName");
                SelectList RefList = new SelectList(refList.Where(y => int.Parse(y.GradeName) > grade && y.EmployeeID != ReportTo).Select(x => new { EmpNo = x.EmpNo, FullName = x.FullName + " (" + x.DesignationName + ")" }), "EmpNo", "FullName");
                //model.REFERNAMELIST = new SelectList(refList.Where(y => int.Parse(y.GradeName) > grade).Select(x => new { EmpNo = x.EmpNo, FullName = x.FullName + " (" + x.DesignationName + ")" }), "EmpNo", "FullName");
                model.REFERNAMELIST = new SelectList(ReportToItem.Union(RefList), "Value", "Text");

                return PartialView("NewLeave", model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public int CalculateDays(string txtLeaveFrom, string txtLeaveTo, string LeaveType)
        {
            //dvmessage.Visible = false;
            int days = 0;
            //System.Threading.Thread.Sleep(1000);
            if (!txtLeaveFrom.Equals("") && !txtLeaveTo.Equals(""))
            {

                DateTime d0 = DateTime.ParseExact(txtLeaveFrom, "dd/MM/yyyy", null);
                DateTime d1 = DateTime.ParseExact(txtLeaveTo, "dd/MM/yyyy", null);
                days = (LeaveType.Equals("CASUAL") ?
                      this.CalcBizDays(d0, d1) :
                     1 + Convert.ToInt32((d1 - d0).TotalDays));
            }
            return days;
            //ViewState["Days"] = days;
            //lblDays.InnerHtml = "=" + ViewState["Days"] + " Days";
            //lblDays.InnerHtml =(lstLeaveType.SelectedValue.Equals("1") || lstLeaveType.SelectedItem.Text.Equals("CASUAL") ? "=" + days + " Days" : "");             
        }

        [HttpGet]
        public int LeaveOverLapping(string txtLeaveFrom, string txtLeaveTo, string EmpID)
        {
            
            int overlappingFlag = 0;
            if (!txtLeaveFrom.Equals("") && !txtLeaveTo.Equals(""))
            {

                DateTime d0 = DateTime.ParseExact(txtLeaveFrom, "dd/MM/yyyy", null);
                DateTime d1 = DateTime.ParseExact(txtLeaveTo, "dd/MM/yyyy", null);
                if(LeaveUtil.LeavesOverLapping(EmpID, txtLeaveFrom.ToString(), txtLeaveTo.ToString()))
                {
                    overlappingFlag = 1;
                }
                
            }
            return overlappingFlag;
            //ViewState["Days"] = days;
            //lblDays.InnerHtml = "=" + ViewState["Days"] + " Days";
            //lblDays.InnerHtml =(lstLeaveType.SelectedValue.Equals("1") || lstLeaveType.SelectedItem.Text.Equals("CASUAL") ? "=" + days + " Days" : "");             
        }

        private int CalcBizDays(DateTime d0, DateTime d1)
        {
            DateTime thisDate = d0;
            DateTime December25 = new DateTime(d0.Year, 12, 25); //Convert.ToDateTime("25/12/2017");// DateTime.Parse("25/12/2017", culture, DateTimeStyles.NoCurrentDateDefault);
            DateTime August14 = new DateTime(d0.Year, 08, 14);
            DateTime May01 = new DateTime(d0.Year, 05, 01);
            DateTime March23 = new DateTime(d0.Year, 03, 23);
            DateTime Februray05 = new DateTime(d0.Year, 02, 05);
            int weekDays = 0;
            while (thisDate <= d1)
            {
                if (thisDate.DayOfWeek != DayOfWeek.Saturday && thisDate.DayOfWeek != DayOfWeek.Sunday && thisDate != December25
                    && thisDate != August14 && thisDate != May01 && thisDate != March23 && thisDate != Februray05)
                { weekDays++; }
                thisDate = (d1 >= d0 ? thisDate.AddDays(1) : thisDate.AddDays(-1));
            }
            //if (weekDays == 0 && d0 == d1)
            //    weekDays = 1;
            return (d1 >= d0 ? weekDays : weekDays * -1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewLeave(ModelLeaveRequest model)
        {

            try
            {
                ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                auditOperation = Convert.ToInt16(AuditManager.AuditOperation.Add);
                auditForm = Convert.ToInt16(AuditManager.AuditForm.Leave);
                int TypeID = Convert.ToInt16(Utilities.NotificationType.Leave);
                HR_LeaveReq modelReq = new HR_LeaveReq();
                int empID = 0;
                int leaveID = 0;
                if (ModelState.IsValid)
                {
                    empID = Session["EMP_ID"] != null ? Convert.ToInt32(Session["EMP_ID"]) : 0;
                    string path = string.Empty;

                    modelReq.APPDATE = DateTime.Now;
                    modelReq.FROMDATE = DateTime.ParseExact(model.FROMDATE, "dd/MM/yyyy", null); ;
                    modelReq.TODATE = DateTime.ParseExact(model.TODATE, "dd/MM/yyyy", null); ;
                    modelReq.STATUS = "Pending";
                    modelReq.REASON = model.REASON;
                    modelReq.LEAVETYPE = model.LEAVETYPE;
                    modelReq.EMPLOYEEID = empID;
                    modelReq.STATIONLEAVE = model.STATIONLEAVE;
                    modelReq.ADDRESS = model.ADDRESS;
                    modelReq.TELNO = model.TELNO;
                    modelReq.DUELEAVE = model.DUELEAVE;
                    modelReq.SUBSTITUTENAME = db.EmpViews.FirstOrDefault(x => x.EmployeeID == model.SUBSTITUTEID)?.FullName;
                    modelReq.SUBSTITUTEEMAIL = db.EmpViews.FirstOrDefault(x => x.EmployeeID == model.SUBSTITUTEID)?.EmailID;
                    db.HR_LeaveReq.Add(modelReq);
                    db.SaveChanges();

                    leaveID = modelReq.LEAVEID;
                    int recID = db.HR_LeaveRec.OrderByDescending(x => x.RECID).Where(z => z.LEAVEID == leaveID).FirstOrDefault()?.RECID ?? 0;
                    recID = recID + 1;
                    if(leaveID > 0 )
                    {
                        HR_LeaveRec modelRec = new HR_LeaveRec();
                        modelRec.LEAVEID = leaveID;
                        modelRec.RECID = recID;
                        modelRec.REFERNAME= db.EmpViews.FirstOrDefault(x => x.EmployeeID == model.REFERNAMEID)?.FullName;
                        modelRec.REFEREMAIL = db.EmpViews.FirstOrDefault(x => x.EmployeeID == model.REFERNAMEID)?.EmailID;
                        modelRec.ONDESKSINCE = DateTime.Now.ToString("dd-MMM-yy");

                        db.HR_LeaveRec.Add(modelRec);
                        db.SaveChanges();
                    }

                    
                    
                    //int id = entity.ExperienceID;
                    //string TypeName = Utilities.NotificationType.Leave.ToString();
                    //if (LoggedInUser.HRRequest == true && entity.ExperienceID > 0)
                    //    Utilities.InsertEMPNotification(TypeID, TypeName, LoggedInUser.UserID, id, empID, BaseURL + "HumanResource/Profile/EmpDetails/" + empID + "?prejobhistory");
                    //else if (LoggedInUser.HRRequest != true)
                    //    Utilities.InsertHRNotification("HRModule", TypeID, TypeName, empID, id, BaseURL + "HumanResource/HR/GetPendingPreJobHistory");
                    AuditManager.SaveAuditLog(LoggedInUser.UserID, auditForm, auditOperation, DateTime.Now, leaveID);

                }

                return RedirectToAction("EmpDetails", new { ID = empID, @class = "leave" });

            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        public ActionResult GetAssets(int id)
        {
            using (ARADBEntities1 assets = new ARADBEntities1())
            {
                try
                {
                    int empID = Session["EMP_ID"] != null ? Convert.ToInt32(Session["EMP_ID"]) : 0;
                    AMS2ESSP entity = new AMS2ESSP();
                    List<object> models = new List<object>();
                    List<AMS2ESSP> aModelList = new List<AMS2ESSP>();//assets.AMS2ESSP.OrderByDescending(x => x.Employee_No).Where(x => x.Employee_No == id.ToString()).ToList();
                    AMS2ESSP model = new AMS2ESSP();
                    model.Employee_No = empID.ToString();
                    model.Asset = "Scanner Canon DR-C130 with FB 101 ADF + FB";
                    model.Allocation_Date = DateTime.Now;
                    model.BarCode = "01001204";
                    aModelList.Add(model);
                    models.Add(aModelList);
                    ViewUserEmp LoggedInUser = Session["LoggedUser"] as ViewUserEmp;
                    ViewBag.TypeID = Convert.ToInt16(Utilities.NotificationType.Assets);

                    //if (empID == LoggedInUser.EmployeeID)
                    //    Utilities.DeleteNotificationByType(empID, Convert.ToInt16(Utilities.NotificationType.Assets));
                    return PartialView("EmpAssets", models);
                }
                catch (Exception)
                {

                    assets.Dispose();
                }
                return PartialView("EmpDependents");
            }

        }

        [HttpGet]
        public bool DeleteEmpRecord(int TypeID, int ID)
        {

            try
            {
                if (TypeID == Convert.ToInt16(Utilities.NotificationType.Qualification))
                {
                    var entity = db.HR_Emp_Qualification.Where(x => x.QualificationID == ID).ToList();
                    entity.ForEach(m => m.Deleted = true);
                    db.SaveChanges();
                }
                else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Dependent))
                {
                    var entity = db.HR_Emp_Dependents.Where(x => x.DependentID == ID).ToList();
                    entity.ForEach(m => m.Deleted = true);
                    db.SaveChanges();
                }
                else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Posting_Transfer))
                {
                    var entity = db.HR_EmpTranfers.Where(x => x.TransferID == ID).ToList();
                    entity.ForEach(m => m.Deleted = true);
                    db.SaveChanges();
                }
                else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Promotion))
                {
                    var entity = db.HR_EmpPromotions.Where(x => x.PromotionID == ID).ToList();
                    entity.ForEach(m => m.Deleted = true);
                    db.SaveChanges();
                }
                else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Pre_Job_History))
                {
                    var entity = db.HR_Emp_Experience.Where(x => x.ExperienceID == ID).ToList();
                    entity.ForEach(m => m.Deleted = true);
                    db.SaveChanges();
                }
                else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Appreciation))
                {
                    var entity = db.HR_EmpAppreciations.Where(x => x.AppreciationID == ID).ToList();
                    entity.ForEach(m => m.Deleted = true);
                    db.SaveChanges();
                }
                else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Warning))
                {
                    var entity = db.HR_EmpWarnings.Where(x => x.WarningID == ID).ToList();
                    entity.ForEach(m => m.Deleted = true);
                    db.SaveChanges();
                }
                else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Training))
                {
                    var entity = db.HR_EmpTrainings.Where(x => x.TrainingID == ID).ToList();
                    entity.ForEach(m => m.Deleted = true);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {


            }
            return false;

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
                ViewBag.Values = GetValues(ID, TypeID);
                ViewBag.Area = Enum.GetName(typeof(Utilities.NotificationType), TypeID).Replace("_", " ");
                if (StatusID == Convert.ToInt16(Utilities.ProfileStatus.Query_by_HR))
                    ViewBag.ShowMessageBox = true;
                return PartialView("EmpCommunication", model);
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
                    Session["EMP_ID"] = LoggedInUser.EmpID;
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
                        db.SaveChanges();
                        string TypeName = Enum.GetName(typeof(Utilities.NotificationType), TypeID);
                        TypeName = TypeName.Replace("_", "");
                        Utilities.SetStatus(ID, TypeID, Convert.ToUInt16(Utilities.ProfileStatus.Pending));
                        if (EmpID != LoggedInUser.EmpID)
                            Utilities.InsertEMPNotification(TypeID, TypeName, LoggedInUser.UserID, ID, EmpID, BaseURL+"HumanResource/Profile/EmpDetails/" + EmpID + "?" + TypeName);
                        else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Pre_Job_History))
                        {
                            Utilities.InsertHRNotification("HRModule", TypeID, TypeName, LoggedInUser.UserID, ID, Utilities.HRPreJobNotificationURL);
                            AuditManager.SaveAuditLog(LoggedInUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Pre_Job_History), Convert.ToInt16(AuditManager.AuditOperation.Comment), DateTime.Now, ID);
                        }
                        else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Appreciation))
                        {
                            Utilities.InsertHRNotification("HRModule", TypeID, TypeName, LoggedInUser.UserID, ID, Utilities.HRAppreciationNotificationURL);
                            AuditManager.SaveAuditLog(LoggedInUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Appreciation), Convert.ToInt16(AuditManager.AuditOperation.Comment), DateTime.Now, ID);
                        }
                        else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Warning))
                        {
                            Utilities.InsertHRNotification("HRModule", TypeID, TypeName, LoggedInUser.UserID, ID, Utilities.HRWarningNotificationURL);
                            AuditManager.SaveAuditLog(LoggedInUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Warning), Convert.ToInt16(AuditManager.AuditOperation.Comment), DateTime.Now, ID);
                        }
                        else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Training))
                        {
                            Utilities.InsertHRNotification("HRModule", TypeID, TypeName, LoggedInUser.UserID, ID, Utilities.HRTrainingNotificationURL);
                            AuditManager.SaveAuditLog(LoggedInUser.UserID, Convert.ToInt16(AuditManager.AuditForm.Training), Convert.ToInt16(AuditManager.AuditOperation.Comment), DateTime.Now, ID);
                        }
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

        public FileResult DownloadDocument(int ID, int TypeID)
        {
            string path = string.Empty;
            if (Convert.ToInt16(Utilities.NotificationType.Qualification) == TypeID)
            {
                path = db.HR_Emp_Qualification.FirstOrDefault(x => x.QualificationID == ID).DocumentPath;
            }
            else if (Convert.ToInt16(Utilities.NotificationType.Dependent) == TypeID)
            {
                path = db.HR_Emp_Dependents.FirstOrDefault(x => x.DependentID == ID).DocumentPath;
            }
            else if (Convert.ToInt16(Utilities.NotificationType.Pre_Job_History) == TypeID)
            {
                path = db.HR_Emp_Experience.FirstOrDefault(x => x.ExperienceID == ID).ExperienceLetterPath;
            }
            else if (Convert.ToInt16(Utilities.NotificationType.Appreciation) == TypeID)
            {
                path = db.HR_EmpAppreciations.FirstOrDefault(x => x.AppreciationID == ID).DocumentPath;
            }
            else if (Convert.ToInt16(Utilities.NotificationType.Warning) == TypeID)
            {
                path = db.HR_EmpWarnings.FirstOrDefault(x => x.WarningID == ID).DocumentPath;
            }
            else if (Convert.ToInt16(Utilities.NotificationType.Training) == TypeID)
            {
                path = db.HR_EmpTrainings.FirstOrDefault(x => x.TrainingID == ID).DocumentPath;
            }
            string fielName = Path.GetFileName(path);
            int start = path.IndexOf('.') + 1;
            string ext = path.Substring(start, path.Length - start);
            string contentType = "application/" + ext;
            return File(path, contentType, fielName);
        }

        private string SaveDocumentFile(HttpPostedFileBase file, int EmpID)
        {
            // Define destination
            try
            {
                var folderName = ConfigurationSettings.AppSettings["DocumentPath"].ToString() + EmpID.ToString();
                var serverPath = HttpContext.Server.MapPath(folderName);
                if (Directory.Exists(serverPath) == false)
                {
                    Directory.CreateDirectory(serverPath);
                }

                // Generate unique file name
                var fileName = Path.GetFileName(file.FileName);
                fileName = SaveDocumentFilePath(file, serverPath, fileName);

                return Path.Combine(folderName, fileName);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private string SaveDocumentFilePath(HttpPostedFileBase file, string serverPath, string fileName)
        {
            try
            {
                //var img = new WebImage(file.InputStream);
                //double ratio = (double)img.Height / (double)img.Width;

                string fullFileName = Path.Combine(serverPath, fileName);
                ///*img.Resize(400, (int)(400 * ratio)); // ToD*/o - Change the value of the width of the image oin the screen

                if (System.IO.File.Exists(fullFileName))
                    System.IO.File.Delete(fullFileName);

                file.SaveAs(fullFileName);

                return Path.GetFileName(fullFileName);
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
    }

}