using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APEC.DOCS.Controllers
{
    public class FilesController : BaseController
    {
        public static List<FileUpload> list;
        // GET: Files
        public ActionResult Index()
        {
            return View(list);
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase[] files)
        {
            FileUpload model = new FileUpload();
            foreach (HttpPostedFileBase file in files)
            {
                if (files != null)
                {
                    var Extension = Path.GetExtension(file.FileName);
                    var fileName = "my-file-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Extension;
                    string path = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                    model.FileUrl = Url.Content(Path.Combine("~/UploadedFiles/", fileName));
                    model.FileName = fileName;

                    if (SaveFile(model))
                    {
                        file.SaveAs(path);
                        TempData["AlertMessage"] = "Uploaded Successfully !!";
                        return RedirectToAction("Index", "Files");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error In Add File. Please Try Again !!!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please Choose Correct File Type !!");
                    return View(model);
                }
            }

            return RedirectToAction("Index", "Files");
        }

        private bool SaveFile(FileUpload model)
        {
            list = new List<FileUpload>();
            list.Add(model);
            if (list.Count > 0)
                return true;
            else
                return false;
        }

        public ActionResult DownloadDoc(string fileName)
        {
            string filePath = "/UploadedFiles/" + fileName;
            string fullName = Server.MapPath("~" + filePath);

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);
        }

        public ActionResult DownloadDocZip(string fileName)
        {
            var docsName = fileName.Split(',');

            List<FileUpload> list = new List<FileUpload>();
            for (int i = 0; i < docsName.Length; i++)
            {
                string fileSavePath = Server.MapPath("~/UploadedFiles/" + docsName[i]);
                list.Add(new FileUpload()
                {
                    FileId = i + 1,
                    FileName = docsName[i],
                    FileUrl = fileSavePath
                });
            }

            var filesCol = list.ToList();
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    for (int i = 0; i < filesCol.Count; i++)
                    {
                        ziparchive.CreateEntryFromFile(filesCol[i].FileUrl, filesCol[i].FileName);
                    }
                }
                return File(memoryStream.ToArray(), "application /zip", "Docs.zip");
            }
        }



        public ActionResult DownloadFile(string filePath)
        {
            string fullName = Server.MapPath("~" + filePath);

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);
        }

        public ActionResult DownloadFileZip()
        {
            var filesCol = GetFileZip().ToList();
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    for (int i = 0; i < filesCol.Count; i++)
                    {
                        ziparchive.CreateEntryFromFile(filesCol[i].FileUrl, filesCol[i].FileName);
                    }
                }
                return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
            }
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

        public List<FileUpload> GetFileZip()
        {
            List<FileUpload> listFiles = new List<FileUpload>();
            string fileSavePath = System.Web.Hosting.HostingEnvironment.MapPath("~/UploadedFiles");
            DirectoryInfo dirInfo = new DirectoryInfo(fileSavePath);
            int i = 0;
            foreach (var item in dirInfo.GetFiles())
            {
                listFiles.Add(new FileUpload()
                {
                    FileId = i + 1,
                    FileName = item.Name,
                    FileUrl = dirInfo.FullName + @"\" + item.Name
                });
                i = i + 1;
            }

            return listFiles;
        }

        public List<FileUpload> GetFileTest()
        {
            List<FileUpload> listFiles = new List<FileUpload>();
            listFiles.Add(new FileUpload()
            {
                FileId = 1,
                FileName = "1.jpg",
                FileUrl = @"E:\APEC\Projects\ManageFiles\ManageFiles\UploadedFiles\1.jpg"
            });
            listFiles.Add(new FileUpload()
            {
                FileId = 2,
                FileName = "2.png",
                FileUrl = @"E:\APEC\Projects\ManageFiles\ManageFiles\UploadedFiles\2.png"
            });
            return listFiles;
        }

        public class FileUpload
        {
            public int FileId { get; set; }
            public string FileName { get; set; }
            public string FileUrl { get; set; }
            public IEnumerable<FileUpload> FileList { get; set; }
        }
    }
}