using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcMusicStore.Models;
using MvcMusicStore.Models.DataAccess;

namespace MvcMusicStore.Controllers
{
    public class HomeController : Controller
    {
        readonly Func<int, List<Album>> getTopSellingAlbums;
        public HomeController(Func<int, List<Album>> getTopSellingAlbums)
        {
            this.getTopSellingAlbums = getTopSellingAlbums;
        }

        public HomeController(IAlbumRepository albumRepository)
        {
            getTopSellingAlbums = albumRepository.GetTopSellingAlbums;
        }

        public ActionResult Index()
        {
            var albums = getTopSellingAlbums(6);
            return View(albums);
        }
    }
}