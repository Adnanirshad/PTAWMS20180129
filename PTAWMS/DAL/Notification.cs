using PTAWMS.App_Start;
using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PTAWMS.DAL
{
    public class Notification
    {
        private string HRQualificationNotificationURL = "/HumanResource/WMS/Index";
        private string HRDependentNotificationURL = "/HumanResource/WMS/Dependent";
        private string HRPreJobNotificationURL = "/HumanResource/WMS/PreJobHistory";
        private HRMEntities db = new HRMEntities();
        public bool InsertNotification(string roleType, int Type, string notification, int EmpID, int ID)
        {
            try
            {
                HR_Emp_Notification entity = new HR_Emp_Notification();
                entity.Notification = notification;
                entity.NotificationTypeID = Type;
                if (Type == Convert.ToInt16(Utilities.NotificationType.Qualification))
                    entity.NotificationURL = HRQualificationNotificationURL;
                else if (Type == Convert.ToInt16(Utilities.NotificationType.Dependent))
                    entity.NotificationURL = HRDependentNotificationURL;
                else if (Type == Convert.ToInt16(Utilities.NotificationType.Pre_Job_History))
                    entity.NotificationURL = HRPreJobNotificationURL;
                entity.ActionBY = EmpID;
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

                throw;
            }
            return false;
        }

        public bool DeleteHRQualificationNotification(int ID)
        {
            try
            {
                HR_Emp_Notification entity = db.HR_Emp_Notification.FirstOrDefault(x=>x.RecordID == ID);
                if(entity != null && entity.NotificationID > 0)
                    db.Entry(entity).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {

                throw;
            }
            return false;
        }
    }
}