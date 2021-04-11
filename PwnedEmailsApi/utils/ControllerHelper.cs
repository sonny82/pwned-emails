using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;

namespace PwnedEmailsApi.Utils
{
    public static class ControllerHelper
    {
        public static ProblemDetails CreateProblemDetails(int status, string title, string instance) =>
            new()
            {
                Status = status,
                Title = title,
                Instance = instance
            };

        public static string ExtractDomain(string email) => new MailAddress(email).Host;

        public static bool IsValidEmail(string email) => new EmailAddressAttribute().IsValid(email);

        public static ResponseBody CreateResponseBody(int statusCode, string message) =>
            new(statusCode, message);
    }
}