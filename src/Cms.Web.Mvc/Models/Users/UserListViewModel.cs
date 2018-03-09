using System.Collections.Generic;
using Cms.Roles.Dto;
using Cms.Users.Dto;

namespace Cms.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<UserDto> Users { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
