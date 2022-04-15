using Application.Blog.Request;
using Application.Presistence;
using Domain.Blog;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blog
{
    public record CreateCategoryCommand(CreateCategoryRequest Payload) : IRequest<Guid>;
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly IRepository<Categorys> _categorysRepository;
        public CreateCategoryCommandHandler(
            IRepository<Categorys> categorysRepository)
        {
            _categorysRepository = categorysRepository;
        }

        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var cate = new Categorys(request.Payload.DisplayName, request.Payload.Note);
            cate.PostId = request.Payload.PostId;
            await _categorysRepository.AddAsync(cate);
            return cate.Id;
        }
    }
}
