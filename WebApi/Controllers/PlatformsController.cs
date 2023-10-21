/*
 * @author: Cesar Lopez
 * @copyright 2023 - All rights reserved
 */


using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _repo;

    public PlatformsController(IPlatformRepo repo)
    {
        _repo = repo;
    }

    [HttpGet("{id}", Name = nameof(GetPlatformById))]
    public async Task<ActionResult<Platform>> GetPlatformById(string id)
    {
        var platform = await _repo.GetPlatformByIdAsync(id);

        if (platform is null)
        {
            return NotFound();
        }

        return Ok(platform);
    }

    [HttpPost(Name = nameof(CreatePlatform))]
    public async Task<ActionResult<Platform>> CreatePlatform(Platform platform)
    {
        await _repo.CreatePlatformAsync(platform);

        return CreatedAtRoute(nameof(GetPlatformById), new { id = platform.Id }, platform);
    }



}
