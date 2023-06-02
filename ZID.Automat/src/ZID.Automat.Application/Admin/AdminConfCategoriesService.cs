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
        IEnumerable<CategoryUpdateDto> AllCategories();
        void UpdateCategory(CategoryUpdateDto category);
        void AddCategory(CategoryAddDto category);


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

       public IEnumerable<CategoryUpdateDto> AllCategories()
        {
            return _mapper.Map<IEnumerable<Categorie>, IEnumerable<CategoryUpdateDto>>(_repositoryRead.GetAll<Categorie>());
        }

        public void UpdateCategory(CategoryUpdateDto categoryi)
        {
            var category = _repositoryRead.FindById<Categorie>(categoryi.Id);

            if (category != null)
            {
                category.Description = categoryi.Description ?? category.Description;
                category.Name = categoryi.Name ?? category.Name;
                _repositoryWrite.Update(category);
            }
        }

        public void AddCategory(CategoryAddDto category)
        {
            if(_repositoryRead.FindByName<Categorie>(category.Name) == null)
            {
                _repositoryWrite.Add(new Categorie() { Name = category.Name, Description = category.Description});
            }
        }
    }
}
    
