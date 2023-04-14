using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;
using ZID.Automat.Configuration;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;
using ZID.Automat.Exceptions;
using ZID.Automat.Repository;

namespace ZID.Automat.Application.Test
{
    public class BorrowTest
    {
        [Fact]
        public void TestBorrow_work()
        {
            // arange

            var Username = "GreateUsername";
            var dto = new BorrowDataDto() { DueTime = DateTime.Parse("14/4/2023 07:22:16"), ItemId = 1 };
            var now = DateTime.Parse("14/4/2023 07:22:16");

            var readmock = new Mock<IRepositoryRead>();
            var writemock = new Mock<IRepositoryWrite>();
            var borrowConf = new BorrowCo() { MaxBorrowTime = 7 };
            var IMappermock = new Mock<IMapper>();
            var borrow = new BorrowService(borrowConf, readmock.Object, writemock.Object, IMappermock.Object);

            readmock.Setup(r => r.FindById<Item>(dto.ItemId)).Returns(new Item() { ItemInstances = new List<ItemInstance>(new ItemInstance[] { 
                new ItemInstance(){Id = 2}
            })});

            readmock.Setup(r => r.FindByName<User>(Username)).Returns(new User() { Name = Username});

            //act

            var guid = borrow.Borrow(dto, Username, now);

            //assert

            readmock.Verify(r => r.FindById<Item>(dto.ItemId), Times.Once);
            readmock.Verify(r => r.FindByName<User>(Username), Times.Once);

            writemock.Verify(w => w.Add(It.Is<Borrow>((b) => b.GUID == guid )),Times.Once);
            writemock.Verify(w => w.Add(It.Is<Borrow>((b) => b.User.Name == Username)),Times.Once);
            writemock.Verify(w => w.Add(It.Is<Borrow>((b) => b.PredictedReturnDate == dto.DueTime)),Times.Once);
            writemock.Verify(w => w.Add(It.Is<Borrow>((b) => b.BorrowDate == now)),Times.Once);
            writemock.Verify(w => w.Add(It.Is<Borrow>((b) => b.ItemInstance!.Id == 2)),Times.Once);
            writemock.Verify(w => w.Add(It.Is<Borrow>((b) => b.CollectDate == null)),Times.Once);
            writemock.Verify(w => w.Add(It.Is<Borrow>((b) => b.ReturnDate == null)),Times.Once);
        }

        [Fact]
        public void TestBorrow_NoFreeItem()
        {
            // arange

            var Username = "GreateUsername";
            var dto = new BorrowDataDto() { DueTime = DateTime.Parse("14/4/2023 07:22:16"), ItemId = 1 };
            var now = DateTime.Parse("14/4/2023 07:22:16");

            var readmock = new Mock<IRepositoryRead>();
            var writemock = new Mock<IRepositoryWrite>();
            var borrowConf = new BorrowCo() { MaxBorrowTime = 7 };
            var IMappermock = new Mock<IMapper>();
            var borrow = new BorrowService(borrowConf, readmock.Object, writemock.Object, IMappermock.Object);

            readmock.Setup(r => r.FindById<Item>(dto.ItemId)).Returns(new Item() 
            {
                ItemInstances = new List<ItemInstance>(new ItemInstance[] {
                new ItemInstance(){Id = 2, borrow = new Borrow()}
            })
            });

            readmock.Setup(r => r.FindByName<User>(Username)).Returns(new User() { Name = Username });

            //act
            Guid guid = Guid.Empty;
            Assert.ThrowsAny<NoItemAvailable>(() =>
            {
                guid = borrow.Borrow(dto, Username, now);
            });

            //assert

            readmock.Verify(r => r.FindById<Item>(dto.ItemId), Times.Once);
            readmock.Verify(r => r.FindByName<User>(Username), Times.Never);

            writemock.Verify(w => w.Add(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public void TestBorrow_DueTimeToLate()
        {
            // arange

            var Username = "GreateUsername";
            var dto = new BorrowDataDto() { DueTime = DateTime.Parse("14/5/2023 07:22:16"), ItemId = 1 };
            var now = DateTime.Parse("14/4/2023 07:22:16");

            var readmock = new Mock<IRepositoryRead>();
            var writemock = new Mock<IRepositoryWrite>();
            var borrowConf = new BorrowCo() { MaxBorrowTime = 7 };
            var IMappermock = new Mock<IMapper>();
            var borrow = new BorrowService(borrowConf, readmock.Object, writemock.Object, IMappermock.Object);

            readmock.Setup(r => r.FindById<Item>(dto.ItemId)).Returns(new Item()
            {
                ItemInstances = new List<ItemInstance>(new ItemInstance[] {
                new ItemInstance(){Id = 2}
            })
            });

            readmock.Setup(r => r.FindByName<User>(Username)).Returns(new User() { Name = Username });

            //act
            Guid guid = Guid.Empty;
            Assert.ThrowsAny<BorrowDueTimeInvalidException>(() =>
            {
                guid = borrow.Borrow(dto, Username, now);
            });

            //assert

            readmock.Verify(r => r.FindById<Item>(dto.ItemId), Times.Once);
            readmock.Verify(r => r.FindByName<User>(Username), Times.Once);

            writemock.Verify(w => w.Add(It.IsAny<Borrow>()), Times.Never);
        }

        [Fact]
        public void TestBorrow_UsernameNotExisting()
        {
            // arange

            var Username = "GreateUsername";
            var dto = new BorrowDataDto() { DueTime = DateTime.Parse("14/4/2023 07:22:16"), ItemId = 1 };
            var now = DateTime.Parse("14/4/2023 07:22:16");

            var readmock = new Mock<IRepositoryRead>();
            var writemock = new Mock<IRepositoryWrite>();
            var borrowConf = new BorrowCo() { MaxBorrowTime = 7 };
            var IMappermock = new Mock<IMapper>();
            var borrow = new BorrowService(borrowConf, readmock.Object, writemock.Object, IMappermock.Object);

            readmock.Setup(r => r.FindById<Item>(dto.ItemId)).Returns(new Item()
            {
                ItemInstances = new List<ItemInstance>(new ItemInstance[] {
                new ItemInstance(){Id = 2}
            })
            });


            //act
            Guid guid = Guid.Empty;
            Assert.ThrowsAny<NoUserFoundException>(() =>
            {
                guid = borrow.Borrow(dto, Username, now);
            });

            //assert

            readmock.Verify(r => r.FindById<Item>(dto.ItemId), Times.Once);
            readmock.Verify(r => r.FindByName<User>(Username), Times.Once);

            writemock.Verify(w => w.Add(It.IsAny<Borrow>()), Times.Never);
        }



    }
}