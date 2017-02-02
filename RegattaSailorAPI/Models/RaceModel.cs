using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegattaSailorAPI.Models
{
    public class RaceModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual List<DivisionModel> Divisions { get; set; }
        public virtual List<RaceLegModel> Legs { get; set; }
        public DateTime StartTime { get; set; }
    }
}