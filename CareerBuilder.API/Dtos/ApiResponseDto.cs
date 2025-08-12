using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerBuilder.API.Dtos
{
    public class ApiResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
        public static ApiResponseDto Success(string message = "Success", object? data = null) =>
     new() { IsSuccess = true, Message = message, Data = data };

        public static ApiResponseDto Fail(string message = "Something went wrong") =>
            new() { IsSuccess = false, Message = message };
    }

}
