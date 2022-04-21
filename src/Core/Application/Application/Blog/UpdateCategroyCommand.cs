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

    public record UpdateCategroyCommand(CreateCategoryRequest Payload) : IRequest<Guid>;
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategroyCommand, Guid>
    {
        private readonly IRepository<Categorys> _categorysRepository;
        public UpdateCategoryCommandHandler(
            IRepository<Categorys> categorysRepository)
        {
            _categorysRepository = categorysRepository;
        }

        public async Task<Guid> Handle(UpdateCategroyCommand request, CancellationToken cancellationToken)
        {
            var cate = new Categorys(request.Payload.DisplayName, request.Payload.Note);
            cate.PostId = request.Payload.PostId;
            await _categorysRepository.UpdateAsync(cate);
            return cate.Id;
        }
    }
}
