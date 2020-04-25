/*
 * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
 * AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PWANotesApp.Web.Models
{
    public class NoteItem
    {
        [Required]
        public int Id
        {
            get;
            set;
        }

        [Required]
        public int NoteId
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }

        [Required]
        public NoteItemType Type
        {
            get;
            set;
        }

        [Required]
        public int Order
        {
            get;
            set;
        }

        public virtual Note Note
        {
            get;
            set;
        }
    }
}