using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommandsService.Dtos
{
    public class CommandToCreate
    {
        [Required]
        public string HowTo { get; set; }

        [Required]
        public string CommandLine { get; set; }
    }
}
