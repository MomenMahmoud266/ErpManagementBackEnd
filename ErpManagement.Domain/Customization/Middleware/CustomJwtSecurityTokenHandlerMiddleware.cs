namespace ErpManagement.Domain.Customization.Middleware;

public class CustomJwtSecurityTokenHandler : JwtSecurityTokenHandler
{
    protected override ClaimsIdentity? CreateClaimsIdentity(JwtSecurityToken jwtToken, string issuer, TokenValidationParameters validationParameters)
    {
        var originalIdentity = base.CreateClaimsIdentity(jwtToken, issuer, validationParameters);

        if (originalIdentity != null && originalIdentity.IsAuthenticated)
        {
            // Claims to exclude (role claims with the same value as user claims in this example)
            var excludedClaims = originalIdentity.FindAll(ClaimTypes.Role)
                .Where(roleClaim => originalIdentity.HasClaim(c => c.Type == ClaimTypes.Name && c.Value == roleClaim.Value))
                .ToList();

            foreach (var excludedClaim in excludedClaims)
                originalIdentity.RemoveClaim(excludedClaim);
        }

        return originalIdentity;
    }
}

