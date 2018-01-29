using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace PTAWMS.App_Start
{
    public static class Utilities
    {
        public enum HR_ListType
        {
            DegreeName = 1,
            InstituteName = 2,
            ProfileStatus = 3,
            Relationship = 5
        }
        public enum DegreeName
        {
            Matric = 1,
            Intermediate = 2
        }

        public enum InstituteName
        {
            NUST = 1
        }

        public enum ProfileStatus
        {
            Approved = 213,
            Rejected = 215,
            Pending = 212,
            Query_by_HR = 230
        }

        public enum NotificationType
        {
            Qualification = 216,
            Dependent = 217,
            Pre_Job_History = 218,
            Posting_Transfer = 231,
            Assets = 232,
            Appreciation=233,
            Warning=234,
            Promotion = 235,
            Training=236,
            Leave = 237,
            Job_Card = 238,
            Pending_Job_Card = 239,
            PendingVisitorEntry = 240,
            PendingOvertime= 241
        }

        public static string HRQualificationNotificationURL = "/WMS/HumanResource/HR/Index";
        public static string HRDependentNotificationURL = "/WMS/HumanResource/HR/Dependent";
        public static string HRPreJobNotificationURL = "/WMS/HumanResource/HR/PreJobHistory";
        public static string HRAppreciationNotificationURL = "/WMS/HumanResource/HR/GetPendingAppreciation";
        public static string HRWarningNotificationURL = "/WMS/HumanResource/HR/GetPendingWarning";
        public static string HRTrainingNotificationURL = "/WMS/HumanResource/HR/GetPendingTraining";


        public static bool InsertHRNotification(string roleType, int Type, string notification, int ActionByID, int ID, string URL)
        {
            using (HRMEntities db = new HRMEntities())
            {
                try
                {

                    HR_Emp_Notification entity = new HR_Emp_Notification();
                    entity.Notification = notification;
                    entity.NotificationTypeID = Type;
                    entity.NotificationURL = URL;
                    entity.ActionBY = ActionByID;
                    entity.RoleType = roleType;
                    entity.DateCreated = DateTime.Now;
                    entity.Active = true;
                    entity.Deleted = false;
                    entity.RecordID = ID;
                    
                    db.HR_Emp_Notification.Add(entity);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                    db.Dispose();
                }
            }
            return false;
        }

        public static bool InsertEMPNotification(int Type, string notification, int ActionByID, int ID, int UserID, string URL)
        {
            using (HRMEntities db = new HRMEntities())
            {
                try
                {

                    HR_Emp_Notification entity = new HR_Emp_Notification();
                    entity.Notification = notification;
                    entity.NotificationTypeID = Type;
                    entity.UserSpecific = true;
                    entity.EmployeeID = UserID;
                    entity.NotificationURL = URL;
                    entity.ActionBY = ActionByID;
                    entity.DateCreated = DateTime.Now;
                    entity.Active = true;
                    entity.Deleted = false;
                    entity.RecordID = ID;


                    db.HR_Emp_Notification.Add(entity);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                    db.Dispose();
                }
            }
            return false;
        }

        public static bool SetStatus(int ID, int TypeID, int StatusID)
        {
            using (HRMEntities db = new HRMEntities())
            {
                try
                {
                    if (TypeID == Convert.ToInt16(Utilities.NotificationType.Qualification))
                    {
                        HR_Emp_Qualification model = db.HR_Emp_Qualification.FirstOrDefault(x => x.QualificationID == ID) ?? new HR_Emp_Qualification();
                        model.StatusID = StatusID;
                        model.DateModified = DateTime.Now;
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    }
                    else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Dependent))
                    {
                        HR_Emp_Dependents model = db.HR_Emp_Dependents.FirstOrDefault(x => x.DependentID == ID) ?? new HR_Emp_Dependents();
                        model.StatusID = StatusID;
                        model.DateModified = DateTime.Now;
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    }
                    else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Pre_Job_History))
                    {
                        HR_Emp_Experience model = db.HR_Emp_Experience.FirstOrDefault(x => x.ExperienceID == ID) ?? new HR_Emp_Experience();
                        model.StatusID = StatusID;
                        model.DateModified = DateTime.Now;
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        
                    }
                    else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Appreciation))
                    {
                        HR_EmpAppreciations model = db.HR_EmpAppreciations.FirstOrDefault(x => x.AppreciationID == ID) ?? new HR_EmpAppreciations();
                        model.StatusID = StatusID;
                        model.DateModified = DateTime.Now;
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    }
                    else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Warning))
                    {
                        HR_EmpWarnings model = db.HR_EmpWarnings.FirstOrDefault(x => x.WarningID == ID) ?? new HR_EmpWarnings();
                        model.StatusID = StatusID;
                        model.DateModified = DateTime.Now;
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    }
                    else if (TypeID == Convert.ToInt16(Utilities.NotificationType.Training))
                    {
                        HR_EmpTrainings model = db.HR_EmpTrainings.FirstOrDefault(x => x.TrainingID == ID) ?? new HR_EmpTrainings();
                        model.StatusID = StatusID;
                        model.DateModified = DateTime.Now;
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                    db.Dispose();
                }
            }

            return false;
        }

        public static bool DeleteNotification(int ID, int notificationType)
        {
            using (HRMEntities db = new HRMEntities())
            {
                try
                {
                    var entity = db.HR_Emp_Notification.Where(x => x.RecordID == ID && x.NotificationTypeID == notificationType).ToList();
                    entity.ForEach(m => m.Deleted = true);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                    db.Dispose();
                }
            }

            return false;
        }

        public static bool DeleteNotificationByType(int EmpID, int notificationType)
        {
            using (HRMEntities db = new HRMEntities())
            {
                try
                {

                    var entity = db.HR_Emp_Notification.Where(x => x.EmployeeID == EmpID && x.NotificationTypeID == notificationType).ToList();
                    entity.ForEach(m => m.Deleted = true);
                    db.SaveChanges();

                    return true;
                }
                catch (Exception)
                {

                    db.Dispose();
                }
            }

            return false;
        }

        private static string logFile = AppDomain.CurrentDomain.BaseDirectory + "MyLogFile.txt";
        public static void WriteToLogFile(string strMessage)
        {
            try
            {

                string line = DateTime.Now.ToString() + " | ";
                line += strMessage;
                FileStream fs = new FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.None);
                StreamWriter swFromFileStream = new StreamWriter(fs);
                swFromFileStream.WriteLine(line);
                swFromFileStream.Flush();
                swFromFileStream.Close();
            }
            catch (Exception ex)
            {

            }
        }

    }
}