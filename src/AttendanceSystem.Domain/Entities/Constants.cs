namespace AttendanceSystem.Domain.Entities
{
    public class Constants
    {
        public const string Microservice = "Attendance Management";
        public const string ExportTypeCsv = "csv";
        public const string ExportTypeExcel = "excel";
        public const string ExportTypePdf = "pdf";

        public const string SuccessResponse = "Completed successfully";
        public const string ErrorResponse = "System error. Please contact support if this persists.";
        public const string LoggedForApproval = "Request successfully logged for approval.";

        public const string ErrorCode_MissingSystemAccounts = "900";
        public const string ErrorCode_MemberRecordNotFound = "901";
        public const string ErrorCode_UserProfileNotFound = "902";
        public const string ErrorCode_ReportNotFound = "903";
        public const string ErrorCode_InactiveAccount = "904";
        public const string ErrorCode_InvalidDetails = "905";
        public const string ErrorCode_RecordNotFound = "906";
        public const string ErrorCode_SystemError = "99";
    }

    public class CustomClaimTypes
    {
        public const string PermissionClaimType = "Permissions";
        public const string RoleClaimType = "Roles";
    }
}