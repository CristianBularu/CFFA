using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFFA_API.Models.ViewModels
{
    public class PostFullWithCommentsViewModel : PostFullViewModel
    {
        public virtual List<CommentViewModel> Comments { get; set; }
    }
}
