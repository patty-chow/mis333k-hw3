using Chow_Patty_HW3.DAL;
using Chow_Patty_HW3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Chow_Patty_HW3.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context;
        public HomeController(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        // GET: Home
        public ActionResult Index(string SearchString)
        {
            var query = from r in _context.Repositories
                        select r;
            if (SearchString is not null)
            {
                query = query.Where(r => r.RepositoryName.Contains(SearchString) || r.Description.Contains(SearchString));
            }
            List<Repository> SelectedRepositories = query.Include(r => r.Language).ToList();

            //Populate the view bag with a count of all repositories
            ViewBag.AllRepositories = _context.Repositories.Count();
            //Populate the view bag with a count of selected repositories
            ViewBag.SelectedRepositories = SelectedRepositories.Count;

            return View(SelectedRepositories.OrderByDescending(r => r.StarCount));
        }
        
        public IActionResult Details(int? id)//id is the id of the repo you want to see
        {
            if (id == null) //RepositoryID not specified
            {
                //user did not specify a RepositoryID – take them to the error view
                return View("Error", new String[] { "RepositoryID not specified - which repository do you want to view ? " });
            }
            //look up the repo in the database based on the id;
            // be sure to include the language
            Repository repository = _context.Repositories.Include(b => b.Language).FirstOrDefault(b => b.RepositoryID == id);
            if (repository == null) //No repository with this id exists in the database
            {
                //there is not a repository with this id in the database – show an error view
            return View("Error", new String[] { "Repository not found in database" });
            }
            //if code gets this far, all is well – display the details
            return View(repository);
        }

        public IActionResult DetailedSearch()
        {
            ViewBag.AllLangs = GetAllLangsSelectList();
            SearchViewModel svm = new SearchViewModel();
 
            return View(svm);
        }

        private SelectList GetAllLangsSelectList()
        {
            //Get the list of months from the database
            List<Language> languageList = _context.Languages.ToList();

            //add a dummy entry so the user can select all months
            Language SelectNone = new Language() { LanguageID = 0, LanguageName = "All Languages" };
            languageList.Add(SelectNone);

            //convert the list to a SelectList by calling SelectList constructor
            //MonthID and MonthName are the names of the properties on the Month class
            //MonthID is the primary key
            SelectList languageSelectList = new SelectList(languageList.OrderBy(m => m.LanguageID), "LanguageID", "LanguageName");

            //return the electList
            return languageSelectList;
        }

        public ActionResult DisplaySearchResults(SearchViewModel svm)
        {
            var query = from r in _context.Repositories
                        select r;

            if (svm.Title is not null)
            {
                query = query.Where(r => r.RepositoryName.Contains(svm.Title));
            }

            if (svm.Description is not null)
            {
                query = query.Where(r => r.Description.Contains(svm.Description));
            }

            if (svm.SelectedCategories is not null)
            {
                query = query.Where(r => r.Category == svm.SelectedCategories);
            }
            
            if (svm.SelectedLangID != 0)
            {
                Language LanguageToDisplay = _context.Languages.Find(svm.SelectedLangID);
                query = query.Where(r => r.Language == LanguageToDisplay);
            }

            if (svm.SearchStarCount is not null)
            {
                if (svm.SearchType == SearchType.GreaterThan)
                {
                    query = query.Where(r => r.StarCount >= svm.SearchStarCount);
                }
                else
                {
                    query = query.Where(r => r.StarCount <= svm.SearchStarCount);
                }
            }

            if (svm.SelectedDate is not null)
            {
                query = query.Where(r => r.LastUpdate >= svm.SelectedDate);
            }

            List<Repository> SelectedRepositories = query.Include(r => r.Language).ToList();

            //Populate the view bag with a count of all repositories
            ViewBag.AllRepositories = _context.Repositories.Count();
            //Populate the view bag with a count of selected repositories
            ViewBag.SelectedRepositories = SelectedRepositories.Count;

            return View("Index", SelectedRepositories.OrderByDescending(r => r.StarCount));

        }

    }
}