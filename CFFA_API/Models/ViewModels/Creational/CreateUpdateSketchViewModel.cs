using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CFFA_API.Models.ViewModels.Creational
{
    public class CreateUpdateSketchViewModel
    {
        [NonSerialized]
        public string UserId;
        [NonSerialized]
        public string Extension;

        public long? Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int? PageCount { get; set; }
        [Required]
        public float? PageHeight { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}
