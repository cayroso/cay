using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace app.ServiceDtos
{
    [Route("/request", "GET")]
    public class Dto : IReturn<DtoResponse>
    {
    }
    public class DtoResponse : IHasResponseStatus
    {
        public ResponseStatus ResponseStatus { get; set; }

        public string Message { get; set; }

        public DtoResponse()
        {
            Message = "the quick";
        }
    }
}
