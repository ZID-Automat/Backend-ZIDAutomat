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
    public class ItemTest
    {

    /*    [Fact]
        public void TestAllItems()
        {
            //arrange
            var readmock = new Mock<IRepositoryRead>();
            var writemock = new Mock<IRepositoryWrite>();
            var IMappermock = new Mock<IMapper>();
            var ItemService = new ItemService(readmock.Object, writemock.Object, IMappermock.Object);

            readmock.Setup(r => r.GetAll<Item>()).Returns(new List<Item>(new Item[] {
                new Item(){Id = 1, ItemInstances = new List<ItemInstance>(new ItemInstance[] {
                    new ItemInstance(){Id = 1, borrow = new Borrow()}
                })},
                 new Item(){Id = 2, ItemInstances = new List<ItemInstance>(new ItemInstance[] {
                    new ItemInstance(){Id = 1, borrow = new Borrow()},
                    new ItemInstance(){Id = 1}
                })},
                  new Item(){Id = 1, ItemInstances = new List<ItemInstance>(new ItemInstance[] {
                    new ItemInstance(){Id = 1, borrow = new Borrow() },
                    new ItemInstance(){Id = 1}
                })}
            }));

            IMappermock.Setup(m => m.Map<IEnumerable<Item>, IEnumerable<ItemDisplayDto>>(It.IsAny<IEnumerable<Item>>())).Returns(new List<ItemDisplayDto>(new ItemDisplayDto[] { new ItemDisplayDto(), new ItemDisplayDto() }));


            //act
            var items = ItemService.AllDisplayItems();

            //assert
            IMappermock.Verify(m => m.Map<IEnumerable<Item>, IEnumerable<ItemDisplayDto>>(It.Is<IEnumerable<Item>>(i => i.Count() == 3)), Times.Once);
        }

        [Fact]
        public void TestPrevBorrowedItems()
        {
            //arrange
            var Username = "Useri12345";
            var readmock = new Mock<IRepositoryRead>();
            var writemock = new Mock<IRepositoryWrite>();
            var IMappermock = new Mock<IMapper>();
            var ItemService = new ItemService(readmock.Object, writemock.Object, IMappermock.Object);

            var Itemi = new Item() { Name = "hi" };


            readmock.Setup(r => r.GetAll<Borrow>()).Returns
            (
                new List<Borrow>(
                        new Borrow[]
                        {
                            new Borrow()
                            {
                                User = new User()
                                {
                                    Name = Username
                                },
                                ItemInstance = new ItemInstance()
                                {
                                    Item = Itemi
                                }
                            },
                            new Borrow()
                            {
                                User = new User()
                                {
                                    Name = Username
                                },
                                ItemInstance = new ItemInstance()
                                {
                                    Item = Itemi
                                }
                            },
                            new Borrow()
                            {
                                User = new User()
                                {
                                    Name = Username
                                },
                                ItemInstance = new ItemInstance()
                                {
                                    Item = Itemi
                                }
                            },
                            new Borrow()
                            {
                                User = new User()
                                {
                                    Name = Username+"asd"
                                },
                                ItemInstance = new ItemInstance()
                                {
                                    Item = Itemi
                                }
                            },
                              new Borrow()
                            {
                                User = new User()
                                {
                                    Name = Username+"asd"
                                },
                                ItemInstance = new ItemInstance()
                                {
                                    Item = new Item()
                                }
                            }
                        }
                    )
            );

            IMappermock.Setup(m => m.Map<IEnumerable<Item>, IEnumerable<ItemDisplayDto>>(It.IsAny<IEnumerable<Item>>())).Returns(new List<ItemDisplayDto>(new ItemDisplayDto[] { new ItemDisplayDto(), new ItemDisplayDto() }));


            //act
            var items = ItemService.PrevBorrowedDisplayItemsUser(Username);

            //assert
            IMappermock.Verify(m => m.Map<IEnumerable<ItemDisplayDto>>(It.Is<IEnumerable<Item>>(i => i.Count() == 1)), Times.Once);
        }*/
    }
}