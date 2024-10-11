using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_Model.Model.Response
{
    public class ResponseModel
    {
        public long Id { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
