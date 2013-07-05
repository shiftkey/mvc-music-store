using System.Web.Mvc;
using MvcMusicStore.Models.DataAccess;

namespace MvcMusicStore.Controllers
{
    public class StoreController : Controller
    {
        readonly IGenresRepository genres;
        readonly IAlbumRepository albumRepository;

        public StoreController(IGenresRepository genres,
            IAlbumRepository albumRepository)
        {
            this.genres = genres;
            this.albumRepository = albumRepository;
        }

        public ActionResult Index()
        {
            var allGenres = genres.GetAll();
            return View(allGenres);
        }

        //
        // GET: /Store/Browse?genre=Disco
        public ActionResult Browse(string genre)
        {
            // Retrieve Genre genre and its Associated associated Albums albums from database
            var genreModel = genres.Get(genre);
            return View(genreModel);
        }

        public ActionResult Details(int id)
        {
            var album = albumRepository.GetById(id);
            return View(album);
        }

        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            var allGenres = genres.GetSorted(9);
            return PartialView(allGenres);
        }
    }
}