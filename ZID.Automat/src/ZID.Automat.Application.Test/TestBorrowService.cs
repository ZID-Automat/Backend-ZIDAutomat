using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;
using ZID.Automat.Repository;

namespace ZID.Automat.Application.Test
{
    public class TestBorrowService
    {
        [Fact]
        public void TestBorrow()
        {
            var BS = new BorrowService(new TestBorrowItemRepo(), new TestBorrowUserRepo());
            BS.Borrow(new BorrowDataDto() { DueTime = DateTime.Now.AddDays(4), ItemId = 0 }, "TestUser",DateTime.Now);
        }
    }

    internal class TestBorrowUserRepo : IUserRepository
    {
        public void AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public User? FindUser(string UserName)
        {
             return new User() { Username = "TestUser", Joined = DateTime.Now };
        }

        public bool UserExists(string UserName)
        {
            return UserName == "TestUser";
        }
    }

    internal class TestBorrowItemRepo : IItemRepository
    {
        public ItemInstance? getFreeItemInstance(int itemId, DateTime t)
        {
            return new ItemInstance() { FirstAdded = DateTime.Now, Item = new Item(), ItemId = 0, Id = 0 };
        }

        public IReadOnlyList<ItemInstance> getFreeItemInstances(int itemId, DateTime t)
        {
            throw new NotImplementedException();
        }

        public Item? getItem(int ItemId)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Item> getItemWithItemInstance()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Item> getPrevBorrowedItemsOfUser(int UserID)
        {
            throw new NotImplementedException();
        }

        public bool isItemAvalable(int ItemID, DateTime t)
        {
            return ItemID == 0;
        }
    }
}
