using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.API.Models;
using ToDoAPI.DATA.EF;
using System.Web.Http.Cors;

namespace ToDoAPI.API.Controllers
{
    //GET = READ
    //POST = CREATE
    //PUT = EDIT
    //DELETE = DELETE
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ToDoController : ApiController
    {
        //Create a connection to the db
        ToDoEntities db = new ToDoEntities();

        //GET - /api/resources
        public IHttpActionResult GetToDos()
        {
            //Below we create a list of EF resource objects. In an API, it's best practice to install EF to the API layer when needing to accomplish this task.
            List<ToDoViewModel> toDos = db.ToDoItems.Include("Category").Select(t => new ToDoViewModel()
            {
                //Assign the columns of the Resources db table to the ResourceViewModel object, so we can use the data (send the data back to requesting app)
                TodoId = t.TodoId,
                Action = t.Action,
                Done = t.Done,
                CategoryId = t.CategoryId,
                Category = new CategoryViewModel()
                {
                    CategoryId = t.Category.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description
                }
            }).ToList<ToDoViewModel>();

            //Check the results and handle accordingly below
            if (toDos.Count == 0)
            {
                return NotFound();
            }
            //Everything is good, return the data
            return Ok(toDos);//resources are being passed in the response back to the requesting app.
        }//end GetResources()

        //GET - api/resources/id
        public IHttpActionResult GetToDo(int id)
        {
            //Create a new REsourceViewModel object and assign it to the appropriate resource from the db
            ToDoViewModel toDos = db.ToDoItems.Include("Category").Where(t => t.TodoId == id).Select(t =>
                new ToDoViewModel()
                {
                    //Coopy the assignments from the GetResources() and paste below
                    TodoId = t.TodoId,
                    Action = t.Action,
                    Done = t.Done,
                    CategoryId = t.CategoryId,
                    Category = new CategoryViewModel()
                    {
                        CategoryId = t.Category.CategoryId,
                        Name = t.Category.Name,
                        Description = t.Category.Description
                    }
                }).FirstOrDefault();
            //scopeless if - once the return executes the scopes are closed.
            if (toDos == null)
                return NotFound();

            return Ok(toDos);

        }//end GetResource

        //POST - api/Resources (HttpPost)
        public IHttpActionResult PostToDo(ToDoViewModel todo)
        {
            //1. Check to validate the object - we need to know that all the data necessary to create a resource is there.
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            ToDoItem newToDo = new ToDoItem()
            {
                Action = todo.Action,
                Done = todo.Done,
                CategoryId = todo.CategoryId
            };

            //add a record and save changes
            db.ToDoItems.Add(newToDo);
            db.SaveChanges();

            return Ok(newToDo);

        }

        //PUT - api/resources (HTTPPut)
        public IHttpActionResult PutToDo(ToDoViewModel todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            //We get the resource from the db so we can modify it
            ToDoItem existingToDo = db.ToDoItems.Where(r => r.TodoId == todo.TodoId).FirstOrDefault();

            if (existingToDo != null)
            {
                existingToDo.TodoId = todo.TodoId;
                existingToDo.Action = todo.Action;
                existingToDo.CategoryId = todo.CategoryId;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }
        //DELETE - api/Resources/id (HTTPDelete)
        public IHttpActionResult DeleteToDo(int id)
        {
            //Get resource from the api to make sure theres a resource with this id
            ToDoItem toDo = db.ToDoItems.Where(t => t.TodoId == id).FirstOrDefault();

            if (toDo != null)
            {
                db.ToDoItems.Remove(toDo);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }

        //We use Dispose() below to dispose of any connection to the database after we are done with them - best practice to handle performance - dispose of the instance of the controller and db connection when we are done with it.

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }//end class
}//end namespace