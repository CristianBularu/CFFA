using System;
using System.Collections.Generic;

namespace CFFA_API.Models.ViewModels
{
    public class PostFullViewModel : PostAuthorViewModel
    {
        public string BodyText { get; set; }

        public string CreationTime { get; set; }

        public string ViewerId { get; set; }

        //public long SketchId { get; set; }
        public SketchViewModel sketch { get; set; }

        public UnderPostViewModel UnderPost { get; set; }
    }
}
