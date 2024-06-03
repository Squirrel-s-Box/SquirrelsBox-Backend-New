using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Generic.Extensions
{
    public static class ErrorMessagesExtensions
    {
        public static List<string> GetErrorMessages(Dictionary<string, List<string>> errorState)
        {
            var errors = new List<string>();
            foreach (var item in errorState)
            {
                errors.AddRange(item.Value);
            }
            return errors;
        }
    }
}
