using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RegattaSailorAPI.Models
{
    [DataContract(IsReference = true)]
    public class DivisionModel
    {
        [Key]
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<YachtModel> Yachts { get; set; }
        [DataMember]
        public int Index { get; set; }
        [DataMember]
        public DateTime? StartTime { get; set; }

        [DataMember]
        public Guid RaceId { get; set; }
        [ForeignKey("RaceId")]
        public RaceModel Race {get; set;}
    }
}