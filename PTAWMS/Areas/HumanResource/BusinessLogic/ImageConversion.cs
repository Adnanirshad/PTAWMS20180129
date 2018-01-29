using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace PTAWMS.Areas.HumanResource.BusinessLogic
{
    public class ImageConversion
    {

        public int UploadImageInDataBase(string Path, HR_Employee _Emp)
        {
            using (var context = new HRMEntities())
            {
                List<HR_EmpImage> _empPhotoList = new List<HR_EmpImage>();
                HR_EmpImage _EmpPhoto = new HR_EmpImage();
                int empPhotoID = 0;
                _empPhotoList = context.HR_EmpImage.Where(aa => aa.EmpID == _Emp.EmployeeID).ToList();
                Image img = Image.FromFile(Path);
                byte[] bArr = imgToByteArray(img);
                _EmpPhoto.EmpPic = bArr;
                if (_empPhotoList.Count > 0)
                {
                    //Update Existing Image
                    _EmpPhoto = context.HR_EmpImage.First(aa => aa.EmpID == _Emp.EmployeeID);
                    _EmpPhoto.EmpID = _empPhotoList.FirstOrDefault().EmpID;
                    _EmpPhoto.EmpPic = bArr;
                }
                else
                {
                    //Add New Image
                    _EmpPhoto.EmpID = _Emp.EmployeeID;
                    context.HR_EmpImage.Add(_EmpPhoto);
                }
                try
                {
                    //empPhotoID = _EmpPhoto.EmpPic;
                    context.SaveChanges();
                    return empPhotoID;
                }
                catch (Exception ex)
                {
                    return empPhotoID;
                }
            }

        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            Image img = Image.FromStream(image.InputStream);
            Image conImage = ScaleImage(img, 230, 500);
            byte[] imageBytes = null;
            imageBytes = imgToByteArray(conImage);
            return imageBytes;
        }
        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
        public byte[] imgToByteArray(Image img)
        {
            var ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
        public int UploadImageInDataBase(HttpPostedFileBase file, int empID)
        {

            using (var context = new HRMEntities())
            {
                List<HR_EmpImage> _empPhotoList = new List<HR_EmpImage>();
                HR_EmpImage _EmpPhoto = new HR_EmpImage();
                HR_Employee _Emp = new HR_Employee();
                int empPhotoID = 0;
                _Emp = context.HR_Employee.First(aa => aa.EmployeeID == empID);
                if (context.HR_EmpImage.Where(aa => aa.EmpID == _Emp.EmployeeID).Count() > 0)
                {
                    //Update Existing Image
                    _EmpPhoto = context.HR_EmpImage.First(aa => aa.EmpID == _Emp.EmployeeID);
                    _EmpPhoto.EmpPic = ConvertToBytes(file);
                }
                else
                {
                    //Add New Image
                    _EmpPhoto.EmpPic = ConvertToBytes(file);
                    _EmpPhoto.EmpID = _Emp.EmployeeID;
                    context.HR_EmpImage.Add(_EmpPhoto);
                }
                try
                {
                    empPhotoID = _EmpPhoto.EmpID;
                    context.SaveChanges();
                    return empPhotoID;
                }
                catch (Exception ex)
                {
                    return empPhotoID;
                }
            }
        }
    }
}