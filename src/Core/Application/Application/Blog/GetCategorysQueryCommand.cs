
using Application.Blog.Request;
using Application.Presistence;
using Domain.Blog;
using MediatR;
namespace Application.Blog;

public record GetCategorysQueryCommand():IRequest<IReadOnlyList<CategoryDto>>;
public class GetCategorysQueryCommandHandler : IRequestHandler<GetCategorysQueryCommand, IReadOnlyList<CategoryDto>>
{
    private readonly IRepository<Categorys> _catRepo;
    public GetCategorysQueryCommandHandler(IRepository<Categorys> catRepo) => _catRepo = catRepo;

    public async Task<IReadOnlyList<CategoryDto>> Handle(GetCategorysQueryCommand request, CancellationToken cancellationToken)
    {
        var cats = await _catRepo.SelectAsync(c=>new(c.DisplayName),CategoryDto.EntitySelector,null);
        return cats;    
    }
}
