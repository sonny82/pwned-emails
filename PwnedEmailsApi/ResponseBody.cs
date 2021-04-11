using System;
using System.ComponentModel.DataAnnotations;

namespace PwnedEmailsApi
{
    [Serializable]
    public class ResponseBody
    {
        public ResponseBody(int status, string message)
        {
            Status = status;
            Message = message;
        }

        /// <summary>
        /// HTTP status code
        /// </summary>
        [Required]
        public int Status { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        [Required]
        public string Message { get; set; }
    }
}