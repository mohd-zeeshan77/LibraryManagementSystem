using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApi.Core.Requests;

public sealed class CreateIssuedBookRequest(decimal Dues,
                                            DateOnly IssuedDate,
                                            DateOnly ReturnedDate,
                                            bool RenewStatus,
                                            DateOnly? RenewDate,
                                            bool IsReturned)
{
    public decimal Dues { get; } = Dues;
    public DateOnly IssuedDate { get; } = IssuedDate;
    public DateOnly ReturnedDate { get; } = ReturnedDate;
    public bool RenewStatus { get; } = RenewStatus;
    public DateOnly? RenewDate { get; } = RenewDate;
    public bool IsReturned { get; } = IsReturned;
}
