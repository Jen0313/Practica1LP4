using Microsoft.EntityFrameworkCore;
using UT2024P4LP4.Web.Data;
using UT2024P4LP4.Web.Data.Dtos;
using UT2024P4LP4.Web.Data.Entities;

namespace UT2024P4LP4.Web.Services;

public interface ICategoriaService
{
	Task<Result> Create(CategoriaRequest categoria);
	Task<Result> Delete(int Id);
	Task<ResultList<CategoriaDto>> GetAll(CancellationToken cancellationToken = default);
	Task<Result> Update(CategoriaRequest categoria);
}

public class CategoriaService(IApplicationDbContext context) : ICategoriaService
{
	public async Task<ResultList<CategoriaDto>> GetAll(CancellationToken cancellationToken = default)
	{
		var categorias = await context.Categorias
		.Select(x => x.ToDto())
		.ToListAsync(cancellationToken);
		return ResultList<CategoriaDto>.Success(categorias);
	}

	public async Task<Result> Create(CategoriaRequest categoria)
	{
		try
		{
			var entity = Categoria.Create(categoria.Nombre);
			context.Categorias.Add(entity);
			await context.SaveChangesAsync();
			return Result.Success("✅Categoria Registrada con exito!");
		}
		catch (Exception Ex)
		{
			return Result.Failure($"☠️ Error: {Ex.Message}");
		}
	}
	public async Task<Result> Update(CategoriaRequest categoria)
	{
		try
		{
			var entity = context.Categorias.Where(c => c.Id == categoria.Id).FirstOrDefault();
			if (entity == null)
				return Result.Failure($"La categoria '{categoria.Id}' no existe!");
			if (entity.Update(categoria.Nombre))
			{
				await context.SaveChangesAsync();
				return Result.Success("✅Catgoria modificada con exito!");
			}
			return Result.Success("🐫 No has realizado ningun cambio!");
		}
		catch (Exception Ex)
		{
			return Result.Failure($"☠️ Error: {Ex.Message}");
		}
	}
	public async Task<Result> Delete(int Id)
	{
		try
		{
			var entity = context.Categorias.Where(c => c.Id == Id).FirstOrDefault();
			if (entity == null)
				return Result.Failure($"La categoria '{Id}' no existe!");
			context.Categorias.Remove(entity);
			await context.SaveChangesAsync();
			return Result.Success("✅Categoria eliminada con exito!");
		}
		catch (Exception Ex)
		{
			return Result.Failure($"☠️ Error: {Ex.Message}");
		}
	}

}
