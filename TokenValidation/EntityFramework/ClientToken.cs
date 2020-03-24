using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TokenValidation.EntityFramework
{
    public class ClientToken
    {
        public Guid Id { get; set; }

        [Required, MaxLength(4000)]
        public string Token { get; set; }
        
        public DateTime CreatedTs { get; set; }
    }
}
