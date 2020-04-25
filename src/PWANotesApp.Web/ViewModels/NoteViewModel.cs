/*
 * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
 * AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PWANotesApp.Web.ViewModels
{
    public class NoteViewModel
    {
        public string Title
        {
            get;
            set;
        }
    }

    public class IndexNoteViewModel : NoteViewModel
    {
        public int Id
        {
            get;
            set;
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }

        public DateTime LastUpdatedDate
        {
            get;
            set;
        }
    }

    public class NoteDetailsViewModel : IndexNoteViewModel
    {
        public List<NoteItemDetailsViewModel> Items
        {
            get;
            set;
        }
    }

    public class NewNoteViewModel : NoteViewModel
    { }

    public class EditNoteTitleViewModel : NoteViewModel
    {
        public int Id
        {
            get;
            set;
        }
    }
}
