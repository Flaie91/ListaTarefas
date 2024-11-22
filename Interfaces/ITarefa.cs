using System;
using ListaTarefa.Models;

namespace ListaTarefa.Interfaces
{
    public interface ITarefa
    {
        List<Tarefa> Todos(int pagina = 1, string? titulo = null, string? descricao = null, DateTime? data = null, EnumStatusTarefa? status = null);

        Tarefa? BuscaPorId (int Id);

        List<Tarefa>?  BuscaPorTitulo (string titulo);
        List<Tarefa>?  BuscaPorDescricao (string descricao);
        List<Tarefa>?  BuscaPorData (DateTime data);
        List<Tarefa>?  BuscaPorStatus (EnumStatusTarefa status);

        void Incluir (Tarefa tarefa);

        void Atualizar (Tarefa tarefa);

        void Apagar (Tarefa tarefa);
        
    }
}