using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;
using ZID.Automat.Repository;

namespace ZID.Automat.Application.Admin
{
    public interface IConfCategoriesService
    {
        IEnumerable<CategoryDto> AllCategories();
        void UpdateCategoryDescription(int categoryId, string newDescription);

    }

    public class ConfCategoriesService : IConfCategoriesService
    {
        private readonly IRepositoryRead _repositoryRead;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrite _repositoryWrite;

        public ConfCategoriesService(IRepositoryRead repositoryRead, IMapper mapper, IRepositoryWrite repositoryWrite)
        {
            _repositoryRead = repositoryRead;
            _mapper = mapper;
            _repositoryWrite = repositoryWrite;


        }

        public void UpdateCategoryDescription(int categoryId, string newDescription)
        {
            var category = _repositoryRead.FindById<Categorie>(categoryId);

            if (category != null)
            {
                category.Description = newDescription;
                _repositoryWrite.Update(category);
            }
        }




       public IEnumerable<CategoryDto> AllCategories()
        {
            return _mapper.Map<IEnumerable<Categorie>, IEnumerable<CategoryDto>>(_repositoryRead.GetAll<Categorie>());
        }
    }
}
    
