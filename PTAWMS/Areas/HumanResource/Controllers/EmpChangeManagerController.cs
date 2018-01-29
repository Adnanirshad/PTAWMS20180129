using HRM_IKAN.Authentication;
using PTAWMS.Areas.HumanResource.BusinessLogic;
using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTAWMS.Areas.HumanResource.Controllers
{
    public class EmpChangeManagerController : Controller
    {
        //
        // GET: /HumanResource/EmpChangeManager/
        public ActionResult Index()
        {
            return View();
        }
        // 
        HRMEntities db = new HRMEntities();
        public ActionResult Personal()
        {
            List<ViewHREmpChange> empChanges = db.ViewHREmpChanges.Where(aa=>aa.ChangeType=="Personal").ToList();
            ModelPersonal vm = new ModelPersonal();
            vm.EmpChangesList = empChanges;
            vm.Count = empChanges.Count;
            return View(vm);
        }
        public List<ModelDecision> GetDecision()
        {
            List<ModelDecision> list = new List<ModelDecision>();
            ModelDecision obj = new ModelDecision();
            obj.ID = 1;
            obj.Name = "Approved";
            list.Add(obj);
            ModelDecision obj1 = new ModelDecision();
            obj1.ID = 1;
            obj1.Name = "Reject";
            list.Add(obj);
            return list;
        }
	}
    public class ModelPersonal
    {
        public List<ViewHREmpChange> EmpChangesList { get; set; }
        public int Count { get; set; }
    }
    public class ModelDecision
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}