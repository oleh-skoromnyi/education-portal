using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EducationPortal.Core
{
    public class Specification<TEntity>
        where TEntity : Entity
    {
        public Expression<Func<TEntity, bool>> Expression { get; }
        public List<Expression<Func<TEntity, object>>> Include { get; set; }

        public Func<TEntity, bool> Func => this.Expression.Compile();

        public Specification(Expression<Func<TEntity, bool>> expression, List<Expression<Func<TEntity, object>>> include = default)
        {
            this.Expression = expression;
            if (include == default)
            { 
                this.Include = new List<Expression<Func<TEntity, object>>>(); 
            }
            else 
            {
                this.Include = include;
            }
        }

        public bool IsSatisfiedBy(TEntity entity)
        {
            return this.Func(entity);
        }
    }

    public class FindByIdSpecification<TEntity> : Specification<TEntity>
        where TEntity : Entity
    {
        public FindByIdSpecification(int id)
            :base(x => x.Id == id){}
    }


    public class FindUserByLoginSpecification : Specification<User>
    {
        public FindUserByLoginSpecification(string login)
            : base(x => x.Login == login) { }
    }
}