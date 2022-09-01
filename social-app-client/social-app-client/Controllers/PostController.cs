﻿using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using social_app_client.Models.Post;
using social_app_client.Repository.Post;

namespace social_app_client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        
        private readonly IPostRepository _repository;

        public PostController(IPostRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(PostRequest request)
        {
            if (request is null)
                return BadRequest();

            _repository.InsertIntoQueue(request);
            return Ok();
        }
    }
}
