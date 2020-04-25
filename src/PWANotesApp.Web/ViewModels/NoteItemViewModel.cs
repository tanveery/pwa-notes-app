/*
 * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
 * AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using PWANotesApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PWANotesApp.Web.ViewModels
{
    public class NoteItemViewModel
    {
        public int NoteId
        {
            get;
            set;
        }

        public NoteItemType Type
        {
            get;
            set;
        }
    }

    public class NewTextNoteItemViewModel : NoteItemViewModel
    {
        public string TextContent
        {
            get;
            set;
        }
    }

    public class EditTextNoteItemViewModel : NewTextNoteItemViewModel
    {
        public int Id
        {
            get;
            set;
        }
    }

    public class NoteItemDetailsViewModel : NoteItemViewModel
    {
        public int Id
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }
    }
}
