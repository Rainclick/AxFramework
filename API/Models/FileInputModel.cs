using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace API.Models
{
    public class FileInputModel
    {
        public IFormFile File { get; set; }
        public string Param { get; set; }
    }
}
