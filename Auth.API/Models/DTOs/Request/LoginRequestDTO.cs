﻿namespace Auth.API.Models.DTOs.Request
{
    public class LoginRequestDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
