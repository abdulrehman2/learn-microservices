using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;

namespace PlatformService.Models
{
    public class Platform
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Publisher { get; set; }

        [Required]
        public string Cost { get; set; }
    }





    public class Solution
    {
        public int MinOperations(string s)
        {
            var temp = s.ToArray();
            int noOfOperations = 0;
            for (var i = 0; i < temp.Count(); i++)
            {
                if (i + 1 < temp.Count())
                {
                    if (temp[i] != temp[i + 1])
                    {
                        noOfOperations++;
                    }
                }
                else
                {
                    if (temp[i - 1] != temp[i])
                    {
                        noOfOperations++;
                    }
                }
            }
            return noOfOperations;
        }
    }
}
