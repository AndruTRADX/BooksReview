using System;
using System.Net;
using Application.Core;
using Application.DTOs;
using Application.Strategies.GenerateId;
using AutoMapper;
using Core.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Books.Commands;

public class CreateBook
{
    public class Command : IRequest<Result<string>>
    {
        public required CreateBookDTO CreateBookDTO { get; set; }
    }

    public class Handler(ApplicationDbContext context, IMapper mapper, IGenerateIdStrategy<Book> generateIdStrategy) : IRequestHandler<Command, Result<string>>
    {
        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var book = mapper.Map<Book>(request.CreateBookDTO);

            bool bookExists = await CheckIfBookExistsAsync(request.CreateBookDTO.Title, request.CreateBookDTO.Author, request.CreateBookDTO.ISBN, cancellationToken);
            
            if (bookExists)
            {
                return Result<string>.Failure("There is already a book with the same title, author and ISBN", (int)HttpStatusCode.Conflict);
            }

            string generatedId = generateIdStrategy.GenerateId(book);

            book.Id = generatedId;

            context.Books.Add(book);

            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<string>.Failure("Failed to create the book", 400);

            return Result<string>.Success(book.Id);
        }

        private async Task<bool> CheckIfBookExistsAsync(string title, string author, string? isbn, CancellationToken cancellationToken)
        {
            var query = context.Books.AsQueryable();

            query = query.Where(b =>
                b.Title == title &&
                b.Author == author);

            if (isbn == null)
            {
                query = query.Where(b => b.ISBN == null);
            }
            else
            {
                query = query.Where(b => b.ISBN == isbn);
            }

            return await query.AnyAsync(cancellationToken);
        }
    }
}
