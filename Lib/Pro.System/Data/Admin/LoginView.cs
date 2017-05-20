using Nistec.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProSystem.Data.Entities
{
    public class LoginView
    {


        [EntityProperty(EntityPropertyType.Default)]
        public string UserName { get; set; }
        [EntityProperty(EntityPropertyType.Default)]
        public string Password { get; set; }
        [EntityProperty(EntityPropertyType.Default)]
        public bool RememberMe { get; set; }
    }
}
