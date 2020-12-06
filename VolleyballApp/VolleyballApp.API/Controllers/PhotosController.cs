using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VolleyballApp.API.Data;
using VolleyballApp.API.Dtos;
using VolleyballApp.API.Helpers;
using VolleyballApp.API.Models;

namespace VolleyballApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IVolleyballRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        public PhotosController(IVolleyballRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repo = repo;
            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }


        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var userFromRepo = await _repo.GetUser(userId);
            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            photoForCreationDto.Url = uploadResult.Url.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;
            if (userFromRepo.Photo != null)
            {
                var photoId = userFromRepo.Photo.Id;
                var photoToDelete = await _repo.GetPhoto(photoId);
                var deleteParams = new DeletionParams(photoToDelete.PublicId);
                var result = _cloudinary.Destroy(deleteParams);
                if (result.Result == "ok")
                {
                    _repo.Delete(photoToDelete);
                }
                userFromRepo.Photo = null;
            }
            var photo = _mapper.Map<Photo>(photoForCreationDto);

            userFromRepo.Photo = photo;     
            if (await _repo.saveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { userId = userId, id = photo.Id}, photoToReturn);
            }

            return BadRequest("Could not add the photo");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserPhoto(int userId) 
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var user = await _repo.GetUser(userId);
            if (user.Photo == null)
            {
                return BadRequest("User does not have a photo");
            }
            var photoFromRepo = await _repo.GetPhoto(user.Photo.Id);
            var deleteParams = new DeletionParams(photoFromRepo.PublicId);
            var result = _cloudinary.Destroy(deleteParams);
            if (result.Result == "ok")
            {
                _repo.Delete(photoFromRepo);
            }
            if (await _repo.saveAll())
            {
                return Ok();
            }
            return BadRequest("Failed to delete the photo");
        }


        [HttpPost("{teamId}")]
        public async Task<IActionResult> AddPhotoForTeam(int userId,int teamId, [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var teamFromRepo = await _repo.GetTeam(teamId);
            if (userId != teamFromRepo.OwnerId) return Unauthorized();
            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("auto")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            photoForCreationDto.Url = uploadResult.Url.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;
            if (teamFromRepo.Photo != null)
            {
                var photoId = teamFromRepo.Photo.Id;
                var photoToDelete = await _repo.GetPhoto(photoId);
                var deleteParams = new DeletionParams(photoToDelete.PublicId);
                var result = _cloudinary.Destroy(deleteParams);
                if (result.Result == "ok")
                {
                    _repo.Delete(photoToDelete);
                }
                teamFromRepo.Photo = null;
            }
            var photo = _mapper.Map<Photo>(photoForCreationDto);

            teamFromRepo.Photo = photo;     
            if (await _repo.saveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { userId = userId, id = photo.Id}, photoToReturn);
            }

            return BadRequest("Could not add the photo");
        }

        [HttpDelete("{id}/{teamId}")]
        public async Task<IActionResult> DeleteTeamPhoto(int userId,int teamId) 
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var teamFromRepo = await _repo.GetTeam(teamId);
            if (userId != teamFromRepo.OwnerId) return Unauthorized();
            if (teamFromRepo.Photo == null)
            {
                return BadRequest("Team does not have a photo");
            }
            var photoFromRepo = await _repo.GetPhoto(teamFromRepo.Photo.Id);
            var deleteParams = new DeletionParams(photoFromRepo.PublicId);
            var result = _cloudinary.Destroy(deleteParams);
            if (result.Result == "ok")
            {
                _repo.Delete(photoFromRepo);
            }
            if (await _repo.saveAll())
            {
                return Ok();
            }
            return BadRequest("Failed to delete the photo");
        }
    }
}