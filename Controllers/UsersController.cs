using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Models;
using Contracts;

namespace Controllers
{
    [ApiController]
    [Route("controller/{login}&{password}")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet("getactive/")]
        public async Task<ActionResult<List<UsersResponse>>> GetActiveUsers([FromRoute] string login, [FromRoute] string password)
        {
            await _usersService.EnsureAdminExists();

            (var userAuth, var errorAuth) = await _usersService.Authorization(login, password);

            if (errorAuth != "")
            {
                return BadRequest(errorAuth);
            }

            if (!userAuth!.Admin)
            {
                return BadRequest("This operation is not available");
            }

            var users = await _usersService.GetAllActiveUsers();

            var response = users.Select(u => new UsersResponse(u.Guid, u.Login, u.Password, u.Name, u.Gender, u.Birthday, u.Admin, u.CreatedOn, u.CreatedBy, u.ModifiedOn, u.ModifiedBy, u.RevokedOn, u.RevokedBy));

            return Ok(response);
        }

        [HttpGet("gettarget/{targetLogin}")]
        public async Task<ActionResult<List<UsersResponse>>> GetTargetUser(string targetLogin, [FromRoute] string login, [FromRoute] string password)
        {
            await _usersService.EnsureAdminExists();

            (var userAuth, var errorAuth) = await _usersService.Authorization(login, password);

            if (errorAuth != "")
            {
                return BadRequest(errorAuth);
            }

            if (!userAuth!.Admin)
            {
                return BadRequest("This operation is not available");
            }

            var users = await _usersService.GetTargetUser(targetLogin);

            var response = users.Select(u => new UsersTargetResponse(u.Name, u.Gender, u.Birthday, u.RevokedOn));

            return Ok(response);
        }

        [HttpGet("getageuser/{age}")]
        public async Task<ActionResult<List<UsersResponse>>> GetAgeUser(int age, [FromRoute] string login, [FromRoute] string password)
        {
            await _usersService.EnsureAdminExists();

            (var userAuth, var errorAuth) = await _usersService.Authorization(login, password);

            if (errorAuth != "")
            {
                return BadRequest(errorAuth);
            }

            if (!userAuth!.Admin)
            {
                return BadRequest("This operation is not available");
            }

            var users = await _usersService.GetAgeUser(age);

            var response = users.Select(u => new UsersResponse(u.Guid, u.Login, u.Password, u.Name, u.Gender, u.Birthday, u.Admin, u.CreatedOn, u.CreatedBy, u.ModifiedOn, u.ModifiedBy, u.RevokedOn, u.RevokedBy));

            return Ok(response);
        }

        [HttpGet("me/")]
        public async Task<ActionResult<List<UsersResponse>>> GetSelfUser([FromRoute] string login, [FromRoute] string password)
        {
            await _usersService.EnsureAdminExists();

            (var userAuth, var errorAuth) = await _usersService.Authorization(login, password);

            if (errorAuth != "")
            {
                return BadRequest(errorAuth);
            }

            var users = await _usersService.GetTargetUser(login);

            var response = users.Select(u => new UsersResponse(u.Guid, u.Login, u.Password, u.Name, u.Gender, u.Birthday, u.Admin, u.CreatedOn, u.CreatedBy, u.ModifiedOn, u.ModifiedBy, u.RevokedOn, u.RevokedBy));

            return Ok(response);
        }

        [HttpPost("create/")]
        public async Task<ActionResult<Guid>> CreateUser([FromBody] UsersRequest request, [FromRoute] string login, [FromRoute] string password)
        {
            await _usersService.EnsureAdminExists();

            (var userAuth, var errorAuth) = await _usersService.Authorization(login, password);

            if (errorAuth != "")
            {
                return BadRequest(errorAuth);
            }

            if (!userAuth!.Admin)
            {
                return BadRequest("This operation is not available");
            }

            var (user, error) = Models.User.Create(
                Guid.NewGuid(),
                request.Login,
                request.Password,
                request.Name,
                request.Gender,
                request.Birthday,
                request.Admin,
                DateTime.Now.ToUniversalTime(),
                userAuth.Login,
                null,
                "",
                null,
                ""
            );

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            (var userId, error) = await _usersService.CreateUser(user);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            return Ok(userId);
        }

