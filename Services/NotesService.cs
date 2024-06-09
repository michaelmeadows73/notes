using NotesApi.Dtos;
using NotesApi.Exceptions;

namespace NotesApi.Services;

public class NotesService : INotesService
{
    private static IList<NoteDto> _notes = new List<NoteDto>
    {
        new NoteDto
        {
            NoteIdentifier = Guid.NewGuid(),
            NoteText = "A very fine man."
        }
    };

    public IEnumerable<NoteDto> GetNotes()
    {
        return _notes;
    }

    public NoteDto GetNote(Guid noteIdentifier)
    {
        NoteDto? noteDto = _notes.FirstOrDefault(n => n.NoteIdentifier == noteIdentifier);

        if (noteDto == null)
        {
            throw new NoteNotFoundException();
        }

        return noteDto;
    }

    public NoteDto CreateNote(NoteCreateUpdateDto noteCreateUpdateDto)
    {
        NoteDto noteDto = new NoteDto { NoteIdentifier = Guid.NewGuid(), NoteText = noteCreateUpdateDto.NoteText };

        _notes.Add(noteDto);

        return noteDto;
    }

    public NoteDto UpdateNote(Guid noteIdentifier, NoteCreateUpdateDto noteCreateUpdateDto)
    {
        NoteDto? noteDto = _notes.FirstOrDefault(n => n.NoteIdentifier == noteIdentifier);

        if (noteDto == null)
        {
            throw new NoteNotFoundException();
        }

        noteDto.NoteText = noteCreateUpdateDto.NoteText;

        return noteDto;
    }

    public void DeleteNote(Guid noteIdentifier)
    {
        NoteDto? noteDto = _notes.FirstOrDefault(n => n.NoteIdentifier == noteIdentifier);

        if (noteDto == null)
        {
            throw new NoteNotFoundException();
        }

        _notes.Remove(noteDto);
    }
}
