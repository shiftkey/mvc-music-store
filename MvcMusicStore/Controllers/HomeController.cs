using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcMusicStore.Models;
using MvcMusicStore.Models.DataAccess;

namespace MvcMusicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAlbumRepository albums;

        public HomeController(IAlbumRepository albums)
        {
            this.albums = albums;
        }

        public ActionResult Index()
        {
            var topAlbums = albums.GetTopSellingAlbums(6);
            return View(topAlbums);
        }
    }
}