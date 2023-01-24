namespace ReBook.Models
{
    public class Edition
    {
        [Key]
        public int Id { get; set; }
        //[Required(ErrorMessage = "Please enter an editor")]
        public string Editor { get; set; }
        public Book Book { get; set; }

        [Required(ErrorMessage = "Please enter 13 or 10 digits")]
        [RegularExpression(@"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$",
         ErrorMessage = "Please enter 13 or 10 digits")]
        [Display(Name = "ISBN")]
        public string ISBN13 { get; set; }
        [Required(ErrorMessage = "Invalid Language")]
        public Language Language { get; set; }
        public string Cover { get; set; }
        [Display(Name = "Published Date")]
        public string publishedDate { get; set; }
        [Display(Name = "Pages Count")]
        public int? pageCount { get; set; }

        [Display(Name = "Synopsis")]
        [MaxLength(2000)]
        public string Description { get; set; }

        public Edition()
        {
            Cover = "https://res.cloudinary.com/dngz4sjc2/image/upload/v1624033702/Editions/book-placeholder.jpg";
        }
    }
}
