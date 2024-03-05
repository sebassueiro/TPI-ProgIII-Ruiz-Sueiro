using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TPI_ProgramacionIII.Data.Entities;
using TPI_ProgramacionIII.Models;
using TPI_ProgramacionIII.Services.Interfaces;

namespace TPI_ProgramacionIII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IUserService _userService;

        public ClientController(IUserService userService, IClientService clientService)
        {
            _userService = userService;
            _clientService = clientService;
        }

        [HttpGet("GetAllClients")]
        [Authorize]
        public IActionResult GetClients()
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin")
            {
                var clients = _clientService.GetClients();

                try
                {
                    return Ok(clients.Where(x => x.State == true)); //solo los activos
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return Forbid();
        }


        [HttpGet("GetClientById{id}")]
        [Authorize]
        public IActionResult GetClientById(int id)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin")
            {
                var client = _clientService.GetClientById(id);

                if (client == null)
                {
                    return NotFound($"El cliente de ID: {id} no fue encontrado");
                }

                return Ok(client);
            }
            return Forbid();
        }

        [HttpPost("CreateNewClient")]
        public IActionResult CreateClient([FromBody] ClientPostDto dto)
        {
           
                if (dto.Name == "string" || dto.LastName == "string" || dto.Email == "string" || dto.UserName == "string" || dto.Password == "string" || dto.Adress == "string")
                {
                    return BadRequest("Cliente no creado, por favor completar los campos");
                }
                try
                {
                    var client = new Client()
                    {
                        Email = dto.Email,
                        Name = dto.Name,
                        LastName = dto.LastName,
                        Password = dto.Password,
                        UserName = dto.UserName,
                        Address = dto.Adress,
                        UserType = "Client"
                    };
                    int id = _userService.CreateUser(client);
                    return Ok($"Cliente creado exitosamente con id: {id}");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                
        }


        [HttpDelete("DeleteClient/{id}")]
        [Authorize]
        public IActionResult DeleteClient(int id)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin")
            {
                try
                {
                    var existingClient = _clientService.GetClientById(id);
                    if (existingClient == null)
                    {
                        return NotFound($"No se encontró ningún Cliente con el ID: {id}");
                    }
                    _userService.DeleteUser(id);
                    return Ok($"Cliente con ID: {id} eliminado");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return Forbid();
        }

        [HttpPut("UpdateClient{id}")]
        [Authorize]
        public IActionResult UpdateClient([FromRoute] int id, [FromBody] ClientPutDto client)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin" || role == "Client")
            {
                var clientToUpdate = _clientService.GetClientById(id);
                if (clientToUpdate == null)
                {
                    return NotFound($"Cliente con ID {id} no encontrado");
                }
                if (client.Email == "string" || client.UserName == "string" || client.Password == "string" || client.Adress == "string")
                {
                    return BadRequest("Cliente no actualizado, por favor completar los campos");
                }
                try
                {
                    clientToUpdate.Email = client.Email;
                    clientToUpdate.Password = client.Password;
                    clientToUpdate.UserName = client.UserName;
                    clientToUpdate.Address = client.Adress;

                    clientToUpdate = _clientService.UpdateClient(clientToUpdate);
                    return Ok($"Cliente actualizado exitosamente");
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error al actualizar cliente: {ex.Message}");
                }
            }
            return Forbid();
        }
    }
}
