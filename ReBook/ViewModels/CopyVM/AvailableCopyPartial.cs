using ReBook.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Condition = ReBook.Models.Condition;

namespace ReBook.ViewModels.CopyVM
{
    public class AvailableCopyPartial
    {
        public int CopyId { get; set; }
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        public string Visuals { get; set; }

        [Display(Name = "Condition :")]
        public Condition Condition { get; set; }

        [MaxLength(2000)]
        public string Comments { get; set; }
        public bool isUserPretender { get; set; } = false;
    }
}
