using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
 

namespace Core.Api
{
    public class Response<TData>
    {
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
        public TData Data { get; set; } 

        public JsonResult JsonResult()
        {
            return new JsonResult(this);
        }

        public JsonResult ErrorResult(string mesaj)
        {
            this.IsError = true;
            this.ErrorMessage = mesaj;
            return new JsonResult(this);
        }

        public JsonResult SuccesResult()
        {
            this.IsError = false;
            return new JsonResult(this);
        }

        public JsonResult SuccesResult(TData data)
        {
            this.IsError = false;
            this.Data = data;
            return new JsonResult(this);
        }
    }

    public class Response
    {
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }

        public JsonResult JsonResult()
        {
            return new JsonResult(this);
        }

        public JsonResult SuccesResult()
        {
            this.IsError = false;
            return new JsonResult(this);
        }

        public JsonResult ErrorResult(string mesaj)
        {
            this.IsError = true;
            this.ErrorMessage = mesaj;
            return new JsonResult(this);
        }
    }
}
 
