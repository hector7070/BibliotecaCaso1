using Abstracciones.Excepciones;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaCaso1.Errores
{
    public class ManejadorExcepcionesGlobal : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemas;
        private readonly ILogger<ManejadorExcepcionesGlobal> _logger;

        public ManejadorExcepcionesGlobal(IProblemDetailsService problemas, ILogger<ManejadorExcepcionesGlobal> logger)
        {
            _problemas = problemas;
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext contexto, Exception excepcion, CancellationToken ct)
        {
            var (estado, titulo) = excepcion switch
            {
                NoEncontradoException => (StatusCodes.Status404NotFound, "Recurso no encontrado"),
                ConflictoException => (StatusCodes.Status409Conflict, "Conflicto con el estado actual"),
                ValidacionNegocioException => (StatusCodes.Status422UnprocessableEntity, "No se puede procesar la solicitud"),
                _ => (0, string.Empty)
            };

            if (estado == 0)
            {
                _logger.LogError(excepcion, "Error no controlado en {Metodo} {Ruta}", contexto.Request.Method, contexto.Request.Path);
                return false;
            }

            contexto.Response.StatusCode = estado;
            return await _problemas.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = contexto,
                Exception = excepcion,
                ProblemDetails = new ProblemDetails
                {
                    Status = estado,
                    Title = titulo,
                    Detail = excepcion.Message
                }
            });
        }
    }
}
