using Core.Global.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Core.Global.Concrete
{
    public class Response<TData>  : IResponse<TData>
    {
        public Response()
        {
        }

        public Response(TData data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public Response(string message)
        {
            Succeeded = false;
            Message = message;
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }


        //public virtual Response<TData> SuccesResult()
        //{
        //    this.IsError = false;
        //    return this;
        //}

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public TData Data { get; set; }
 
    }

    //public class Response : Response<string>
    //{

    //}
}
