using Microsoft.AspNetCore.Mvc;
using PlaygroundAPI.Data;
using PlaygroundAPI.DTOs;
using PlaygroundAPI.Models;

namespace PlaygroundAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    DataContextDapper _dapper;
    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadSingleData<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _dapper.LoadData<User>("SELECT * FROM TutorialAppSchema.Users");
        return users;
    }

    [HttpGet("GetUser/{userId}")]
    public User GetUser(int userId)
    {
        User user = _dapper.LoadSingleData<User>($"SELECT * FROM TutorialAppSchema.Users WHERE UserId = {userId}");
        return user;
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        string queryString = $@"
            UPDATE TutorialAppSchema.Users
                    SET [FirstName] = '{user.FirstName}',
                    [LastName] = '{user.LastName}',
                    [Email] = '{user.Email}',
                    [Gender] = '{user.Gender}',
                    [Active] = '{user.Active}'
                WHERE UserId = {user.UserId}
            ";

        bool result = _dapper.ExecuteInsert(queryString);
        if (result)
        {
            return Ok(result);
        }

        throw new Exception("Failed to update User");
    }

    [HttpPut("CreateUser")]
    public IActionResult CreateUser(UserDTO user)
    {
        string queryString = $@"
            INSERT INTO TutorialAppSchema.Users
            (
                FirstName,
                LastName,
                Email,
                Gender,
                Active
            )
            VALUES
            (
                '{user.FirstName}',
                '{user.LastName}',
                '{user.Email}',
                '{user.Gender}',
                '{user.Active}'
            )";

        Console.WriteLine(queryString);
        bool result = _dapper.ExecuteInsert(queryString);
        if (result)
        {
            return Ok(result);
        }

        throw new Exception("Failed to create User");
    }

    [HttpDelete("DeleteUser")]
    public IActionResult DeleteUser(int userId)
    {
        string queryString = $"DELETE FROM TutorialAppSchema.Users WHERE UserId = {userId}";
        bool result = _dapper.ExecuteInsert(queryString);

        if (result)
        {
            return Ok();
        }

        throw new Exception("Failed to delete User");
    }

    [HttpGet("GetUserJobInfo/{userId}")]
    public UserJobInfo GetUserJobInfo(int userId)
    {
        UserJobInfo userJobInfo = _dapper.LoadSingleData<UserJobInfo>($"SELECT * FROM TutorialAppSchema.UserJobInfo WHERE UserId = {userId}");
        return userJobInfo;
    }

    [HttpGet("GetUserSalary/{userId}")]
    public UserSalary GetUserSalary(int userId)
    {
        UserSalary userSalary = _dapper.LoadSingleData<UserSalary>($"SELECT * FROM TutorialAppSchema.UserSalary WHERE UserId = {userId}");
        return userSalary;
    }
}
