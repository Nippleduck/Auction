using Auction.Domain.Entities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Auction.Data.Tests
{
    internal class AuctionStatusEqualityComparer : IEqualityComparer<AuctionStatus>
    {
        public bool Equals([AllowNull] AuctionStatus x, [AllowNull] AuctionStatus y)
        {
            if (x == null && y == null) return true;

            if (x == null || y == null) return false;

            return x.Name.Equals(y.Name);
        }

        public int GetHashCode([DisallowNull] AuctionStatus obj) => obj.GetHashCode();
    }

    internal class BidEqualityComparer : IEqualityComparer<Bid>
    {
        public bool Equals([AllowNull] Bid x, [AllowNull] Bid y)
        {
            if (x == null && y == null) return true;

            if (x == null || y == null) return false;

            return 
                x.Id == y.Id &&
                x.Price == y.Price && 
                x.PlacedOn == y.PlacedOn && 
                x.BiddingDetailsId == y.BiddingDetailsId &&
                x.BidderId == y.BidderId;
        }

        public int GetHashCode([DisallowNull] Bid obj) => obj.GetHashCode();
    }

    internal class BiddingDetailsEqualityComparer : IEqualityComparer<BiddingDetails>
    {
        public bool Equals([AllowNull] BiddingDetails x, [AllowNull] BiddingDetails y)
        {
            if (x == null && y == null) return true;

            if (x == null || y == null) return false;

            return
                x.Id == y.Id &&
                x.LotId == y.LotId &&
                x.MinimalBid == y.MinimalBid &&
                x.CurrentBid == y.CurrentBid &&
                x.Sold == y.Sold;
        }

        public int GetHashCode([DisallowNull] BiddingDetails obj) => obj.GetHashCode();
    }

    internal class CategoryEqualityComparer : IEqualityComparer<Category>
    {
        public bool Equals([AllowNull] Category x, [AllowNull] Category y)
        {
            if( x == null && y == null) return true;

            if (x == null || y == null) return false;

            return x.Name.Equals(y.Name);
        }

        public int GetHashCode([DisallowNull] Category obj) => obj.GetHashCode();   
    }

    internal class LotEqualityComparer : IEqualityComparer<Lot>
    {
        public bool Equals([AllowNull] Lot x, [AllowNull] Lot y)
        {
            if (x == null && y == null) return true;

            if (x == null || y == null) return false;

            return
                x.Id == y.Id &&
                x.BuyerId == y.BuyerId &&
                x.StartPrice == y.StartPrice &&
                x.CloseDate == y.CloseDate &&
                x.OpenDate == y.OpenDate &&
                x.CategoryId == y.CategoryId &&
                x.StatusId == y.StatusId &&
                x.Name == y.Name &&
                x.Description == y.Description;

        }

        public int GetHashCode([DisallowNull] Lot obj) => obj.GetHashCode();
    }

    internal class PersonEqualityComparer : IEqualityComparer<Person>
    {
        public bool Equals([AllowNull] Person x, [AllowNull] Person y)
        {
            if (x == null && y == null) return true;

            if (x == null || y == null) return false;

            return
                x.Id == y.Id &&
                x.Name.Equals(y.Name) &&
                x.Surname.Equals(y.Surname) &&
                x.BirthDate == y.BirthDate;

        }

        public int GetHashCode([DisallowNull] Person obj) => obj.GetHashCode();
    }

    internal class ReviewDetailsEqualityComparer : IEqualityComparer<ReviewDetails>
    {
        public bool Equals([AllowNull] ReviewDetails x, [AllowNull] ReviewDetails y)
        {
            if (x == null && y == null) return true;

            if (x == null || y == null) return false;

            return
                x.Id == y.Id &&
                x.LotId == y.LotId &&
                x.Status == y.Status &&
                x.Feedback == y.Feedback;
        }

        public int GetHashCode([DisallowNull] ReviewDetails obj) => obj.GetHashCode();
    }
}
