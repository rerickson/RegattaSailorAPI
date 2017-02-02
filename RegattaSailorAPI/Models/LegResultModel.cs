using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RegattaSailorAPI.Models
{
    [DataContract(IsReference = true)]
    public class LegResultModel
    {
        [Key]
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public Guid YachtId { get; set; }
        [DataMember]
        public DateTime? StartTime { get; set; }
        [DataMember]
        public DateTime? EndTime { get; set; }
        public virtual RaceLegModel Leg { get; set; }
    }
}