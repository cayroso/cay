using app.ServiceDtos;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace app.ServiceInterfaces
{
    [Authenticate]
    public class TestService : Service
    {
        public DtoResponse Get(Dto request)
        {
            return new DtoResponse { };
        }
    }
}
