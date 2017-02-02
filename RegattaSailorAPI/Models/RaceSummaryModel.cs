using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegattaSailorAPI.Models
{
    public class RaceSummaryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
    }
}