using AutoMapper;
using ReStudyAPI.Entities;
using ReStudyAPI.Interfaces.Operation;
using ReStudyAPI.Interfaces.Repositories;
using ReStudyAPI.Models.Operation;
using ReStudyAPI.Utility.Helpers;

namespace ReStudyAPI.Services.Operation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ICurrentSessionHelper _sessionHelper;
        private readonly IMapper _mapper;

        public CategoryService(
            ICategoryRepository categoryRepository,
            ISubjectRepository subjectRepository,
            ICurrentSessionHelper sessionHelper,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _subjectRepository = subjectRepository;
            _sessionHelper = sessionHelper;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var session = _sessionHelper.GetCurrentSession();
            int userId = session?.UserId ?? 0;

            var subjects = await _subjectRepository.GetSubjectsByUserIdAsync(userId);
            subjects.AddRange(await _subjectRepository.GetPresetSubjectsAsync());

            var subjectIds = subjects.Select(s => s.Id).Distinct().ToList();
            var categories = await _categoryRepository.GetCategoriesBySubjectIdsAsync(subjectIds);

            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category == null ? null : _mapper.Map<CategoryDto>(category);
        }

        public async Task<int> CreateAsync(AddCategoryDto dto)
        {
            var session = _sessionHelper.GetCurrentSession();
            if (session == null)
            {
                throw new Exception("Operation Not Allowed");
            };
            int userId = session?.UserId ?? 0;
            var category = _mapper.Map<Category>(dto);
            category.CreatedTime = category.ModifiedTime = DateTime.UtcNow;
            category.ModifiedByUserId = userId;
            var categoryId = await _categoryRepository.CreateAsync(category);
            await _categoryRepository.AssignCategoryToUserAsync(userId, categoryId);
            return categoryId;
        }

        public async Task<bool> UpdateAsync(EditCategoryDto dto)
        {
            var session = _sessionHelper.GetCurrentSession();
            var category = _mapper.Map<Category>(dto);
            category.ModifiedTime = DateTime.UtcNow;
            category.ModifiedByUserId = session?.UserId;
            return await _categoryRepository.UpdateAsync(category);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _categoryRepository.SoftDeleteAsync(id);
        }
    }
}
