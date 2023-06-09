﻿using API_Completa.Datos;
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
    public class ApiController : ControllerBase
    {
        // Inyeccion de dependencias. Se deben agregar todas al constructor(ctor). Se deben agregar previamente a Program.cs

        private readonly ILogger<ApiController> _logger;
        private readonly IApiRepositorio _apiRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response; // para dar respuestas personalizadas. No hace falta inyectar la dependencia


        public ApiController(ILogger<ApiController> logger, IApiRepositorio apiRepo, IMapper mapper)
        {
            _logger = logger;
            _apiRepo = apiRepo;
            _mapper = mapper;
            _response = new();
        }



        // Api de tipo get que devuelve una lista completa

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetApis()
        {
            try
            {
                _logger.LogInformation("Obtener datos de una api");

                IEnumerable<Api> apiList = await _apiRepo.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<ApiDto>>(apiList);
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

        [HttpGet("{id:int}", Name = "GetApi")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Documentar Status
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetApi(int id)
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
                var api = await _apiRepo.Obtener(a => a.Id == id); // filtrar datos por id

                if (api == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<ApiDto>(api);
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
        public async Task<ActionResult<ApiResponse>> PostApi([FromBody] ApiCreateDto apiCreateDto)
        {
            try
            {
                if (!ModelState.IsValid) // Para validar que los campos esten como corresponden
                {
                    return BadRequest(ModelState);
                }

                if (await _apiRepo.Obtener(a => a.Nombre.ToLower() == apiCreateDto.Nombre.ToLower()) != null) // Validacion personalizada
                {
                    ModelState.AddModelError("NombreExiste", "El usuario con ese Nombre ya existe!");
                    return BadRequest(ModelState);
                }

                if (apiCreateDto == null)
                {
                    return BadRequest(apiCreateDto);
                }

                Api modelo = _mapper.Map<Api>(apiCreateDto);

                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;

                await _apiRepo.Crear(modelo);

                _response.Resultado = modelo;
                _response.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetApi", new { id = modelo.Id }, modelo);
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
        public async Task<IActionResult> EliminarApi(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var idYaCreado = await _apiRepo.Obtener(a => a.Id == id);
                if (idYaCreado == null)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _apiRepo.Remover(idYaCreado);
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
        public async Task<IActionResult> UpdateApi(int id, [FromBody] ApiUpdateDto apiUpdateDto)
        {
            if (apiUpdateDto == null || id != apiUpdateDto.Id)
            {
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Api modelo = _mapper.Map<Api>(apiUpdateDto);

            await _apiRepo.Actualizar(modelo); // El metodo Update es siempre sincrono
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
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
            var idYaCreado = await _apiRepo.Obtener(a => a.Id == id, tracked: false);

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

            await _apiRepo.Actualizar(modelo);
            _response.statusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
