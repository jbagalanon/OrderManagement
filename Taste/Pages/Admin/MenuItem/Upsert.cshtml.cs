using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models.ViewModels;


//Notes: When working in mvvm model, the stardard initialization of data must
 //be MVVM + Model + Id
namespace Taste.Pages.Admin.MenuItem
{
    public class UpsertModel : PageModel
    {

        private readonly IUnitOfWork _unitOfWork;

        //this is use to move images in hosting from the server
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }


        //this binding prop is from mvvm menuitem property
        [BindProperty] 
        public MenuItemVM MenuItemObj { get; set; }

        public IActionResult OnGet(int? id)
        {

            //load dropdown list
            MenuItemObj = new MenuItemVM
            {
                CategoryList = _unitOfWork.Category.GetCategoryListForDropdown(),
                FoodTypeList = _unitOfWork.FoodType.GetFoodTypeListForDropdown(),
                
                //then initialized the main menu item

                MenuItem = new Models.MenuItem()
            };

            //check if it is empty and initialize menu item
            if (id != null)
            {
                MenuItemObj.MenuItem= _unitOfWork.MenuItem.GetFirstOrDefault(u => u.Id == id);
                if (MenuItemObj.MenuItem == null)
                {
                    return NotFound();
                }
            }

            return Page();

        }

        public IActionResult OnPost()
        {

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (MenuItemObj.MenuItem.Id == 0)
            {
                //change userimage filename to string
                string fileName = Guid.NewGuid().ToString();
                //find upload path which is located form images subdir to menuitems
                var uploads = Path.Combine(webRootPath, @"images\menuItems");

                //once the upload is finish the ext must combine
                var extension = Path.GetExtension(files[0].FileName);

                //once the upload is finish then combine the file and extension
                using (var fileStream = new FileStream(Path.Combine(uploads, fileName+extension),
                    FileMode.Create))
                {
                 files[0].CopyTo(fileStream);   
                }

                MenuItemObj.MenuItem.Image = @"\images\menuItems" + fileName + extension;
                
                _unitOfWork.MenuItem.Add(MenuItemObj.MenuItem);
            }
            else
            {
                //Edit the menu item if is already in database

                // difference between single model to mvvm
                var objFromDb = _unitOfWork.MenuItem.Get(MenuItemObj.MenuItem.Id);

                //check and validate the file count of image upload
                if (files.Count > 0)
                {
                    //change userimage filename to string
                    string fileName = Guid.NewGuid().ToString();
                    //find upload path which is located form images subdir to menuitems
                    var uploads = Path.Combine(webRootPath, @"images\menuItems");

                    //once the upload is finish the ext must combine
                    var extension = Path.GetExtension(files[0].FileName);

                    //this is for updating.. Check file in database
                    var imagePath = Path.Combine(webRootPath, objFromDb.Image.TrimStart('\\'));

                    //then set file condition
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    //once the upload is finish then combine the file and extension
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension),
                        FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    MenuItemObj.MenuItem.Image = @"\images\menuItems" + fileName + extension;

                    _unitOfWork.MenuItem.Remove(MenuItemObj.MenuItem);
                }
                else
                {
                    //if not uploaded
                    MenuItemObj.MenuItem.Image = objFromDb.Image;
                }

                _unitOfWork.MenuItem.Update(MenuItemObj.MenuItem);
            }

            _unitOfWork.Save();
            return RedirectToPage("./Index");
        }
    }

}