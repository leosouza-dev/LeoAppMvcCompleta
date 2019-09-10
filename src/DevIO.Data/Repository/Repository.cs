using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new() //Esse new() é para conseguirmos criar uma instancia de TEntity
    {
        protected readonly MeuDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        protected Repository(MeuDbContext db)
        {
            Db = db;
            DbSet = Db.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync(); //Tracking do EF -> Traz dados que no momento atrapalham o desempenho
        }

        public virtual async Task<TEntity> ObterPorId(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task Adicionar(TEntity entity)
        {
            DbSet.Add(entity);
            await SaveChanges(); //é um metodo assincrono -> usar o "await"
        }

        public virtual async Task Atualizar(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChanges();
        }

        public virtual async Task Remover(Guid id)
        {
            //DbSet.Remove(await DbSet.FindAsync(id)); //Remove espera um Tentity, então temos que ir buscar com o Id
            DbSet.Remove(new TEntity { Id = id }); //criando um tentity -> Vantagem: Não é necessário fazer uma busca no banco!
            await SaveChanges();

        }

        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync(); //Usa O DB (salvar o Bando de dados)
        }

        public void Dispose()
        {
            Db?.Dispose(); //boa pratica - ? se existir, faz dispose 
        }
    }
}
