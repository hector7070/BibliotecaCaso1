using Abstracciones.Comunes;
using Abstracciones.Excepciones;
using Abstracciones.Usuarios;
using DA.Comunes;
using DA.Contexto;
using Microsoft.EntityFrameworkCore;

namespace DA.Usuarios
{
    public class UsuarioDA : IUsuarioDA
    {
        private readonly BibliotecaContexto _ctx;

        public UsuarioDA(BibliotecaContexto ctx) => _ctx = ctx;

        public async Task<ResultadoPaginado<Usuario>> BuscarAsync(UsuarioFiltro filtro, CancellationToken ct)
        {
            IQueryable<Usuario> consulta = _ctx.Usuarios.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filtro.Q))
            {
                var texto = filtro.Q.Trim();
                consulta = consulta.Where(u =>
                    u.Nombre.Contains(texto) ||
                    u.Correo.Contains(texto));
            }

            var total = await consulta.CountAsync(ct);

            var items = await consulta
                .OrderBy(u => u.Nombre).ThenBy(u => u.Id)
                .Skip((filtro.Pagina - 1) * filtro.Tamano)
                .Take(filtro.Tamano)
                .ToListAsync(ct);

            return new ResultadoPaginado<Usuario>
            {
                Items = items,
                Total = total,
                Pagina = filtro.Pagina,
                Tamano = filtro.Tamano
            };
        }

        public Task<Usuario?> ObtenerPorIdAsync(int id, CancellationToken ct) =>
            _ctx.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);

        public Task<Usuario?> ObtenerPorCorreoAsync(string correo, CancellationToken ct) =>
            _ctx.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Correo == correo, ct);

        public async Task<Usuario> AgregarAsync(Usuario usuario, CancellationToken ct)
        {
            _ctx.Usuarios.Add(usuario);
            try
            {
                await _ctx.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            {
                throw new ConflictoException($"Ya existe un usuario con el correo '{usuario.Correo}'.");
            }
            return usuario;
        }

        public async Task<bool> ActualizarAsync(Usuario datos, CancellationToken ct)
        {
            var existente = await _ctx.Usuarios.FirstOrDefaultAsync(u => u.Id == datos.Id, ct);
            if (existente is null) return false;

            existente.Nombre = datos.Nombre;
            existente.Correo = datos.Correo;
            existente.Telefono = datos.Telefono;

            try
            {
                await _ctx.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsDuplicado(ex))
            {
                throw new ConflictoException($"Ya existe otro usuario con el correo '{datos.Correo}'.");
            }
            return true;
        }

        public async Task<bool> EliminarAsync(int id, CancellationToken ct)
        {
            var existente = await _ctx.Usuarios.FirstOrDefaultAsync(u => u.Id == id, ct);
            if (existente is null) return false;

            _ctx.Usuarios.Remove(existente);
            try
            {
                await _ctx.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ErroresSql.EsLlaveForanea(ex))
            {
                throw new ConflictoException("El usuario tiene préstamos o libros asociados y no se puede eliminar.");
            }
            return true;
        }

        public Task<bool> TieneLibrosAsync(int usuarioId, CancellationToken ct) =>
            Task.FromResult(false);
    }
}
