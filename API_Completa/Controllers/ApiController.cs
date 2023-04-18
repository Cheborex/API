using API_Completa.Datos;
using API_Completa.Modelos;
using API_Completa.Modelos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace API_Completa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        // Login integrado en ASP.NET 

        private readonly ILogger<ApiController> _logger;
        private readonly AplicationDbContext _db;


        public ApiController(ILogger<ApiController> logger, AplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }



        // Api de tipo get que devuelve una lista completa

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ApiDto>> GetApis()
        {
            _logger.LogInformation("Obtener datos de una api");
            //return Ok(ApiDatos.apiList); // esa es para traer los datos falsos
            return Ok(_db.Apis.ToList());
        }

        // Api de tipo get que pide un parametro

        [HttpGet("{id:int}", Name = "GetApi")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Documentar Status
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ApiDto> GetApi(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error al cargar datos con el ID " + id);
                return BadRequest();
            }

            // var api = ApiDatos.apiList.FirstOrDefault(a => a.Id == id);
            var api = _db.Apis.FirstOrDefault(a => a.Id == id); // filtrar datos por id

            if (api == null)
            {
                return NotFound();
            }

            return Ok(api);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<ApiDto> PostApi([FromBody] ApiDto apiDto)
        {
            if (!ModelState.IsValid) // Para validar que los campos esten como corresponden
            {
                return BadRequest(ModelState);
            }

            if (_db.Apis.FirstOrDefault(a => a.Nombre.ToLower() == apiDto.Nombre.ToLower()) != null) // Validacion personalizada
            {
                ModelState.AddModelError("NombreExiste", "El usuario con ese Nombre ya existe!");
                return BadRequest(ModelState);
            }

            if (apiDto == null)
            {
                return BadRequest(apiDto);
            }
            if (apiDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Api modelo = new()
            {
                Nombre = apiDto.Nombre,
                Detalle = apiDto.Detalle,
                ImagenUrl = apiDto.ImagenUrl,
                Ocupantes = apiDto.Ocupantes,
                Tarifa = apiDto.Tarifa,
                MetrosCuadrados = apiDto.MetrosCuadrados,
                Amenidad = apiDto.Amenidad
            };

            _db.Apis.Add(modelo);
            _db.SaveChanges();

            return CreatedAtRoute("GetApi", new { id = apiDto.Id }, apiDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult EliminarApi(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var idYaCreado = _db.Apis.FirstOrDefault(a => a.Id == id);
            if (idYaCreado == null)
            {
                return NotFound();
            }
            _db.Apis.Remove(idYaCreado);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateApi(int id, [FromBody] ApiDto apiDto)
        {
            if (apiDto == null || id != apiDto.Id)
            {
                return BadRequest();
            }
            Api modelo = new()
            {
                Id = apiDto.Id,
                Nombre = apiDto.Nombre,
                Detalle = apiDto.Detalle,
                ImagenUrl = apiDto.ImagenUrl,
                Ocupantes = apiDto.Ocupantes,
                Tarifa = apiDto.Tarifa,
                MetrosCuadrados = apiDto.MetrosCuadrados,
                Amenidad = apiDto.Amenidad
            };

            _db.Apis.Update(modelo);
            _db.SaveChanges();

            return NoContent();
        }


        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PatchApi(int id, JsonPatchDocument<ApiDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            var idYaCreado = _db.Apis.AsNoTracking().FirstOrDefault(a => a.Id == id);

            ApiDto apiDto = new()
            {
                Id = idYaCreado.Id,
                Nombre = idYaCreado.Nombre,
                Detalle= idYaCreado.Detalle,
                ImagenUrl = idYaCreado.ImagenUrl,
                Ocupantes= idYaCreado.Ocupantes,
                Tarifa= idYaCreado.Tarifa,
                MetrosCuadrados = idYaCreado.MetrosCuadrados,
                Amenidad = idYaCreado.Amenidad
            };

            if(idYaCreado == null)
            {
                return BadRequest();
            }

            patchDto.ApplyTo(apiDto, ModelState);

            if(!ModelState.IsValid)
            {
                return(BadRequest(ModelState));
            }

            Api Modelo = new()
            {
                Id = apiDto.Id,
                Nombre = apiDto.Nombre,
                Detalle = apiDto.Detalle,
                ImagenUrl = apiDto.ImagenUrl,
                Ocupantes = apiDto.Ocupantes,
                Tarifa = apiDto.Tarifa,
                MetrosCuadrados = apiDto.MetrosCuadrados,
                Amenidad = apiDto.Amenidad
            };
            _db.Apis.Update(Modelo);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
