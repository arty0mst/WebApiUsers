using System.Runtime.CompilerServices;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Models;

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

        [HttpGet]
        public async Task<ActionResult<List<UsersResponse>>> GetUsers([FromRoute] string login, [FromRoute] string password)
        {
            await _usersService.EnsureAdminExists();

            var users = await _usersService.GetAllUsers();

            var userCurrent = users.FirstOrDefault(u => u.Login == login);

            if (userCurrent == null)
            {
                return BadRequest("Пользователь не найден");
            }

            if (userCurrent?.Password != password)
            {
                return BadRequest("Неверный пароль");
            }

            var response = users.Select(u => new UsersResponse(u.Guid, u.Login, u.Password, u.Name, u.Gender, u.Birthday, u.Admin, u.CreatedOn, u.CreatedBy, u.ModifiedOn, u.ModifiedBy, u.RevokedOn, u.RevokedBy));

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateUser([FromBody] UsersRequest request)
        {
            await _usersService.EnsureAdminExists();
            
            var (user, error) = Models.User.Create(
                Guid.NewGuid(),
                request.Login,
                request.Password,
                request.Name,
                request.Gender,
                request.Birthday,
                request.Admin,
                request.CreatedOn,
                request.CreatedBy,
                request.ModifiedOn,
                request.ModifiedBy,
                request.RevokedOn,
                request.RevokedBy
            );

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var userId = await _usersService.CreateUser(user);

            return Ok(userId);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateUsers(Guid id, [FromBody] UsersRequest request)
        {
            await _usersService.EnsureAdminExists();
            
            var userId = await _usersService.UpdateUser(
                id,
                request.Login,
                request.Password,
                request.Name,
                request.Gender,
                request.Birthday,
                request.Admin,
                request.CreatedOn,
                request.CreatedBy,
                request.ModifiedOn,
                request.ModifiedBy,
                request.RevokedOn,
                request.RevokedBy
            );

            return Ok(userId);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteUsers(Guid id)
        {
            await _usersService.EnsureAdminExists();
            
            return Ok(await _usersService.DeleteUser(id));
        }

    }
}