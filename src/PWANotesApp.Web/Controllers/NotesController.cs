/*
 * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
 * AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PWANotesApp.Web.Data;
using PWANotesApp.Web.Models;
using PWANotesApp.Web.ViewModels;

namespace PWANotesApp.Web.Controllers
{
    [Authorize]
    public class NotesController : PWANotesApp.Web.Base.ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const int POSITION_ABOVE = 0;
        private const int POSITION_BELOW = 1;

        public NotesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
        }

        #region Note Actions

        public async Task<IActionResult> Index()
        {
            var notes = await _context.Notes
                .Where(m => m.Owner == GetUserId()).ToListAsync();
            var model = new List<IndexNoteViewModel>();

            foreach(var note in notes)
            {
                model.Add(new IndexNoteViewModel()
                {
                    Id = note.Id,
                    Title = note.Title,
                    CreatedDate = note.CreatedDate,
                    LastUpdatedDate = note.LastUpdatedDate
                });
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes.Include(m => m.Items)
                .FirstOrDefaultAsync(m => m.Id == id && m.Owner == GetUserId());
            
            if (note == null)
            {
                return NotFound();
            }

            var model = new NoteDetailsViewModel()
            {
                Id = note.Id,
                Title = note.Title,
                CreatedDate = note.CreatedDate,
                LastUpdatedDate = note.LastUpdatedDate                
            };

            if (note.Items != null)
            {
                model.Items = new List<NoteItemDetailsViewModel>();

                foreach (var item in note.Items.OrderBy(m => m.Order))
                {
                    model.Items.Add(new NoteItemDetailsViewModel()
                    {
                        Id = item.Id,
                        Content = item.Content,
                        NoteId = item.NoteId,
                        Type = item.Type,
                        Order = item.Order
                    });
                }
            }

            return View(model);
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(NewNoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = GetUserId();

                var note = new Note()
                {
                    Title = model.Title,
                    Owner = userId,
                    CreatedBy = userId,
                    CreatedDate = DateTime.UtcNow,
                    LastUpdatedBy = userId,
                    LastUpdatedDate = DateTime.UtcNow
                };

                _context.Notes.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> EditTitle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == id && m.Owner == GetUserId());
            
            if (note == null)
            {
                return NotFound();
            }

            var model = new EditNoteTitleViewModel()
            {
                Id = note.Id,
                Title = note.Title
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTitle(int id, EditNoteTitleViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == id && m.Owner == GetUserId());

            if (note == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    note.Title = model.Title;
                    note.LastUpdatedBy = GetUserId();
                    note.LastUpdatedDate = DateTime.UtcNow;
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction(nameof(Details), new { id = id });
            }
            return View(note);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == id && m.Owner == GetUserId());
            
            if (note == null)
            {
                return NotFound();
            }

            var model = new IndexNoteViewModel()
            {
                Id = note.Id,
                Title = note.Title,
                CreatedDate = note.CreatedDate,
                LastUpdatedDate = note.LastUpdatedDate
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == id && m.Owner == GetUserId());

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion Note Actions

        #region Note Item Actions

        public async Task<IActionResult> AddText(int? id, int? position, int? relItemId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == (int)id && m.Owner == GetUserId());

            if (note == null)
            {
                return NotFound();
            }

            var model = new NewTextNoteItemViewModel()
            {
                NoteId = note.Id,
                TextContent = "",
                Type = NoteItemType.Text,
                Position = position,
                RelativeItemId = relItemId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddText(int? id, NewTextNoteItemViewModel model)
        {
            if(id == null)
            {
                return NotFound();
            }

            if(id != model.NoteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var note = await _context.Notes
                    .FirstOrDefaultAsync(m => m.Id == (int)id && m.Owner == GetUserId());
                
                if(note == null)
                {
                    return NotFound();
                }

                var noteItem = new NoteItem()
                {
                    Content = model.TextContent,
                    NoteId = (int)id,
                    Type =  NoteItemType.Text,
                    Order = 1
                };

                _context.NoteItems.Add(noteItem);
                await _context.SaveChangesAsync();

                await UpdateNoteItemsOrderAfterInsertAsync(note.Id, model.Position, model.RelativeItemId, noteItem.Id);

                return RedirectToAction(nameof(Details), new { id = note.Id });
            }

            return View(model);
        }

        public async Task<IActionResult> EditText(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noteItem = await _context.NoteItems.Include(m => m.Note)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (noteItem == null)
            {
                return NotFound();
            }

            if(noteItem.Note.Owner != GetUserId())
            {
                return NotFound();
            }

            var model = new EditTextNoteItemViewModel()
            {
                Id = noteItem.Id,
                NoteId = noteItem.NoteId,
                Type =  noteItem.Type,
                TextContent = noteItem.Content
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditText(int id, EditTextNoteItemViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            
            var noteItem = await _context.NoteItems.Include(m => m.Note)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (noteItem == null)
            {
                return NotFound();
            }

            if (noteItem.Note.Owner != GetUserId())
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    noteItem.Content = model.TextContent;
                    noteItem.Note.LastUpdatedBy = GetUserId();
                    noteItem.Note.LastUpdatedDate = DateTime.UtcNow;
                    _context.Update(noteItem);
                    _context.Update(noteItem.Note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction(nameof(Details), new { id = noteItem.NoteId });
            }
            return View(model);
        }

        public async Task<IActionResult> AddImage(int? id, int? position, int? relItemId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == (int)id && m.Owner == GetUserId());

            if (note == null)
            {
                return NotFound();
            }

            var model = new NewImageNoteItemViewModel()
            {
                NoteId = note.Id,
                Type = NoteItemType.Text,
                Position = position,
                RelativeItemId = relItemId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(int? id, NewImageNoteItemViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id != model.NoteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var note = await _context.Notes
                    .FirstOrDefaultAsync(m => m.Id == (int)id && m.Owner == GetUserId());

                if (note == null)
                {
                    return NotFound();
                }

                var fileName = UploadedImage(model);

                var noteItem = new NoteItem()
                {
                    Content = fileName,
                    NoteId = (int)id,
                    Type = NoteItemType.Picture,
                    Order = 0
                };

                _context.NoteItems.Add(noteItem);
                await _context.SaveChangesAsync();

                await UpdateNoteItemsOrderAfterInsertAsync(note.Id, model.Position, model.RelativeItemId, noteItem.Id);

                return RedirectToAction(nameof(Details), new { id = note.Id });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGeoLoc(int? id, int? position, int? relItemId, double? longitude, double? latitude)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == (int)id && m.Owner == GetUserId());

            if (note == null)
            {
                return NotFound();
            }

            var geoLoc = new
            {
                Longitude = longitude,
                Latitude = latitude
            };

            string strGeoLocJson = JsonConvert.SerializeObject(geoLoc);

            var noteItem = new NoteItem()
            {
                Content = strGeoLocJson,
                NoteId = (int)id,
                Type = NoteItemType.GeoLocation,
                Order = 1
            };

            _context.NoteItems.Add(noteItem);
            await _context.SaveChangesAsync();

            await UpdateNoteItemsOrderAfterInsertAsync(note.Id, position, relItemId, noteItem.Id);

            return RedirectToAction(nameof(Details), new { id = note.Id });
        }  

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(int id, int? position, int? relItemId)
        {
            var noteItem = await _context.NoteItems.Include(m => m.Note)
                .FirstOrDefaultAsync(m => m.Id == id);

            if(noteItem == null)
            {
                return NotFound();
            }

            if(noteItem.Note.Owner != GetUserId())
            {
                return NotFound();
            }

            var noteId = noteItem.NoteId;
            var deletedItemOrder = noteItem.Order;

            _context.NoteItems.Remove(noteItem);
            await _context.SaveChangesAsync();

            await UpdateNoteItemsOrderAfterDeleteAsync(noteId, deletedItemOrder);

            return RedirectToAction(nameof(Details), new { id = noteId });
        }

        private async Task UpdateNoteItemsOrderAfterInsertAsync(int noteId, int? position, int? relativeItemId, int itemId)
        {
            var note = await _context.Notes.Where(m => m.Id == noteId).Include(m => m.Items).FirstOrDefaultAsync();

            if (note == null)
            {
                throw new Exception("Note not found.");
            }

            if(note.Items.Count == 1)
            {
                var newNoteItem = note.Items.FirstOrDefault();
                newNoteItem.Order = 1;
                _context.Update(newNoteItem);
                return;
            }

            if(position == null)
            {
                throw new ArgumentNullException(nameof(position));
            }

            if (relativeItemId == null)
            {
                throw new ArgumentNullException(nameof(relativeItemId));
            }

            var relativeItem = note.Items.Where(m => m.Id == relativeItemId).SingleOrDefault();

            int currentOrder = relativeItem.Order;
            int desiredOrder = currentOrder + 1;

            if (currentOrder == 1 && position == POSITION_ABOVE)
            {
                desiredOrder = currentOrder;
            }
            else
            {
                if (position == POSITION_ABOVE)
                {
                    desiredOrder = currentOrder;
                }
                else
                {
                    desiredOrder = currentOrder + 1;
                }
            }

            IEnumerable<NoteItem> items = null;

            if (position == POSITION_BELOW)
            {
                items = note.Items.Where(m => m.Order > currentOrder && m.Id != itemId);
            }
            else if (position == POSITION_ABOVE)
            {
                items = note.Items.Where(m => m.Order >= desiredOrder && m.Id != itemId);
            }

            foreach (var item in items)
            {
                item.Order = item.Order + 1;
                _context.Entry<NoteItem>(item).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            var newItem = note.Items.Where(m => m.Id == itemId).SingleOrDefault();

            newItem.Order = desiredOrder;
            _context.Entry<NoteItem>(newItem).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        private async Task UpdateNoteItemsOrderAfterDeleteAsync(int noteId, int deletedItemOrder)
        {
            var note = _context.Notes.Where(m => m.Id == noteId).SingleOrDefault();

            if (note == null)
            {
                throw new Exception("Note item not found.");
            }

             var items = _context.NoteItems.Where(m => m.NoteId == note.Id && m.Order > deletedItemOrder);

            foreach (var item in items)
            {
                item.Order = item.Order - 1;
                _context.Entry<NoteItem>(item).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }

        private string UploadedImage(NewImageNoteItemViewModel model)
        {
            string uniqueFileName = null;

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "content/img");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        #endregion Note Item Actions
    }
}
