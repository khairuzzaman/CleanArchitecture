﻿using System.Threading;
using System.Threading.Tasks;
using Identity.Domain.Entities;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace Identity.Application.Queries.UserQueries
{
    public class GetPasswordResetCodeQuery : IRequest<PasswordResetCode>
    {
        public GetPasswordResetCodeQuery(string email, string code)
        {
            Email = email.ThrowIfNotValidEmail(nameof(email));
            Code = code.ThrowIfNullOrEmpty(nameof(code));
        }

        public string Email { get; }

        public string Code { get; }

        private class GetPasswordResetCodeQueryHandler : IRequestHandler<GetPasswordResetCodeQuery, PasswordResetCode>
        {
            private readonly IRepository _repository;

            public GetPasswordResetCodeQueryHandler(IRepository repository)
            {
                _repository = repository;
            }

            public async Task<PasswordResetCode> Handle(GetPasswordResetCodeQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                PasswordResetCode passwordResetCode = await _repository
                .GetAsync<PasswordResetCode>(evc => evc.Email == request.Email && evc.Code == request.Code && evc.UsedAtUtc == null);

                return passwordResetCode;
            }
        }
    }
}