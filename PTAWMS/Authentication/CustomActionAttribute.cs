using PTAWMS.App_Start;
using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PTA.Authentication
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    //public class EncryptedActionParameterAttribute : ActionFilterAttribute
    //{

    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {

    //        Dictionary<string, object> decryptedParameters = new Dictionary<string, object>();
    //        if (HttpContext.Current.Request.QueryString.Get("q") != null)
    //        {
    //            string encryptedQueryString = HttpContext.Current.Request.QueryString.Get("q");
    //            string decrptedString = Decrypt(encryptedQueryString.ToString());
    //            string[] paramsArrs = decrptedString.Split('?');

    //            for (int i = 0; i < paramsArrs.Length; i++)
    //            {
    //                string[] paramArr = paramsArrs[i].Split('=');
    //                decryptedParameters.Add(paramArr[0], Convert.ToInt32(paramArr[1]));
    //            }
    //        }
    //        for (int i = 0; i < decryptedParameters.Count; i++)
    //        {
    //            filterContext.ActionParameters[decryptedParameters.Keys.ElementAt(i)] = decryptedParameters.Values.ElementAt(i);
    //        }
    //        base.OnActionExecuting(filterContext);

    //    }

    //    private string Decrypt(string encryptedText)
    //    {

    //        string key = "jdsg432387#";
    //        byte[] DecryptKey = { };
    //        byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
    //        byte[] inputByte = new byte[encryptedText.Length];

    //        DecryptKey = System.Text.Encoding.UTF8.GetBytes(key.Substring(0, 8));
    //        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
    //        inputByte = Convert.FromBase64String(encryptedText);
    //        MemoryStream ms = new MemoryStream();
    //        CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(DecryptKey, IV), CryptoStreamMode.Write);
    //        cs.Write(inputByte, 0, inputByte.Length);
    //        cs.FlushFinalBlock();
    //        System.Text.Encoding encoding = System.Text.Encoding.UTF8;
    //        return encoding.GetString(ms.ToArray());
    //    }

    //}
    //public static class MyExtensions
    //{
    //    public static MvcHtmlString EncodedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName,string Area, object routeValues, object htmlAttributes)
    //    {
    //        string queryString = string.Empty;
    //        string htmlAttributesString = string.Empty;
    //        if (routeValues != null)
    //        {
    //            RouteValueDictionary d = new RouteValueDictionary(routeValues);
    //            for (int i = 0; i < d.Keys.Count; i++)
    //            {
    //                if (i > 0)
    //                {
    //                    queryString += "?";
    //                }
    //                queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
    //            }
    //        }

    //        if (htmlAttributes != null)
    //        {
    //            RouteValueDictionary d = new RouteValueDictionary(htmlAttributes);
    //            for (int i = 0; i < d.Keys.Count; i++)
    //            {
    //                htmlAttributesString += " " + d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
    //            }
    //        }

    //        //<a href="/Answer?questionId=14">What is Entity Framework??</a>
    //        StringBuilder ancor = new StringBuilder();
    //        ancor.Append("<a ");
    //        if (htmlAttributesString != string.Empty)
    //        {
    //            ancor.Append(htmlAttributesString);
    //        }
    //        ancor.Append(" href='");
    //        if (Area != string.Empty)
    //        {
    //            ancor.Append("/" + Area);
    //        }
    //        if (controllerName != string.Empty)
    //        {
    //            ancor.Append("/" + controllerName);
    //        }

    //        if (actionName != "Index")
    //        {
    //            ancor.Append("/" + actionName);
    //        }
    //        if (queryString != string.Empty)
    //        {
    //            ancor.Append("?q=" + Encrypt(queryString));
    //        }
    //        ancor.Append("'");
    //        ancor.Append(">");
    //        ancor.Append(linkText);
    //        ancor.Append("</a>");
    //        return new MvcHtmlString(ancor.ToString());
    //    }

    //    private static string Encrypt(string plainText)
    //    {
    //        string key = "jdsg432387#";
    //        byte[] EncryptKey = { };
    //        byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
    //        EncryptKey = System.Text.Encoding.UTF8.GetBytes(key.Substring(0, 8));
    //        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
    //        byte[] inputByte = Encoding.UTF8.GetBytes(plainText);
    //        MemoryStream mStream = new MemoryStream();
    //        CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(EncryptKey, IV), CryptoStreamMode.Write);
    //        cStream.Write(inputByte, 0, inputByte.Length);
    //        cStream.FlushFinalBlock();
    //        return Convert.ToBase64String(mStream.ToArray());
    //    }
    //}
    public class CustomActionAttribute : FilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.OnActionExecuted = "IActionFilter.OnActionExecuted filter called";
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            ViewUserEmp _User = session["LoggedUser"] as ViewUserEmp;
            bool HavePermission = false;
            try
            {
                bool IsPost = filterContext.HttpContext.Request.HttpMethod == "POST";
                switch (filterContext.ActionDescriptor.ActionName)
                {
                    case "Create":
                        if (CheckCreateActionPermission(_User))
                            HavePermission = true;
                        break;
                    case "Edit":
                        if (CheckEditActionPermission(_User))
                            HavePermission = true;

                        break;
                    case "Details":
                        if (CheckDetailActionPermission(_User))
                            HavePermission = true;
                        break;
                    case "Delete":
                        if (CheckDeleteActionPermission(_User))
                            HavePermission = true;
                        break;
                    case "EmpProfileIndex":
                        if (CheckDetailActionPermission(_User))
                            HavePermission = true;
                        break;
                    case "EPAttendance":
                        if (CheckDetailActionPermission(_User))
                            HavePermission = true;
                        break;
                    case "EPContact":
                        if (CheckDetailActionPermission(_User))
                            HavePermission = true;
                        break;
                    case "EPJobDetail":
                        if (CheckDetailActionPermission(_User))
                            HavePermission = true;
                        break;
                    case "EPPersonal":
                        if (CheckDetailActionPermission(_User))
                            HavePermission = true;
                        break;
                }
                if (HavePermission == false)
                {
                    //filterContext.Result = new HttpUnauthorizedResult();
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, action = "Index" }));
                    filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);

                }
            }
            catch (Exception ex)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "VDSContainer" }));
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            }
        }

        private bool CheckDeleteActionPermission(ViewUserEmp _User)
        {
            if (_User.CanDelete == true)
                return true;
            else
                return false;
        }

        private bool CheckDetailActionPermission(ViewUserEmp _User)
        {
            if (_User.CanView == true)
                return true;
            else
                return false;
        }

        private bool CheckEditActionPermission(ViewUserEmp _User)
        {
            if (_User.CanEdit == true)
                return true;
            else
                return false;
        }

        private bool CheckCreateActionPermission(ViewUserEmp _User)
        {
            if (_User.CanAdd == true)
                return true;
            else
                return false;
        }
    }
}