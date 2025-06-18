// Services/CertificateService.cs
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AureusVitalis.Data;
using AureusVitalis.Data.Entities;
using AureusVitalis.Infrastructure;          // EmailOptions
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using SkiaSharp;

namespace AureusVitalis.Services;

public sealed class CertificateService : ICertificateService
{
    private readonly AppDbContext        _db;
    private readonly IWebHostEnvironment _env;
    private readonly EmailOptions        _smtp;

    private const string CourseKey = "AV_Base_v1";      // ID курса

    public CertificateService(
        AppDbContext            db,
        IWebHostEnvironment     env,
        IOptions<EmailOptions>  smtpOpts)
    {
        _db   = db;
        _env  = env;
        _smtp = smtpOpts.Value;                          // Server, Port, User, Pass, From, FromName
    }

    public async Task<string> GenerateAsync(int userId)
    {
        var user = _db.Users.First(u => u.Id == userId);     // uid из cookie ⇒ всегда существует

        /* 1. Имя и папка */
        var certDir  = Path.Combine(_env.WebRootPath, "certificates");
        Directory.CreateDirectory(certDir);

        var fileName = $"certificate_{userId}_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
        var fullPath = Path.Combine(certDir, fileName);

        /* 2. Рисуем PDF (A5 landscape @300 dpi) */
        const int w = 1684, h = 1191;
        using (var doc = SKDocument.CreatePdf(fullPath))
        using (var canvas = doc.BeginPage(w, h))
        {
            canvas.Clear(new SKColor(0xFF, 0xFF, 0xFA));

            var paint = new SKPaint
            {
                Color       = SKColors.DarkGreen,
                IsAntialias = true,
                Typeface    = SKTypeface.FromFamilyName("Arial"),
                TextAlign   = SKTextAlign.Center
            };

            paint.TextSize = 70;
            canvas.DrawText("СЕРТИФИКАТ", w / 2f, 250, paint);

            paint.TextSize = 38;
            canvas.DrawText("подтверждает, что", w / 2f, 380, paint);

            paint.TextSize = 55;
            paint.Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold);
            canvas.DrawText(user.Username, w / 2f, 500, paint);

            paint.Typeface = SKTypeface.FromFamilyName("Arial");
            paint.TextSize = 38;
            canvas.DrawText("успешно завершил курс Aureus & Vitalis", w / 2f, 620, paint);

            paint.TextSize = 28;
            canvas.DrawText($"{DateTime.UtcNow:dd.MM.yyyy}", w / 2f, 760, paint);

            doc.EndPage();
            doc.Close();
        }

        if (!_db.Certificates.Any(c => c.UserId == userId && c.CourseKey == CourseKey))
        {
            _db.Certificates.Add(new Certificate
            {
                UserId    = userId,
                CourseKey = CourseKey,
                IssuedAt  = DateTime.UtcNow,
                FilePath  = $"/certificates/{fileName}"   
            });
            await _db.SaveChangesAsync();
        }

        return fullPath;                                
    }

    public async Task SendByEmailAsync(int userId, string pdfPath)
    {
        if (!Path.IsPathRooted(pdfPath))
            pdfPath = Path.Combine(_env.WebRootPath,
                                   pdfPath.TrimStart('/', '\\'));

        if (!File.Exists(pdfPath))
            throw new FileNotFoundException("Certificate file not found", pdfPath);

        var user = _db.Users.Find(userId)!;

        var msg = new MimeMessage();
        msg.From.Add(new MailboxAddress(_smtp.FromName, _smtp.From));
        msg.To  .Add(MailboxAddress.Parse(user.Email));
        msg.Subject = "Ваш сертификат Aureus & Vitalis";

        var body = new BodyBuilder
        {
            TextBody = "Поздравляем! Во вложении — ваш сертификат."
        };
        body.Attachments.Add(pdfPath);
        msg.Body = body.ToMessageBody();

        using var smtp = new SmtpClient();

        var secure = _smtp.Port == 465
                   ? SecureSocketOptions.SslOnConnect
                   : SecureSocketOptions.StartTls;

        await smtp.ConnectAsync(_smtp.Server, _smtp.Port, secure);
        await smtp.AuthenticateAsync(_smtp.User, _smtp.Pass);
        await smtp.SendAsync(msg);
        await smtp.DisconnectAsync(true);
    }

    public Certificate? GetLatest(int userId) =>
        _db.Certificates
           .Where (c => c.UserId == userId && c.CourseKey == CourseKey)
           .OrderByDescending(c => c.IssuedAt)
           .FirstOrDefault();
}