using Microsoft.AspNetCore.Http;

namespace Vezeeta.Service.Helpers
{
	public static class DocumentSettings
	{
		public static async Task<string> UploadFile(IFormFile file, string folderName)
		{
			string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

			string fileName = $"{Guid.NewGuid()}{file.FileName}";

			string filePath = Path.Combine(folderPath, fileName);

			using var fs = new FileStream(filePath, FileMode.Create);

			await file.CopyToAsync(fs);

			return fileName;
		}

		public static void DeleteFile(string fileName, string folderName)
		{
			string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName, fileName);

			if (File.Exists(filePath))
				File.Delete(filePath);

		}
	}
}