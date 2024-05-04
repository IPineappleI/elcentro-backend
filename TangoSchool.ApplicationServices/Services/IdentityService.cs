using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TangoSchool.ApplicationServices.Constants;
using TangoSchool.ApplicationServices.Extensions;
using TangoSchool.ApplicationServices.Models.Emails;
using TangoSchool.ApplicationServices.Models.Identities;
using TangoSchool.ApplicationServices.Options;
using TangoSchool.ApplicationServices.Services.Interfaces;
using TangoSchool.ApplicationServices.Utilities;
using TangoSchool.DataAccess.DatabaseContexts.Interfaces;
using TangoSchool.DataAccess.Entities;
using TangoSchool.DataAccess.Enums;
using TangoSchool.DataAccess.Repositories.Interfaces;

namespace TangoSchool.ApplicationServices.Services;

internal class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IReadOnlyTangoSchoolDbContext _readOnlyTangoSchoolDbContext;
    private readonly JwtOptions _jwtOptions;
    private readonly IEmailSender _emailSender;
    private readonly IStudentsRepository _studentsRepository;
    private readonly ITeachersRepository _teachersRepository;
    private readonly IAdministratorsRepository _administratorsRepository;
    private readonly IAuditLogsRepository _auditLogsRepository;

    public IdentityService
    (
        UserManager<ApplicationUser> userManager,
        IOptions<JwtOptions> jwtOptions,
        IEmailSender emailSender,
        IStudentsRepository studentsRepository,
        IReadOnlyTangoSchoolDbContext readOnlyTangoSchoolDbContext,
        IAuditLogsRepository auditLogsRepository,
        ITeachersRepository teachersRepository,
        IAdministratorsRepository administratorsRepository
    )
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
        _emailSender = emailSender;
        _studentsRepository = studentsRepository;
        _readOnlyTangoSchoolDbContext = readOnlyTangoSchoolDbContext;
        _auditLogsRepository = auditLogsRepository;
        _teachersRepository = teachersRepository;
        _administratorsRepository = administratorsRepository;
    }

    public async Task<AuthResponse> Authenticate(AuthRequest request, CancellationToken cancellationToken)
    {
        var managedUser = await _userManager.FindByEmailAsync(request.Email);

        if (managedUser is null)
        {
            throw new ApplicationException(GeneralErrorMessages.BadCredentials);
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);

        if (!isPasswordValid)
        {
            throw new ApplicationException(GeneralErrorMessages.BadCredentials);
        }

        var roleIds = await _readOnlyTangoSchoolDbContext.UserRoles
            .Where(r => r.UserId == managedUser.Id)
            .Select(x => x.RoleId)
            .ToListAsync(cancellationToken);

        var roles = _readOnlyTangoSchoolDbContext.Roles.Where(x => roleIds.Contains(x.Id)).ToList();

        var accessToken = CreateToken(managedUser, roles);
        managedUser.RefreshToken = JwtHelpers.GenerateRefreshToken();
        managedUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.TokenValidityInMinutes);

        var auditLog = new AuditLog()
        {
            ApplicationUser = managedUser,
            Timestamp = DateTime.UtcNow,
            AuditLogType = AuditLogType.Login
        };

        _auditLogsRepository.Add(auditLog);

        await _auditLogsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return new()
        {
            Username = managedUser.UserName!,
            Email = managedUser.Email!,
            AccessToken = accessToken,
            RefreshToken = managedUser.RefreshToken
        };
    }

    public async Task<UserInformationWithRoles> GetUserInformation(Guid userId, CancellationToken cancellationToken)
    {
        var managedUser = await _userManager.FindByIdAsync(userId.ToString());

        if (managedUser is null)
        {
            throw new ApplicationException(GeneralErrorMessages.BadCredentials);
        }

        return new()
        {
            UserId = managedUser.Id,
            Email = managedUser.Email!,
            FirstName = managedUser.FirstName,
            MiddleName = managedUser.MiddleName,
            LastName = managedUser.LastName,
            Photo = managedUser.Photo,
            PhoneNumber = managedUser.PhoneNumber,
            Roles = await _userManager.GetRolesAsync(managedUser)
        };
    }

    private async Task<ApplicationUser> RegisterUser
    (
        RegisterUserPayload userPayload,
        CancellationToken cancellationToken
    )
    {
        var user = new ApplicationUser
        {
            FirstName = userPayload.FirstName,
            LastName = userPayload.LastName,
            MiddleName = userPayload.MiddleName,
            Email = userPayload.Email,
            UserName = userPayload.Email,
            PhoneNumber = userPayload.PhoneNumber,
            Photo = userPayload.Photo
        };

        var result = await _userManager.CreateAsync(user, userPayload.Password);

        ProcessIdentityResult(result);

        try
        {
            var email = new SendEmailPayload
            (
                EmailConstants.SuccessRegistration.Subject,
                string.Format
                (
                    EmailConstants.SuccessRegistration.Body,
                    user.FirstName,
                    userPayload.Password
                ),
                user.Email
            );

            await _emailSender.SendEmail(email, cancellationToken);
        }
        catch (Exception)
        {
            await _userManager.DeleteAsync(user);

            throw;
        }
        
        var auditLog = new AuditLog()
        {
            ApplicationUser = user,
            Timestamp = DateTime.UtcNow,
            AuditLogType = AuditLogType.Registration
        };

        _auditLogsRepository.Add(auditLog);

        await _auditLogsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task RegisterStudent
    (
        RegisterStudentPayload userPayload,
        CancellationToken cancellationToken
    )
    {
        var password = GeneratePassword(PasswordConstants.DefaultPasswordLength);

        var registerUserData = new RegisterUserPayload()
        {
            Email = userPayload.Email,
            Password = password,
            FirstName = userPayload.FirstName,
            MiddleName = userPayload.MiddleName,
            LastName = userPayload.LastName,
            PhoneNumber = userPayload.PhoneNumber,
            Photo = userPayload.Photo,
            Description = userPayload.Description
        };

        var user = await RegisterUser(registerUserData, cancellationToken);

        var result = await _userManager.AddToRoleAsync(user, RoleConstants.Student);

        ProcessIdentityResult(result);

        var student = new Student()
        {
            ApplicationUser = user,
            Level = userPayload.Level
        };

        _studentsRepository.Add(student);

        await _studentsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RegisterTeacher
    (
        RegisterTeacherPayload userPayload,
        CancellationToken cancellationToken
    )
    {
        var password = GeneratePassword(PasswordConstants.DefaultPasswordLength);

        var registerUserData = new RegisterUserPayload()
        {
            Email = userPayload.Email,
            Password = password,
            FirstName = userPayload.FirstName,
            MiddleName = userPayload.MiddleName,
            LastName = userPayload.LastName,
            PhoneNumber = userPayload.PhoneNumber,
            Photo = userPayload.Photo,
            Description = userPayload.Description
        };

        var user = await RegisterUser(registerUserData, cancellationToken);

        var result = await _userManager.AddToRoleAsync(user, RoleConstants.Teacher);

        ProcessIdentityResult(result);

        var teacher = new Teacher()
        {
            ApplicationUser = user,
        };

        _teachersRepository.Add(teacher);

        await _teachersRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RegisterAdministrator
    (
        RegisterAdministratorPayload userPayload,
        CancellationToken cancellationToken
    )
    {
        var password = GeneratePassword(PasswordConstants.DefaultPasswordLength);

        var registerUserData = new RegisterUserPayload()
        {
            Email = userPayload.Email,
            Password = password,
            FirstName = userPayload.FirstName,
            MiddleName = userPayload.MiddleName,
            LastName = userPayload.LastName,
            PhoneNumber = userPayload.PhoneNumber,
            Photo = userPayload.Photo,
            Description = userPayload.Description
        };

        var user = await RegisterUser(registerUserData, cancellationToken);

        var result = await _userManager.AddToRoleAsync(user, RoleConstants.Administrator);

        ProcessIdentityResult(result);

        var administrator = new Administrator()
        {
            ApplicationUser = user
        };

        _administratorsRepository.Add(administrator);

        await _administratorsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetAllUsersResponse> GetAllUsers(GetAllUsersPayload payload, CancellationToken cancellationToken)
    {
        var usersInformation = await _readOnlyTangoSchoolDbContext
            .Users
            .Join
            (
                _readOnlyTangoSchoolDbContext
                    .UserRoles,
                x => x.Id,
                x => x.UserId,
                (x, y) => new {User = x, y.RoleId}
            )
            .Join
            (
                _readOnlyTangoSchoolDbContext
                    .Roles,
                x => x.RoleId,
                x => x.Id,
                (x, y) => new {x.User, RoleName = y.Name}
            )
            .Paginate(payload.ItemsPerPage, payload.Page)
            .Select(x => new UserInformation()
            {
                UserId = x.User.Id,
                Email = x.User.Email!,
                FirstName = x.User.FirstName,
                MiddleName = x.User.MiddleName,
                LastName = x.User.LastName,
                PhoneNumber = x.User.PhoneNumber,
                Photo = x.User.Photo,
                Role = x.RoleName
            })
            .ToListAsync(cancellationToken);

        var total = await _readOnlyTangoSchoolDbContext.Users.CountAsync(cancellationToken);

        return new()
        {
            Users = usersInformation,
            TotalCount = total
        };
    }

    public async Task Delete(Guid userId, CancellationToken cancellationToken)
    {
        var managedUser = await _userManager.FindByIdAsync(userId.ToString());

        if (managedUser is null)
        {
            throw new ApplicationException(GeneralErrorMessages.UserWasNotFound);
        }

        var result = await _userManager.DeleteAsync(managedUser);

        ProcessIdentityResult(result);
    }

    public async Task UpdateUser(Guid userId, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var managedUser = await _userManager.FindByIdAsync(userId.ToString());

        if (managedUser is null)
        {
            throw new ApplicationException(GeneralErrorMessages.UserWasNotFound);
        }

        managedUser.PhoneNumber = request.PhoneNumber;
        managedUser.FirstName = request.FirstName;
        managedUser.LastName = request.LastName;
        managedUser.MiddleName = request.MiddleName;
        managedUser.Photo = request.Photo;

        managedUser.AuditLogs.Add(new()
        {
            Timestamp = DateTime.UtcNow,
            AuditLogType = AuditLogType.UserUpdated
        });

        var result = await _userManager.UpdateAsync(managedUser);

        ProcessIdentityResult(result);
    }

    public async Task ChangePassword(Guid userId, ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var managedUser = await _userManager.FindByIdAsync(userId.ToString());

        if (managedUser is null)
        {
            throw new ApplicationException(GeneralErrorMessages.UserWasNotFound);
        }

        var result = await _userManager.ChangePasswordAsync(managedUser, request.CurrentPassword, request.NewPassword);

        ProcessIdentityResult(result);

        managedUser.AuditLogs.Add(new()
        {
            Timestamp = DateTime.UtcNow,
            AuditLogType = AuditLogType.PasswordChanged
        });

        result = await _userManager.UpdateAsync(managedUser);

        ProcessIdentityResult(result);
    }

    public async Task<TokenModel> RefreshToken(TokenModel tokenModel, CancellationToken cancellationToken)
    {
        var accessToken = tokenModel.AccessToken;
        var refreshToken = tokenModel.RefreshToken;
        var principal = JwtHelpers.GetPrincipalFromExpiredToken(_jwtOptions, accessToken);

        if (principal is null)
        {
            throw new ApplicationException(GeneralErrorMessages.InvalidToken);
        }

        var username = principal.Identity!.Name;
        var user = await _userManager.FindByNameAsync(username!);

        if (user == null
            || user.RefreshToken != refreshToken
            || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new ApplicationException(GeneralErrorMessages.InvalidToken);
        }

        var newAccessToken = JwtHelpers.CreateToken(_jwtOptions, principal.Claims.ToList());
        var newRefreshToken = JwtHelpers.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;

        user.AuditLogs.Add(new()
        {
            Timestamp = DateTime.UtcNow,
            AuditLogType = AuditLogType.TokenRefresh
        });

        var result = await _userManager.UpdateAsync(user);

        ProcessIdentityResult(result);

        return new()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = newRefreshToken
        };
    }

    public async Task RevokeToken(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            throw new ApplicationException(GeneralErrorMessages.UserWasNotFound);
        }

        user.RefreshToken = null;
        user.AuditLogs.Add(new()
        {
            Timestamp = DateTime.UtcNow,
            AuditLogType = AuditLogType.TokenRefreshRevoked
        });

        var result = await _userManager.UpdateAsync(user);

        ProcessIdentityResult(result);
    }

    public async Task RevokeAll(CancellationToken cancellationToken)
    {
        var users = _userManager.Users.ToList();
        foreach (var user in users)
        {
            user.RefreshToken = null;
            user.AuditLogs.Add(new()
            {
                Timestamp = DateTime.UtcNow,
                AuditLogType = AuditLogType.TokenRefreshRevoked
            });

            var result = await _userManager.UpdateAsync(user);

            ProcessIdentityResult(result);
        }
    }

    public async Task RequestResetPassword
    (
        RequestResetPasswordPayload payload,
        CancellationToken cancellationToken
    )
    {
        var user = await _userManager.FindByEmailAsync(payload.Email);

        if (user is null)
        {
            throw new ApplicationException(GeneralErrorMessages.UserWasNotFound);
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var email = new SendEmailPayload
        (
            EmailConstants.ResetPassword.Subject,
            string.Format
            (
                EmailConstants.ResetPassword.Body,
                user.FirstName,
                user.Email,
                token
            ),
            user.Email!
        );

        await _emailSender.SendEmail(email, cancellationToken);

        user.AuditLogs.Add(new()
        {
            Timestamp = DateTime.UtcNow,
            AuditLogType = AuditLogType.ResetPasswordRequest
        });

        var result = await _userManager.UpdateAsync(user);

        ProcessIdentityResult(result);
    }

    public async Task ConfirmResetPassword(ConfirmResetPasswordPayload payload, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(payload.Email);

        if (user is null)
        {
            throw new ApplicationException(GeneralErrorMessages.UserWasNotFound);
        }

        var result = await _userManager.ResetPasswordAsync(user, payload.ResetPasswordToken, payload.NewPassword);

        ProcessIdentityResult(result);

        user.AuditLogs.Add(new()
        {
            Timestamp = DateTime.UtcNow,
            AuditLogType = AuditLogType.ResetPasswordConfirm
        });

        result = await _userManager.UpdateAsync(user);

        ProcessIdentityResult(result);
    }

    private string CreateToken(ApplicationUser user, List<IdentityRole<Guid>> roles)
    {
        var token = user
            .CreateClaims(roles)
            .CreateJwtToken(_jwtOptions);
        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(token);
    }

    private static void ProcessIdentityResult(IdentityResult result)
    {
        if (result.Errors.Any())
        {
            throw new ApplicationException(string.Join(' ', result.Errors.Select(x => x.Description)));
        }

        if (!result.Succeeded)
        {
            throw new ApplicationException(GeneralErrorMessages.OperationFailed);
        }
    }

    private static string GeneratePassword(int length)
    {
        if (length < PasswordConstants.MinPasswordLength)
        {
            length = PasswordConstants.MinPasswordLength;
        }

        var stringChars = new char[length];
        var random = new Random();

        var charIndex = 0;

        for (; charIndex < 2; charIndex++)
        {
            stringChars[charIndex] =
                PasswordConstants.UpperCaseChars[random.Next(PasswordConstants.UpperCaseChars.Length)];
        }

        for (; charIndex < 4; charIndex++)
        {
            stringChars[charIndex] = PasswordConstants.Numbers[random.Next(PasswordConstants.Numbers.Length)];
        }

        for (; charIndex < 6; charIndex++)
        {
            stringChars[charIndex] = PasswordConstants.SpecialChars[random.Next(PasswordConstants.SpecialChars.Length)];
        }

        for (; charIndex < length; charIndex++)
        {
            stringChars[charIndex] =
                PasswordConstants.LowerCaseChars[random.Next(PasswordConstants.LowerCaseChars.Length)];
        }

        var finalString = new string(stringChars);
        return finalString;
    }
}
