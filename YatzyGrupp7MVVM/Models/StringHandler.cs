using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YatzyGrupp7MVVM.Models
{
    public static class StringHandler
    {
        public static string Message { get; set; }

        public static void SetString(string message)
        {
            Message = message;
        }
    }
}
