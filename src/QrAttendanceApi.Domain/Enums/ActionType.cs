using System.ComponentModel;

namespace QrAttendanceApi.Domain.Enums
{
    public enum ActionType
    {
        [Description("Create")]
        Create,
        [Description("Update")]
        Update,
        [Description("Delete")]
        Delete,
        [Description("Admin Override")]
        AdminOverride,
        [Description("Added Attendance Manually")]
        ManualAttendanceAdd,
        [Description("Updated Attendance Manually")]
        ManualAttendanceEdit,
        [Description("Deleted Attendance Manually")]
        ManualAttendanceDelete,
        [Description("User Account Update")]
        UserAccountUpdate,
        [Description("Role Change")]
        RoleChange,
        [Description("Login Success")]
        LoginSuccess,
        [Description("Login Failed")]
        LoginFailed
    }
}