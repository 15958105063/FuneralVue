﻿using System.Collections.Generic;
using System.Security.Claims;

namespace Funeral.Core.Common.HttpContextUser
{
    public interface IUser
    {
        string Name { get; }
        int ID { get; }
        string TID { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();
        List<string> GetClaimValueByType(string ClaimType);

        string GetToken();
        List<string> GetUserInfoFromToken(string ClaimType);
    }
}
