using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deeplay.PersonnelRecords.Models
{
    public class Position
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
