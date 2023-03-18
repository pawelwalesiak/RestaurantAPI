using Microsoft.AspNetCore.Authorization;
using System;
using System.Globalization;
using System.Security.Claims;

namespace RestaurantAPI.Authorization
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger<MinimumAgeRequirement> _logger;

        public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirement> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
             // var dateOfBirth = (context.User.FindFirst(c => c.Type == "DateOfBirth").Value);
           //
           //var dateOfBirth = DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth").Value);
            
            var dateOfBirthClaim = context.User.FindFirst(c => c.Type == "DateOfBirth");
            if (dateOfBirthClaim == null || !DateTime.TryParseExact(dateOfBirthClaim.Value, "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime dateOfBirth))
            {
                _logger.LogInformation("Date of birth claim not found or invalid format");
                context.Fail();
                return Task.CompletedTask;
            }
            
            var userEmail = context.User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;

            _logger.LogInformation($"User: {userEmail} with date of birth : [{dateOfBirth}] ");

            if (dateOfBirth.AddYears(requirement.MinimumAge) < DateTime.Today)
            {
                _logger.LogInformation($"Authorization succedded");
                context.Succeed(requirement);

            }
            else
            {
                _logger.LogInformation($"Authorization failed");
            }
            return Task.CompletedTask;
        }
    }
}
