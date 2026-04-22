namespace TextileApp.Domain.Constants;

public static class RolesConstants
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string Sewer = "Sewer";
    public const string Warehouse = "Warehouse";

    public static readonly string[] All =
    {
        Admin, Manager, Sewer, Warehouse
    };
}