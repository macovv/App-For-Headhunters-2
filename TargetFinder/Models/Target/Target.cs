using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TargetFinder.Models.Target
{
    public class Target
    {
        public int TargetId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public decimal Reward { get; set; }
        public string LastLocation { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}

