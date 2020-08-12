using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Common;

namespace APEC.DOCS.Models
{
    public class DocumentModel
    {
        [Column(Name = "DOCUMENTID")]
        [Display(Name = "Id văn bản")]
//        [StringLength(20, ErrorMessage = "Bạn không được nhập quá 20 kí tự")]
        [Required(ErrorMessage = "Mã văn bản không được để trống")]
        public int DocumentId { get; set; }

        [Column(Name = "DOCUMENTTYPEID")]
        [Display(Name = "Danh mục tài liệu (*)")]
//        [StringLength(20, ErrorMessage = "Bạn không được nhập quá 20 kí tự")]
        [Required(ErrorMessage = "Danh mục tài liệu không được để trống")]
        public int DocumentTypeId { get; set; }

        [Column(Name = "DOCUMENTCODE")]
        [Display(Name = "Mã văn bản (*)")]
        [StringLength(20, ErrorMessage = "Bạn không được nhập quá 20 kí tự")]
        [Required(ErrorMessage = "Mã văn bản không được để trống")]
        public string DocumentCode { get; set; }

        [Column(Name = "DOCUMENTNO")]
        [Display(Name = "Số hiệu văn bản")]
        [StringLength(20, ErrorMessage = "Bạn không được nhập quá 20 kí tự")]
//        [Required(ErrorMessage = "Số hiệu văn bản không được để trống")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public string DocumentNo { get; set; }

        [Column(Name = "URL")]
        public string DocumentUrl { get; set; }

        [Column(Name = "NAME")]
        [Display(Name = "Tên tài liệu")]
//        [StringLength(400, ErrorMessage = "Bạn không được nhập quá 20 kí tự")]
//        [Required(ErrorMessage = "Tên tài liệu không được để trống")]
        public string DocumentName { get; set; }

        [Column(Name = "DISPLAYNAME")]
        [Display(Name = "Tên văn bản (*)")]
        [StringLength(400, ErrorMessage = "Bạn không được nhập quá 20 kí tự")]
        [Required(ErrorMessage = "Tên văn bản không được để trống")]
        public string DisplayName { get; set; }

        [Column(Name = "ORGPUBLISH")]
        [Display(Name = "Cơ quan ban hành")]
        [StringLength(400, ErrorMessage = "Bạn không được nhập quá 20 kí tự")]
//        [Required(ErrorMessage = "Cơ quan ban hành không được để trống")]
        public string OrgPublish { get; set; }

        [Column(Name = "DOCTYPENAME")]
        [Display(Name = "Tên dự án")]
        [StringLength(400, ErrorMessage = "Bạn không được nhập quá 20 kí tự")]
//        [Required(ErrorMessage = "Danh mục tài liệu không được để trống")]
        public string DocTypeName { get; set; }

        [Column(Name = "ACTIVEDATE")]
        [Display(Name = "Ngày hiệu lực")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ActiveDate { get; set; }

        [Column(Name = "EXPIREDATE")]
        [Display(Name = "Ngày hết hiệu lực")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpireDate { get; set; }


        [Column(Name = "BRIEFDESCRIPTION")]
        [Display(Name = "Mô tả")]
//        [StringLength(500, ErrorMessage = "Bạn không được nhập quá 20 kí tự")]
        public string BriefDescription { get; set; }

        public int Status { get; set; }

        public int IsActive { get; set; }

        [Display(Name = "Vị trí kho")]
        public string Ext { get; set; }

        [Display(Name = "Link tài liệu tham chiếu khi hết hiệu lực")]
        public string ExpireLink { get; set; }

        [Display(Name = "Tài liệu")]
        public HttpPostedFileBase[] Files { get; set; }
    }
}