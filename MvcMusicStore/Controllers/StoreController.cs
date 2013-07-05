using System.Web.Mvc;
using MvcMusicStore.Models.DataAccess;

namespace MvcMusicStore.Controllers
{
    public class StoreController : Controller
    {
        readonly IGenresRepository genresRepository;
        readonly IAlbumRepository albumRepository;

        public StoreController(IGenresRepository genresRepository,
            IAlbumRepository albumRepository)
        {
            this.genresRepository = genresRepository;
            this.albumRepository = albumRepository;
        }

        public ActionResult Index()
        {
            var genres = genresRepository.GetAll();
            return View(genres);
        }

        //
        // GET: /Store/Browse?genre=Disco
        public ActionResult Browse(string genre)
        {
            // Retrieve Genre genre and its Associated associated Albums albums from database
            var genreModel = genresRepository.Get(genre);
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
            var genres = genresRepository.GetSorted(9);
            return PartialView(genres);
        }
    }
}