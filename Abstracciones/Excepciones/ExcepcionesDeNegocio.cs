namespace Abstracciones.Excepciones
{
    public abstract class ExcepcionDeNegocio : Exception
    {
        protected ExcepcionDeNegocio(string mensaje) : base(mensaje) { }
    }

    public class NoEncontradoException : ExcepcionDeNegocio
    {
        public NoEncontradoException(string entidad, object id)
            : base($"{entidad} con identificador '{id}' no existe.") { }
    }

    public class ConflictoException : ExcepcionDeNegocio
    {
        public ConflictoException(string mensaje) : base(mensaje) { }
    }

    public class ValidacionNegocioException : ExcepcionDeNegocio
    {
        public ValidacionNegocioException(string mensaje) : base(mensaje) { }
    }
}
