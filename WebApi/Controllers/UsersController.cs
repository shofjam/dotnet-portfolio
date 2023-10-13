namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;
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
        try
        {
            var users = _userService.GetAll(limit, keyword, page);
            return Ok(users);
        }
        catch(AppException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route("get-by-id")]
    public IActionResult GetById(int id)
    {
        try
        {
            var result = _userService.GetById(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        catch (AppException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route("create")]
    public IActionResult Create(CreateModel model)
    {
        try
        {
            _userService.Create(model);
            return Ok(new { message = "User created" });
        }
        catch (AppException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route("update")]
    public IActionResult Update(int id, UpdateModel payload)
    {
        try
        {
            var oldData = _userService.GetById(id);
            if (oldData == null)
            {
                return NotFound();
            }

            _userService.Update(oldData, payload);
            return Ok(new { message = "User updated" });
        }
        catch (AppException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route("change-password")]
    public IActionResult ChangePassword(int id, ChangePasswordModel payload)
    {
        try
        {
            var data = _userService.GetById(id);
            if (data == null)
            {
                return NotFound();
            }

            return _userService.ChangePassword(data, payload);
        }
        catch (AppException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete]
    [Route("delete")]
    public IActionResult Delete(int id)
    {
        try
        {
            _userService.Delete(id);
            return Ok(new { message = "User deleted" });
        }
        catch (AppException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}