using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class EmployeesNotFoundException : NotFoundException
    {
        public EmployeesNotFoundException(Guid companyId) : base($"The Employees with id: {companyId} doesn't exist in the database.")
        {

        }
    }
}
