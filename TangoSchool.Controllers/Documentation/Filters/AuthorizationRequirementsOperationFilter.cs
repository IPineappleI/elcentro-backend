using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TangoSchool.Documentation.Filters;

internal class AuthorizationRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.DeclaringType is null)
        {
            return;
        }

        var authAttributes = context.MethodInfo.DeclaringType
            .GetCustomAttributes(true)
            .Union(context.MethodInfo.GetCustomAttributes(true))
            .OfType<AuthorizeAttribute>()
            .ToList();

        if (authAttributes.Any())
        {
            operation.Responses.Add
            (
                "401",
                new()
                {
                    Description = "Unauthorized - The JWT Bearer Authorization is missing or invalid"
                }
            );

            operation.Responses.Add
            (
                "403",
                new()
                {
                    Description = "Forbidden - The user does not have the necessary permissions for the operation"
                }
            );

            var requiredRoles = authAttributes
                .Where(x => !string.IsNullOrWhiteSpace(x.Roles))
                .SelectMany(x => x.Roles!.Split(','))
                .Distinct()
                .ToList();

            if (requiredRoles.Any())
            {
                operation.Description += $"\nRoles required: {string.Join(", ", requiredRoles)}";
            }
        }
    }
}
