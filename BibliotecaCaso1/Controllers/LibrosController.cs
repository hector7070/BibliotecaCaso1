using Abstracciones.Libros;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaCaso1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly ILibroFlujo _flujo;

        public LibrosController(ILibroFlujo flujo) => _flujo = flujo;

        [HttpGet]
        [ProducesResponseType<IReadOnlyList<LibroDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<LibroDto>>> Listar([FromQuery] LibroFiltro filtro, CancellationToken ct)
        {
            var resultado = await _flujo.ListarAsync(filtro, ct);
            Response.Headers["X-Total-Count"] = resultado.Total.ToString();
            Response.Headers["X-Page"] = resultado.Pagina.ToString();
            Response.Headers["X-Page-Size"] = resultado.Tamano.ToString();
            return Ok(resultado.Items);
        }

        [HttpGet("{id:int:min(1)}")]
        [ProducesResponseType<LibroDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LibroDto>> Obtener(int id, CancellationToken ct) =>
            Ok(await _flujo.ObtenerAsync(id, ct));

        [HttpPost]
        [ProducesResponseType<LibroDto>(StatusCodes.Status201Created)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<LibroDto>> Crear(LibroCrearDto dto, CancellationToken ct)
        {
            var creado = await _flujo.CrearAsync(dto, ct);
            return CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
        }

        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Actualizar(int id, LibroActualizarDto dto, CancellationToken ct)
        {
            await _flujo.ActualizarAsync(id, dto, ct);
            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
        {
            await _flujo.EliminarAsync(id, ct);
            return NoContent();
        }
    }
}
