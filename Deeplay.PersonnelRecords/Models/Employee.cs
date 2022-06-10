using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deeplay.PersonnelRecords.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        public int GenderId { get; set; }
        public int PositionId { get; set; }
        public int SubdivisionId { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Position Position { get; set; }
        public virtual Subdivision Subdivision { get; set; }

        [NotMapped]
        public string UniqueInfo
        {
            get
            {
                switch (PositionId)
                {
                    case 2:
                        return Subdivision.Title;
                    case 4:
                        var director = Subdivision.Employees.SingleOrDefault(x => x.PositionId == 2);
                        if (director is not null)
                            return $"{director.LastName} {director.FirstName} {director.MiddleName}";
                        return string.Empty;
                }
                return string.Empty;
            }
        }
    }
}
