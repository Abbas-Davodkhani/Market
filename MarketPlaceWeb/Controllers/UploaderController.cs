using Application.Extensions;
using Application.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;

namespace MarketPlaceWeb.Controllers
{
    public class UploaderController : SiteBaseController
    {
        [HttpPost]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncName, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;
            if(!upload.IsImage())
            {
                var notImageMessage = "لطفا یک تصویر انتخاب کنید ";
                var notImage = JsonConvert.DeserializeObject("{'Uploaded':0,'error':{'message':\" " + notImageMessage + " \"}}");
                return Json(notImage);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();
            upload.AddImageToServer(fileName, PathExtension.UploadImageOriginServer, null, null);

            return Json(new
            {
                Uploaded = true , 
                url = $"{PathExtension.UploadImageOrigin}{fileName}"
            });
        }
    }
}
