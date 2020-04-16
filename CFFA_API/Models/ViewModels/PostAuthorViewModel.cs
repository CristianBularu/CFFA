using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFFA_API.Models.ViewModels
{
    public class PostAuthorViewModel : PostOnlyViewModel
    {
        public virtual ProfileViewModel User { get; set; }
    }
}
