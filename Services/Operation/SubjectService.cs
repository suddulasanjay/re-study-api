using AutoMapper;
using ReStudyAPI.Entities;
using ReStudyAPI.Interfaces.Operation;
using ReStudyAPI.Interfaces.Repositories;
using ReStudyAPI.Models.Operation;
using ReStudyAPI.Utility.Helpers;

namespace ReStudyAPI.Services.Operation
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly ICurrentSessionHelper _currentSessionHelper;
        private readonly IMapper _mapper;

        public SubjectService(
            ISubjectRepository subjectRepository,
            ICurrentSessionHelper currentSessionHelper,
            IMapper mapper)
        {
            _subjectRepository = subjectRepository;
            _currentSessionHelper = currentSessionHelper;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(AddSubjectDto subjectDto)
        {
            var session = _currentSessionHelper.GetCurrentSession();
            int userId = await _currentSessionHelper.GetUserId(session);
            var subject = _mapper.Map<Subject>(subjectDto);
            subject.CreatedTime = DateTime.UtcNow;
            subject.ModifiedTime = DateTime.UtcNow;
            subject.ModifiedByUserId = userId;
            var subjectId = await _subjectRepository.CreateAsync(subject);
            await _subjectRepository.AssignSubjectToUserAsync(userId, subjectId);
            return subjectId;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var session = _currentSessionHelper.GetCurrentSession();
            int userId = await _currentSessionHelper.GetUserId(session);
            return await _subjectRepository.SoftDeleteAsync(id, userId);
        }

        public async Task<List<SubjectDto>> GetAllAsync()
        {
            var session = _currentSessionHelper.GetCurrentSession();
            int userId = await _currentSessionHelper.GetUserId(session);
            var userSubjects = await _subjectRepository.GetSubjectsByUserIdAsync(userId);
            var presetSubjects = await _subjectRepository.GetPresetSubjectsAsync();
            var subjects = userSubjects.Concat(presetSubjects).GroupBy(s => s.Id).Select(g => g.First()).ToList();
            return _mapper.Map<List<SubjectDto>>(subjects);
        }

        public async Task<SubjectDto?> GetByIdAsync(int id)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);
            return subject != null ? _mapper.Map<SubjectDto>(subject) : null;
        }

        public async Task<bool> UpdateAsync(EditSubjectDto subjectDto)
        {
            var session = _currentSessionHelper.GetCurrentSession();
            int userId = await _currentSessionHelper.GetUserId(session);
            var existing = await _subjectRepository.GetByIdAsync(subjectDto.Id);
            if (existing == null) return false;
            existing.Description = subjectDto.Description;
            existing.ModifiedTime = DateTime.UtcNow;
            existing.ModifiedByUserId = userId;

            return await _subjectRepository.UpdateAsync(existing);
        }
    }
}
