﻿namespace Vezeeta.APIs.Errors
{
	public class ApiResponse
	{
		public int StatusCode { get; set; }

		public string Message { get; set; }

		public ApiResponse(int statusCode, string? message = null)
		{
			StatusCode = statusCode;
			Message = message ?? GetDefaultMessageForStatusCode(statusCode);
		}

		private string? GetDefaultMessageForStatusCode(int statusCode)
		{
			return statusCode switch
			{
				400 => "A bad request, you have made",
				401 => "You are not Authorized",
				404 => "Resource was not found",
				500 => "Internal Server Errror",
				_ => null

			};


		}
	}
}
