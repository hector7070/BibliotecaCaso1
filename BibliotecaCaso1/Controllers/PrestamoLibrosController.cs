using Abstracciones.PrestamoLibros;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaCaso1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamoLibrosController : ControllerBase
    {
        private readonly IPrestamoLibroFlujo _flujo;

        public PrestamoLibrosController(IPrestamoLibroFlujo flujo) => _flujo = flujo;

        [HttpGet]
        [ProducesResponseType<IReadOnlyList<PrestamoLibroDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<PrestamoLibroDto>>> Listar([FromQuery] PrestamoLibroFiltro filtro, CancellationToken ct)
        {
            var resultado = await _flujo.ListarAsync(filtro, ct);
            Response.Headers["X-Total-Count"] = resultado.Total.ToString();
            Response.Headers["X-Page"] = resultado.Pagina.ToString();
            Response.Headers["X-Page-Size"] = resultado.Tamano.ToString();
            return Ok(resultado.Items);
        }

        [HttpGet("{id:int:min(1)}")]
        [ProducesResponseType<PrestamoLibroDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PrestamoLibroDto>> Obtener(int id, CancellationToken ct) =>
            Ok(await _flujo.ObtenerAsync(id, ct));

        [HttpPost]
        [ProducesResponseType<PrestamoLibroDto>(StatusCodes.Status201Created)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<PrestamoLibroDto>> Crear(PrestamoLibroCrearDto dto, CancellationToken ct)
        {
            var creado = await _flujo.CrearAsync(dto, ct);
            return CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
        }

        [HttpPatch("{id:int:min(1)}/devolucion")]
        [ProducesResponseType<PrestamoLibroDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<PrestamoLibroDto>> RegistrarDevolucion(int id, CancellationToken ct) =>
            Ok(await _flujo.RegistrarDevolucionAsync(id, ct));

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
