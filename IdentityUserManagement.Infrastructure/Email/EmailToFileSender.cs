using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Core.Models;

namespace IdentityUserManagement.Infrastructure.Email;

public class EmailToFileSender(string directory) : ISender
{
    public SendResponse Send(IFluentEmail email, CancellationToken? token = null)
    {
        try
        {
            return SendAsync(email, token).GetAwaiter().GetResult();
        }
        catch (AggregateException ex)
        {
            throw ex.InnerException ?? ex;
        }
    }

    public async Task<SendResponse> SendAsync(IFluentEmail email, CancellationToken? token = null)
    {
        var response = new SendResponse();
        await SaveEmailToDiskAsync(email);
        return response;
    }
    
    private async Task SaveEmailToDiskAsync(IFluentEmail email, string fileExtension = ".txt")
    {
        // Create a directory for today's emails
        var todayDirectory = Path.Combine(directory, $"{DateTime.Now:yyyyMMdd}");
        
        // Create the directory if it doesn't exist
        if (!Directory.Exists(todayDirectory))
        {
            Directory.CreateDirectory(todayDirectory);
        }
        
        // Create a unique filename
        var filename = Path.Combine(todayDirectory, $"{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid()}{fileExtension}");

        await using var stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
        await using var writer = new StreamWriter(stream);
        
        await writer.WriteLineAsync($"From: {FormatAddress(email.Data.FromAddress)}");
        await writer.WriteLineAsync($"To: {FormatAddresses(email.Data.ToAddresses)}");
        await writer.WriteLineAsync($"Cc: {FormatAddresses(email.Data.CcAddresses)}");
        await writer.WriteLineAsync($"Bcc: {FormatAddresses(email.Data.BccAddresses)}");
        await writer.WriteLineAsync($"ReplyTo: {FormatAddresses(email.Data.ReplyToAddresses)}");
        await writer.WriteLineAsync($"Subject: {email.Data.Subject}");

        foreach (var dataHeader in email.Data.Headers)
        {
            await writer.WriteLineAsync($"{dataHeader.Key}: {dataHeader.Value}");
        }

        await writer.WriteLineAsync();
        await writer.WriteAsync(email.Data.Body);
    }
    
    private static string FormatAddress(Address address) 
        => $"{address.Name} <{address.EmailAddress}>";

    private static string FormatAddresses(IEnumerable<Address> addresses) 
        => string.Join(", ", addresses.Select(FormatAddress));
}