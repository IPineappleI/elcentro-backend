namespace TangoSchool.DataAccess.Enums;

public enum AuditLogType
{
    Registration,
    Login,
    TokenRefresh,
    PasswordChanged,
    UserUpdated,
    TokenRefreshRevoked,
    ResetPasswordRequest,
    ResetPasswordConfirm
}
