using API_Completa.Datos;
using API_Completa.Modelos;
using API_Completa.Modelos.Dto;
using API_Completa.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API_Completa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumeroApiController : ControllerBase
    {
        // Inyeccion de dependencias. Se deben agregar todas al constructor(ctor). Se deben agregar previamente a Program.cs

        private readonly ILogger<NumeroApiController> _logger;
        private readonly IApiRepositorio _apiRepo;
        private readonly INumeroApiRepositorio _numeroRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response; // para dar respuestas personalizadas. No hace falta inyectar la dependencia


        public NumeroApiController(ILogger<NumeroApiController> logger, IApiRepositorio apiRepo, INumeroApiRepositorio numeroRepo, IMapper mapper)
        {
            _logger = logger;
            _apiRepo = apiRepo;
            _numeroRepo = numeroRepo;
            _mapper = mapper;
            _response = new();
        }



        // Api de tipo get que devuelve una lista completa

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetNumeroApis()
        {
            try
            {
                _logger.LogInformation("Obtener datos de una api");

                IEnumerable<NumeroApi> NumeroApiList = await _numeroRepo.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<NumeroApiDto>>(NumeroApiList);
                _response.statusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        // Api de tipo get que pide un parametro

        [HttpGet("{id:int}", Name = "GetNumeroApi")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Documentar Status
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetNumeroApi(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al cargar datos con el ID " + id);
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }

                // var api = ApiDatos.apiList.FirstOrDefault(a => a.Id == id);
                var numeroApi = await _numeroRepo.Obtener(a => a.ApiNo == id); // filtrar datos por id

                if (numeroApi == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<NumeroApiDto>(numeroApi);
                _response.statusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse>> PostNumeroApi([FromBody] NumeroApiCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid) // Para validar que los campos esten como corresponden
                {
                    return BadRequest(ModelState);
                }

                if (await _numeroRepo.Obtener(a => a.ApiNo == createDto.ApiNo) != null) // Validacion personalizada
                {
                    ModelState.AddModelError("NombreExiste", "El numero de Api ya existe!");
                    return BadRequest(ModelState);
                }

                if (await _apiRepo.Obtener(a => a.Id == createDto.ApiId) == null)
                {
                    ModelState.AddModelError("ClaveForanea", "El Id de Api no existe!");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                NumeroApi modelo = _mapper.Map<NumeroApi>(createDto);

                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;

                await _numeroRepo.Crear(modelo);

                _response.Resultado = modelo;
                _response.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetNumeroApi", new { id = modelo.ApiNo }, modelo);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EliminarNumeroApi(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var numeroIdYaCreado = await _numeroRepo.Obtener(a => a.ApiNo == id);
                if (numeroIdYaCreado == null)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _numeroRepo.Remover(numeroIdYaCreado);
                _response.statusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateNumeroApi(int id, [FromBody] NumeroApiUpdateDto apiUpdateDto)
        {
            if (apiUpdateDto == null || id != apiUpdateDto.ApiNo)
            {
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (await _apiRepo.Obtener(a => a.Id == apiUpdateDto.ApiId) == null)
            {
                ModelState.AddModelError("ClaveForanea", "El Id de Api no existe!");
                return BadRequest(ModelState);
            }

            NumeroApi modelo = _mapper.Map<NumeroApi>(apiUpdateDto);

            await _numeroRepo.Actualizar(modelo); // El metodo Update es siempre sincrono
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }
    }
}
