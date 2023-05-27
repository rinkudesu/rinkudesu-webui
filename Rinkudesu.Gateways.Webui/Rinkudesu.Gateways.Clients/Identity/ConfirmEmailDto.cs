using System;
using System.ComponentModel.DataAnnotations;

namespace Rinkudesu.Gateways.Clients.Identity;

public class ConfirmEmailDto
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public string EmailToken { get; set; } = string.Empty;
}
