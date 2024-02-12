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
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService userService)
        {
            _usuarioService = userService;
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
                var oUserResponse = _usuarioService.Authenticate(oModel);

                // Check if the user response is null
                if (oUserResponse == null)
                {
                    oApiResponse.Success = 0;
                    oApiResponse.Message = "Usuario y/o contraseña incorrecta";
                }
                else
                {
                    // Set the success flag and welcome message
                    oApiResponse.Success = 1;
                    oApiResponse.Message = $"Bienvenido {oUserResponse.Correo}";
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
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Get([FromQuery] QueryParameters oQueryParameters)
        {
            ApiResponse<Usuario> oResponse = new ApiResponse<Usuario>();
            try
            {
                // Call the usuario service to retrieve users based on the provided query parameters
                oResponse = _usuarioService.GetUsuarios(oQueryParameters);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the user retrieval process
                oResponse.Success = 0;
                oResponse.Message = $"An error occurred while searching for users: {ex.Message}";
                oResponse.Data = new List<Usuario>();
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);
        }

        /// <summary>
        /// Gets a user by ID
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <returns>The user information as ApiResponse</returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            // Initialize ApiResponse for the user
            ApiResponse<Usuario> oApiResponse = new ApiResponse<Usuario>();
            try
            {
                // Call the usuarioService to get user by ID
                oApiResponse = _usuarioService.GetUsuario(id);
            }
            catch (Exception ex)
            {
                // If an error occurs, set error message in ApiResponse
                oApiResponse.Success = 0;
                oApiResponse.Message = $"An error occurred while searching for the user: {ex.Message}";
                oApiResponse.Data = new List<Usuario>(); // Initialize an empty list for Data
                oApiResponse.TotalCount = 0; // Set TotalCount to 0
            }
            return Ok(oApiResponse); // Return ApiResponse as Ok result
        }


        // Specifies that only users in the "Administrador" role are allowed to access this endpoint
        [Authorize(Roles = "Administrador")]
        [HttpDelete("{Id}")]
        /// <summary>
        /// Deletes a user by Id
        /// </summary>
        /// <param name="Id">The Id of the user to be deleted</param>
        /// <returns>Returns the result of the deletion operation</returns>
        public IActionResult Delete(long Id)
        {
            // Initializes the response object
            ApiResponse<Usuario> oResponse = new ApiResponse<Usuario>();
            try
            {
                // Attempts to delete the user using the service
                oResponse = _usuarioService.DeleteUsuario(Id);
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
        /// <param name="oUsuarioRequest">The user information to be updated</param>
        /// <returns>Returns the result of the update operation</returns>
        [HttpPatch]
        [Authorize(Roles = "Administrador")]
        public IActionResult Update(UsuarioRequest oUsuarioRequest)
        {
            ApiResponse<Usuario> oApiResponse = new ApiResponse<Usuario>();
            try
            {
                // Update the user information
                oApiResponse = _usuarioService.UpdateUsuario(oUsuarioRequest);
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
        /// <param name="oUsuarioRequest">The request object for the new user.</param>
        /// <returns>Returns the result of adding the new user.</returns>
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Add(UsuarioRequest oUsuarioRequest)
        {
            ApiResponse<Usuario> oApiResponse = new();
            try
            {
                // Call the service to add the new user
                oApiResponse = _usuarioService.AddUsuario(oUsuarioRequest);
            }
            catch (Exception ex)
            {
                // Handle and log any exceptions
                oApiResponse.Success = 0;
                oApiResponse.Message = $"An error occurred while adding the user: {ex.Message}";
                oApiResponse.Data = new List<Usuario>();
                oApiResponse.TotalCount = 0;
            }
            return Ok(oApiResponse);
        }


        [HttpGet("check")]
        [Authorize(Roles = "Administrador")]
        public IActionResult IsEmailValid([FromForm] string Correo)
        {
            ApiResponse<Usuario> oResponse = new ApiResponse<Usuario>();
            try
            {
                oResponse = _usuarioService.CorreoExiste(Correo);
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error buscando el usuario {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);


        }

        [HttpDelete("fulldelete/{Id}")]
        [Authorize(Roles = "Administrador")]
        public IActionResult FullDeleteUsuario(long Id)
        {
            ApiResponse<Usuario> oResponse = new ApiResponse<Usuario>();
            try
            {
                oResponse = _usuarioService.FullDeleteUsuario(Id);

            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = $"Ocurrio un error eliminando el usuario {ex.Message}";
                oResponse.Data = [];
                oResponse.TotalCount = 0;
            }
            return Ok(oResponse);

        }

    }

}