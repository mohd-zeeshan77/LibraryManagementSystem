using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApi.Core.Dtos;

public sealed class BookIssuedByMember(
    string MemberName,
    string MemberType,
    string BookName,
    string AuthorName,
    string Publisher,
    string Edition,
    decimal Price,
    string CategoryType,
    int StockRemain,
    DateOnly IssuedDate,
    DateOnly ReturnDate,
    bool RenewStatus,
    DateOnly? RenewDate,
    bool IsReturned,
    decimal Dues)
{
    public string MemberName { get; } = MemberName;
    public string MemberType { get; }= MemberType;
    public string BookName { get; }= BookName;
    public string AuthorName { get; }= AuthorName;
    public string Publisher {  get; }= Publisher;
    public string Edition {  get; }= Edition;
    public decimal Price {  get; }= Price;
    public string CategoryType {  get; }= CategoryType;
    public int StockRemain {  get; }= StockRemain;
    public DateOnly IssuedDate {  get; }= IssuedDate;
    public DateOnly ReturnDate { get; }= ReturnDate;
    public bool RenewStatus {  get; }= RenewStatus;
    public DateOnly? RenewDate { get; } = RenewDate;
    public bool IsReturned { get; }= IsReturned;
    public decimal Dues {  get; }= Dues;
}
