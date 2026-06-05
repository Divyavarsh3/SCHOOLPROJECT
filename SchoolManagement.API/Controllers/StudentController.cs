using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Model.DTOs;
using SchoolManagement.Service.Interfaces;

namespace SchoolManagement.API.Controllers
{
    /// <summary>
    /// Handles Student CRUD Operations, Pagination, Filtering and Asynchronous Database Processing.
    /// Demonstrates the use of async/await for non-blocking API operations.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Asynchronously retrieves all students with pagination and filtering.
        /// Demonstrates async/await for efficient and non-blocking database access.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Number of records per page.</param>
        /// <param name="studentName">Filter by student name.</param>
        /// <param name="gender">Filter by gender.</param>
        /// <param name="classId">Filter by class.</param>
        /// <returns>Paginated list of students.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string? studentName = null,
            [FromQuery] string? gender = null,
            [FromQuery] int? classId = null)
        {
            var result = await _studentService.GetAllAsync(
                pageNumber,
                pageSize,
                studentName,
                gender,
                classId);

            return Ok(result);
        }

        /// <summary>
        /// Asynchronously retrieves student details by Guid.
        /// Uses async/await to perform non-blocking data retrieval operations.
        /// </summary>
        /// <param name="studentGuid">Student Guid.</param>
        /// <returns>Student details.</returns>
        [HttpGet("{studentGuid}")]
        public async Task<IActionResult> GetByGuid(Guid studentGuid)
        {
            var result = await _studentService.GetByGuidAsync(studentGuid);

            if (result == null)
                return NotFound("Student not found.");

            return Ok(result);
        }

        /// <summary>
        /// Asynchronously creates a new student record.
        /// Demonstrates asynchronous database insertion using async/await.
        /// </summary>
        /// <param name="dto">Student information.</param>
        /// <returns>Created student.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(StudentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _studentService.CreateAsync(dto);

            return Ok(result);
        }

        /// <summary>
        /// Asynchronously updates existing student details.
        /// Uses async programming to execute non-blocking update operations.
        /// </summary>
        /// <param name="dto">Updated student information.</param>
        /// <returns>Update status.</returns>
        [HttpPut]
        public async Task<IActionResult> Update(StudentUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _studentService.UpdateAsync(dto);

            if (result == 0)
                return NotFound("Student not found.");

            return Ok("Student updated successfully.");
        }

        /// <summary>
        /// Asynchronously deletes a student by Guid.
        /// Demonstrates asynchronous database deletion using async/await.
        /// </summary>
        /// <param name="studentGuid">Student Guid.</param>
        /// <returns>Delete status.</returns>
        [HttpDelete("{studentGuid}")]
        public async Task<IActionResult> Delete(Guid studentGuid)
        {
            var result = await _studentService.DeleteAsync(studentGuid, 1);

            if (result == 0)
                return NotFound("Student not found.");

            return Ok("Student deleted successfully.");
        }
    }
}