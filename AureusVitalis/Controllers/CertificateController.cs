using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using AureusVitalis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AureusVitalis.Controllers;

[Authorize]
public sealed class CertificateController : Controller
{
    private readonly ICertificateService _cert;
    private readonly IWebHostEnvironment _env;

    public CertificateController(ICertificateService cert,
        IWebHostEnvironment env)
    {
        _cert = cert;
        _env  = env;
    }

    private int CurrentUserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> Download()
    {
        var path = await EnsureCertificateAsync();
        return PhysicalFile(path, "application/pdf", Path.GetFileName(path));
    }

    [HttpPost]
    public async Task<IActionResult> Send()
    {
        var path = await EnsureCertificateAsync();
        await _cert.SendByEmailAsync(CurrentUserId, path);
        return Ok(new { sent = true });
    }

    private async Task<string> EnsureCertificateAsync()
    {
        var record = _cert.GetLatest(CurrentUserId);
        var path   = record?.FilePath ?? await _cert.GenerateAsync(CurrentUserId);

        if (!Path.IsPathRooted(path))
            path = Path.Combine(_env.WebRootPath, path.TrimStart('/','\\'));

        if (!System.IO.File.Exists(path))
            path = await _cert.GenerateAsync(CurrentUserId);

        return path;
    }
}