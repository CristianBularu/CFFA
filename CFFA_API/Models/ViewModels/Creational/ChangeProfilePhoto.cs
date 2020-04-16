using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFFA_API.Models.ViewModels.Creational
{
    public class ChangeProfilePhoto
    {
        public IFormFile File { get; set; }
    }
}
