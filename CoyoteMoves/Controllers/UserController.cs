using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.DirectoryServices.AccountManagement;
using System.Collections;

namespace CoyoteMoves.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {

        // GET api/User/GetUserName
        public string GetUserName()
        {
            return User.Identity.Name;
        }

        // GET api/User/GetUserEmail
        public string GetUserEmail()
        {
            UserPrincipal user = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain, "CoyoteLogistics"), User.Identity.Name);
            return user.EmailAddress;
        }

        // GET api/User/GetUserAuthType
        public string GetUserAuthType()
        {
            return User.Identity.AuthenticationType;
        }

        // GET api/User/GetUserRoles
        public ArrayList GetUserRoles()
        {
            ArrayList Roles = new ArrayList();

            UserPrincipal user = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain, "CoyoteLogistics"), User.Identity.Name);

            foreach (Principal Result in user.GetAuthorizationGroups())
            {
                Roles.Add(Result.Name);
            }
            return Roles;
        }

    }
}
