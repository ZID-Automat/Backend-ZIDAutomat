using AutoMapper;
using System.Collections.Generic;
using Xunit;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;

namespace ZID.Automat.AutoMapper.Test
{
    public class ItemProfileTest
    {

        private readonly IMapper _mapper;
        public ItemProfileTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ItemProfile());
            });
            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public void ItemToItemDisplayDto_ShouldMapCorrectly_Avalable()
        {
            // Arrange
            var item = new Item()
            {
                Id = 0,
                Image = "Image",
                SubName = "SubName",
                Name = "Name",
                ItemInstances = new List<ItemInstance>(new ItemInstance[] {
                    new ItemInstance()
                        {
                            Id = 0,
                            borrow = null
                        }
                    })
            };
            
            // Act
            var ItemDto = _mapper.Map<ItemDisplayDto>(item);

            // Assert
            Assert.Equal(ItemDto.Id, item.Id);
            Assert.Equal(ItemDto.Image, item.Image);
            Assert.Equal(ItemDto.SubName, item.SubName);
            Assert.Equal(ItemDto.Name, item.Name);
            Assert.True(ItemDto.Available);
        }

        [Fact]
        public void ItemToItemDisplayDto_ShouldMapCorrectly_NotAvalable()
        {
            // Arrange
            var item = new Item()
            {
                Id = 0,
                Image = "Image",
                SubName = "SubName",
                Name = "Name",
                ItemInstances = new List<ItemInstance>(new ItemInstance[] {
                    new ItemInstance()
                        {
                            Id = 0,
                            borrow = new Borrow()
                        }
                    })
            };
          
            // Act
            var ItemDto = _mapper.Map<ItemDisplayDto>(item);

            // Assert
            Assert.Equal(ItemDto.Id, item.Id);
            Assert.Equal(ItemDto.Image, item.Image);
            Assert.Equal(ItemDto.SubName, item.SubName);
            Assert.Equal(ItemDto.Name, item.Name);
            Assert.False(ItemDto.Available);
        }


        [Fact]
        public void ItemToItemDetaieldDto_ShouldMapCorrectly_Avalable()
        {
            // Arrange
            var item = new Item()
            {
                Id = 0,
                Image = "Image",
                SubName = "SubName",
                Name = "Name",
                Description = "Description",
                Price = 0,
                Categorie = new Categorie()
                {
                    Name = "Categorie"
                },
                ItemInstances = new List<ItemInstance>(new ItemInstance[] {
                    new ItemInstance()
                        {
                            Id = 0,
                            borrow = null
                        }
                    })

            };

            // Act
            var ItemDto = _mapper.Map<ItemDetailedDto>(item);

            // Assert
            Assert.Equal(ItemDto.Id, item.Id);
            Assert.Equal(ItemDto.Image, item.Image);
            Assert.Equal(ItemDto.SubName, item.SubName);
            Assert.Equal(ItemDto.Name, item.Name);
            Assert.Equal(ItemDto.Price, item.Price);
            Assert.Equal(ItemDto.Categorie, item.Categorie.Name);
            Assert.Equal(ItemDto.Description, item.Description);
            Assert.True(ItemDto.Available);
        }

        [Fact]
        public void ItemToItemDetaieldDto_ShouldMapCorrectly_NotAvalable() 
        {
            // Arrange
            var item = new Item()
            {
                Id = 0,
                Image = "Image",
                SubName = "SubName",
                Name = "Name",
                Description = "Description",
                Price = 0,
                Categorie = new Categorie()
                {
                    Name = "Categorie"
                },
                ItemInstances = new List<ItemInstance>(new ItemInstance[] {
                    new ItemInstance()
                        {
                            Id = 0,
                            borrow = new Borrow()
                        }
                    })
            };

            // Act
            var ItemDto = _mapper.Map<ItemDetailedDto>(item);

            // Assert
            Assert.Equal(ItemDto.Id, item.Id);
            Assert.Equal(ItemDto.Image, item.Image);
            Assert.Equal(ItemDto.SubName, item.SubName);
            Assert.Equal(ItemDto.Name, item.Name);
            Assert.Equal(ItemDto.Price, item.Price);
            Assert.Equal(ItemDto.Categorie, item.Categorie.Name);
            Assert.Equal(ItemDto.Description, item.Description);
            Assert.False(ItemDto.Available);
        } 
    }
}