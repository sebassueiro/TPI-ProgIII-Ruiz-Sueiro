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
    public class LineOfOrderController : ControllerBase
    {
        private readonly ILineOfOrderService _lineOfOrderService;

        public LineOfOrderController(ILineOfOrderService lineOfOrderService)
        {
            _lineOfOrderService = lineOfOrderService;
        }

        [HttpGet("GetAllBySaleOrder/{saleOrderId}")]
        public IActionResult GetAllBySaleOrder([FromRoute] int saleOrderId)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin")
            {
                try
                {
                    var lo = _lineOfOrderService.GetAllBySaleOrder(saleOrderId);
                    if (lo.Count == 0)
                    {
                        return NotFound("Líneas de venta no encontradas");
                    }
                    return Ok(lo);

                }
                catch (Exception ex)
                {
                    return BadRequest($"Error: {ex.Message}");
                }
            }
            return Forbid();
        }

        [HttpGet("GetAllByProduct/{productId}")]
        public IActionResult GetAllByProduct([FromRoute] int productId)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin")
            {
                try
                {
                    var lo = _lineOfOrderService.GetAllByProduct(productId);
                    if (lo.Count == 0)
                    {
                        return NotFound("Líneas de venta no encontradas");
                    }
                    return Ok(lo);

                }
                catch (Exception ex)
                {
                    return BadRequest($"Error: {ex.Message}");
                }
            }
            return Forbid();
        }


        [HttpGet("GetSOLById/{solId}")]
        public IActionResult GetOne([FromRoute] int solId)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin")
            {
                try
                {
                    var lo = _lineOfOrderService.GetOne(solId);

                    if (lo == null)
                    {
                        return NotFound($"Línea de venta con id {solId} no encontrada");
                    }

                    return Ok(lo);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error: {ex.Message}");
                }
            }
            return Forbid();
        }

        [HttpPost("CreateSaleOrderLine")]
        public IActionResult CreateSaleOrderLine([FromBody] LineOfOrderPostDto dto)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin" || role == "Client")
            {
                if (dto.ProductId == 0 || dto.SaleOrderId == 0 || dto.Amount == 0)
                {
                    return BadRequest("Por favor complete los campos");
                }

                try
                {
                    var newSaleOrderLine = new LineOfOrder()
                    {
                        ProductId = dto.ProductId,
                        SaleOrderId = dto.SaleOrderId,
                        Amount = dto.Amount,
                        
                    };

                    newSaleOrderLine = _lineOfOrderService.CreateSaleOrderLine(newSaleOrderLine);
                    return Ok($"Línea de orden de venta creada exitosamente con id {newSaleOrderLine.Id}");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return Forbid();
        }

        [HttpDelete("DeleteSOL{id}")]
        public IActionResult DeleteSaleOrderLine([FromRoute] int id)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin" || role == "Client")
            {
                try
                {
                    var existingLineOfOrder = _lineOfOrderService.GetOne(id);

                    if (existingLineOfOrder == null)
                    {
                        return NotFound($"No se encontró línea de venta con el ID: {id}");
                    }

                    _lineOfOrderService.DeleteSaleOrderLine(id);
                    return Ok($"Línea de venta con ID: {id} eliminada");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return Forbid();
        }


        [HttpPut("UpdateSaleOrderLine{id}")]
        public IActionResult UpdateSaleOrderLine([FromRoute] int id, [FromBody] LineOfOrderPutDto dto)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin" || role == "Client")
            {
                var lineOfOrderToUpdate = _lineOfOrderService.GetOne(id);
                if (lineOfOrderToUpdate == null)
                {
                    return NotFound($"Líne de venta con ID {id} no encontrada");
                }
                if (dto.ProductId == 0 || dto.Amount == 0)
                {
                    return BadRequest("Línea de venta no actualizado, por favor completar los campos");
                }

                try
                { 
                    lineOfOrderToUpdate.ProductId = dto.ProductId;
                    lineOfOrderToUpdate.Amount = dto.Amount;
                    

                    lineOfOrderToUpdate = _lineOfOrderService.UpdateSaleOrderLine(lineOfOrderToUpdate);
                    return Ok($"Línea de venta actualizada exitosamente");
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error al actualizar línea de venta: {ex.Message}");
                }
            }
            return Forbid();
        }
    }
}
