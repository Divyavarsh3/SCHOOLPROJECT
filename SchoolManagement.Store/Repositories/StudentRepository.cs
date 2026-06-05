using Microsoft.Data.SqlClient;
using SchoolManagement.Model.DTOs;
using SchoolManagement.Model.Entities;
using SchoolManagement.Store.Interfaces;
using System.Data;

namespace SchoolManagement.Store.Repositories
{
    /// <summary>
    /// Handles Student Database Operations.
    /// Executes Student Stored Procedures, Bulk Insert Operations,
    /// and supports asynchronous database access using Async/Await.
    /// </summary>
    public class StudentRepository : IStudentRepository
    {
        private readonly DatabaseContext _context;


    public StudentRepository(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new student record.
        /// </summary>
        /// <param name="student">Student creation details.</param>
        /// <returns>Number of rows affected.</returns>
        public async Task<int> CreateAsync(StudentCreateDto student)
        {
            try
            {
                using var connection = _context.CreateConnection();

                using var command = new SqlCommand("usp_Student_Insert", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ClassId", student.ClassId);
                command.Parameters.AddWithValue("@StudentName", student.StudentName);
                command.Parameters.AddWithValue("@Gender", student.Gender);
                command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                command.Parameters.AddWithValue("@Email", student.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CreatedBy", 1);

                await connection.OpenAsync();

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while creating student.", ex);
            }
        }

        /// <summary>
        /// Retrieves students with pagination and filtering.
        /// </summary>
        /// <param name="pageNumber">Current page number.</param>
        /// <param name="pageSize">Number of records per page.</param>
        /// <param name="studentName">Student name filter.</param>
        /// <param name="gender">Gender filter.</param>
        /// <param name="classId">Class filter.</param>
        /// <returns>List of students.</returns>
        public async Task<IEnumerable<Student>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? studentName,
            string? gender,
            int? classId)
        {
            try
            {
                var students = new List<Student>();

                using var connection = _context.CreateConnection();

                using var command = new SqlCommand("usp_Student_GetAll", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@PageNumber", pageNumber);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@StudentName", (object?)studentName ?? DBNull.Value);
                
                await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    students.Add(new Student
                    {
                        StudentGuid = Guid.Parse(reader["StudentGuid"].ToString()!),
                        StudentName = reader["StudentName"].ToString()!,
                        Gender = reader["Gender"].ToString()!,
                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                        Email = reader["Email"]?.ToString(),
                        PhoneNumber = reader["PhoneNumber"]?.ToString(),
                        IsActive = Convert.ToBoolean(reader["IsActive"]),
                        CreatedOn = Convert.ToDateTime(reader["CreatedOn"]),
                        CreatedBy = reader["CreatedBy"] == DBNull.Value
                            ? null
                            : Convert.ToInt32(reader["CreatedBy"]),
                        UpdatedOn = reader["UpdatedOn"] == DBNull.Value
                            ? null
                            : Convert.ToDateTime(reader["UpdatedOn"]),
                        UpdatedBy = reader["UpdatedBy"] == DBNull.Value
                            ? null
                            : Convert.ToInt32(reader["UpdatedBy"])
                    });
                }

                return students;

                
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving students.", ex);
            }
        }

        /// <summary>
        /// Retrieves student details by Guid.
        /// </summary>
        /// <param name="studentGuid">Student Guid.</param>
        /// <returns>Student details.</returns>
        public async Task<Student?> GetByGuidAsync(Guid studentGuid)
        {
            try
            {
                using var connection = _context.CreateConnection();

                using var command = new SqlCommand("usp_Student_GetByGuid", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@StudentGuid", studentGuid);

                await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new Student
                    {
                        
                        StudentGuid = Guid.Parse(reader["StudentGuid"].ToString()!),
                        
                        StudentName = reader["StudentName"].ToString()!,
                        Gender = reader["Gender"].ToString()!,
                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                        Email = reader["Email"]?.ToString()!,
                        PhoneNumber = reader["PhoneNumber"]?.ToString()!,
                        IsActive = Convert.ToBoolean(reader["IsActive"]),
                        CreatedOn = Convert.ToDateTime(reader["CreatedOn"]),
                        CreatedBy = reader["CreatedBy"] == DBNull.Value ? null : Convert.ToInt32(reader["CreatedBy"]),
                        UpdatedOn = reader["UpdatedOn"] == DBNull.Value ? null : Convert.ToDateTime(reader["UpdatedOn"]),
                        UpdatedBy = reader["UpdatedBy"] == DBNull.Value ? null : Convert.ToInt32(reader["UpdatedBy"])
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving student by Guid.", ex);
            }
        }

        /// <summary>
        /// Updates existing student information.
        /// </summary>
        /// <param name="student">Student update details.</param>
        /// <returns>Number of rows affected.</returns>
        public async Task<int> UpdateAsync(StudentUpdateDto student)
        {
            try
            {
                using var connection = _context.CreateConnection();

                using var command = new SqlCommand("usp_Student_Update", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@StudentGuid", student.StudentGuid);
                command.Parameters.AddWithValue("@StudentName", student.StudentName);
                command.Parameters.AddWithValue("@Email", student.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@UpdatedBy", 1);

                await connection.OpenAsync();

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating student.", ex);
            }
        }

        /// <summary>
        /// Deletes student by Guid.
        /// </summary>
        /// <param name="studentGuid">Student Guid.</param>
        /// <param name="updatedBy">Updated user id.</param>
        /// <returns>Number of rows affected.</returns>
        public async Task<int> DeleteAsync(Guid studentGuid, int updatedBy)
        {
            try
            {
                using var connection = _context.CreateConnection();

                using var command = new SqlCommand("usp_Student_Delete", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@StudentGuid", studentGuid);
                command.Parameters.AddWithValue("@UpdatedBy", updatedBy);

                await connection.OpenAsync();

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting student.", ex);
            }
        }

        /// <summary>
        /// Performs bulk student insertion using User Defined Table Type.
        /// </summary>
        /// <param name="studentsTable">Student DataTable.</param>
        /// <param name="createdBy">Created user id.</param>
        /// <returns>Number of rows inserted.</returns>
        public async Task<int> BulkInsertAsync(DataTable studentsTable, int createdBy)
        {
            try
            {
                using var connection = _context.CreateConnection();

                using var command = new SqlCommand("usp_Student_BulkInsert", connection);

                command.CommandType = CommandType.StoredProcedure;

                var tvpParam = new SqlParameter("@Students", SqlDbType.Structured)
                {
                    TypeName = "dbo.Student_Type",
                    Value = studentsTable
                };

                command.Parameters.Add(tvpParam);
                command.Parameters.AddWithValue("@CreatedBy", createdBy);

                await connection.OpenAsync();

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while performing bulk student insert.", ex);
            }
        }
    }


}
