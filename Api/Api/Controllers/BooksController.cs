using System;
using API.Controllers;
using Application.Books.Commands;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class BooksController() : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<string>> CreateBook(CreateBookDTO createBookDTO)
    {
        return HandleResult(await Mediator.Send(new CreateBook.Command { CreateBookDTO = createBookDTO }));
    }
}
