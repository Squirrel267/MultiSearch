using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Search.Domain.Models
{
    public class SearchResultItem
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public string Request { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
    }
}
