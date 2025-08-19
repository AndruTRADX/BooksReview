using System;
using Application.Core;
using Application.DTOs;
using AutoMapper;
using Core.Entities;
using Infrastructure.Data;
using MediatR;

namespace Application.Books.Commands;

public class CreateBook
{
    public class Command : IRequest<Result<string>>
    {
        public required CreateBookDTO CreateBookDTO { get; set; }
    }

    public class Handler(ApplicationDbContext context, IMapper mapper) : IRequestHandler<Command, Result<string>>
    {
        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var book = mapper.Map<Book>(request.CreateBookDTO);

            context.Books.Add(book);

            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<string>.Failure("Failed to create the book", 400);

            return Result<string>.Success(book.Id);
        }
    }
}
