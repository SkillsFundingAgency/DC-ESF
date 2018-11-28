using System;

namespace ESFA.DC.ESF.Models
{
    public class ILRFileDetailsModel
    {
        public string FileName { get; set; }

        public DateTime? LastSubmission { get; set; }

        public int Year { get; set; }
    }
}