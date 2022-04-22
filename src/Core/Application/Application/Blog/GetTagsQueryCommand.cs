

using Application.Presistence;
using Domain.Blog;
using MediatR;
using System.Linq.Expressions;

namespace Application.Blog;

public record GetTagsQueryCommand():IRequest<IReadOnlyList<Tags>>;
public class GetTagsQueryCommandHandler : IRequestHandler<GetTagsQueryCommand, IReadOnlyList<Tags>>
{
    private readonly IRepository<Tags> _tagRepo;
    private readonly Expression<Func<IGrouping<Tags, Tags>, Tags>> _expression =
        t => new(t.Key.DisplayName);
    public GetTagsQueryCommandHandler(IRepository<Tags> tagRepo) => _tagRepo = tagRepo;

    public async Task<IReadOnlyList<Tags>> Handle(GetTagsQueryCommand request, CancellationToken cancellationToken)
    {
        var cats = await _tagRepo.SelectAsync(c => new(c.DisplayName), _expression, null);
        return cats;
    }
}

