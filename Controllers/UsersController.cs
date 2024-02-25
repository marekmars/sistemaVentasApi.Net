using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Response;
using Web_Service_.Net_Core.Models.Tools;
using Web_Service_.Net_Core.Services;

namespace Web_Service_.Net_Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Authenticates the user based on the provided credentials.
        /// </summary>
        /// <param name="oModel">The authentication request model.</param>
        /// <returns>Returns the authentication response.</returns>

        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] AuthRequest oModel)
        {
            // Create a new ApiResponse for UserResponse
            ApiResponse<UserResponse> oApiResponse = new ApiResponse<UserResponse>();
            try
            {
                // Attempt to authenticate the user
                var oUserResponse = _userService.Authenticate(oModel);

                // Check if the user response is null
                if (oUserResponse == null)
                {
                    oApiResponse.Success = 0;
                    oApiResponse.Message = "User y/o contraseña incorrecta";
                }
                else
                {
                    // Set the success flag and welcome message
                    oApiResponse.Success = 1;
                    oApiResponse.Message = $"Bienvenido {oUserResponse.Mail}";
                    oApiResponse.Data = new List<UserResponse> { oUserResponse };
                }
            }
            catch (System.Exception)
            {
                // Rethrow the exception
                throw;
            }

            // Return the ApiResponse as Ok result
            return Ok(oApiResponse);
        }

        /// <summary>
        /// Retrieve a list of users based on the provided query parameters
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Get([FromQuery] QueryParameters oQueryParameters)
        {
            ApiResponse<User> oResponse = new ApiResponse<User>();
            try
            {
                // Call the user service to retrieve users based on the provided query parameters
                oResponse = _userService.GetUsers(oQueryParameters);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the user retrieval process
                oResponse.Success = 0;
                oResponse.Message = $"An error occurred while searching for users: {ex.Message}";
                oResponse.Data = new List<User>();
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);
        }

        /// <summary>
        /// Gets a user by ID
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <returns>The user information as ApiResponse</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            // Initialize ApiResponse for the user
            ApiResponse<User> oApiResponse = new ApiResponse<User>();
            try
            {
                // Call the userService to get user by ID
                oApiResponse = _userService.GetUser(id);
            }
            catch (Exception ex)
            {
                // If an error occurs, set error message in ApiResponse
                oApiResponse.Success = 0;
                oApiResponse.Message = $"An error occurred while searching for the user: {ex.Message}";
                oApiResponse.Data = new List<User>(); // Initialize an empty list for Data
                oApiResponse.TotalCount = 0; // Set TotalCount to 0
            }
            return Ok(oApiResponse); // Return ApiResponse as Ok result
        }


        // Specifies that only users in the "Administrador" role are allowed to access this endpoint
        [Authorize(Roles = "Admin")]
        [HttpDelete("{Id}")]
        /// <summary>
        /// Deletes a user by Id
        /// </summary>
        /// <param name="Id">The Id of the user to be deleted</param>
        /// <returns>Returns the result of the deletion operation</returns>
        public IActionResult Delete(long Id)
        {
            // Initializes the response object
            ApiResponse<User> oResponse = new ApiResponse<User>();
            try
            {
                // Attempts to delete the user using the service
                oResponse = _userService.DeleteUser(Id);
            }
            catch (Exception ex)
            {
                // Handles any exceptions that occur during the deletion process
                oResponse.Success = 0;
                oResponse.Message = $"An error occurred while deleting the user: {ex.Message}";
                oResponse.Data = null;
                oResponse.TotalCount = 0;
            }
            // Returns the response object as an HTTP OK result
            return Ok(oResponse);
        }




        /// <summary>
        /// Update user information
        /// </summary>
        /// <param name="oUserRequest">The user information to be updated</param>
        /// <returns>Returns the result of the update operation</returns>
        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(UserRequest oUserRequest)
        {
            ApiResponse<User> oApiResponse = new ApiResponse<User>();
            try
            {
                // Update the user information
                oApiResponse = _userService.UpdateUser(oUserRequest);
            }
            catch (Exception ex)
            {
                // Handle error and set response properties
                oApiResponse.Success = 0;
                oApiResponse.Message = $"An error occurred while updating the user: {ex.Message}";
                oApiResponse.Data = null;
                oApiResponse.TotalCount = 0;
            }
            return Ok(oApiResponse);
        }

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="oUserRequest">The request object for the new user.</param>
        /// <returns>Returns the result of adding the new user.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(UserRequest oUserRequest)
        {
            ApiResponse<User> oApiResponse = new();
            try
            {
                // Call the service to add the new user
                oApiResponse = _userService.AddUser(oUserRequest);
            }
            catch (Exception ex)
            {
                // Handle and log any exceptions
                oApiResponse.Success = 0;
                oApiResponse.Message = $"An error occurred while adding the user: {ex.Message}";
                oApiResponse.Data = new List<User>();
                oApiResponse.TotalCount = 0;
            }
            return Ok(oApiResponse);
        }


        [HttpGet("check")]
        [Authorize(Roles = "Admin")]
        public IActionResult IsEmailValid([FromForm] string Mail)
        {
            ApiResponse<User> oResponse = new ApiResponse<User>();
            try
            {
                oResponse = _userService.MailExist(Mail);
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error buscando el user {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);


        }

        [HttpDelete("fulldelete/{Id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult FullDeleteUser(long Id)
        {
            ApiResponse<User> oResponse = new ApiResponse<User>();
            try
            {
                oResponse = _userService.FullDeleteUser(Id);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error eliminando el user {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);

        }

    }

}