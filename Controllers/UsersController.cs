using System.Runtime.CompilerServices;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("controller")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsersResponse>>> GetUsers()
        {
            var users = await _usersService.GetAllUsers();

            var response = users.Select(u => new UsersResponse(u.Guid, u.Login, u.Password, u.Name, u.Gender, u.Birthday, u.Admin, u.CreatedOn, u.CreatedBy, u.ModifiedOn, u.ModifiedBy, u.RevokedOn, u.RevokedBy));

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateUser([FromBody] UsersRequest request)
        {
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
            return Ok(await _usersService.DeleteBook(id));
        }

    }
}