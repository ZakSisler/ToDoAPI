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
    public class CategoriesController : ApiController
    {
        ToDoEntities db = new ToDoEntities();

        //GET - /api/resources
        public IHttpActionResult GetCats()
        {
            //Below we create a list of EF resource objects. In an API, it's best practice to install EF to the API layer when needing to accomplish this task.
            List<CategoryViewModel> cat = db.Categories.Include("Category").Select(c => new CategoryViewModel()
            {
                //Assign the columns of the Resources db table to the ResourceViewModel object, so we can use the data (send the data back to requesting app)
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description

            }).ToList<CategoryViewModel>();

            //Check the results and handle accordingly below
            if (cat.Count == 0)
            {
                return NotFound();
            }
            //Everything is good, return the data
            return Ok(cat);//resources are being passed in the response back to the requesting app.
        }//end GetResources()

        //GET - api/resources/id
        public IHttpActionResult GetCat(int id)
        {
            //Create a new REsourceViewModel object and assign it to the appropriate resource from the db
            CategoryViewModel cat = db.Categories.Where(c => c.CategoryId == id).Select(c =>
                new CategoryViewModel()
                {
                    //Coopy the assignments from the GetResources() and paste below

                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    Description = c.Description

                }).FirstOrDefault();
            //scopeless if - once the return executes the scopes are closed.
            if (cat == null)
                return NotFound();

            return Ok(cat);

        }//end GetResource

        //POST - api/Resources (HttpPost)
        public IHttpActionResult PostCat(CategoryViewModel cat)
        {
            //1. Check to validate the object - we need to know that all the data necessary to create a resource is there.
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            Category newCat = new Category()
            {
                CategoryId = cat.CategoryId,
                Name = cat.Name,
                Description = cat.Description
            };

            //add a record and save changes
            db.Categories.Add(newCat);
            db.SaveChanges();

            return Ok(newCat);

        }

        //PUT - api/resources (HTTPPut)
        public IHttpActionResult PutCat(CategoryViewModel cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            //We get the resource from the db so we can modify it
            Category existingCat = db.Categories.Where(c => c.CategoryId == cat.CategoryId).FirstOrDefault();

            if (existingCat != null)
            {
                existingCat.CategoryId = cat.CategoryId;
                existingCat.Name = cat.Name;
                existingCat.CategoryId = cat.CategoryId;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }

        //DELETE - api/Resources/id (HTTPDelete)
        public IHttpActionResult DeleteCat(int id)
        {
            //Get resource from the api to make sure theres a resource with this id
            Category cat = db.Categories.Where(c => c.CategoryId == id).FirstOrDefault();

            if (cat != null)
            {
                db.Categories.Remove(cat);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
