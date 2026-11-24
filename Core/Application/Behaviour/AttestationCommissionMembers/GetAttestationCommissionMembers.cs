using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Behaviour.AttestationCommissionMembers;

public sealed record GetAttestationCommissionMembersQuery(int? CommissionId, string? FullName, string? Position, string? Role, int? SortOrder) : IRequest<IReadOnlyList<AttestationCommissionMember>>;

public sealed class GetAttestationCommissionMembersHandler(IAttestationCommissionMemberRepository repository) : IRequestHandler<GetAttestationCommissionMembersQuery, IReadOnlyList<AttestationCommissionMember>>
{
    public async Task<IReadOnlyList<AttestationCommissionMember>> Handle(GetAttestationCommissionMembersQuery request, CancellationToken cancellationToken)
        => await repository.GetAllAsync(request.CommissionId, request.FullName, request.Position, request.Role, request.SortOrder, cancellationToken);
}