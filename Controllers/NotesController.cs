using Microsoft.AspNetCore.Mvc;
using NotesApi.Dtos;

namespace NotesApi.Controllers;

[ApiController]
[Route("notes")]
public class NotesController : ControllerBase
{
    private static IList<NoteDto> _notes = new List<NoteDto>
    {
        new NoteDto
        {
            NoteIdentifier = Guid.NewGuid(),
            NoteText = "A very fine man."
        }
    };

    private readonly ILogger<NotesController> _logger;

    public NotesController(ILogger<NotesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<NoteDto>> Get()
    {
        _logger.LogInformation("Retrieving notes.");

        return Ok(_notes);
    }

    [HttpGet("{noteIdentifier}")]
    public ActionResult<NoteDto> Get(Guid noteIdentifier) 
    {
        NoteDto noteDto = _notes.FirstOrDefault(n => n.NoteIdentifier == noteIdentifier);

        if (noteDto == null)
        {
            return NotFound();
        }

        return Ok(noteDto);
    }

    [HttpPost]
    public ActionResult<NoteDto> Post(NoteCreateUpdateDto noteCreateUpdateDto) 
    {
        NoteDto noteDto = new NoteDto { NoteIdentifier = Guid.NewGuid(), NoteText = noteCreateUpdateDto.NoteText };

        _notes.Add(noteDto);

        return Ok(noteDto);
    }

    [HttpPut("{noteIdentifier}")]
    public ActionResult<NoteDto> Put(Guid noteIdentifier, NoteCreateUpdateDto noteCreateUpdateDto) 
    {
        NoteDto noteDto = _notes.FirstOrDefault(n => n.NoteIdentifier == noteIdentifier);

        if (noteDto == null)
        {
            return NotFound();   
        }

        noteDto.NoteText = noteCreateUpdateDto.NoteText;

        return Ok(noteDto);
    }


    [HttpDelete("{noteIdentifier}")]
    public ActionResult Delete(Guid noteIdentifier) 
    {
        NoteDto noteDto = _notes.FirstOrDefault(n => n.NoteIdentifier == noteIdentifier);

        if (noteDto == null)
        {
            return NotFound();
        }

        _notes.Remove(noteDto);
    
        return Ok();
    }    
}
