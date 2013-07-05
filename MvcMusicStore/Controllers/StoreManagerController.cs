using System.Web.Mvc;
using MvcMusicStore.Models;
using MvcMusicStore.Models.DataAccess;

namespace MvcMusicStore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class StoreManagerController : Controller
    {
        readonly IAlbumRepository albums;
        readonly IGenresRepository genres;
        readonly IArtistsRepository artists;

        public StoreManagerController(
            IAlbumRepository albums,
            IGenresRepository genres,
            IArtistsRepository artists)
        {
            this.albums = albums;
            this.genres = genres;
            this.artists = artists;
        }

        //
        // GET: /StoreManager/

        public ActionResult Index()
        {
            var allAlbums = albums.GetAllSortedByPrice();
            return View(allAlbums);
        }

        //
        // GET: /StoreManager/Details/5

        public ActionResult Details(int id = 0)
        {
            var album = albums.GetById(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        //
        // GET: /StoreManager/Create

        public ActionResult Create()
        {
            ViewBag.GenreId = new SelectList(genres.GetAll(), "GenreId", "Name");
            ViewBag.ArtistId = new SelectList(artists.GetAll(), "ArtistId", "Name");
            return View();
        }

        //
        // POST: /StoreManager/Create

        [HttpPost]
        public ActionResult Create(Album album)
        {
            if (ModelState.IsValid)
            {
                albums.Insert(album);
                albums.Save();
                return RedirectToAction("Index");
            }

            ViewBag.GenreId = new SelectList(genres.GetAll(), "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(artists.GetAll(), "ArtistId", "Name", album.ArtistId);
            return View(album);
        }

        //
        // GET: /StoreManager/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var album = albums.GetById(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            ViewBag.GenreId = new SelectList(genres.GetAll(), "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(artists.GetAll(), "ArtistId", "Name", album.ArtistId);
            return View(album);
        }

        //
        // POST: /StoreManager/Edit/5

        [HttpPost]
        public ActionResult Edit(Album album)
        {
            if (ModelState.IsValid)
            {
                albums.Update(album);
                albums.Save();
                
                return RedirectToAction("Index");
            }
            ViewBag.GenreId = new SelectList(genres.GetAll(), "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(artists.GetAll(), "ArtistId", "Name", album.ArtistId);
            return View(album);
        }

        //
        // GET: /StoreManager/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var album = albums.GetById(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        //
        // POST: /StoreManager/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            albums.Delete(id);
            albums.Save();
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            albums.Dispose();
            genres.Dispose();
            artists.Dispose();
            base.Dispose(disposing);
        }
    }
}