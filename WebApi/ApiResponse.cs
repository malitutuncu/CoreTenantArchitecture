using Core.Global.Concrete;
using Core.Global.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    public class ApiResponse<TData> : IResponse<TData>
    {
        public ApiResponse()
        {
        }

        public ApiResponse(TData data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public ApiResponse(string message)
        {
            Succeeded = false;
            Message = message;
        }

        public JsonResult Result()
        {
            return new JsonResult(this);
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public TData Data { get; set; }

    }

    public class ApiResponse : ApiResponse<string>
    {

    }
}
