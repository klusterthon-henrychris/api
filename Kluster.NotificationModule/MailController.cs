using Kluster.NotificationModule.Models;
using Kluster.NotificationModule.Services.Contracts;
using Kluster.Shared.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kluster.NotificationModule;

[AllowAnonymous]
public class MailController(IMailService mailService) : BaseController
{
    [HttpPost("sendemailwithattachment")]
    public async Task<IActionResult> SendMailWithAttachmentAsync([FromForm] MailDataWithAttachments mailData)
    {
        var result = await mailService.SendWithAttachmentsAsync(mailData, new CancellationToken());

        if (result)
        {
            return Ok("Mail with attachment has successfully been sent.");
        }

        return StatusCode(StatusCodes.Status500InternalServerError,
            "An error occured. The Mail with attachment could not be sent.");
    }

    [HttpPost("sendmail")]
    public async Task<IActionResult> SendMailAsync([FromBody] MailData mailData)
    {
        var result = await mailService.SendAsync(mailData, new CancellationToken());

        if (result)
        {
            return StatusCode(StatusCodes.Status200OK, "Mail has successfully been sent.");
        }

        return StatusCode(StatusCodes.Status500InternalServerError,
            "An error occured. The Mail could not be sent.");
    }
}