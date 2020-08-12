using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AppEntities;
using APEC.DOCS.Models;
using DataAccess;
using APEC.DOCS.Common;
using APEC.DOCS.Helpers;

namespace APEC.DOCS.Controllers
{
//    [AuthorizationFilter]
    public class HomeController : BaseController
    {
        public string ConnectionString = ConfigurationManager.AppSettings.Get("OracleConnection");

        public static string[] lstDocName = null;

        public ActionResult Index()
        {
            //            string sqlGetListDocs = "SELECT * FROM DOCUMENT";
            //            List<DocumentModel> lstDocs = OracleHelper.ExecuteCommandText<DocumentModel>(ConnectionString, sqlGetListDocs);

            //            string sqlGetMenu = "SELECT * FROM APEC_DOCS_DOCTYPE WHERE TYPEID = 1";
            //            List<MenuModel> lstMenu = OracleHelper.ExecuteCommandText<MenuModel>(ConnectionString, sqlGetMenu);
            //            ViewBag.Menu = GenerateMenu(lstMenu);

            var sessionUser = (LoginModel)Session[CommonConstants.UserSession];
            if (sessionUser != null)
            {
                ViewBag.SSK = sessionUser.SessionKey;
            }
            string sqlGetRoleType = "select\r\n        ur.ROLETYPE_LIST || ',' || (select dr.ROLETYPE_LIST from apec_docs_department_role dr where dr.DEPARTMENT_ID = sd.dep_id) ROLETYPE_LIST\r\n        from sessions s\r\n       left join users u on s.user_id = u.user_id\r\n       left join apec_docs_user_role ur on u.staff_id = ur.staff_id\r\n       left join apec_staff_deparment sd on u.staff_id = sd.staff_id\r\n        where s.session_key = :0 and sd.dep_id is not null";
            List<RoleType> lstRoleType = OracleHelper.ExecuteCommandText<RoleType>(ConnectionString, sqlGetRoleType, sessionUser.SessionKey);
            if (lstRoleType.Count > 0)
            {
                ViewBag.RoleTypeString = lstRoleType[0].RoleTypeString;
            }

            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(DocumentModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var sessionUser = (LoginModel)Session[CommonConstants.UserSession];
                    List<string> lstNameDocs = new List<string>();
                    if (model.Files != null)
                    {
                        foreach (HttpPostedFileBase file in model.Files)
                        {
                            var fileName = file.FileName;
                            if (CheckFileType(fileName) == false)
                            {
                                ModelState.AddModelError("Files", "Bạn vui lòng chọn file đúng các định dạng trên!");
                                return View();
                            }

                            if (file.ContentLength > 52428800)
                            {
                                ModelState.AddModelError("Files", "Bạn vui lòng chọn file nhỏ hơn 50Mb!");
                                return View();
                            }
                            string path = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                            string url = Url.Content(Path.Combine("~/UploadedFiles/", fileName));
                            lstNameDocs.Add(fileName);
                            file.SaveAs(path);
                        }
                    }
                    string docName = string.Join(",", lstNameDocs);
                    string sql = "INSERT INTO APEC_DOCS_DOCUMENT(DOCUMENTID, DOCUMENTCODE, DOCUMENTNO, DOCUMENTTYPEID, DOCTYPENAME, NAME, DISPLAYNAME, ACTIVEDATE, ORGPUBLISH, BRIEFDESCRIPTION, STATUS, ISACTIVE, EXT, EXPIREDATE, EXPIRELINK, CREATEDDATE, CREATEDBY)" +
                                        "VALUES(SEQ_DOCUMENT.NEXTVAL, :0, :1, :2, :3, :4, :5, :6, :7, :8, :9, :10, :11, :12, :13, :14, :15)";
                    OracleHelper.ExecuteCommandText(ConnectionString, sql, model.DocumentCode, model.DocumentNo, model.DocumentTypeId, model.DocTypeName, docName, model.DisplayName, model.ActiveDate, model.OrgPublish, model.BriefDescription, model.Status, model.IsActive, model.Ext, model.ExpireDate, model.ExpireLink, DateTime.Now, sessionUser.Username);
                    //                    OracleHelper.ExecuteStoreProcedure(ConnectionString, "SP_INSERT_DOC", model.DocumentCode, model.DocumentNo, model.DocumentTypeId, model.DocTypeName, model.DocTypeName, model.DisplayName, model.ActiveDate, model.OrgPublish, model.BriefDescription, model.Status);
                    TempData["Message"] = "Added";
                    return RedirectToAction("Create", "Home");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            string sqlGetDocs = "SELECT * FROM APEC_DOCS_DOCUMENT WHERE DOCUMENTID = :0";
            List<Document> lstDocs = OracleHelper.ExecuteCommandText<Document>(ConnectionString, sqlGetDocs, id);
            var model = new DocumentModel();
            model.DocumentCode = lstDocs[0].DocumentCode;
            model.DisplayName = lstDocs[0].DisplayName;
            model.ActiveDate = lstDocs[0].ActiveDate;
            model.Status = lstDocs[0].Status;
            model.BriefDescription = lstDocs[0].BriefDescription;
            model.DocTypeName = lstDocs[0].DocTypeName;
            model.Ext = lstDocs[0].Ext;
            model.ExpireDate = lstDocs[0].ExpireDate;
            model.IsActive = lstDocs[0].IsActive;
            model.OrgPublish = lstDocs[0].OrgPublish;
            model.DocumentNo = lstDocs[0].DocumentNo;
            model.DocumentTypeId = lstDocs[0].DocumentTypeId;
            model.ExpireLink = lstDocs[0].ExpireLink;
            model.DocumentId = lstDocs[0].DocumentId;
            ViewBag.ListDocName = lstDocs[0].DocumentName.Split(',');
            lstDocName = lstDocs[0].DocumentName.Split(',');
            ViewBag.DocTypeId = lstDocs[0].DocumentTypeId;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(DocumentModel model)
        {
            ViewBag.ListDocName = lstDocName;
            if (ModelState.IsValid)
            {
                try
                {
                    var sessionUser = (LoginModel)Session[CommonConstants.UserSession];
                    List<string> lstNameDocs = new List<string>();
                    foreach (HttpPostedFileBase file in model.Files)
                    {
                        if (file != null)
                        {
                            var fileName = file.FileName;
                            if (CheckFileType(fileName) == false)
                            {
                                ModelState.AddModelError("Files", "Bạn vui lòng chọn file đúng các định dạng trên!");
                                return View();
                            }

                            if (file.ContentLength > 52428800)
                            {
                                ModelState.AddModelError("Files", "Bạn vui lòng chọn file nhỏ hơn 50Mb!");
                                return View();
                            }
                            string path = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                            string url = Url.Content(Path.Combine("~/UploadedFiles/", fileName));
                            lstNameDocs.Add(fileName);
                            file.SaveAs(path);
                        }
                    }
                    lstNameDocs.AddRange(lstDocName);
                    string docName = string.Join(",", lstNameDocs);
                    ViewBag.Err = "2" + docName;
                    string sql = "update APEC_DOCS_DOCUMENT set \r\nDOCUMENTCODE = :0, DOCUMENTNO = :1, DOCUMENTTYPEID = :2, DOCTYPENAME = :3, NAME = :4, DISPLAYNAME = :5\r\n, ACTIVEDATE = :6, ORGPUBLISH = :7, BRIEFDESCRIPTION = :8, STATUS = :9, ISACTIVE = :10, EXT = :11, EXPIREDATE = :12, EXPIRELINK = :13, MODIFIEDDATE = :14, MODIFIEDBY = :15\r\nwhere DOCUMENTID = :16";
                    OracleHelper.ExecuteCommandText(ConnectionString, sql, model.DocumentCode, model.DocumentNo, model.DocumentTypeId, model.DocTypeName, docName, model.DisplayName, model.ActiveDate, model.OrgPublish, model.BriefDescription, model.Status, model.IsActive, model.Ext, model.ExpireDate, model.ExpireLink, DateTime.Now, sessionUser.Username, model.DocumentId);
                    TempData["Message"] = "Edited";
                    return RedirectToAction("Index", "Home");
                    //                    OracleHelper.ExecuteStoreProcedure(ConnectionString, "SP_INSERT_DOC", model.DocumentCode, model.DocumentNo, model.DocumentTypeId, model.DocTypeName, model.DocTypeName, model.DisplayName, model.ActiveDate, model.OrgPublish, model.BriefDescription, model.Status);
                    //                    return RedirectToAction("Index", "Home");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View();
        }

        public JsonResult Delete(int id)
        {
            string sql = "UPDATE APEC_DOCS_DOCUMENT SET IS_DELETED = 'Y' WHERE DOCUMENTID = :0";
            OracleHelper.ExecuteCommandText(ConnectionString, sql, id);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveDocType(DocumentTypeModel model)
        {
            var sessionUser = (LoginModel)Session[CommonConstants.UserSession];
            
            if (model.Action == "Create")
            {
                List<DocumentType> lst = OracleHelper.ExecuteStoreProcedure<DocumentType>(ConnectionString, "sp_insert_doc_type", model.Name, model.ParentId, sessionUser.Username);
                return Json(lst[0].MenuUserId, JsonRequestBehavior.AllowGet);
            }
            if(model.Action == "Edit")
            {
                List<DocumentType> lst = OracleHelper.ExecuteStoreProcedure<DocumentType>(ConnectionString, "sp_update_doc_type", model.Id, model.Name, model.ParentId, sessionUser.Username);
                return Json(lst[0].MenuUserId, JsonRequestBehavior.AllowGet);
            }
            if (model.Action == "Delete")
            {
                string sql = "UPDATE apec_docs_doctype SET IS_DELETED = 'Y' WHERE MENUUSERID = :0";
                OracleHelper.ExecuteCommandText(ConnectionString, sql, model.Id);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            throw new NotImplementedException();
        }


        public ActionResult CheckPermission()
        {
            return View();
        }

        public string GenerateMenu(List<MenuModel> lst)
        {
            var strBuilder = new StringBuilder();
            string sqlGetMenu = "SELECT * FROM APEC_DOCS_DOCTYPE WHERE PARENTID IS NULL and TYPEID = 1";
            List<MenuModel> parentItems = OracleHelper.ExecuteCommandText<MenuModel>(ConnectionString, sqlGetMenu);
            //            List<MenuModel> parentItems = (from a in lst where a.ParentId == 0 select a).ToList();
            strBuilder.Append("<ul>");
            foreach (var pItem in parentItems)
            {
                //                strBuilder.Append("<li onClick='getListDocs(" + pItem.Id + ")'>");
                strBuilder.Append("<li id='" + pItem.Id + "'>");
                strBuilder.Append(pItem.Name);
                string sqlGetMenuChilds = "SELECT * FROM APEC_DOCS_DOCTYPE WHERE PARENTID = :0";
                List<MenuModel> childItems = OracleHelper.ExecuteCommandText<MenuModel>(ConnectionString, sqlGetMenuChilds, pItem.Id);
                if (childItems.Count > 0)
                    AddChildItem(lst, pItem, strBuilder);
                strBuilder.Append("</li>");
            }
            strBuilder.Append("</ul>");
            return strBuilder.ToString();
        }

        private void AddChildItem(List<MenuModel> lst, MenuModel item, StringBuilder strBuilder)
        {
            strBuilder.Append("<ul>");
            string sqlGetMenuChilds = "SELECT * FROM APEC_DOCS_DOCTYPE WHERE PARENTID = :0";
            List<MenuModel> childItems = OracleHelper.ExecuteCommandText<MenuModel>(ConnectionString, sqlGetMenuChilds, item.Id);
            //            List<MenuModel> childItems = (from a in lst where a.ParentId == item.Id select a).ToList();
            foreach (var cItem in childItems)
            {
                //                strBuilder.Append("<li onClick='getListDocs("+ cItem.Id + ")'>");
                strBuilder.Append("<li id='" + cItem.Id + "'>");
                strBuilder.Append(cItem.Name);
                string sqlGetMenuSubChilds = "SELECT * FROM APEC_DOCS_DOCTYPE WHERE PARENTID = :0";
                List<MenuModel> subChilds = OracleHelper.ExecuteCommandText<MenuModel>(ConnectionString, sqlGetMenuSubChilds, cItem.Id);
                //                List<MenuModel> subChilds = (from a in lst where a.ParentId == cItem.Id select a).ToList();
                if (subChilds.Count > 0)
                {
                    AddChildItem(lst, cItem, strBuilder);
                }
                strBuilder.Append("</li>");
            }
            strBuilder.Append("</ul>");
        }

        public JsonResult GetListMenu()
        {
            var sessionUser = (LoginModel)Session[CommonConstants.UserSession];
            //            string sqlGetMenu = "SELECT * FROM APEC_DOCS_DOCTYPE WHERE TYPEID = 1";
            List<MenuModel> lstMenu = OracleHelper.ExecuteStoreProcedure<MenuModel>(ConnectionString, "SP_GET_DOCTYPE",
                sessionUser.SessionKey);
            List<MenuModel> lstAllMenu = OracleHelper.ExecuteCommandText<MenuModel>(ConnectionString, "SELECT * FROM apec_docs_doctype a WHERE a.is_deleted = 'N'");
            return Json(new { ListMenu = lstMenu, SSK = sessionUser, ListAllMenu = lstAllMenu }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListDocs(int id)
        {
            var sessionUser = (LoginModel)Session[CommonConstants.UserSession];
            List<Document> lstDocs = OracleHelper.ExecuteStoreProcedure<Document>(ConnectionString, "SP_GET_DOCS", id, sessionUser.SessionKey);
            return Json(new { ListDocs = lstDocs }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchListDocs(string groupId, string docName, string docType, string orgPublish, string docContent)
        {
            var sessionUser = (LoginModel)Session[CommonConstants.UserSession];
            //            string sqlGetListDocs = "select * from document d where d.documenttypeid in ( \r\nselect menuuserid\r\nfrom menuuser a\r\nwhere a.typeid = 1\r\nstart with parentid = :0 \r\nconnect by prior menuuserid = parentid\r\nunion\r\nselect menuuserid from menuuser where menuuserid = :1 )\r\nand (d.displayname like '%' || :2  ||  '%' or :2 is null) \r\nand (d.doctypename like '%' || :3  ||  '%' or :3 is null) \r\nand (d.orgpublish like '%' || :4  ||  '%' or :4 is null) \r\nand (to_char(d.activedate, 'dd/mm/yyyy')  like '%' || :5 ||  '%' or :5 is null)";
            //            List<DocumentModel> lstDocs = OracleHelper.ExecuteCommandText<DocumentModel>(ConnectionString, sqlGetListDocs, groupId, groupId, docName, docType, orgPublish, activeDate);
            List<Document> lstDocs = OracleHelper.ExecuteStoreProcedure<Document>(ConnectionString, "SP_SEARCH_DOCS", groupId,
                docName, docType, orgPublish, docContent, sessionUser.SessionKey);
            return Json(new { ListDocs = lstDocs}, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult ChangeDocumentUpload(string docName)
        {
            try
            {
                var listArray = lstDocName.Where(x => x != docName).ToArray();
                lstDocName = listArray;
                return Json(new { ListArray = listArray, Status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Dictionary<string, object> GetErrorsFromModelState()
        {
            var errors = new Dictionary<string, object>();

            foreach (var key in ModelState.Keys)
            {
                if (ModelState[key].Errors.Count > 0)
                {
                    errors[key] = ModelState[key].Errors;
                }
            }

            return errors;
        }

        bool CheckFileType(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            switch (ext.ToLower())
            {
                case ".txt":
                    return true;
                case ".doc":
                    return true;
                case ".docx":
                    return true;
                case ".pptx":
                    return true;
                case ".ppt":
                    return true;
                case ".xls":
                    return true;
                case ".xlsx":
                    return true;
                case ".pdf":
                    return true;
                case ".tif":
                    return true;
                case ".zip":
                    return true;
                case ".rar":
                    return true;
                case ".xps":
                    return true;
                default:
                    return false;
            }
        }
    }
}