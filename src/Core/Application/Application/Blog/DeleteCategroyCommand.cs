using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Blog.Request;
using Application.Presistence;
using Domain.Blog;
using MediatR;

namespace Application.Blog
{

    public record DeleteCategroyCommand(Guid key) : IRequest<bool>;
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategroyCommand, bool>
    {
        private readonly IRepository<Categorys> _categorysRepository;
        public DeleteCategoryCommandHandler(
            IRepository<Categorys> categorysRepository)
        {
            _categorysRepository = categorysRepository;
        }

        public async Task<bool> Handle(DeleteCategroyCommand request, CancellationToken cancellationToken)
        {
            await _categorysRepository.DeleteAsync(request.key);
            return true;
        }

    }
}
