﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Developer.Models;
using Developer.Models.EntityModels;
using Developer.Models.EntityModels.Interfaces;
using Developer.Models.ViewModels;
using Developer.Services.Home;

namespace Developer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApplicationContext _context;
        private readonly IEmailService _emailService;
        private readonly ISearchService _searchService;
        private readonly IShowAdvertService _showAdvertService;
        private readonly ICounterService _counterService;
        // GET: Home
        public HomeController(IApplicationContext context, IEmailService emailService, ISearchService searchService, IShowAdvertService showAdvertService, ICounterService counterService)
        {
            _context = context;
            _emailService = emailService;
            _searchService = searchService;
            _showAdvertService = showAdvertService;
            _counterService = counterService;
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Credits()
        {
            return View();
        }

        public ActionResult Search(string key)
        {
            return RedirectToAction("Show", new {key});
        }

        public ActionResult Where()
        {
            return View();
        }

        public ActionResult SendEmail(ContactEmail contactEmail)
        {
            if (ModelState.IsValid)
            {
                var result = _emailService.SendAndSaveOfferQuestion(contactEmail);
            }
            else
            {
                return View("_ContactForm", contactEmail);
            }

            return View("Index");
        }

        public ActionResult Index()
        {
            return View();        
        }
        public ActionResult House()
        {
            return View();
        }
        public ActionResult Flat()
        {
            return View();
        }
        public ActionResult Land()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Show(ContactEmail contactEmail, string key)
        {
            if (ModelState.IsValid)
            {
                _emailService.SendAndSaveOfferQuestion(contactEmail);
                return View("ConfirmMail");
            }

            var parsedSearch = _searchService.ParseKey(key);
            if (parsedSearch.Id > 0)
            {
                var advert = _showAdvertService.GetAdvert(parsedSearch.AdType, parsedSearch.Id);
                if (advert.Success)
                {
                    if (parsedSearch.AdType == AdType.Flat)
                    {
                        advert.Data.Flat.ContactEmail = contactEmail;
                    }
                    else if (parsedSearch.AdType == AdType.House)
                    {
                        advert.Data.House.ContactEmail = contactEmail;
                    }
                    else
                    {
                        advert.Data.Land.ContactEmail = contactEmail;
                    }
                    return View(advert.Data);
                }
                return RedirectToAction("NotFound");
            }
            return RedirectToAction("NotFound");
        }

        public ActionResult Show(string key)
        {
            var parsedSearch = _searchService.ParseKey(key);

            if (parsedSearch.Id > 0)
            {
                var result = _showAdvertService.GetAdvert(parsedSearch.AdType, parsedSearch.Id);

                if (result.Success)
                {
                    if (((List<int>) Session["Visited"]).Find(x => x == Convert.ToInt32(key)) == 0)
                    {
                        ((List<int>)Session["Visited"]).Add(Convert.ToInt32(key));
                        _counterService.AddHit(key);
                    }

                    return View(result.Data);
                }
                return RedirectToAction("NotFound");
            }

            return RedirectToAction("NotFound");
        }

        public ActionResult CreateOffer()
        {
            return View(new CreateOffer());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOffer(CreateOffer createOffer)
        {
            if (ModelState.IsValid)
            {
                var offer = AutoMapper.Mapper.Map<Offer>(createOffer);
                _context.Offers.Add(offer);
                _context.SaveChanges();
                return View("ConfirmOffer", createOffer);
            }
            return View(createOffer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactEmail contactEmail)
        {
            if (ModelState.IsValid)
            {
                _emailService.SendAndSaveContactQuestion(contactEmail);
                return View("ConfirmMail");
            }
            return View(contactEmail);
        }

        public ActionResult Contact()
        {
            return View(new ContactEmail());
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}