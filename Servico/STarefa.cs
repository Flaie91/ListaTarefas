using ListaTarefa.Context;
using ListaTarefa.Models;
using ListaTarefa.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ListaTarefa.Servico
{
    public class Starefa : ITarefa
    {
        
    private readonly OrganizadorContext _contexto;

    public Starefa(OrganizadorContext contexto)
    {
      _contexto = contexto;
    }

    public void Incluir(Tarefa tarefa)
    {
        _contexto.Tarefas.Add(tarefa);
        _contexto.SaveChanges();
    }


    public List<Tarefa> Todos(int pagina = 1, string? titulo = null, string? descricao = null, DateTime? data = null, EnumStatusTarefa? status = null)
    {
        int itensPorPagina = 10;
        var query = _contexto.Tarefas.AsQueryable();
        if(!string.IsNullOrEmpty(titulo))
        {
          query = query.Where(v => EF.Functions.Like(v.Titulo, $"%{titulo}%"));
        }

        if (!string.IsNullOrEmpty(descricao))
        {
        query = query.Where(v => EF.Functions.Like(v.Descricao, $"%{descricao}%"));
        }

        if (data.HasValue)
        {
        query = query.Where(v => v.Data.Date == data.Value.Date); 
        }   

        if (status.HasValue)
        {
        query = query.Where(v => v.Status == status.Value);
        }        

        if(pagina < 1) pagina = 1;
        query = query.Skip((pagina - 1) * itensPorPagina).Take(itensPorPagina);     

        return query.ToList();          

    }   

    public Tarefa? BuscaPorId(int id)
    {
        return _contexto.Tarefas.Where(v => v.Id == id).FirstOrDefault();
        
    }  

    public void Apagar(Tarefa tarefa)
    {
        _contexto.Tarefas.Remove(tarefa);
        _contexto.SaveChanges();
    }

    public void Atualizar(Tarefa tarefa)
    {       
        _contexto.Tarefas.Update(tarefa);
        _contexto.SaveChanges();
    }

    
    public List<Tarefa>? BuscaPorTitulo(string titulo)
    {
        var query = _contexto.Tarefas.AsQueryable();
        if(!string.IsNullOrEmpty(titulo))
        {
          query = query.Where(v => EF.Functions.Like(v.Titulo, $"%{titulo}%"));
        }
        return query.ToList();
        
    }

    public List<Tarefa>?  BuscaPorDescricao (string descricao)
    {
        var query = _contexto.Tarefas.AsQueryable();
        if(!string.IsNullOrEmpty(descricao))
        {
          query = query.Where(v => EF.Functions.Like(v.Descricao, $"%{descricao}%"));
        }
        return query.ToList();
        
    }

    public List<Tarefa>?  BuscaPorStatus (EnumStatusTarefa status)
    {
        var query = _contexto.Tarefas.AsQueryable();
        if(!string.IsNullOrEmpty(Convert.ToString(status)))
        {        
        query = query.Where(v => v.Status == status);
        }
        return query.ToList();
        
    }

        
    public List<DateTime> ObterDatasDisponiveis()
    {
    return _contexto.Tarefas
        .Select(t => t.Data.Date) 
        .Distinct()
        .OrderBy(d => d)
        .ToList();
    }

    public DateTime? ObterDataPorId(int dateId)
    {

    var data = _contexto.Tarefas
                        .Select(t => t.Data.Date) 
                        .Distinct()               
                        .OrderBy(d => d)          
                        .Skip(dateId - 1)         
                        .FirstOrDefault();        

    return data;
    }

    public List<Tarefa> BuscarTarefasPorData(DateTime data)
    {
    return _contexto.Tarefas
                    .Where(t => t.Data.Date == data.Date) 
                    .ToList();
    }
    }
}
