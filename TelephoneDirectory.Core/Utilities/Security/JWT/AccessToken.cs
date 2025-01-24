﻿using System.Security.Claims;

namespace TelephoneDirectory.Core.Utilities.Security.JWT
{
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public IEnumerable<Claim>? Claims { get; set; }
    }
}
