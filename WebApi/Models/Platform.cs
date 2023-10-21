/*
 * @author: Cesar Lopez
 * @copyright 2023 - All rights reserved
 */

using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class Platform
{
    [Required]
    public string Id { get; set; } = $"platform:{Guid.NewGuid()}";
    [Required]
    public string Name { get; set; } = String.Empty;
}
