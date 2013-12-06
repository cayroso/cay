using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace core.ServiceModels
{
    [Alias("core.Users")]
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        //public string Passw
    }
}
