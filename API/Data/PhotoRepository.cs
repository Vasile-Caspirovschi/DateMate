using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public PhotoRepository(DataContext dataContext, IMapper mapper)
        {
            _mapper = mapper;
            _dataContext = dataContext;
        }

        public Task<Photo> GetPhotoByIdAsync(int id)
        {
            var photo = _dataContext.Photos.IgnoreQueryFilters().
                SingleOrDefaultAsync(photo => photo.Id == id);
            return photo!;
        }

        public async Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotosAsync()
        {
            return await _dataContext.Photos.IgnoreQueryFilters()
                .Where(photo => photo.IsApproved == false).ProjectTo<PhotoForApprovalDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public void RemovePhoto(Photo photo)
        {
            _dataContext.Photos.Remove(photo);
        }   
    }
}