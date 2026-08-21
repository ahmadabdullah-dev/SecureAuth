using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace Business.Services;

public static class ServiceHelper
{
    public static string GetFirstError(IdentityResult result) =>
        result.Errors.FirstOrDefault()?.Description ?? "Unexpected error happened";
}
