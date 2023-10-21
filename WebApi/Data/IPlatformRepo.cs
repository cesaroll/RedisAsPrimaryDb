/*
 * @author: Cesar Lopez
 * @copyright 2023 - All rights reserved
 */

using WebApi.Models;

namespace WebApi.Data;

public interface IPlatformRepo
{
    Task<Platform> CreatePlatformAsync(Platform platform);
    Task<Platform?> GetPlatformByIdAsync(string id);
    Task<IEnumerable<Platform>> GetAllPlatformsAsync();
}
