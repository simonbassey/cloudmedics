using System;
namespace CloudMedics.Domain.Enumerations
{
    [Flags]
    public enum RoleNames
    {
	    User,
        Staff,
        Doctor,
        Administrator,
        SuperAdministrator
    }
}
