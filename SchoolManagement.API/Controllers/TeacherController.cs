using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Model.DTOs;
using SchoolManagement.Service.Interfaces;

namespace SchoolManagement.API.Controllers
{
    /// <summary>
    /// Handles Teacher CRUD Operations using Asynchronous Programming.
    /// Accessible only by Admin and Teacher roles.
    /// Demonstrates async/await for efficient and non-blocking database operations.
    /// </summary>
    [Authorize(Roles = "Admin,Teacher")]
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        /// <summary>
        /// Asynchronously retrieves all teachers.
        /// Uses async/await for efficient and non-blocking database access.
        /// </summary>
        /// <returns>List of teachers.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _teacherService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Asynchronously retrieves teacher details by Guid.
        /// Demonstrates async data retrieval operations.
        /// </summary>
        /// <param name="teacherGuid">Teacher Guid.</param>
        /// <returns>Teacher details.</returns>
        [HttpGet("{teacherGuid}")]
        public async Task<IActionResult> GetByGuid(Guid teacherGuid)
        {
            var result = await _teacherService.GetByGuidAsync(teacherGuid);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Asynchronously creates a new teacher record.
        /// Demonstrates asynchronous database insertion using async/await.
        /// </summary>
        /// <param name="dto">Teacher information.</param>
        /// <returns>Created teacher.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(TeacherCreateDto dto)
        {
            var result = await _teacherService.CreateAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Asynchronously updates existing teacher details.
        /// Uses async programming for non-blocking update operations.
        /// </summary>
        /// <param name="dto">Updated teacher information.</param>
        /// <returns>Updated teacher.</returns>
        [HttpPut]
        public async Task<IActionResult> Update(TeacherUpdateDto dto)
        {
            var result = await _teacherService.UpdateAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Asynchronously deletes a teacher by Guid.
        /// Demonstrates asynchronous database deletion using async/await.
        /// </summary>
        /// <param name="teacherGuid">Teacher Guid.</param>
        /// <returns>Delete status.</returns>
        [HttpDelete("{teacherGuid}")]
        public async Task<IActionResult> Delete(Guid teacherGuid)
        {
            var result = await _teacherService.DeleteAsync(teacherGuid, 1);
            return Ok(result);
        }
    }
}