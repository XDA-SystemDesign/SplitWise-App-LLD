using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Business.Exceptions
{
    public class ValidationException: Exception
    {
        public ValidationException(string ex): base(ex)
        {

        }
    }
}
