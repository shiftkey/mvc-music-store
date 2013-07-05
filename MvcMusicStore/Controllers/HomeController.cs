using System.Web.Mvc;

namespace MvcMusicStore.Controllers
{
    public class HomeController : Controller
    {
        readonly IAlbumRepository albumRepository;

        public HomeController(IAlbumRepository albumRepository)
        {
            this.albumRepository = albumRepository;
        }

        public ActionResult Index()
        {
            // Get most popular albums
            var albums = albumRepository.GetTopSellingAlbums(6);
            return View(albums);
        }
    }
}