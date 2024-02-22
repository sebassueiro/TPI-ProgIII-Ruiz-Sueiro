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
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("GetAllByClient{clientId}")]
        public IActionResult GetAllByClient([FromRoute] int clientId)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin" || role == "Client")
            {
                try
                {
                    var orders = _orderService.GetAllByClient(clientId);
                    if (orders.Count == 0)
                    {
                        return NotFound("Órdenes de venta no encontradas");
                    }
                    return Ok(orders);

                }
                catch (Exception ex)
                {
                    return BadRequest($"Error: {ex.Message}");
                }
            }
            return Forbid();
        }

        [HttpGet("GetAllByDate/{date}")]
        public IActionResult GetAllByDate([FromRoute] DateTime date)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin")
            {
                try
                {
                    var orders = _orderService.GetAllByDate(date);
                    if (orders.Count == 0)
                    {
                        return NotFound($"No hay Órdenes de venta para la fecha seleccionada");
                    }
                    return Ok(orders);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error: {ex.Message}");
                }
            }
            return Forbid();
        }

        [HttpGet("GetSaleOrderById/{orderId}")]
        public IActionResult GetOne([FromRoute] int orderId)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin")
            {
                try
                {
                    var order = _orderService.GetOne(orderId);

                    if (order == null)
                    {
                        return NotFound($"Orden de venta con id {orderId} no encontrada");
                    }

                    return Ok(order);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error: {ex.Message}");
                }
            }
            return Forbid();
        }

        [HttpPost("CreateSaleOrder")]
        public IActionResult CreateSaleOrder([FromBody] OrderDto dto)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin" || role == "Client")
            {
                if (dto == null)
                {
                    return BadRequest("Por favor complete los campos");
                }
                try
                {
                    var newSaleOrder = new Order()
                    {
                        ClientId = dto.ClientId,
                        Date = DateTime.Now
                    };
                    newSaleOrder = _orderService.CreateSaleOrder(newSaleOrder);
                    return Ok($"Orden de venta creada exitosamente con ID: {newSaleOrder.Id}");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return Forbid();
        }

        [HttpDelete("DeleteSaleOrder{id}")]
        public IActionResult DeleteSaleOrder([FromRoute] int id)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin" || role == "Client")
            {
                try
                {
                    var existingOrder = _orderService.GetOne(id);

                    if (existingOrder == null)
                    {
                        return NotFound($"No se encontró orden de venta con el ID: {id}");
                    }

                    _orderService.DeleteSaleOrder(id);
                    return Ok($"Orden de venta con ID: {id} eliminada");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return Forbid();
        }


        [HttpPut("UpdateSaleOrder{id}")]
        public IActionResult UpdateSaleOrder([FromRoute] int id, [FromBody] OrderDto dto)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin")
            {
                var orderToUpdate = _orderService.GetOne(id);
                if (orderToUpdate == null)
                {
                    return NotFound($"Orden de venta con ID {id} no encontrada");
                }
                if (dto.ClientId == 0)
                {
                    return BadRequest("Orden de venta no actualizado, por favor completar los campos");
                }

                try
                {
                    orderToUpdate.ClientId = dto.ClientId;

                    orderToUpdate = _orderService.UpdateSaleOrder(orderToUpdate);
                    return Ok($"Orden de venta actualizada exitosamente");
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error al actualizar Orden de venta: {ex.Message}");
                }
            }
            return Forbid();
        }
    }
}
