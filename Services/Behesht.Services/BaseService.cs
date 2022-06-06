using Behesht.Core.Data;
using Behesht.Core;
using Behesht.Core.Models;
using Behesht.Core.Models.Paging;
using Behesht.Data.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;

namespace Behesht.Services
{
    public class BaseService<TEntity, TModel> : IBaseService<TEntity, TModel> where TEntity : BaseEntity<long> where TModel : BaseModel
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        public BaseService(IRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual PagedList<TModel> Get(PagedListInputMeta meta)
        {
            return _repository.EntitiesTableNoTracking.ProjectToType<TModel>().ToPagedList(meta);
        }

        public virtual TModel GetById(long id)
        {
            var entity = _repository.FindById(id);
            if (entity == null)
            {
                return null;
            }
            var model = _mapper.Map<TModel>(entity);
            return model;
        }


        public virtual void Insert(TModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var entity = _mapper.Map<TModel, TEntity>(model);
            _repository.Insert(entity);
            model.Id = entity.Id;
        }

        public virtual void Insert(IEnumerable<TModel> models)
        {
            if (models == null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            var entitis = _mapper.Map<IEnumerable<TEntity>>(models);
            _repository.Insert(entitis);
        }


        public virtual void Update(TModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var entity = _repository.FindById(model.Id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{model.Id} Id not found for {nameof(TEntity)}");
            }

            _mapper.Map<TModel, TEntity>(model, entity);
            _repository.Update(entity);
        }

        public virtual void Update(IEnumerable<TModel> models)
        {
            var entities = _repository.FindByIds(models.Select(p => p.Id).ToArray());
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            if (entities.Count() != models.Count())
            {
                throw new Exception("update transaction faild");
            }

            foreach (var item in entities)
            {
                var model = models.FirstOrDefault(p => p.Id == item.Id);
                _mapper.Map<TModel, TEntity>(model, item);
            }
            _repository.Update(entities);
        }

        public virtual void Delete(long id, bool softDelete = true)
        {
            var entity = _repository.FindById(id);
            if (softDelete)
            {
                entity.IsDelete = true;
                _repository.Update(entity);
            }
            else
            {
                _repository.Delete(entity);
            }
        }

        public virtual void Delete(long[] ids, bool softDelete = true)
        {
            if (ids is null || !ids.Any())
            {
                throw new ArgumentException(nameof(ids) + "shouldn't be null or empty");
            }

            var entities = _repository.EntitiesTable.Where(p => ids.Contains(p.Id)).ToList();
            if (softDelete)
            {
                entities.ForEach(e =>
                {
                    e.IsDelete = true;
                });
                _repository.Update(entities);
            }
            else
            {
                _repository.Delete(entities);
            }
        }

    }
}
