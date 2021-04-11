using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;

namespace PwnedEmailsApi.Utils
{
    public class ControllerHelper
    {
        internal static ProblemDetails CreateProblemDetails(int status, string title, string instance) =>
            new ProblemDetails
            {
                Status = status,
                Title = title,
                Instance = instance
            };

        internal static string ExtractDomain(string email) => new MailAddress(email).Host;

        internal static bool IsValidEmail(string email) => new EmailAddressAttribute().IsValid(email);

        internal static ResponseBody CreateResponseBody(int statusCode, string message) =>
            new ResponseBody(statusCode, message);
    }
}