using NFChawk.Models.Entities;

namespace NFChawk.Models.ViewModels
{
    public class BlogSingleViewModel
    {
        public BlogPost Post { get; set; }
        public CommentFormViewModel CommentForm { get; set; }
    }
}
