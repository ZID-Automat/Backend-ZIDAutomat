using AutoMapper;
using Xunit;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;

namespace ZID.Automat.AutoMapper.Test
{
    public class BorrowProfileTest
    {

        private readonly IMapper _mapper;
        public BorrowProfileTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new BorrowProfile());
            });
            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public void BorrowToBorrowDtoMapping_ShouldMapCorrectly()
        {
            // Arrange
            var borrow = new Borrow
            {
                Id = 1,
                BorrowDate = new DateTime(2022, 04, 08),
                ReturnDate = new DateTime(2022, 04, 12),
                ItemInstance = new ItemInstance
                {
                    Id = 1,
                    ItemId = 8,
                    Item = new Item()
                    {
                        Id = 8,
                        Name = "Hallo Welt"
                    },
                    FirstAdded = new DateTime(2021, 04, 01)
                },
            };
            // Act
            var borrowDto = _mapper.Map<BorrowDto>(borrow);

            // Assert
            Assert.Equal(borrow.BorrowDate, borrowDto.BorrowDate);
            Assert.Equal(borrow.ReturnDate, borrowDto.ReturnDate);
            Assert.Equal(borrow.PredictedReturnDate, borrowDto.DueDate);
            Assert.Equal(borrow.CollectDate, borrowDto.CollectDate);

            Assert.Equal(borrow.ItemInstance.Id, borrowDto.ItemInstanceId);
            Assert.Equal(borrow.ItemInstance.ItemId, borrowDto.ItemId);
            Assert.Equal(borrow.ItemInstance.Item.Name, borrowDto.ItemName);
        }
    }
}