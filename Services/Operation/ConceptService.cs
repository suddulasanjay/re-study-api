using AutoMapper;
using ReStudyAPI.Entities;
using ReStudyAPI.Interfaces.Operation;
using ReStudyAPI.Interfaces.Repositories;
using ReStudyAPI.Models.Operation;
using ReStudyAPI.Utility.Constants;
using ReStudyAPI.Utility.Helpers;

namespace ReStudyAPI.Services.Operation
{
    public class ConceptService : IConceptService
    {
        private readonly IConceptRepository _conceptRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentSessionHelper _sessionHelper;

        public ConceptService(IConceptRepository conceptRepository, ICategoryRepository categoryRepository, ISubjectRepository subjectRepository, IMapper mapper, ICurrentSessionHelper sessionHelper)
        {
            _conceptRepository = conceptRepository;
            _mapper = mapper;
            _sessionHelper = sessionHelper;
            _subjectRepository = subjectRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<ConceptDto>> GetAllAsync()
        {
            var session = _sessionHelper.GetCurrentSession();
            int userId = await _sessionHelper.GetUserId(session);
            var subjects = await _subjectRepository.GetSubjectsByUserIdAsync(userId);
            subjects.AddRange(await _subjectRepository.GetPresetSubjectsAsync());
            var subjectIds = subjects.Select(subject => subject.Id).Distinct().ToList();
            var categories = await _categoryRepository.GetCategoriesBySubjectIdsAsync(subjectIds);
            var categoryIds = categories.Select(category => category.Id).Distinct().ToList();
            var concepts = await _conceptRepository.GetConceptsByCategoryIdsAsync(categoryIds);
            return _mapper.Map<List<ConceptDto>>(concepts);
        }

        public async Task<ConceptDto?> GetByIdAsync(int id)
        {
            var concept = await _conceptRepository.GetByIdAsync(id);
            return concept == null ? null : _mapper.Map<ConceptDto>(concept);
        }

        public async Task<int> CreateAsync(AddConceptDto dto)
        {
            var session = _sessionHelper.GetCurrentSession();
            var concept = _mapper.Map<Concept>(dto);
            concept.CreatedTime = concept.ModifiedTime = DateTime.UtcNow;
            concept.ModifiedByUserId = await _sessionHelper.GetUserId(session);
            return await _conceptRepository.CreateAsync(concept);
        }

        public async Task<bool> UpdateAsync(EditConceptDto dto)
        {
            var session = _sessionHelper.GetCurrentSession();
            var concept = _mapper.Map<Concept>(dto);
            concept.ModifiedTime = DateTime.UtcNow;
            concept.ModifiedByUserId = await _sessionHelper.GetUserId(session);
            return await _conceptRepository.UpdateAsync(concept);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _conceptRepository.SoftDeleteAsync(id);
        }

        public async Task<bool> RecordStudySessionAsync(AddStudySessionDto dto)
        {
            var session = _sessionHelper.GetCurrentSession();

            var activity = new UserConceptActivity
            {
                ConceptId = dto.ConceptId,
                UserId = await _sessionHelper.GetUserId(session),
                ActivityDate = DateTime.UtcNow,
                Duration = dto.Duration,
                ConceptStateId = dto.ConceptStateId,
                Comment = dto.Comment,
                Status = CommonStatus.Enabled,
            };
            activity.CreatedTime = activity.ModifiedTime = DateTime.UtcNow;
            activity.ModifiedByUserId = await _sessionHelper.GetUserId(session);

            return await _conceptRepository.TrackUserConceptActivityAsync(activity);
        }

        public async Task<StudySessionDto> GetStudySessionDetailsAsync(int conceptId)
        {
            var session = _sessionHelper.GetCurrentSession();
            int userId = await _sessionHelper.GetUserId(session);
            return await _conceptRepository.GetStudySessionDetailsAsync(conceptId, userId, DateTime.UtcNow);
        }
    }
}
