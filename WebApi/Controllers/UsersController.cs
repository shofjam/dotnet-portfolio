namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Users;
using WebApi.Services;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private IUserService _userService;
    private IMapper _mapper;

    public UsersController(
        IUserService userService,
        IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("get")]
    public IActionResult GetAll(int limit = 10, string? keyword = null, int page = 1)
    {
        var users = _userService.GetAll(limit, keyword, page);
        return Ok(users);
    }

    [HttpGet]
    [Route("get-by-id")]
    public IActionResult GetById(int id)
    {
        var result = _userService.GetById(id);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public IActionResult Create(CreateModel model)
    {
        _userService.Create(model);
        return Ok(new { message = "User created" });
    }

    [HttpPut]
    [Route("update")]
    public IActionResult Update(int id, UpdateModel payload)
    {
        var oldData = _userService.GetById(id);
        if (oldData == null)
        {
            return NotFound();
        }

        _userService.Update(oldData, payload);
        return Ok(new { message = "User updated" });
    }

    [HttpPost]
    [Route("change-password")]
    public IActionResult ChangePassword(int id, ChangePasswordModel payload)
    {
        var data = _userService.GetById(id);
        if (data == null)
        {
            return NotFound();
        }

        return _userService.ChangePassword(data, payload);
    }

    [HttpDelete]
    [Route("delete")]
    public IActionResult Delete(int id)
    {
        _userService.Delete(id);
        return Ok(new { message = "User deleted" });
    }
}