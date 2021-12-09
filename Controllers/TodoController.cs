using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using net_mongo.Database;
using net_mongo.Models;

namespace net_mongo.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase 
    {
        private readonly ILogger<TodoController> _logger;
        private readonly MongoDbContext _mongoDbContext;

        public TodoController(ILogger<TodoController> logger)
        {
            _mongoDbContext = new MongoDbContext();
            _logger = logger;
        }

        [HttpGet("{id}")]
        public Todo Get(string id)
        {
            _logger.LogInformation("Get todo by id");
            
            var result =  _mongoDbContext.Todo
                            .Find(x => x.Id == id)
                            .FirstOrDefault();

            return result;
        }

        [HttpGet]
        public IEnumerable<Todo> Get()
        {
            _logger.LogInformation("Get todo");

            var result = _mongoDbContext.Todo.Find(_ => true).ToList();

            return result;
        }

        [HttpPost]
        public Todo Insert(Todo todo)
        {
            _logger.LogInformation("Insert todo");

            todo.Id = Guid.NewGuid().ToString();

            _mongoDbContext.Todo.InsertOne(todo);

            return todo;
        }

        [HttpPut("{id}")]
        public void Update(string id, Todo todo)
        {
            todo.Id = id;
            
            _mongoDbContext.Todo.ReplaceOne(x => x.Id == x.Id, todo);

            _logger.LogInformation("Update todo by id");
        }
    }

}