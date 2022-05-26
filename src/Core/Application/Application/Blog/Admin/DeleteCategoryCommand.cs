

using Application.Blog.Spc;
using Application.Common.Exceptions;
using Application.Presistence;
using Domain.Blog;
using MediatR;

namespace Application.Blog.Admin;

public record DeleteCategoryCommand(string DisplayName) : IRequest;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly IRepository<Categorys> _catRepo;
    public DeleteCategoryCommandHandler(IRepository<Categorys> catRepo) => _catRepo = catRepo;

    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var list=await _catRepo.GetListAsync(new CategorySpec(request.DisplayName));
        _ = list ?? throw new NotFoundException("categorys.notfound");
        foreach (var cs in list)
        {
           await _catRepo.DeleteAsync(cs);
        }
        return Unit.Value;

    }
}

