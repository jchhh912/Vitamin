using Application.Blog.Request;
using Application.Presistence;
using Domain.Blog;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Application.Blog
{
    public record GetCategoryCommand<T>(Guid? guid = null, Guid? postId = null, string? note = null, string? displayName = null) : IRequest<List<T>> where T : class;
    public class GetCategoryCommandHandler<T> : IRequestHandler<GetCategoryCommand<T>, IReadOnlyList<T>> where T : class
    {
        private readonly IDapperRepository _categorysRepository;
        public GetCategoryCommandHandler(
            IDapperRepository categorysRepository)
        {
            _categorysRepository = categorysRepository;
        }

        public async Task<IReadOnlyList<T>> Handle(GetCategoryCommand<T> request, CancellationToken cancellationToken)
        {
            List<string> where = new List<string>();
            DynamicParameters p = new DynamicParameters();
            if (request.guid != null)
            {
                p.Add("Id", request.guid);
                where.Add("Id=@Id");
            }
            if (request.postId != null)
            {
                p.Add("PostId", request.postId);
                where.Add("PostId=@PostId");
            }
            if (string.IsNullOrWhiteSpace(request.note))
            {
                p.Add("Note", request.note);
                where.Add("Note=@Note");
            }
            if (string.IsNullOrWhiteSpace(request.displayName))
            {
                p.Add("DisplayName", request.displayName);
                where.Add("DisplayName=@DisplayName");
            }
            string sql = $"SELECT * FROM Categorys";
            if (where.Count > 0)
            {
                sql += "WHERE" + string.Join(" AND ", where);
            }
            var result = await _categorysRepository.QueryAsync<T>(sql, p, cancellationToken: cancellationToken);
            return result;
        }
    }
    public record GetSingleCategoryCommand<T>(Guid? guid) : IRequest<T> where T : class;
    public class GetSingleCategoryCommandHandler<T> : IRequestHandler<GetSingleCategoryCommand<T>, T> where T : class
    {
        private readonly IDapperRepository _categorysRepository;
        public GetSingleCategoryCommandHandler(
            IDapperRepository categorysRepository)
        {
            _categorysRepository = categorysRepository;
        }

        public async Task<T> Handle(GetSingleCategoryCommand<T> request, CancellationToken cancellationToken)
        {
            List<string> where = new List<string>();
            DynamicParameters p = new DynamicParameters();
            if (request.guid != null)
            {
                p.Add("Id", request.guid);
                where.Add("Id=@Id");
            }
            string sql = $"SELECT * FROM Categorys";
            if (where.Count > 0)
            {
                sql += "WHERE" + string.Join(" AND ", where);
            }
            var result = await _categorysRepository.QuerySingleAsync<T>(sql, p, cancellationToken: cancellationToken);
            return result;
        }
    }
}
