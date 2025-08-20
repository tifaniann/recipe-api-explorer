using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheMealDBApp.Models
{
    public class Categories
    {
        public string IdCategory { get; set; }
        public string StrCategory { get; set; }
        public string StrCategoryThumb { get; set; }
        public string StrCategoryDescription { get; set; }

    }

    public class CategoriesResponse
    {
        public List<Categories> Categories { get; set; }
    }
}