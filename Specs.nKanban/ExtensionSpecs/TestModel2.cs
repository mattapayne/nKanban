using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Specs.nKanban.Extensions
{
    public class TestModel2
    {
        [Display(Name = "FullName")]
        public string Name { get; set; }
    }
}
