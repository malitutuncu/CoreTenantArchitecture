using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Global.Interfaces
{
    public interface IResponse<TData>
    {
        bool Succeeded { get; set; }
        string Message { get; set; }
        List<string> Errors { get; set; }
        TData Data { get; set; }
    }
}
