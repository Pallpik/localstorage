using System;
using System.Collections.Generic;
using System.Text;

namespace HomeLocalStorage.Services
{
    public class AuthSettings
    {
        public TimeSpan Expires { get; set; }
        public string SecretKey { get; set; } = string.Empty;

    }
}
