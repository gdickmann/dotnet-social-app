using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using social_app_client.Models.User;
using social_app_client.Repository.User;

namespace social_app_client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(UserRequest request)
        {
            await _repository.InsertIntoQueue(request);
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UserRequest request, Guid id)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7091");
            var client = new UserStream.UserStreamClient(channel);

            var response = await client.UpdateAsync(new UpdateUser { Id = id.ToString(), Name = request.Name, Email = request.Email, Password = request.Password }); ;
            return Ok(response);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7091");
            var client = new UserStream.UserStreamClient(channel);

            var response = await client.DeleteAsync(new UserIdGrpc { Id = id.ToString() });
            return Ok(response);
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid id)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7091");
            var client = new UserStream.UserStreamClient(channel);

            var response = await client.GetAsync(new UserIdGrpc { Id = id.ToString() });
            return Ok(response);
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAll()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7091");
            var client = new UserStream.UserStreamClient(channel);

            var response = await client.GetAllAsync(new EmptyGrpc());
            return Ok(response);
        }

    }
}