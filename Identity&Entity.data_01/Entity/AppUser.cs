using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity_Entity.data_01.Entity
{
    public class AppUser :IdentityUser
    {

        public List<Quote> Quotes { get; set; } = new List<Quote>();
    }
}
