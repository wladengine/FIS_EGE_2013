
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIS_EGE_2013
{
    class EgeCertificateClass
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public int Year { get; set; }
        public string Number { get; set; }
        public string TypographicNumber { get; set; }
        public EgeStatus Status { get; set; }
        public List<EgeMarkClass> Marks { get; set; }
        public string FBSComment { get; set; }
    }

    public enum EgeStatus { IsOk, IsDeny }
}
