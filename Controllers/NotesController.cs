using Microsoft.AspNetCore.Mvc;
using NotesApi.Dtos;
using NotesApi.Exceptions;
using NotesApi.Services;

namespace NotesApi.Controllers;

[ApiController]
[Route("notes")]
public class NotesController : ControllerBase
{
    private readonly INotesService _notesService;
    
    public NotesController(INotesService notesService)
    {
        _notesService = notesService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<NoteDto>> Get()
    {
        return Ok(_notesService.GetNotes());
    }

    [HttpGet("{noteIdentifier}")]
    public ActionResult<NoteDto> Get(Guid noteIdentifier) 
    {
        try
        {
            return Ok(_notesService.GetNote(noteIdentifier));
        }
        catch (NoteNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public ActionResult<NoteDto> Post(NoteCreateUpdateDto noteCreateUpdateDto) 
    {
        return Ok(_notesService.CreateNote(noteCreateUpdateDto));
    }

    [HttpPut("{noteIdentifier}")]
    public ActionResult<NoteDto> Put(Guid noteIdentifier, NoteCreateUpdateDto noteCreateUpdateDto) 
    {
        try
        {
            return Ok(_notesService.UpdateNote(noteIdentifier, noteCreateUpdateDto));
        }
        catch (NoteNotFoundException)
        {
            return NotFound();
        }
    }


    [HttpDelete("{noteIdentifier}")]
    public ActionResult Delete(Guid noteIdentifier) 
    {
        try
        {
            _notesService.DeleteNote(noteIdentifier);

            return NoContent();
        }
        catch (NoteNotFoundException)
        {
            return NotFound();
        }
    }    
}
