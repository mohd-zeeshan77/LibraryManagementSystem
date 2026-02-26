using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApi.Persistence
{
    public sealed class IssuedBook
    {
        public int Id { get; set; }
        public required int UserId { get; set; }
        public required int BookId { get; set; }
        public required decimal Dues {  get; set; }
        public required DateOnly IssuedDate { get; set; }
        public required DateOnly ReturnDate { get; set; }
        public bool RenewStatus {  get; set; }
        public DateOnly? RenewDate { get; set; }
        public bool IsReturned { get; set; }
        public required User User { get; set; }
        public required Book Book { get; set; }


    }
}
