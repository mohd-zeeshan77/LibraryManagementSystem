using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApi.Core.Requests;

public sealed class CreateBookRequest(string Name,
                                        string AuthorName,
                                        string Publisher,
                                        string Edition,
                                        decimal Price,
                                        int Stock)
{
    public string Name { get; } = Name;
    public string AuthorName { get; }= AuthorName;
    public string Publisher {  get; }= Publisher;
    public string Edition { get; }= Edition;
    public decimal Price { get; }= Price;
    public int Stock {  get; }= Stock;
}
