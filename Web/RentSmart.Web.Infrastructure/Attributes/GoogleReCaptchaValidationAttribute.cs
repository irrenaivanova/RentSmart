﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace RentSmart.Web.Infrastructure.Attributes
{
    public class GoogleReCaptchaValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(
                    "Google reCAPTCHA validation failed. Value is null or empty.",
                    new[] { validationContext.MemberName });
            }

            var configuration = (IConfiguration)validationContext.GetService(typeof(IConfiguration));
            //if (configuration == null || string.IsNullOrWhiteSpace(configuration["GoogleReCaptcha:Secret"]))
                if (configuration == null )
                {
                return new ValidationResult(
                    "Google reCAPTCHA validation failed. Secret key not found.",
                    new[] { validationContext.MemberName });
            }

            var httpClient = new HttpClient();
            var content = new FormUrlEncodedContent(
                new[]
                    {
                        new KeyValuePair<string, string>("secret","6LfKc5EqAAAAAEoHcXE5CPK7O0LIa3Io-W8dCk_4"),
                        new KeyValuePair<string, string>("response", value.ToString()),
                    });
            var httpResponse = httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify", content)
                .GetAwaiter().GetResult();
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return new ValidationResult(
                    $"Google reCAPTCHA validation failed. Status code: {httpResponse.StatusCode}.",
                    new[] { validationContext.MemberName });
            }

            var jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
            var siteVerifyResponse = JsonSerializer.Deserialize<ReCaptchaSiteVerifyResponse>(jsonResponse);
            return siteVerifyResponse.Success
                       ? ValidationResult.Success
                       : new ValidationResult(
                           "Google reCAPTCHA validation failed.",
                           new[] { validationContext.MemberName });
        }

        public class ReCaptchaSiteVerifyResponse
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }

            [JsonPropertyName("score")]
            public float Score { get; set; }

            [JsonPropertyName("action")]
            public string Action { get; set; }

            [JsonPropertyName("challenge_ts")]
            public DateTime ChallengeTime { get; set; }

            [JsonPropertyName("hostname")]
            public string Hostname { get; set; }

            [JsonPropertyName("error-codes")]
            public string[] ErrorCodes { get; set; }
        }
    }
}
