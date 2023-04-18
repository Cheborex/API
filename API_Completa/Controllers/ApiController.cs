using API_Completa.Datos;
using API_Completa.Modelos;
using API_Completa.Modelos.Dto;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Completa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        // Inyeccion de dependencias. Se deben agregar todas al constructor(ctor). Se deben agregar previamente a Program.cs

        private readonly ILogger<ApiController> _logger;
        private readonly AplicationDbContext _db;
        private readonly IMapper _mapper;


        public ApiController(ILogger<ApiController> logger, AplicationDbContext db, IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }



        // Api de tipo get que devuelve una lista completa

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ApiDto>>> GetApis()
        {
            _logger.LogInformation("Obtener datos de una api");

            IEnumerable<Api> apiList = await _db.Apis.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ApiDto>>(apiList));
        }

        // Api de tipo get que pide un parametro

        [HttpGet("{id:int}", Name = "GetApi")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Documentar Status
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiDto>> GetApi(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error al cargar datos con el ID " + id);
                return BadRequest();
            }

            // var api = ApiDatos.apiList.FirstOrDefault(a => a.Id == id);
            var api = await _db.Apis.FirstOrDefaultAsync(a => a.Id == id); // filtrar datos por id

            if (api == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ApiDto>(api));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiDto>> PostApi([FromBody] ApiCreateDto apiCreateDto)
        {
            if (!ModelState.IsValid) // Para validar que los campos esten como corresponden
            {
                return BadRequest(ModelState);
            }

            if (await _db.Apis.FirstOrDefaultAsync(a => a.Nombre.ToLower() == apiCreateDto.Nombre.ToLower()) != null) // Validacion personalizada
            {
                ModelState.AddModelError("NombreExiste", "El usuario con ese Nombre ya existe!");
                return BadRequest(ModelState);
            }

            if (apiCreateDto == null)
            {
                return BadRequest(apiCreateDto);
            }

            Api modelo = _mapper.Map<Api>(apiCreateDto);

            await _db.Apis.AddAsync(modelo);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetApi", new { id = modelo.Id }, modelo);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EliminarApi(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var idYaCreado = await _db.Apis.FirstOrDefaultAsync(a => a.Id == id);
            if (idYaCreado == null)
            {
                return NotFound();
            }
            _db.Apis.Remove(idYaCreado); //el metodo Remove es siempre sincrono
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateApi(int id, [FromBody] ApiUpdateDto apiUpdateDto)
        {
            if (apiUpdateDto == null || id != apiUpdateDto.Id)
            {
                return BadRequest();
            }

            Api modelo = _mapper.Map<Api>(apiUpdateDto);

            _db.Apis.Update(modelo); // El metodo Update es siempre sincrono
            await _db.SaveChangesAsync();

            return NoContent();
        }


        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchApi(int id, JsonPatchDocument<ApiUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            var idYaCreado = await _db.Apis.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

            ApiUpdateDto apiDto = _mapper.Map<ApiUpdateDto>(idYaCreado);

            if (idYaCreado == null)
            {
                return BadRequest();
            }

            patchDto.ApplyTo(apiDto, ModelState);

            if (!ModelState.IsValid)
            {
                return (BadRequest(ModelState));
            }

            Api modelo = _mapper.Map<Api>(apiDto);

            _db.Apis.Update(modelo);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