        [HttpPut("update/{targetLogin}")]
        public async Task<ActionResult<Guid>> UpdateUsers(string targetLogin, [FromBody] UsersUpdateRequest request, [FromRoute] string login, [FromRoute] string password)
        {
            await _usersService.EnsureAdminExists();

            (var userAuth, var errorAuth) = await _usersService.Authorization(login, password);

            if (errorAuth != "")
            {
                return BadRequest(errorAuth);
            }

            var users = await _usersService.GetAllUsers();
            var userCheck = users.FirstOrDefault(u => u.Login == targetLogin);

            if (userCheck == null)
            {
                return BadRequest("User not found");
            }

            if (!userAuth!.Admin && userCheck.Login != userAuth!.Login)
            {
                return BadRequest("This operation is not available");
            }

            if (request.Login != userCheck.Login)
            {
                var usersLogin = await _usersService.GetAllUsers();
                var usersLoginCheck = users.FirstOrDefault(u => u.Login == request.Login);

                if (usersLoginCheck != null)
                {
                    return BadRequest("User already exist");
                }
            }

            var userId = await _usersService.UpdateUser(
                userCheck.Guid,
                request.Login,
                request.Password,
                request.Name,
                request.Gender,
                request.Birthday,
                userCheck.Admin,
                userCheck.CreatedOn,
                userCheck.CreatedBy,
                DateTime.Now.ToUniversalTime(),
                userAuth.Login,
                userCheck.RevokedOn,
                userCheck.RevokedBy
            );

            return Ok(userId);
        }

        [HttpPut("restore/{targetLogin}")]
        public async Task<ActionResult<Guid>> RestoreUsers(string targetLogin, [FromRoute] string login, [FromRoute] string password)
        {
            await _usersService.EnsureAdminExists();

            (var userAuth, var errorAuth) = await _usersService.Authorization(login, password);

            if (errorAuth != "")
            {
                return BadRequest(errorAuth);
            }

            var users = await _usersService.GetAllUsers();
            var userCheck = users.FirstOrDefault(u => u.Login == targetLogin);

            if (userCheck == null)
            {
                return BadRequest("User not found");
            }

            if (!userAuth!.Admin)
            {
                return BadRequest("This operation is not available");
            }

            var userId = await _usersService.UpdateUser(
                userCheck.Guid,
                userCheck.Login,
                userCheck.Password,
                userCheck.Name,
                userCheck.Gender,
                userCheck.Birthday,
                userCheck.Admin,
                userCheck.CreatedOn,
                userCheck.CreatedBy,
                DateTime.Now.ToUniversalTime(),
                userAuth.Login,
                null,
                ""
            );

            return Ok(userId);
        }

        [HttpDelete("deletefull/{targetLogin}")]
        public async Task<ActionResult<Guid>> DeleteFullUsers(string targetLogin, [FromRoute] string login, [FromRoute] string password)
        {
            await _usersService.EnsureAdminExists();

            (var userAuth, var errorAuth) = await _usersService.Authorization(login, password);

            if (errorAuth != "")
            {
                return BadRequest(errorAuth);
            }

            var users = await _usersService.GetAllUsers();
            var userCheck = users.FirstOrDefault(u => u.Login == targetLogin);

            if (userCheck == null)
            {
                return BadRequest("User not found");
            }

            if (!userAuth!.Admin)
            {
                return BadRequest("This operation is not available");
            }

            return Ok(await _usersService.DeleteUser(userCheck.Guid));
        }

        [HttpPut("deletepartly/{targetLogin}")]
        public async Task<ActionResult<Guid>> DeletePartlyUsers(string targetLogin, [FromRoute] string login, [FromRoute] string password)
        {
            await _usersService.EnsureAdminExists();

            (var userAuth, var errorAuth) = await _usersService.Authorization(login, password);

            if (errorAuth != "")
            {
                return BadRequest(errorAuth);
            }

            var users = await _usersService.GetAllUsers();
            var userCheck = users.FirstOrDefault(u => u.Login == targetLogin);

            if (userCheck == null)
            {
                return BadRequest("User not found");
            }

            if (!userAuth!.Admin)
            {
                return BadRequest("This operation is not available");
            }

            var userId = await _usersService.UpdateUser(
                userCheck.Guid,
                userCheck.Login,
                userCheck.Password,
                userCheck.Name,
                userCheck.Gender,
                userCheck.Birthday,
                userCheck.Admin,
                userCheck.CreatedOn,
                userCheck.CreatedBy,
                userCheck.ModifiedOn,
                userCheck.ModifiedBy,
                DateTime.Now.ToUniversalTime(),
                userAuth.Login
            );

            return Ok(userId);
        }

    }
}