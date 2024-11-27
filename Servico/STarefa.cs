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

        public Tarefa? BuscaPorId(int id)
    {
        return _contexto.Tarefas.Where(v => v.Id == id).FirstOrDefault();
        
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

    public List<Tarefa>?  BuscaPorData (DateTime data)
    {
        var query = _contexto.Tarefas.AsQueryable();
        if(!string.IsNullOrEmpty(Convert.ToString(data)))
        {
          query = query.Where(v => EF.Functions.Like(v.Titulo, $"%{data}%"));
        }
        return query.ToList();
        
    }
    public List<Tarefa>?  BuscaPorStatus (EnumStatusTarefa status)
    {
        var query = _contexto.Tarefas.AsQueryable();
        if(!string.IsNullOrEmpty(Convert.ToString(status)))
        {
          query = query.Where(v => EF.Functions.Like(v.Titulo, $"%{status}%"));
        }
        return query.ToList();
        
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

    public List<DateTime> ObterDatasDisponiveis()
    {
    return _contexto.Tarefas
        .Select(t => t.Data.Date) // Garante que apenas a parte da data (sem horário) seja considerada.
        .Distinct()
        .OrderBy(d => d) // Ordena as datas em ordem crescente (opcional).
        .ToList();
    }

    public DateTime? ObterDataPorId(int dateId)
    {
    // Garante que o índice seja baseado em uma lista ordenada de datas
    var data = _contexto.Tarefas
                        .Select(t => t.Data.Date) // Seleciona apenas a parte da data
                        .Distinct()               // Garante que não haja duplicatas
                        .OrderBy(d => d)          // Ordena as datas em ordem crescente
                        .Skip(dateId - 1)         // Pula as primeiras (dateId - 1) datas
                        .FirstOrDefault();        // Obtém a primeira data após o Skip

    return data;
    }

    public List<Tarefa> BuscarTarefasPorData(DateTime data)
    {
    return _contexto.Tarefas
                    .Where(t => t.Data.Date == data.Date) // Compara apenas a parte da data
                    .ToList();
    }
    }
}
