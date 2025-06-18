using AureusVitalis.Data.Entities;

namespace AureusVitalis.Services;

public interface ICertificateService
{
    Task<string>  GenerateAsync   (int userId);
    Task          SendByEmailAsync(int userId, string pdfPath);
    Certificate?  GetLatest       (int userId);
}